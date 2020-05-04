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
        public void Create_ReturnsAViewResult_ReturnTemplate()
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
            var result = controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCreationViewModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(model.Product);
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
            var createdProductVm = new ProductCreationViewModel {Product = ProductsFactory.Product()};

            // Act
            var result = await controller.Create(createdProductVm);

            // Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);

            var actionNameIsIndex = redirectToAction.ActionName == "Index";

            Assert.True(actionNameIsIndex);

            Assert.NotNull(redirectToAction);

            VerifyMockProductRepo();
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
            var result = await controller.Edit(new Random().Next(int.MaxValue));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductCreationViewModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(model.Product);
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
            var createdProductVm = new ProductCreationViewModel {Product = ProductsFactory.Product()};

            // Act
            var result = await controller.Edit(createdProductVm.Product.ProductId, createdProductVm);

            // Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);

            var actionNameIsIndex = redirectToAction.ActionName == "Index";

            Assert.True(actionNameIsIndex);

            Assert.NotNull(redirectToAction);

            VerifyMockProductRepo();
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
            var result = await controller.Delete(new Random().Next(int.MaxValue));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<Products>(
                viewResult.ViewData.Model);

            Assert.NotNull(model);
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
            var result = await controller.DeleteConfirmed(new Random().Next(int.MaxValue));

            // Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);

            var actionNameIsIndex = redirectToAction.ActionName == "Index";

            Assert.True(actionNameIsIndex);

            Assert.NotNull(redirectToAction);

            VerifyMockProductRepo();
        }

        private void SetupMockProductRepo()
        {
            _mockProductRepo.Setup(repo =>
                    repo.GetAsync(It.IsAny<Expression<Func<Products, bool>>>(), null, It.IsAny<string>()))
                .Returns(ProductsFactory.GetTwoProductsAsync());
            _mockProductRepo.Setup(repo =>
                    repo.FindAsync(It.IsAny<int?>()))
                .Returns(Task.FromResult(new Products()));
            _mockProductRepo.Setup(repo =>
                repo.Insert(It.IsAny<Products>()));
            _mockProductRepo.Setup(repo =>
                repo.Delete(It.IsAny<Products>()));
            _mockProductRepo.Setup(repo =>
                repo.Update(It.IsAny<Products>()));
        }

        private void VerifyMockProductRepo()
        {
            _mockProductRepo.Verify(mock =>
                    mock.SaveChangesAsync(),
                Times.Once);
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