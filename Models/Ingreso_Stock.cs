using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoerEsteticaBackend.Models
{
    public class Ingreso_Stock
    {
        [Key]
        public int IdIngreso { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public List<Detalle_Ingreso> Detalles { get; set; } = new List<Detalle_Ingreso>();
    }
}