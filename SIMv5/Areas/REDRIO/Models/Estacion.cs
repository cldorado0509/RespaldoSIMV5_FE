using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoEstaciones
    {
        public List<Estacion> result { get; set; }
    }

    public class Estacion
    {
        public int idEstacion { get; set; }
        public string nombreEstacion { get; set; }
        public string codigo { get; set; }

        public int idMunicipio { get; set; }
        public int IdTipoFuente { get; set; }
        public DateTime? fecha_creacion { get; set; }
        public DateTime? fecha_actualizacion { get; set; }
        public Municipio municipio { get; set; }

        public TipoFuente tipoFuente { get; set; }

    }
    

}
