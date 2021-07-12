using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaBackFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        [HttpGet("proveedores")]
        public IActionResult Proveedores()
        {
            var proveedores = _context.Proveedor.ToList();

            return Ok(proveedores);
        }

        [HttpGet("clientes")]
        public IActionResult Clientes()
        {
            var clientes = _context.Cliente.ToList();

            return Ok(clientes);
        }

        [HttpGet("stock/{almacen?}/{codigo?}")]
        public IActionResult Stock(int almacen, string codigo)
        {
            if (almacen != 0)
            {
                if (codigo != null)
                {
                    var multcodigoalm = _context.Multialmacen.Where(x => x.almacen == almacen && x.codigo == codigo).ToList();
                    return Ok(multcodigoalm);
                }
                else
                {
                    var multcodigoalm = _context.Multialmacen.Where(x => x.almacen == almacen).ToList();
                    return Ok(multcodigoalm);
                }
            }
            else
            {
                if (codigo != null)
                {
                    var multcodigoalm = _context.Multialmacen.Where(x => x.codigo == codigo).ToList();
                    return Ok(multcodigoalm);
                }
                var multialmacen = _context.Multialmacen.ToList();

                return Ok(multialmacen);
            }
        }


        [HttpPost("[action]"), ActionName("subecompra")]
        [Obsolete]
        public ActionResult SubeCompra(CompraMod compra)
        {

            string jsonString = JsonSerializer.Serialize(compra);

            System.Diagnostics.Debug.WriteLine("Json recibido en recibo:  " + jsonString);
            try
            {
                var prov = _context.Proveedor.Find(compra.proveedor);

                if (prov == null)
                {
                    System.Diagnostics.Debug.WriteLine("el proveedor no existe: " + compra.proveedor);
                    return NotFound("El proveedor no existe");
                }
                else
                {
                    Compra comprae = new Compra();

                    comprae.prov = compra.proveedor;
                    comprae.fecha = compra.fecha;
                    comprae.nit = compra.nit;
                    comprae.referencia = compra.referencia;
                    comprae.almacen = compra.almacen;
                    comprae.status = "O";
                    comprae.total = compra.total;

                    _context.Compra.Add(comprae);

                    _context.SaveChanges();

                    int part = 1;
                    foreach (var det in compra.detalle)
                    {

                        CompraDet comprad = new CompraDet();

                        comprad.codigo = det.codigo;
                        comprad.partida = part;
                        comprad.idc = comprae.id;
                        comprad.precio = det.precio;
                        comprad.cantidad = det.cantidad;
                        comprad.precio = det.precio;
                        comprad.almacen = compra.almacen;
                        comprad.totalpart = det.cantidad * det.precio;
                        _context.CompraDet.Add(comprad);

                        _context.SaveChanges();
                        part++;
                        //    System.Diagnostics.Debug.WriteLine("producto: " + det.codigo);

                    }

                    return CreatedAtAction(nameof(Compra), new { id = comprae.id });


                    // System.Diagnostics.Debug.WriteLine("nombre del proveedor " + prov.nombre);
                    // return Ok(prov);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ocurrio un error: " + ex.Message);
                return NoContent();
            }

            // System.Diagnostics.Debug.WriteLine("codigo del proveedor: " + compra.proveedor);
            // return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
        }

        [HttpPost("[action]"), ActionName("subeventa")]
        [Obsolete]
        public ActionResult SubeVenta(VentaMod venta)
        {

            string jsonString = JsonSerializer.Serialize(venta);

            System.Diagnostics.Debug.WriteLine("Json recibido en recibo:  " + jsonString);
            try
            {
                var clie = _context.Cliente.Find(venta.cliente);

                if (clie == null)
                {
                    System.Diagnostics.Debug.WriteLine("el cliente no existe: " + venta.cliente);
                    return NotFound("El cliente no existe");
                }
                else
                {
                    int error = 0;
                    foreach (var dv in venta.detalle)
                    {

                        var mlpd = _context.Multialmacen.FirstOrDefault(x => x.codigo == dv.codigo && x.almacen == venta.almacen);
                        if (mlpd != null)
                        {
                            double exist = mlpd.existencia;

                            if (dv.cantidad > exist)
                            {
                                System.Diagnostics.Debug.WriteLine("el producto :" + dv.codigo + " no tiene existencia (" + exist + ")");
                                error = 1;
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("el producto no existe");
                            error = 2;
                        }
                    }

                    if (error == 0)
                    {
                        Venta ventae = new Venta();

                        ventae.clie = venta.cliente;
                        ventae.fecha = venta.fecha;
                        ventae.nit = venta.nit;
                        ventae.almacen = venta.almacen;
                        ventae.status = "O";
                        ventae.referencia = venta.referencia;
                        ventae.total = venta.total;
                        _context.Venta.Add(ventae);

                        _context.SaveChanges();

                        int part = 1;
                        foreach (var det in venta.detalle)
                        {

                            VentaDet ventad = new VentaDet();

                            ventad.codigo = det.codigo;
                            ventad.partida = part;
                            ventad.idv = ventae.id;
                            ventad.precio = det.precio;
                            ventad.cantidad = det.cantidad;
                            ventad.precio = det.precio;
                            ventad.almacen = venta.almacen;
                            ventad.totalpart = det.cantidad * det.precio;
                            _context.VentaDet.Add(ventad);
                            _context.SaveChanges();
                            part++;
                            //    System.Diagnostics.Debug.WriteLine("producto: " + det.codigo);

                        }

                        return CreatedAtAction(nameof(Compra), new { id = ventae.id });
                    }
                    else if (error == 1)
                    {
                        return BadRequest("Un producto no tiene existencia suficiente");
                    }
                    else
                    {
                        return BadRequest("Un producto no existe");
                    }

                    // System.Diagnostics.Debug.WriteLine("nombre del proveedor " + prov.nombre);
                    // return Ok(prov);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ocurrio un error: " + ex.Message);
                return NoContent();
            }

            // System.Diagnostics.Debug.WriteLine("codigo del proveedor: " + compra.proveedor);
            // return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
        }
    }
}
