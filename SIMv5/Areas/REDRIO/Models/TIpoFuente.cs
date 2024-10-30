using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoTipoFuente
    {
        public List<TipoFuente> result { get; set; }
    }

    public class TipoFuente
    {
        public string NombreTipoFuente { get; set; }

        public DateTime? Fecha_creacion { get;  set; }

        public DateTime? Fecha_actualizacion { get; internal set; }
    }

   public class ResponseTipoFuente
{
    public TipoFuente result { get; set; }
    public bool success { get; set; }
}   
}
