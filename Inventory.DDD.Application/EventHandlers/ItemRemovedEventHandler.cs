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
    /// Manejador del evento que notifica la eliminación de un artículo.
    /// </summary>
    public class ItemRemovedEventHandler : INotificationHandler<ArticleDeleteEvent>
    {
        private readonly ILogger<ItemRemovedEventHandler> _logger;

        public ItemRemovedEventHandler(ILogger<ItemRemovedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ArticleDeleteEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("############## Inventory APIs Domain Event: {DomainEvent} #################", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
