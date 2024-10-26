using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using SistemaILP.comisariato.Data;
using SistemaILP.comisariato.Models;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Operaciones;

namespace SistemaILP.comisariato.Controllers.Areas.Operaciones
{
    public class FacturasController : Controller
    {
        private readonly IPermisosService _permisosService;
        private readonly IRepositorioFacturas _repositorioFacturas;
        private readonly IBreadcrumbService _breadcrumbService;

        public FacturasController(IPermisosService permisosService, IRepositorioFacturas repositorioFacturas, IBreadcrumbService breadcrumbService)
        {
            _permisosService = permisosService;
            _repositorioFacturas = repositorioFacturas;
            _breadcrumbService = breadcrumbService;
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
                //bool esPermitido = await _permisosService.ValidaPermisoPrograma();
                //if (esPermitido == false)
                //{
                //    return RedirectToAction("Error403", "Home");
                //}

                // Obtener el encabezado y los detalles de la factura
                var facturaDTO = await _repositorioFacturas.ObtieneFactura(facturaId);

                if (facturaDTO == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                // Generar PDF vista completa de la factura
                return new ViewAsPdf("ImprimirFactura", facturaDTO)
                {
                    FileName = $"Factura_{facturaDTO.FacVentaId}.pdf", // Nombre del archivo PDF
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

    }
}
