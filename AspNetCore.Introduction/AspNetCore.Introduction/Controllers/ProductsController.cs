using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Introduction.Configuration;
using AspNetCore.Introduction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspNetCore.Introduction.Controllers
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ViewModels;

    public class ProductsController : Controller
    {
        private readonly AspNetCoreIntroductionContext _context;

        private readonly MandatoryInfoConfiguration _configuration;

        private IQueryable<Categories> CategoryQuery =>
            from c in _context.Categories
            orderby c.CategoryName
            select c;

        private IQueryable<Suppliers> SupplierQuery =>
            from e in _context.Suppliers
            orderby e.CompanyName
            select e;

        private SelectList SupplierList
        {
            get
            {
                var value = GetFieldName(typeof(Suppliers), "SupplierId");
                var text = GetFieldName(typeof(Suppliers), "CompanyName");
                return new SelectList(SupplierQuery.ToList(), value, text);
            }
        }

        private SelectList CategoryList
        {
            get
            {
                var value = GetFieldName(typeof(Categories), "CategoryId");
                var text = GetFieldName(typeof(Categories), "CategoryName");
                return new SelectList(CategoryQuery.ToList(), value, text);
            }
        }

        private string GetFieldName(Type type, string supposeName)
        {
            var supplierProperties = type.GetProperties();
            return supplierProperties.First(s => s.Name == supposeName).Name;
        }

        public ProductsController(AspNetCoreIntroductionContext context, IOptions<MandatoryInfoConfiguration> config)
        {
            _configuration = config.Value;
            _context = context;
        }

        // GET: Products
        [HttpGet]
        public async Task<IActionResult> Index(string productCategory, string searchString)
        {
            var products = from p in _context.Products
                select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(productCategory))
            {
                products = products.Where(x => x.Category.CategoryName == productCategory);
            }

            var maxItemInList = _configuration.MaxItemsInList;

            products = products
                .Include(p => p.Category)
                .Include(p => p.Supplier);

            products = maxItemInList > 0
                ? products.Take(maxItemInList)
                : products;

            var productCategoryVM = new ProductCategoryViewModel()
            {
                Categories = new SelectList(await CategoryQuery.Select(c => c.CategoryName).Distinct().ToListAsync()),
                Products = await products.ToListAsync()
            };

            return View(productCategoryVM);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Products = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (Products == null)
            {
                return NotFound();
            }

            return View(Products);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var productCreationVM = new ProductCreationViewModel()
            {
                Product = new Products(),
                Categories = CategoryList,
                Suppliers = SupplierList
            };

            return View(productCreationVM);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Product,Categories,Suppliers")] ProductCreationViewModel productCreationViewModel)
        {
            var productAdding = productCreationViewModel.Product;
            if (ModelState.IsValid)
            {
                _context.Add(productAdding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productAdding);
        }

        // GET: Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            var productCreationVM = new ProductCreationViewModel()
            {
                Product = products,
                Categories = CategoryList,
                Suppliers = SupplierList
            };

            return View(productCreationVM);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Product,Categories,Suppliers")] ProductCreationViewModel productCreationViewModel)
        {
            if (id != productCreationViewModel.Product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCreationViewModel.Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(productCreationViewModel.Product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Products = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (Products == null)
            {
                return NotFound();
            }

            return View(Products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Products = await _context.Products.FindAsync(id);
            _context.Products.Remove(Products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
