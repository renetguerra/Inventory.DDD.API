using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DDD.Domain.Helpers
{
    /// <summary>
    /// Clase de parámetros de búsqueda de la entidad Article, con su paginación a través de herencia (PaginationParams)
    /// </summary>
    public class ArticleParams : PaginationParams
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Type { get; set; } = "";        
        public decimal Price { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.Now;
    }
}
