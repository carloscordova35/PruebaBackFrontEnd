using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Models
{
    [Table("productos")]
    public partial class Producto
    {
        [Key]
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string linea { get; set; }
        public string codigobarra { get; set; }
        public double precio { get; set; }
        public int status { get; set; }
        public double existenciag{ get; set; }
        public string categoria { get; set; }
        public DateTime? fechaultcomp { get; set; }
        public string marca { get; set; }
        public double ultcomp { get; set; }
    }
}
