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

           var students = from s in prods
            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.descripcion.Contains(searchString)
                                       || s.codigo.Contains(searchString)
                                       || s.categoria.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "desc_desc":
                    students = students.OrderBy(s => s.descripcion);
                    break;
                case "cat_desc":
                    students = students.OrderBy(s => s.categoria);
                    break;
                case "cod_desc":
                    students = students.OrderBy(s => s.codigo);
                    break;
                default:
                    students = students.OrderBy(s => s.codigo);
                    break;
            }
            return View(students.ToList());
        }

        public IActionResult CrearMov() {

            return View();
        }

        public IActionResult Mov() {
            return View();
        }

        public IActionResult AgregaMov(Movimiento movn) {

            System.Diagnostics.Debug.WriteLine("tipo: " + movn.tipomov);

            IList<Movimiento.DetalleM> _TableForm = new List<Movimiento.DetalleM>();

            //Loop through the forms
            for (int i = 0; i <= Request.Form.Count; i++)
            {
                var ClientSampleID = Request.Form["ClientSampleID[" + i + "]"];
                var additionalComments = Request.Form["AdditionalComments[" + i + "]"];
                var acidStables = Request.Form["AcidStables[" + i + "]"];

                System.Diagnostics.Debug.WriteLine("columna traida " + ClientSampleID);
                System.Diagnostics.Debug.WriteLine("celda: "+i);

                if (!String.IsNullOrEmpty(ClientSampleID))
                {
                 //   _TableForm.Add(new Movimiento.DetalleM { ClientSampleID = ClientSampleID, AcidStables = acidStables, AdditionalComments = additionalComments });
                }
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
