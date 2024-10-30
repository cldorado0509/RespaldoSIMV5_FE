using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoMunicipios
    {
        public List<Municipio> result { get; set; }
    }

    public class Municipio
    {
        public int idMunicipio { get; set; }
        public string nombre { get; set; }
        public int id_Departamento { get; set; }
        public Departamento departamento { get; set; }
    }

    public class Departamento
    {
        public int idDepartamento { get; set; }
        public string nombreDepartamento { get; set; }
        public DateTime? fecha_creacion { get; set; }
        public DateTime? fecha_actualizacion { get; set; }
    }
}
