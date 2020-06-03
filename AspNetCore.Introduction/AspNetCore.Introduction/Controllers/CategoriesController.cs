using AspNetCore.Introduction.Extensions;
using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;
using AspNetCore.Introduction.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCore.Introduction.Controllers
{
    public class CategoriesController : Controller
    {

        private const int StartBytePositionForImage = 78;

        private readonly IDbCategoryRepository _categoryRepository;

        public CategoriesController(IDbCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        private async Task AddedImageIfUploaded(Categories categories, IFormFile file)
        {
            if (file != null)
            {
                var fileLength = file.Length;
                if (fileLength <= 0)
                {
                    await FillWithImage(categories, file);
                }
            }
        }

        private bool CategoriesExists(int id)
        {
            return _categoryRepository.Get().Any(e => e.CategoryId == id);
        }

        private async Task FillWithImage(Categories category, IFormFile formFile)
        {
            var filePath = Path.GetTempFileName();
            await using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);
            category.Picture = GetImage(stream.ReadToEnd());
        }

        private async Task FillWithImage(Categories category, IFormFile formFile, string filePath)
        {
            await using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);
            category.Picture = GetImage(stream.ReadToEnd());
        }

        private static Expression<Func<Categories, bool>> GetCategoryFilter(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return p => p.CategoryName.Contains(searchString);
            }

            return p => true;
        }

        private static byte[] GetImage(Categories category)
        {
            var categoryLocal = category;
            const int startBytePositionForImage = 78;
            var image = categoryLocal.Picture?.Skip(startBytePositionForImage).ToArray();
            return image;
        }


        private byte[] GetImage(IReadOnlyCollection<byte> image)
        {
            if (image == null || image.Count <= 0)
            {
                return new byte[0];
            }
            var prefix = new byte[78];
            var result = new List<byte>();
            result.AddRange(prefix);
            result.AddRange(image);
            return result.ToArray();
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description,Picture")]
            Categories categories)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Insert(categories);
                await _categoryRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categories);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await _categoryRepository.FindAsync(id);
            _categoryRepository.Delete(categories);
            await _categoryRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,Picture")]
            Categories categories, [Bind("file")] IFormFile file)
        {
            if (id != categories.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await AddedImageIfUploaded(categories, file);

                    _categoryRepository.Update(categories);
                    await _categoryRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(categories.CategoryId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(categories);
        }


        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file, Categories category)
        {
            var size = file.Length;
            var filePaths = new List<string>();
            var formFile = file;
            if (formFile.Length <= 0) return Ok(new { count = 1, size, filePaths });
            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            await FillWithImage(category, formFile, filePath);
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return RedirectToAction("ShowInList", new { id = category.CategoryId });
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
        }

        // GET: Categories
        public async Task<IActionResult> Index(string searchString)
        {
            var categories = await _categoryRepository
                .GetAsync(GetCategoryFilter(searchString));

            return View(categories.ToList());
        }

        // GET: Categories/Show/5
        [HttpGet]
        public async Task<IActionResult> Show(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var picture = category.Picture;

            if (picture == null)
            {
                return NotFound();
            }

            var image = picture.Skip(StartBytePositionForImage).ToArray();

            return File(image, "image/jpg");
        }

        // GET: Categories/Show/5
        [HttpGet]
        public async Task<IActionResult> ShowInList(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                ViewBag.ImageNotFoundId = id;
                return View("ImageNotFound");
            }

            var imageVM = new ImageViewModel { Category = category, Title = category.CategoryName, Image = GetImage(category) };

            if (imageVM.Image == null)
            {
                imageVM.Description = "There is no image for this category.";
                return View("ShowInListNoImage", imageVM);
            }

            return View(imageVM);
        }

        // GET: Categories/Show/5
        [HttpGet]
        public async Task<IActionResult> ShowInListFromCache(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                ViewBag.ImageNotFoundId = id;
                return View("ImageNotFound");
            }

            var imageVM = new ImageViewModel { Category = category, Title = category.CategoryName, Image = GetImage(category) };

            if (imageVM.Image == null)
            {
                imageVM.Description = "There is no image for this category.";
                return View("ShowInListNoImage", imageVM);
            }

            return View(imageVM);
        }
    }
}