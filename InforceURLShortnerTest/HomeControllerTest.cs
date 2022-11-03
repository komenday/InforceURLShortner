using FluentAssertions;
using InforceURLShortner.Controllers;
using InforceURLShortner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InforceURLShortnerTest
{
    public class HomeControllerTest
    {
        private async Task<InforceShortnerContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<InforceShortnerContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            var databaseContext = new InforceShortnerContext(options);
            databaseContext.Database.EnsureCreated();

            await databaseContext.Urls.AddRangeAsync(
                    new Url()
                    {
                        Id = 1,
                        FullURL = "https://getbootstrap.com/docs/5.2/content/typography/",
                        ShortURL = "8Ktn0",
                        CreationDate = DateTime.Now,
                        AuthorLogin = "admin",
                    },
                    new Url()
                    {
                        Id = 2,
                        FullURL = "https://www.linkedin.com/feed/",
                        ShortURL = "bgOtAN",
                        CreationDate = DateTime.Now,
                        AuthorLogin = "admin",
                    }
                 );
            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }

        [Fact]
        public async void HomeController_Index_ReturnsSuccess()
        {
            //Arrange
            var _dbContext = GetDbContext();
            var _productsController = new HomeController(await _dbContext);
            //Act
            var result = _productsController.Index();
            //Assert
            result.Should().NotBeOfType<NotFoundResult>();
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public async void HomeController_Create_ReturnsSuccess()
        {
            //Arrange
            var _dbContext = GetDbContext();
            var _homeController = new HomeController(await _dbContext);
            //Act
            var result = _homeController.Create();
            //Assert
            result.Should().BeAssignableTo<IActionResult>();
        }

        [Fact]
        public async void HomeController_CreateWithParameter_ReturnsSuccess()
        {
            //Arrange
            var _dbContext = GetDbContext();
            var _homeController = new HomeController(await _dbContext);
            var fullUrl = "https://inforce.digital/about-us";
            //Act
            var result = await _homeController.Create(fullUrl);
            //Assert
            result.Should().BeAssignableTo<IActionResult>();
        }

        [Fact]
        public async void HomeController_Details_ReturnsSuccess()
        {
            //Arrange
            var _dbContext = GetDbContext();
            var _homeController = new HomeController(await _dbContext);
            int id = 1;
            //Act
            var result = await _homeController.Details(id);
            //Assert
            result.Should().NotBeOfType<NotFoundResult>();
            result.Should().BeAssignableTo<IActionResult>();
        }

        [Fact]
        public async void HomeController_Delete_ReturnsSuccess()
        {
            //Arrange
            var _dbContext = await GetDbContext();
            var _homeController = new HomeController(_dbContext);
            int id = 1;
            //Act
            var result = await _homeController.Delete(id);
            //Assert
            result.Should().NotBeOfType<NotFoundResult>();
            result.Should().BeAssignableTo<IActionResult>();
        }

        [Fact]
        public async void HomeController_DeleteConfirmed_ReturnsSuccess()
        {
            //Arrange
            var _dbContext = await GetDbContext();
            var _homeController = new HomeController(_dbContext);
            int id = 1;
            //Act
            var result = await _homeController.DeleteConfirmed(id);
            //Assert
            result.Should().NotBeOfType<NotFoundResult>();
            _dbContext.Urls.FirstOrDefault(u => u.Id == id).Should().BeNull();
        }

        [Fact]
        public async void HomeController_Details_IfIdIsNullReturnNotFound()
        {

            var _dbContext = GetDbContext();
            var _homeController = new HomeController(await _dbContext);
            int? id = null;

            var result = _homeController.Details(id).Result as NotFoundResult;

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void HomeController_Create_CorrectRedirecting()
        {
            var _dbContext = GetDbContext();
            var _homeController = new HomeController(await _dbContext);
            var fullUrl = "https://inforce.digital/services";

            var result = _homeController.Create(fullUrl).Result as RedirectToActionResult;

            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToActionResult>();
            Assert.Equal(nameof(_homeController.Index), result.ActionName);
        }

        [Fact]
        public async void HomeController_Delete_ReturnsNullIfIdIsNotExist()
        {

            var _dbContext = await GetDbContext();
            var _homeController = new HomeController(_dbContext);
            int id = 50;

            var result = _homeController.Delete(id).Result as BadRequestResult;

            result.Should().BeNull();
        }
    }
}
