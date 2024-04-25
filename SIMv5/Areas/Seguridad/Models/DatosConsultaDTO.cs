using System.Collections.Generic;

namespace SIM.Areas.Seguridad.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DatosConsultaDTO
    {
        public int numRegistros;
        public IEnumerable<dynamic> datos;
    }
}