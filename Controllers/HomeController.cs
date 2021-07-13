using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PruebaBackFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PruebaBackFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AppDBContext _context;

        public HomeController(ILogger<HomeController> logger, AppDBContext master)
        {
            _logger = logger;

            _context = master;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Clientes()
        {
            var clientes = _context.Cliente.ToList();
            return View(clientes);
        }

        public IActionResult Proveedores()
        {
            var prove = _context.Proveedor.ToList();
            return View(prove);
        }

        public async Task<IActionResult> Productos(string sortOrder, string searchString)
        {
            ViewData["ordenCodigo"] = String.IsNullOrEmpty(sortOrder) ? "cod_desc" : "";
            ViewData["ordenDescr"] = String.IsNullOrEmpty(sortOrder) ? "desc_desc" : "";
            ViewData["ordenCatg"] = String.IsNullOrEmpty(sortOrder) ? "cat_desc" : "";//sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            List<Producto> prods = new List<Producto>();
            try
            {
                using (var httpClient = new HttpClient())
                {

                   // System.Diagnostics.Debug.WriteLine(appParam.apiBaseUrl + "/productos");
                    using (var response = await httpClient.GetAsync(appParam.apiBaseUrl + "/productos"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        //  System.Diagnostics.Debug.WriteLine("respuesta " + apiResponse);

                       prods = JsonConvert.DeserializeObject<List<Producto>>(apiResponse);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Ocurrio un error al llegar a la api: "+e.Message);
            }

            //   var students = from s in _context.Producto
            //                select s;

           var products = from s in prods
            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.descripcion.Contains(searchString)
                                       || s.codigo.Contains(searchString)
                                       || s.categoria.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "desc_desc":
                    products = products.OrderBy(s => s.descripcion);
                    break;
                case "cat_desc":
                    products = products.OrderBy(s => s.categoria);
                    break;
                case "cod_desc":
                    products = products.OrderBy(s => s.codigo);
                    break;
                default:
                    products = products.OrderBy(s => s.codigo);
                    break;
            }
            return View(products.ToList());
        }

        public IActionResult CrearMov() {

            return View();
        }

        public IActionResult Mov(string tipomov) {

            System.Diagnostics.Debug.WriteLine("el tipo fue: " + tipomov);
            if (tipomov != null)
            {
                if (tipomov.Equals("V"))
                {

                    ViewData["lista"] = _context.Cliente.ToList();
                }
                else
                {
                    ViewData["lista"] = _context.Proveedor.ToList();
                }
            }
            else {
                tipomov = "C";
               
                ViewData["lista"] = _context.Proveedor.ToList();
            }
            ViewData["almacenes"] = _context.Almacen.ToList();
            ViewData["productos"] = _context.Producto.ToList();
            ViewData["tipom"] = tipomov;
            return View();
        }

        public async Task<IActionResult> AgregaMov(Movimiento movn) {

            System.Diagnostics.Debug.WriteLine("tipo: " + movn.tipomov);

            IList<Movimiento.DetalleM> _TableForm = new List<Movimiento.DetalleM>();

            string apiurl = "";

            VentaMod venta = new VentaMod();
            CompraMod compra = new CompraMod();

            if (movn.tipomov.Equals("V"))
            {
                apiurl = appParam.apiBaseUrl + "/subeventa";
                venta.cliente = movn.clieprov;
                venta.almacen = movn.almacen;
                venta.fecha = movn.fecha;
                venta.nit = movn.nit;
                venta.total = movn.total;
                venta.referencia = movn.referencia;
                                       
            }
            else if (movn.tipomov.Equals("C"))
            {
                compra.proveedor = movn.clieprov;
                compra.almacen = movn.almacen;
                compra.fecha = movn.fecha;
                compra.nit = movn.nit;
                compra.total = movn.total;
                compra.referencia = movn.referencia;
                apiurl = appParam.apiBaseUrl + "/subecompra";
            }
           
            //Loop through the forms
            for (int i = 0; i <= Request.Form.Count; i++)
            {
                string codigo = "";
                double cantidad = 0.0;
                double precio = 0.0;
                var ClientSampleID = Request.Form["codigo[" + i + "]"];
                var additionalComments = Request.Form["cantidad[" + i + "]"];
                var acidStables = Request.Form["precio[" + i + "]"];
                if (!String.IsNullOrEmpty(ClientSampleID))
                   codigo = ClientSampleID;
                System.Diagnostics.Debug.WriteLine("columna a " + ClientSampleID);
                if (!String.IsNullOrEmpty(additionalComments))
                    System.Diagnostics.Debug.WriteLine("columna b" + additionalComments);
                cantidad = Convert.ToDouble(additionalComments);
                // dtv.cantidad = Double.Parse(additionalComments);
                //dt.cantidad = 0.0;
              
                if (!String.IsNullOrEmpty(acidStables))
                    System.Diagnostics.Debug.WriteLine("columna c " + acidStables);
                // dt.precio = Double.Parse(acidStables);
                // dtv.precio = Double.Parse(acidStables);
                precio = Convert.ToDouble(acidStables);
               

                if (codigo != null && cantidad != 0 && precio != 0)
                {

                    if (movn.tipomov.Equals("C"))
                    {
                        Detalle dt = new Detalle();
                        dt.codigo=codigo;
                        dt.cantidad = cantidad;
                        dt.precio = precio;
                        compra.detalle.Add(dt);
                    }
                    else {
                        Detallev dtv = new Detallev();
                        dtv.codigo = codigo;
                        dtv.cantidad = cantidad;
                        dtv.precio = precio;
                        venta.detalle.Add(dtv);
                    }
                }
                if (!String.IsNullOrEmpty(ClientSampleID))
                {
                 //   _TableForm.Add(new Movimiento.DetalleM { ClientSampleID = ClientSampleID, AcidStables = acidStables, AdditionalComments = additionalComments });
                }
            }

            int codigorespuesta = 0;
            string respuesta = "";


            try
            {

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, apiurl))
                {
                    var jsonv = JsonConvert.SerializeObject(venta);
                    var jsonc = JsonConvert.SerializeObject(compra);

                    string json = "";

                    if (movn.tipomov.Equals("C"))
                    {

                        json = jsonc;
                    }
                    else
                    {
                        json = jsonv;
                    }

                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                                          .ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();

                            System.Diagnostics.Debug.WriteLine("respuesta " + response.Content.ReadAsStringAsync());
                        }

                        
                    }
                }
            }
            catch (Exception e)
            {
                ViewData["message"] = "error";
                System.Diagnostics.Debug.WriteLine("Ocurrio un error al llegar a la api: " + e.Message);
            }

            return Redirect("/Home/Mov");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
