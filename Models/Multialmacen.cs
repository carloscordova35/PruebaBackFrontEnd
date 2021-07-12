using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Models
{
    [Table("multialmacenes")]
    public class Multialmacen
    {
        [Key]
        public string codigo { get; set; }
        [ForeignKey("almacen")]
        public int almacen { get; set; }
        public double existencia { get; set; }
        public double stockmin { get; set; }
        public double stockmax { get; set; }
        public int status { get; set; }
           
    }
}
