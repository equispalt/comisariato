using SistemaILP.comisariato.Models;

namespace SistemaILP.comisariato.Servicios
{

    // IBreadcrumbService.cs
    public interface IBreadcrumbService
    {
        List<BreadcrumbItem> GetBreadcrumbItems(HttpContext httpContext);
    }

    // BreadcrumbService.cs
    public class BreadcrumbService : IBreadcrumbService
    {
        public List<BreadcrumbItem> GetBreadcrumbItems(HttpContext httpContext)
        {
            var items = new List<BreadcrumbItem>();
            var routeData = httpContext.Request.Path.Value?.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            // Agregar el elemento de inicio
            items.Add(new BreadcrumbItem("Home", "/"));

            // Obtener el valor del módulo desde los parámetros de la consulta
            var module = httpContext.Request.Query["module"].ToString();

            // Agregar el módulo al breadcrumb si existe
            if (!string.IsNullOrEmpty(module))
            {
                items.Add(new BreadcrumbItem(module, "#"));
            }

            // Agregar elementos adicionales basados en la ruta
            if (routeData != null)
            {
                for (int i = 0; i < routeData.Length; i++)
                {
                    var route = routeData[i];

                    // Comprobar si el controlador es 'Home' y si estamos en la última parte de la ruta
                    if (route.Equals("home", StringComparison.OrdinalIgnoreCase))
                    {
                        // Si estamos en la última acción del controlador Home, no lo agregamos
                        if (i == routeData.Length - 1)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    string title = route;
                    string url = "/" + string.Join("/", routeData.Take(i + 1)); // Construir la URL hasta el elemento actual

                    items.Add(new BreadcrumbItem(title, url));
                }
            }

            // Marcar el último elemento como activo
            if (items.Count > 0)
            {
                items.Last().IsActive = true;
            }

            return items;
        }


    }





}
