using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.DDD.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventory.DDD.Infrastructure
{
    public class DataContext : DbContext
    {
        private readonly IPublisher _publisher;
        private readonly ILogger<DataContext> _logger;

        public DataContext(DbContextOptions<DataContext> options,
                IPublisher publisher,
                ILogger<DataContext> logger) : base(options)
        {
            _publisher = publisher;
            _logger = logger;
        }
        
        public DbSet<Article> Articles => Set<Article>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            var events = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .ToArray();

            foreach (var @event in events)
            {
                @event.IsPublished = true;

                _logger.LogInformation("New domain event {Event}", @event.GetType().Name);

                await _publisher.Publish(@event);
            }

            return result;
        }        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Article>()
                .Ignore(x => x.DomainEvents);
        }

    }
}
