using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Models
{
    public partial class Movimiento
    {
        [DisplayName("Tipo")]
        public string tipomov { get; set; }
        [DisplayName("Fecha")]
        public DateTime fecha { get; set; }
        [DisplayName("Proveedor o Cliente")]
        public int clieprov { get; set; }
        [DisplayName("Almacen")]
        public int almacen { get; set; }
        public string nit { get; set; }
        public string status { get; set; }
        public string referencia { get; set; }
        public double total { get; set; }
        public double impuesto { get; set; }
        public double descuento { get; set; }
        public List<DetalleM> detalle { get; set; }

        public Movimiento() {

            detalle = new List<DetalleM>();
        }

        public class DetalleM
        {
            public string codigo { get; set; }
            public double cantidad { get; set; }
            public double precio { get; set; }
            public double descuento { get; set; }
        }
    }
}
