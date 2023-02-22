
namespace SIM.Areas.Seguridad.Models
{
    public class PermisosRolModel
    {
        public int IdRol { get; set; }
        public bool CanRead { get; set; }
        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanPrint { get; set; }
    }
}