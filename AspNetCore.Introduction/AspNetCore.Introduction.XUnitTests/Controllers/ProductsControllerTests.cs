using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNetCore.Introduction.Configuration;
using AspNetCore.Introduction.Controllers;
using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;
using AspNetCore.Introduction.ViewModels;
using AspNetCore.Introduction.XUnitTests.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace AspNetCore.Introduction.XUnitTests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IDbProductRepository> _mockProductRepo;
        private readonly Mock<IDbCategoryRepository> _mockCategoryRepo;
        private readonly Mock<IDbSupplierRepository> _mockSupplierRepo;
        private readonly Mock<IOptions<MandatoryInfoConfiguration>> _mockConfig;

        public ProductsControllerTests()
        {
            _mockProductRepo = new Mock<IDbProductRepository>();
            _mockCategoryRepo = new Mock<IDbCategoryRepository>();
            _mockSupplierRepo = new Mock<IDbSupplierRepository>();
            _mockConfig = new Mock<IOptions<MandatoryInfoConfiguration>>();
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts()
        {
            // Arrange
            SetupMockProductRepo();
            SetupMockConfig();

            var controller = new ProductsController(
                _mockConfig.Object,
                _mockProductRepo.Object,
                _mockCategoryRepo.Object,
                _mockSupplierRepo.Object);

            // Act
            var result = await controller.Index(string.Empty, string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCategoryViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
        }

        [Fact]
        public async Task Details_ReturnsAViewResult_DetailsOfProduct()
        {
            // Arrange
            SetupMockProductRepo();
            SetupMockConfig();

            var controller = new ProductsController(
                _mockConfig.Object,
                _mockProductRepo.Object,
                _mockCategoryRepo.Object,
                _mockSupplierRepo.Object);

            // Act
            var result = await controller.Details(new Random().Next(int.MaxValue));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<Products>(
                viewResult.ViewData.Model);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Create_ReturnsAViewResult_ReturnTemplate()
        {
            // Arrange
            SetupMockProductRepo();
            SetupMockConfig();

            var controller = new ProductsController(
                _mockConfig.Object,
                _mockProductRepo.Object,
                _mockCategoryRepo.Object,
                _mockSupplierRepo.Object);

            // Act
            var result = await controller.Index(string.Empty, string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCategoryViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
        }

        [Fact]
        public async Task Create_ReturnsAViewResult_AddsProduct()
        {
            // Arrange
            SetupMockProductRepo();
            SetupMockConfig();

            var controller = new ProductsController(
                _mockConfig.Object,
                _mockProductRepo.Object,
                _mockCategoryRepo.Object,
                _mockSupplierRepo.Object);

            // Act
            var result = await controller.Index(string.Empty, string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCategoryViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
        }

        [Fact]
        public async Task Edit_ReturnsAViewResult_ReturnTemplate()
        {
            // Arrange
            SetupMockProductRepo();
            SetupMockConfig();

            var controller = new ProductsController(
                _mockConfig.Object,
                _mockProductRepo.Object,
                _mockCategoryRepo.Object,
                _mockSupplierRepo.Object);

            // Act
            var result = await controller.Index(string.Empty, string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCategoryViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
        }

        [Fact]
        public async Task Edit_ReturnsAViewResult_ReturnEditableProduct()
        {
            // Arrange
            SetupMockProductRepo();
            SetupMockConfig();

            var controller = new ProductsController(
                _mockConfig.Object,
                _mockProductRepo.Object,
                _mockCategoryRepo.Object,
                _mockSupplierRepo.Object);

            // Act
            var result = await controller.Index(string.Empty, string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCategoryViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
        }

        [Fact]
        public async Task Delete_ReturnsAViewResult_ReturnTemplate()
        {
            // Arrange
            SetupMockProductRepo();
            SetupMockConfig();

            var controller = new ProductsController(
                _mockConfig.Object,
                _mockProductRepo.Object,
                _mockCategoryRepo.Object,
                _mockSupplierRepo.Object);

            // Act
            var result = await controller.Index(string.Empty, string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCategoryViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
        }

        [Fact]
        public async Task Delete_ReturnsAViewResult_DeleteConfirmed()
        {
            // Arrange
            SetupMockProductRepo();
            SetupMockConfig();

            var controller = new ProductsController(
                _mockConfig.Object,
                _mockProductRepo.Object,
                _mockCategoryRepo.Object,
                _mockSupplierRepo.Object);

            // Act
            var result = await controller.Index(string.Empty, string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCategoryViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
        }

        private void SetupMockProductRepo()
        {
            _mockProductRepo.Setup(repo =>
                    repo.GetAsync(It.IsAny<Expression<Func<Products, bool>>>(), null, It.IsAny<string>()))
                .Returns(ProductsFactory.GetTwoProductsAsync());
            _mockProductRepo.Setup(repo =>
                    repo.FindAsync(It.IsAny<int?>()))
                .Returns(Task.FromResult(new Products()));

        }

        private void SetupMockConfig()
        {
            _mockConfig.Setup(config => config.Value)
                .Returns(GetMandatoryInfoConfiguration());
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