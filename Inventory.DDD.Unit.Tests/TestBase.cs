using Inventory.DDD.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Inventory.DDD.Unit.Tests
{
    public class TestBase
    {
        public DataContext Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Context = DbContextFactory.Create();
            Context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}