using System;
using System.Collections.Generic;
using System.Text;
using Inventory.DDD.Domain.Helpers;

namespace Inventory.DDD.Domain
{
    public class Article : RowIACDU, IHasDomainEvent
    {        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime ExpirationDate { get; set; }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();        
    }
}
