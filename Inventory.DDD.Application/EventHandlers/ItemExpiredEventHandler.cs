using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.DDD.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.DDD.Application.EventHandlers
{
    /// <summary>
    /// Manejador del evento que notifica la expiración de un artículo.
    /// </summary>
    public class ItemExpiredEventHandler : INotificationHandler<ArticleExpiredEvent>
    {
        private readonly ILogger<ItemExpiredEventHandler> _logger;

        public ItemExpiredEventHandler(ILogger<ItemExpiredEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ArticleExpiredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("############## Inventory APIs Domain Event: {DomainEvent} #################", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
