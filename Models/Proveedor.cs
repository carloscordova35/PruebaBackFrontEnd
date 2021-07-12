using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Models
{
    [Table("proveedores")]
    public class Proveedor
    {
        [Key]
        [DisplayName("ID")]
        public int id { get; set; }
        [DisplayName("NIT")]
        public string nit { get; set; }
        [DisplayName("Nombre")]
        public string nombre { get; set; }
        [DisplayName("Direccion")]
        public string direccion { get; set; }
        [DisplayName("Telefono")]
        public string telefono { get; set; }
        [DisplayName("Saldo")]
        public double saldo { get; set; }
        [DisplayName("Status")]
        public string status { get; set; }
    }
}
