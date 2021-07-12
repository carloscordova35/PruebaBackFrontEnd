using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Models
{
    public partial class VentaMod
    {

        public VentaMod()
        {
            detalle = new List<Detallev>();
        }
        public int id { get; set; }
        public int cliente { get; set; }
        public DateTime fecha { get; set; }
        public string nit { get; set; }
        public int almacen { get; set; }
        public double total { get; set; }
        public string referencia { get; set; }
        public List<Detallev> detalle { get; set; }

    }

    public class Detallev
    {
        public string codigo { get; set; }
        public double cantidad { get; set; }
        public double precio { get; set; }
        public double descuento { get; set; }

    }
}
