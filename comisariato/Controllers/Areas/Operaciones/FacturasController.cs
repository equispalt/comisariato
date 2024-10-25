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

        public FacturasController(IPermisosService permisosService, IRepositorioFacturas repositorioFacturas)
        {
            _permisosService = permisosService;
            _repositorioFacturas = repositorioFacturas;
        }

        public async Task<IActionResult> Index(int? numpag)
        {
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
                //bool esPermitido = await _permisosService.ValidaPermisoPrograma();
                //if (esPermitido == false)
                //{
                //    return RedirectToAction("Error403", "Home");
                //}

                var oEncFac = await _repositorioFacturas.ObtieneEncFactura(facturaId);
                var oDetFac = await _repositorioFacturas.ObtieneDetFactura(facturaId);

                // Pasar datos a la vista
                ViewBag.VoFacEnc = oEncFac;
                ViewBag.VoFacDet = oDetFac;

                return View();

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
                // Obtener el encabezado y los detalles de la factura
                var oEncFac = await _repositorioFacturas.ObtieneEncFactura(facturaId);
                var oDetFac = await _repositorioFacturas.ObtieneDetFactura(facturaId);

                if (oEncFac == null || oDetFac == null)
                {
                    return RedirectToAction("Error", "Home"); // Manejo de error si no se encuentran datos
                }

                // Pasar datos a la vista
                ViewBag.VoFacEnc = oEncFac;
                ViewBag.VoFacDet = oDetFac;

                // Generar PDF usando la vista parcial o la vista completa de la factura
                return new ViewAsPdf("ImprimirFactura")
                {
                    FileName = $"Factura_{oEncFac.FacVentaId}.pdf", // Nombre del archivo PDF
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter,    // Opcional: Tamaño de la página
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait, // Orientación
                    CustomSwitches = "--no-stop-slow-scripts" // Opcional: configuración adicional
                };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }



    }
}
