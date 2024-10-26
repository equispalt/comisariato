namespace SistemaILP.comisariato.Models
{
    public class BreadcrumbItem
    {
        public string Title { get; set; } // Título que se mostrará
        public string Url { get; set; } // URL a la que se vincula
        public bool IsActive { get; set; } // Indica si este elemento es el activo

        public BreadcrumbItem(string title, string url)
        {
            Title = title;
            Url = url;
            IsActive = false; // Por defecto, no es activo
        }
    }
}
