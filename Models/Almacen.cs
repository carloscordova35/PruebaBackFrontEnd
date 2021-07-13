using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Models
{
    [Table("almacenes")]
    public class Almacen
    {
        [Key]
        public int almacen { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string encargado { get; set; }
        public int status { get; set; }
    }
}
