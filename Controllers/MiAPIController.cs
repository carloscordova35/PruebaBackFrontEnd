using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaBackFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiAPIController : ControllerBase
    {

        public AppDBContext _context;

        public MiAPIController(AppDBContext master)
        {
            this._context = master;
        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Todo funciona en la api");
        }

        [HttpGet("productos")]
        public IActionResult Productos()
        {
            var productos = _context.Producto.ToList();

            return Ok(productos);
        }

    }
}
