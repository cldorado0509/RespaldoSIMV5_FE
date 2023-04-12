using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.AtencionUsuarios.Models
{
    public class DatosValorTramite
    {
        public string TipoTramite { get; set; }
        public decimal DuracionVisita { get; set; }
        public decimal HorasInforme { get; set; }
        public decimal NumeroVisitas { get; set; }
        public int TramitesSINA { get; set; }
        public decimal NumeroProfesionales { get; set; }
        public string CM { get; set; }
        public string Observaciones { get; set; }
        public int Items { get; set; }
        public int Reliquidacion { get; set; }
        public string NIT { get; set; }
        public string Tercero { get; set; }
        public int ConSoportes { get; set; }
        public decimal CantNormas { get; set; }
        public decimal CantLineas { get; set; }
        public int Agno { get; set; }
        public decimal ValorProyecto { get; set; }
        public decimal ValorPublicacion { get; set; }
    }
}