using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.DDD.Domain.DTOs
{
    /// <summary>
    /// Clase que define la transferencia de datos de un artículo (DTO = Data Transfer Object)
    /// </summary>
    public class ArticleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
}
