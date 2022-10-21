using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Inventory.DDD.Application.Commands;
using Inventory.DDD.Domain;
using Inventory.DDD.Domain.Validators;
using NUnit.Framework;
using static Inventory.DDD.Domain.Helpers.RowUpdate;

namespace Inventory.DDD.Unit.Tests.Domain.Entities
{
    public class ArticleTests
    {
        private readonly IValidator<Article> _validator;

        public ArticleTests()
        {
            _validator = new ArticleValidator();
        }

        [Theory]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
        public void ArticleValidation_Name_KO(string name)
        {
            // Arrange
            Article article = new()
            {
                Id = 1,
                Name = name,
                Description = "Description 1",
                Brand = "Brand 1",
                Type = "Type 1",
                Price = 10,
                Stock= 20,
                ExpirationDate = DateTime.Now,
                RowActive = true,
                RowUpdateCode = (int)UpdateType.Insert,
                RowUpdateDate = DateTime.Now,
                RowUpdateUser = User_Test
            };

            // Act
            ValidationResult result = _validator.Validate(article);

            // Assert            
            Assert.False(result.IsValid);
        }

        //[Theory]
        //[Test]
        [TestCase("Article 1")]
        [TestCase("Article 2")]
        public void ArticleValidation_Name_OK(string name)
        {
            // Arrange
            Article article = new()
            {
                Id = 1,
                Name = name,
                Description = "Description 1",
                Brand = "Brand 1",
                Type = "Type 1",
                Price = 10,
                Stock = 20,
                ExpirationDate = DateTime.Now,
                RowActive = true,
                RowUpdateCode = (int)UpdateType.Insert,
                RowUpdateDate = DateTime.Now,
                RowUpdateUser = User_Test
            };

            // Act
            ValidationResult result = _validator.Validate(article);

            // Assert
            Assert.True(result.IsValid);

        }
    }
}
