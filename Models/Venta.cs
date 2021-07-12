using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Models
{
    [Table("ventas")]
    public class Venta
    {
        [Key]
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int clie { get; set; }
        public int almacen { get; set; }
        public string nit { get; set; }
        public string status { get; set; }
        public string referencia { get; set; }
        public double total { get; set; }
        public double impuesto { get; set; }
        public double descuento { get; set; }
    }
}
