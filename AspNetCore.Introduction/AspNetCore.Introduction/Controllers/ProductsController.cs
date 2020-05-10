using AspNetCore.Introduction.Configuration;
using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;
using AspNetCore.Introduction.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCore.Introduction.Controllers
{

    public class ProductsController : Controller
    {
        private readonly IDbCategoryRepository _categoryRepository;

        private readonly MandatoryInfoConfiguration _configuration;
        private readonly IDbProductRepository _productRepository;
        private readonly IDbSupplierRepository _supplierRepository;

        public ProductsController(IOptions<MandatoryInfoConfiguration> config, IDbProductRepository productRepository, IDbCategoryRepository categoryRepository, IDbSupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            _configuration = config.Value;
        }

        private static string GetFieldName(Type type, string supposeName)
        {
            var supplierProperties = type.GetProperties();
            return supplierProperties.First(s => s.Name == supposeName).Name;
        }

        private static Expression<Func<Products, bool>> GetProductsFilter(string productCategory, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                if (!string.IsNullOrEmpty(productCategory))
                {
                    return p => p.ProductName.Contains(searchString) && p.Category.CategoryName == productCategory;
                }

                return p => p.ProductName.Contains(searchString);
            }

            if (!string.IsNullOrEmpty(productCategory))
            {
                return p => p.Category.CategoryName == productCategory;
            }

            return p => true;
        }

        private bool ProductsExists(int id)
        {
            return _productRepository.Get().Any(e => e.ProductId == id);
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

        private IQueryable<Categories> CategoryQuery => _categoryRepository.Queryable().OrderBy(c => c.CategoryName);

        private SelectList SupplierList
        {
            get
            {
                var value = GetFieldName(typeof(Suppliers), "SupplierId");
                var text = GetFieldName(typeof(Suppliers), "CompanyName");
                return new SelectList(SupplierQuery.ToList(), value, text);
            }
        }

        private IQueryable<Suppliers> SupplierQuery => _supplierRepository.Queryable().OrderBy(s => s.CompanyName);

        // GET: Products/Create
        public IActionResult Create()
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
                _productRepository.Insert(productAdding);
                await _productRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productAdding);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _productRepository.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Products = await _productRepository.FindAsync(id);
            _productRepository.Delete(Products);
            await _productRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _productRepository.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _productRepository.FindAsync(id);

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
                    _productRepository.Update(productCreationViewModel.Product);
                    await _productRepository.SaveChangesAsync();
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
            return View(productCreationViewModel);
        }

        // GET: Products
        [HttpGet]
        public async Task<IActionResult> Index(string productCategory, string searchString)
        {
            var products = await _productRepository
                .GetAsync(GetProductsFilter(productCategory, searchString),
                    null, "Category,Supplier");

            var maxItemInList = _configuration.MaxItemsInList;


            products = maxItemInList > 0
                ? products.Take(maxItemInList)
                : products;

            var productCategoryVM = new ProductCategoryViewModel()
            {
                Categories = new SelectList(CategoryQuery.Select(c => c.CategoryName).Distinct().ToList()),
                Products = products.ToList()
            };

            return View(productCategoryVM);
        }
    }
}
