using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Inventory.DDD.Application.Commands;
using Inventory.DDD.Application.Queries;
using Inventory.DDD.Domain;
using NUnit.Framework;
using static Inventory.DDD.Domain.Helpers.RowUpdate;

namespace Inventory.DDD.Integrations.Tests.Repositories
{
    public class ArticleModuleTests : TestBase
    {        
        [Test]
        public async Task GetArticles_ReturnsAllArticlesAsync()
        {
            var article1 = new Article
            {
                Name = "Article 1",
                Description = "Description 1",
                Type = "Type 1",
                Brand = "Brand 1",
                Price = 10,
                Stock = 10,
                ExpirationDate = DateTime.Now.AddDays(5),
                RowActive = true,
                RowUpdateCode = (int)UpdateType.Insert,
                RowUpdateDate = DateTime.Now,
                RowUpdateUser = User_Test
            };

            var article2 = new Article
            {
                Name = "Article 2",
                Description = "Description 2",
                Type = "Type 2",
                Brand = "Brand 2",
                Price = 20,
                Stock = 20,
                ExpirationDate = DateTime.Now.AddDays(5),
                RowActive = true,
                RowUpdateCode = (int)UpdateType.Insert,
                RowUpdateDate = DateTime.Now,
                RowUpdateUser = User_Test
            };

            await AddAsync(article1);
            await AddAsync(article2);

            // Arrange                
            var client = Application.CreateClient();

            // Act
            var articles = await client.GetFromJsonAsync<IEnumerable<GetArticle.GetArticlesResponse>>("/api/article/list");

            // Assert
            Assert.NotNull(articles);                
            
        } 

        [Test]
        public async Task CreateArticle_SavesCorrectData()
        {            
            // Arrange
            var article = new Article
            {
                Name = "Article to create",
                Description = "Description to create",
                Type = "Type to create",
                Brand = "Brand to create",
                Price = 10,
                Stock = 10,
                ExpirationDate = DateTime.Now.AddDays(5),
                RowActive = true,
                RowUpdateCode = (int)UpdateType.Insert,
                RowUpdateDate = DateTime.Now,
                RowUpdateUser = User_Test
            };

            // Act                                              
            var client = Application.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/article/add-article", new CreateArticle.CreateArticleCommand(article));

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Equals(HttpStatusCode.OK);
        }

        [Test]
        public async Task UpdateArticle()
        {
            // Arrenge
            var article1 = new Article
            {
                Name = "Article 1",
                Description = "Description 1",
                Type = "Type 1",
                Brand = "Brand 1",
                Price = 10,
                Stock = 10,
                ExpirationDate = DateTime.Now.AddDays(5),
                RowActive = true,
                RowUpdateCode = (int)UpdateType.Insert,
                RowUpdateDate = DateTime.Now,
                RowUpdateUser = User_Test
            };
            await AddAsync(article1);            

            var client = Application.CreateClient();

            var articleToUpdate = new Article
            {
                Name = "Article to update",
                Description = "Description to update",
                Type = "Type to update",
                Brand = "Brand to update",
                Price = 10,
                Stock = 10,
            };

            // Act
            var response = client.PutAsJsonAsync("api/article/edit-article", new UpdateArticle.UpdateArticleCommand(articleToUpdate)).GetAwaiter().GetResult();                        

            // Assert
            response.EnsureSuccessStatusCode();

            var updated = await FindAsync<Article>(article1.Id);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Equals(HttpStatusCode.OK);
        }

        [Test]
        public async Task DeleteArticle()
        {
            // Arrenge
            var articleToDelete = new Article
            {
                Name = "Article 1",
                Description = "Description 1",
                Type = "Type 1",
                Brand = "Brand 1",
                Price = 10,
                Stock = 10,
                ExpirationDate = DateTime.Now.AddDays(5),
                RowActive = true,
                RowUpdateCode = (int)UpdateType.Insert,
                RowUpdateDate = DateTime.Now,
                RowUpdateUser = User_Test
            };

            await AddAsync(articleToDelete);

            var client = Application.CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/article/remove-article-name/{articleToDelete.Name}");
                
            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Equals(HttpStatusCode.OK);            
        }

        [Test]
        [TestCase("Article xxx")]
        public async Task DeleteProduct_Should_Fail(string name)
        {
            // Arrenge                        
            var client = Application.CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/article/remove-article-name/{name}");

            // Assert
            response.StatusCode.Equals(HttpStatusCode.NotFound);
        }

    }
}
