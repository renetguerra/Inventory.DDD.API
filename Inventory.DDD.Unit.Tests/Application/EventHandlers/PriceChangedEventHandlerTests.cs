using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.DDD.Application.EventHandlers;
using Inventory.DDD.Domain;
using Inventory.DDD.Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Inventory.DDD.Unit.Tests.Application.EventHandlers
{
    public class PriceChangedEventHandlerTests
    {
        [Test]
        public void ItemRemovedEvent_LoggerCalled()
        {
            // Arrenge
            var domainEvent = new ArticleDeleteEvent(It.IsAny<Article>(), "Artículo borrado");
            var handler = new ItemRemovedEventHandler(Mock.Of<ILogger<ItemRemovedEventHandler>>());

            // Act
            handler.Handle(domainEvent, CancellationToken.None);

            // Assert            
            // TODO: Do something first in the event
        }

        [Test]
        public void ItemExpiredEvent_LoggerCalled()
        {
            // Arrenge
            var domainEvent = new ArticleExpiredEvent(It.IsAny<Article>(), "El artículo ha expirado.");
            var handler = new ItemExpiredEventHandler(Mock.Of<ILogger<ItemExpiredEventHandler>>());

            // Act
            handler.Handle(domainEvent, CancellationToken.None);

            // Assert            
            // TODO: Do something first in the event
        }

        [Test]
        public void PriceChangedEvent_LoggerCalled()
        {
            // Arrenge
            var domainEvent = new ArticleUpdatePriceEvent(It.IsAny<Article>(), "El precio del artículo ha cambiado");
            var handler = new PriceChangedEventHandler(Mock.Of<ILogger<PriceChangedEventHandler>>());

            // Act
            handler.Handle(domainEvent, CancellationToken.None);

            // Assert            
            // TODO: Do something first in the event
        }
    }
}
