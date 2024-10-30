using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoCampañas
    {
        public List<Campaña> result { get; set; }
    }

    public class Campaña
    {
        public int idCampaña { get; set; }
        public string nombreCampaña { get; set; } = string.Empty;
        public string descripcion { get; set; }
        public DateTime fecha_inicial { get; set; }
        public DateTime fecha_final { get; set; }
        public int idFase { get; set; }
        public DateTime? fecha_creacion { get; set; }
        public DateTime? fecha_actualizacion { get; set; }
        public Fase fase { get; set; }
    }

    public class Fase
    {
        public int idFase { get; set; }
        public string nombreFase { get; set; }
        public int año { get; set; }
        public int idTipoFase { get; set; }
        public object tipoFase { get; set; }
        public DateTime? fecha_creacion { get; set; }
        public DateTime? fecha_actualizacion { get; set; }
    }
}
