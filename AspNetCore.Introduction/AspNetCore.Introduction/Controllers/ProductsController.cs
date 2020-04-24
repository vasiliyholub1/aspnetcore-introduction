﻿using System.Linq;
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

        public ProductsController(AspNetCoreIntroductionContext context, IOptions<MandatoryInfoConfiguration> config)
        {
            _configuration = config.Value;
            _context = context;
        }

        // GET: Products
        [HttpGet]
        public async Task<IActionResult> Index(string productCategory, string searchString)
        {
            IQueryable<string> categoryQuery = from m in _context.Products
                orderby m.Category.CategoryName
                select m.Category.CategoryName;

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
                Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Products Products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Products);
        }

        // GET: Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Products = await _context.Products.FindAsync(id);
            if (Products == null)
            {
                return NotFound();
            }
            return View(Products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Products Products)
        {
            if (id != Products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(Products.ProductId))
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
