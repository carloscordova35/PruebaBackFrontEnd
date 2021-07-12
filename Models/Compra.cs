using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Models
{
    [Table("compras")]
    public class Compra
    {
        [Key]
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int prov { get; set; }
        public int almacen { get; set; }
        public string nit { get; set; }
        public string status { get; set; }
        public string referencia { get; set; }
        public double total { get; set; }
        public double impuesto { get; set; }
        public double descuento { get; set; }
    }
}
