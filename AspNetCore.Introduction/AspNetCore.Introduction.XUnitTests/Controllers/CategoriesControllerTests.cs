using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNetCore.Introduction.Controllers;
using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;
using AspNetCore.Introduction.XUnitTests.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AspNetCore.Introduction.XUnitTests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<IDbCategoryRepository> _mockCategoryRepo;

        public CategoriesControllerTests()
        {
            _mockCategoryRepo = new Mock<IDbCategoryRepository>();
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts()
        {
            // Arrange
            SetupMockCategoryRepo();

            var controller = new CategoriesController(_mockCategoryRepo.Object);

            // Act
            var result = await controller.Index(string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<Categories>>(
                viewResult.ViewData.Model);

            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsAViewResult_DetailsOfProduct()
        {
            // Arrange
            SetupMockCategoryRepo();

            var controller = new CategoriesController(_mockCategoryRepo.Object);


            // Act
            var result = await controller.Details(new Random().Next(int.MaxValue));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<Categories>(
                viewResult.ViewData.Model);

            Assert.NotNull(model);
        }

        [Fact]
        public void Create_ReturnsAViewResult_ReturnTemplate()
        {
            // Arrange
            SetupMockCategoryRepo();

            var controller = new CategoriesController(_mockCategoryRepo.Object);


            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_ReturnsAViewResult_AddsProduct()
        {
            // Arrange
            SetupMockCategoryRepo();

            var controller = new CategoriesController(_mockCategoryRepo.Object);

            var createdCategory = CategoryFactory.Category();

            // Act
            var result = await controller.Create(createdCategory);

            // Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);

            var actionNameIsIndex = redirectToAction.ActionName == "Index";

            Assert.True(actionNameIsIndex);

            Assert.NotNull(redirectToAction);

            VerifyMockCategoryRepo();
        }

        [Fact]
        public async Task Edit_ReturnsAViewResult_ReturnTemplate()
        {
            // Arrange
            SetupMockCategoryRepo();

            var controller = new CategoriesController(_mockCategoryRepo.Object);


            // Act
            var result = await controller.Edit(new Random().Next(int.MaxValue));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<Categories>(
                viewResult.ViewData.Model);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Edit_ReturnsAViewResult_ReturnEditableProduct()
        {
            // Arrange
            SetupMockCategoryRepo();

            var controller = new CategoriesController(_mockCategoryRepo.Object);

            var editedCategory = CategoryFactory.Category();

            // Act
            var result = await controller.Edit(editedCategory.CategoryId);

            // Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);

            var actionNameIsIndex = redirectToAction.ActionName == "Index";

            Assert.True(actionNameIsIndex);

            Assert.NotNull(redirectToAction);

            VerifyMockCategoryRepo();
        }

        [Fact]
        public async Task Delete_ReturnsAViewResult_ReturnTemplate()
        {
            // Arrange
            SetupMockCategoryRepo();

            var controller = new CategoriesController(_mockCategoryRepo.Object);


            // Act
            var result = await controller.Delete(new Random().Next(int.MaxValue));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<Categories>(
                viewResult.ViewData.Model);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task Delete_ReturnsAViewResult_DeleteConfirmed()
        {
            // Arrange
            SetupMockCategoryRepo();

            var controller = new CategoriesController(_mockCategoryRepo.Object);


            // Act
            var result = await controller.DeleteConfirmed(new Random().Next(int.MaxValue));

            // Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);

            var actionNameIsIndex = redirectToAction.ActionName == "Index";

            Assert.True(actionNameIsIndex);

            Assert.NotNull(redirectToAction);

            VerifyMockCategoryRepo();
        }

        private void SetupMockCategoryRepo()
        {
            _mockCategoryRepo.Setup(repo =>
                    repo.GetAsync(It.IsAny<Expression<Func<Categories, bool>>>(), null, It.IsAny<string>()))
                .Returns(CategoryFactory.GetTwoCategoriesAsync());
            _mockCategoryRepo.Setup(repo =>
                    repo.FindAsync(It.IsAny<int?>()))
                .Returns(Task.FromResult(new Categories()));
            _mockCategoryRepo.Setup(repo =>
                repo.Insert(It.IsAny<Categories>()));
            _mockCategoryRepo.Setup(repo =>
                repo.Delete(It.IsAny<Categories>()));
            _mockCategoryRepo.Setup(repo =>
                repo.Update(It.IsAny<Categories>()));
        }

        private void VerifyMockCategoryRepo()
        {
            _mockCategoryRepo.Verify(mock =>
                    mock.SaveChangesAsync(),
                Times.Once);
        }
    }
}