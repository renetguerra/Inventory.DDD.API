using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DDD.Domain.Events
{
    /// <summary>
    /// Evento que notifica la expiración
    /// </summary>
    public class ArticleExpiredEvent : DomainEvent
    {
        public ArticleExpiredEvent(Article article, string msgNotification)
        {
            Article = article;
            MsgNotification = msgNotification;
        }

        public Article Article { get; set; }
    }
}
