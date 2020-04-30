using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNetCore.Introduction.Configuration;
using AspNetCore.Introduction.Controllers;
using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;
using AspNetCore.Introduction.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace AspNetCore.Introduction.XUnitTests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts()
        {
            // Arrange
            var mockProductRepo = new Mock<IDbProductRepository>();
            var mockCategoryRepo = new Mock<IDbCategoryRepository>();
            var mockSupplierRepo = new Mock<IDbSupplierRepository>();
            var mockConfig = new Mock<IOptions<MandatoryInfoConfiguration>>();

            mockProductRepo.Setup(repo => repo.Get(It.IsAny<Expression<Func<Products, bool>>>(), null, It.IsAny<string>()))
                .Returns(GetProducts());
            mockConfig.Setup(config => config.Value)
                .Returns(GetMandatoryInfoConfiguration());

            var controller = new ProductsController(
                mockConfig.Object,
                mockProductRepo.Object,
                mockCategoryRepo.Object,
                mockSupplierRepo.Object);

            // Act
            var result = controller.Index(string.Empty, string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCategoryViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
        }

        private static IEnumerable<Products> GetProducts()
        {
            var products = new List<Products>
            {
                new Products() {ProductId = 1, ProductName = "Product name 1", UnitPrice = 15},
                new Products() {ProductId = 2, ProductName = "Product name 2", UnitPrice = 25}
            };
            return products;
        }

        private static MandatoryInfoConfiguration GetMandatoryInfoConfiguration()
        {
            return new MandatoryInfoConfiguration()
            {
                MaxItemsInList = 15
            };
        }
    }
}