using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.ExpedienteAmbiental.Models
{
    public class TramitesExpedienteDTO
    {
        public decimal CODTRAMITE { get; set; }

        public decimal CODIGO_PROYECTO { get; set; }

        public string MENSAJE { get; set; }

        public string COMENTARIOS { get; set; }

        public string PROYECTO { get; set; }

        public DateTime? FECHAINI { get; set; }

        public DateTime? FECHAFIN { get; set; }

        public string ESTADO { get; set; }  
    }
}