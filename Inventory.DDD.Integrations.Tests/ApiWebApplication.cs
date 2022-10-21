using System;
using System.Data.Common;
using Inventory.DDD.API;
using Inventory.DDD.Domain;
using Inventory.DDD.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using static Inventory.DDD.Domain.Helpers.RowUpdate;

namespace Inventory.DDD.Integrations.Tests
{
    public class ApiWebApplication : WebApplicationFactory<Api>
    {
        public const string TestConnectionString = "Server=.; Database=Inventory_TestDb; Trusted_Connection = True; MultipleActiveResultSets=false; Encrypt=False";                    

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(sp =>
                {
                    // Usamos una LocalDB para pruebas de integración
                    return new DbContextOptionsBuilder<DataContext>()                    
                    .UseSqlServer(TestConnectionString)
                    .UseApplicationServiceProvider(sp)
                    .Options;
                });
            });

            return base.CreateHost(builder);
        }
    }    
}