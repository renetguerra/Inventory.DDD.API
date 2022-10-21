using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DDD.Domain.Events
{
    /// <summary>
    /// Evento que notifica la actualización del precio de un artículo
    /// </summary>
    public class ArticleUpdatePriceEvent : DomainEvent
    {
        public ArticleUpdatePriceEvent(Article article, string msgNotification)
        {
            Article = article;
            MsgNotification = msgNotification;
        }

        public Article Article { get; set; }
    }
}
