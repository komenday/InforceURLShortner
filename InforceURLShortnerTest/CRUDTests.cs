using FluentAssertions;
using InforceURLShortner.Models;
using Microsoft.EntityFrameworkCore;

namespace InforceURLShortnerTest
{
    public class CRUDTests
    {
        private async Task<InforceShortnerContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<InforceShortnerContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            var databaseContext = new InforceShortnerContext(options);
            databaseContext.Database.EnsureCreated();

            databaseContext.Urls.Add(
                    new Url()
                    {
                        Id = 1,
                        FullURL = "https://getbootstrap.com/docs/5.2/content/typography/",
                        ShortURL = "8Ktn0",
                        CreationDate = DateTime.Now,
                        AuthorLogin = "admin",
                    }
                 );
            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }

        [Fact]
        public async void DataBase_Add_ReturnsBool()
        {
            //Arrange
            var _context = await GetDbContext();
            var url = new Url()
            {
                Id = 2,
                FullURL = "https://dou.ua/forums/topic/40645/?from=fortech",
                ShortURL = "b7s7MS",
                CreationDate = DateTime.Now,
                AuthorLogin = "admin",
            };

            //Act
            _context.Urls.Add(url);
            var result = _context.SaveChanges();
            //Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void DataBase_Read_ReturnsBool()
        {
            //Arrange
            var _context = await GetDbContext();
            int id = 1;
            //Act
            var result = from u in _context.Urls select u;
            //Assert
            result.Should().Contain(x => x.Id == id);
        }

        [Fact]
        public async void DataBase_Update_ReturnsBool()
        {
            //Arrange
            var _context = await GetDbContext();
            var url = _context.Urls.FirstOrDefault(x => x.Id == 1);
            //Act
            _context.Urls.Update(url);
            var result = await _context.SaveChangesAsync();
            //Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void DataBase_Delete_ReturnsBool()
        {
            //Arrange
            var _context = await GetDbContext();
            int id = 1;
            var url = _context.Urls.Find(id);
            //Act
            _context.Urls.Remove(url);
            var result = await _context.SaveChangesAsync();
            //Assert
            result.Should().BeGreaterThan(0);
        }
    }
}