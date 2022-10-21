using System.Threading.Tasks;
using Inventory.DDD.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;

namespace Inventory.DDD.Integrations.Tests
{
    public class TestBase
    {
        protected ApiWebApplication Application;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Application = new ApiWebApplication();

            using var scope = Application.Services.CreateScope();

            EnsureDatabase(scope);            
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            Application.Dispose();
        }

        [SetUp]
        public async Task Setup()
        {
            await ResetState();
        }

        [TearDown]
        public void Down()
        {
            
        }

        protected async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = Application.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            context.Add(entity);

            await context.SaveChangesAsync();

            return entity;
        }

        protected async Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
        {
            using var scope = Application.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }

        private static void EnsureDatabase(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            context.Database.Migrate();
        }

        private async Task ResetState()
        {
            var checkpoint = new Checkpoint
            {
                TablesToIgnore = new Table[]
                {
                    new Table("__EFMigrationsHistory")
                }
            };

            await checkpoint.Reset(ApiWebApplication.TestConnectionString);
        }
    }
}
