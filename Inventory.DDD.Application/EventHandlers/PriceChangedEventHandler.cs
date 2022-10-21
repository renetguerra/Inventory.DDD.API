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
    /// Manejador del evento que notifica la modificación del precio de un artículo.
    /// </summary>
    public class PriceChangedEventHandler : INotificationHandler<ArticleUpdatePriceEvent>
    {
        private readonly ILogger<PriceChangedEventHandler> _logger;

        public PriceChangedEventHandler(ILogger<PriceChangedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ArticleUpdatePriceEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("############## Inventory APIs Domain Event: {DomainEvent} #################", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
