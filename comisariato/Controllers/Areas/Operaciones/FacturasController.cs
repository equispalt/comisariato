using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Rotativa.AspNetCore;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Operaciones;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace SistemaILP.comisariato.Controllers.Areas.Operaciones
{
    public class FacturasController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioFacturas _repositorioFacturas;
        private readonly IBreadcrumbService _breadcrumbService;
        private readonly IDatosDtoService _datosDtoService;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _contextAccessor;

        public FacturasController(IHttpContextAccessor contextAccessor, IConfiguration configuration, IPermisosService permisosService, IRepositorioFacturas repositorioFacturas, IBreadcrumbService breadcrumbService, IDatosDtoService datosDtoService)
        {
            _connectionString = configuration.GetConnectionString("ConnectionComisariato") ?? "";
            _permisosService = permisosService;
            _repositorioFacturas = repositorioFacturas;
            _breadcrumbService = breadcrumbService;
            _datosDtoService = datosDtoService;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index(int? numpag)
        {
            List<BreadcrumbItem> breadcrumbItems = _breadcrumbService.GetBreadcrumbItems(HttpContext);
            ViewBag.BreadcrumbItems = breadcrumbItems;

            bool esPermitido = await _permisosService.ValidaPermisoPrograma();
            if (esPermitido == false)
            {
                return RedirectToAction("Error403", "Home");
            }

            try
            {
                List<FacVentas> Listado = await _repositorioFacturas.ObtieneTodoFacturas();

                int cantidadregistros = 10;

                return View(Paginacion<FacVentas>.CrearPaginacion(Listado, numpag ?? 1, cantidadregistros));

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> VerFactura(int facturaId)
        {
            try
            {
                List<BreadcrumbItem> breadcrumbItems = _breadcrumbService.GetBreadcrumbItems(HttpContext);
                ViewBag.BreadcrumbItems = breadcrumbItems;

                var facturaDTO = await _repositorioFacturas.ObtieneFactura(facturaId);

                if (facturaDTO == null)
                {
                    return RedirectToAction("Error", "Home"); // Manejo de error si no se encuentra la factura
                }
                return View(facturaDTO);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        public async Task<IActionResult> ImprimirFactura(int facturaId)
        {
            try
            {
                var facturaDTO = await _repositorioFacturas.ObtieneFactura(facturaId);

                if (facturaDTO == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                // Generar PDF vista completa de la factura
                return new ViewAsPdf("ImprimirFactura", facturaDTO)
                {
                    FileName = $"Factura_{facturaDTO.Consecutivo}.pdf", // Nombre del archivo PDF
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter,    // Opcional: Tamaño de la página
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait, // Orientación
                    CustomSwitches = "--no-stop-slow-scripts", // Opcional: configuración adicional
                };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult CrearFactura()
        {
            List<BreadcrumbItem> breadcrumbItems = _breadcrumbService.GetBreadcrumbItems(HttpContext);
            ViewBag.BreadcrumbItems = breadcrumbItems;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GuardarFactura([FromBody] FacVentas body)
        {
            try
            {
                string serie = GenerarSerie();
                string consecutivo = GenerarConsecutivo();
                string usuario = UsuarioActivo();

                XElement factura = new XElement("FacVentas",
                    new XElement("Serie", serie),
                    new XElement("Consecutivo", consecutivo),
                    new XElement("NIT", body.NIT),
                    new XElement("Total", body.Total),
                    new XElement("usuario", usuario),
                    new XElement("TipoPagoId", body.TipoPagoId)
                );

                XElement oDetalleFactura = new XElement("FacVentasDet");
                foreach (FacVentasDet item in body.lstFacVentasDet)
                {
                    oDetalleFactura.Add(new XElement("Item",
                        new XElement("CodigoSILP", item.CodigoSILP),
                        new XElement("Cantidad", item.Cantidad),
                        new XElement("PrecioUnidad", item.PrecioUnidad),
                        new XElement("TotalLinea", item.TotalLinea)
                    ));
                }
                factura.Add(oDetalleFactura);

                int idFacturaGenerada;

                using var connection = new SqlConnection(_connectionString);

                connection.Open();
                SqlCommand cmd = new SqlCommand("[GenerarFactura]", connection);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@fac_xml", SqlDbType.Xml).Value = factura.ToString();

                SqlParameter outputIdParam = new SqlParameter("@idFactura_generado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                cmd.ExecuteNonQuery();

                idFacturaGenerada = (int)outputIdParam.Value;

                return Json(new { respuesta = true, idFactura = idFacturaGenerada });

            }
            catch (Exception ex)
            {
                // Log the exception details here if you have a logging system
                Console.WriteLine($"Error al guardar la factura: {ex.Message}");

                // Return a JSON response indicating failure
                return Json(new { respuesta = false, mensaje = "Ocurrió un error al guardar la factura. Intente nuevamente.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AnularFactura(int id)
        {
            //bool esPermitido = await _permisosService.ValidaPermisoPrograma();

            //if (esPermitido == false)
            //{
            //    return RedirectToAction("Error403", "Home");
            //}
            string usuario = UsuarioActivo();

            try
            {

                bool anulado = await _repositorioFacturas.PaAnularFactura(id, usuario);

                if (anulado)
                {
                    return RedirectToAction("Index", "Facturas");
                }

                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        // Metodos que permitiran consultar CLIENTES, PRODUCTOS Y EXISTENCIAS
        [HttpGet]
        public async Task<JsonResult> ObtieneEmpleadoPorNit(string nit)
        {
            var empleado = await _repositorioFacturas.PaObtenerEmpleadoPorNit(nit);

            if (empleado != null)
            {
                return Json(new
                {
                    existe = true,
                    nombreEmpleado = empleado.NombreEmpleado,
                    nit = empleado.NIT
                });
            }
            else
            {
                return Json(new { existe = false, mensaje = "Empleado no encontrado, por favor verifique" });
            }
        }

        [HttpGet]
        public async Task<JsonResult> ObtieneProductoPorCodigo(string codigo)
        {
            var producto = await _repositorioFacturas.PaObtenerProductoPorCodigo(codigo);

            if (producto != null)
            {
                return Json(new
                {
                    existe = true,
                    productoId = producto.ProductoId,
                    nombreProducto = producto.NombreProducto,
                    codigo = producto.CodigoSILP,
                    precio = producto.PrecioUnidad,
                    disponible = producto.Disponible
                });
            }
            else
            {
                return Json(new { existe = false, mensaje = "Producto NO encontrado, por favor verifique" });
            }
        }

        private string GenerarSerie()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Caracteres permitidos
            Random random = new Random();
            char[] serie = new char[8]; // Longitud de la serie

            for (int i = 0; i < serie.Length; i++)
            {
                serie[i] = caracteres[random.Next(caracteres.Length)]; // Selecciona un carácter aleatorio
            }

            return new string(serie); // Convierte el array de caracteres a string
        }


        // Método para generar un número aleatorio
        private string GenerarConsecutivo()
        {
            Random random = new Random();
            long numeroAleatorio = random.Next(1000000000, 2000000000); // Cambia el rango según tus necesidades
            return numeroAleatorio.ToString();
        }

        private string UsuarioActivo()
        {
            string currentUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            return currentUser;
        }


    }
}

