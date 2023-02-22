using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIM.Areas.Facturacion.Models
{
    public class CalculoSeguimiento
    {
        public string Tramite { get; set; }
        public int IdTramite { get; set; }
        public string CM { get; set; }
        public string NIT { get; set; }
        public string Tercero { get; set; }
        public int Items { get; set; }
        public int Tecnicos { get; set; }
        public int Visitas { get; set; }
        public decimal HorasInforme { get; set; }
        public decimal DuracionVisita { get; set; }
        public double Salarios { get; set; }
        public double Transporte { get; set; }
        public double Laboratorio { get; set; }
        public double Administracion { get; set; }
        public double TotalNeto { get; set; }
        public double TopeDeterminado { get; set; }
        public string TotalPagar { get; set; }
        public double TotalPublicacion { get; set; }
        public string Observaciones { get; set; }
        public decimal CantNormas { get; set; }
        public decimal CantLineas { get; set; }
    }
}