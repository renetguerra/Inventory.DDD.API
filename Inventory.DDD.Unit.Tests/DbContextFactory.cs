using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.DDD.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Inventory.DDD.Unit.Tests
{
    public class DbContextFactory
    {
        public static DataContext Create()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                //.UseInMemoryDatabase(nameof(DataContext))
                .UseSqlServer(nameof(DataContext))  
                .Options;


            return new DataContext(options, Mock.Of<IMediator>(), Mock.Of<ILogger<DataContext>>());
        }
    }
}
