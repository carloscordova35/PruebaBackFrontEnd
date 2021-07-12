using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Models
{
    [Table("comprasdet")]
    public class CompraDet
    {
        [Key]
        public int id { get; set; }
        public int idc { get; set; }
        public int partida { get; set; }
        public string codigo { get; set; }
        public double cantidad { get; set; }
        public double precio { get; set; }
        public double costo { get; set; }
        public double impuesto { get; set; }
        public double totalpart { get; set; }
        public int almacen { get; set; }

    }
}
