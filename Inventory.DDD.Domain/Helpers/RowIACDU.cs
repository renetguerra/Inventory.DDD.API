using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Inventory.DDD.Domain.Helpers
{
    /// <summary>
    /// Clase genérica que define campos como el Id de las entidades que heredan
    /// de la misma para lograr un mejor control y seguimiento de trazas de los datos en la bbdd.
    /// </summary>
    public partial class RowIACDU
    {
        public int Id { get; set; }

        public bool RowActive { get; set; }

        public int RowUpdateCode { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime RowUpdateDate { get; set; }

        public int RowUpdateUser { get; set; }

    }
}
