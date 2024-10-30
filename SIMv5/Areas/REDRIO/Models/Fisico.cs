using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SIM.Areas.REDRIO.Models
{
    public class ResultadoFisico
    {
        public List<Fisico> result { get; set; }
    }

    public class Fisico
    {
        public int idFisico { get; set; }
        public decimal? Caudal { get; set; }

        public decimal? ClasificacionCaudal { get; set; }

        public int? NumeroDeVerticales { get; set; }

        public decimal? ColorVerdaderoUPC { get; set; }

        public decimal? ColorTriestimular436nm { get; set; }

        public decimal? ColorTriestimular525nm { get; set; }

        public decimal? ColorTriestimular620nm { get; set; }

        public decimal? SolidosSuspendidosTotales { get; set; }

        public decimal? SolidosTotales { get; set; }

        public decimal? SolidosVolatilesTotales { get; set; }

        public decimal? SolidosDisueltosTotales { get; set; }

        public decimal? SolidosFijosTotales { get; set; }

        public decimal? SolidosSedimentables { get; set; }

        public DateTime? Fecha_creacion { get;  set; }

        public DateTime? Fecha_actualizacion { get;  set; }
        public DateTime? Fecha_Muestra { get;  set; }

    }

    public class ResponseFisico
{
    public Fisico result { get; set; }
    public bool success { get; set; }
}
}
