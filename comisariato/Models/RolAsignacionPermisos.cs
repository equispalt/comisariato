namespace comisariato.Models
{
    public class RolAsignacionPermisos
    {
        public int RolAsigId { get; set; }
        public int RoleId { get; set; }
        public string FormId { get; set; }
        public bool EsPermitido { get; set; }
    }
}
