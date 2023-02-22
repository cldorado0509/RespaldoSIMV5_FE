namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    
    public partial class VW_PMES_EVALUACION
    {
        public Nullable<decimal> ID_TERCERO { get; set; }
        public Nullable<decimal> ID_INSTALACION { get; set; }
        public Nullable<int> ID_EVALUACION_ENCUESTA { get; set; }
        public decimal ID_ESTADO { get; set; }
        public string TERCERO { get; set; }
        public string INSTALACION { get; set; }
        public string ENCUESTA { get; set; }
        public decimal ID_FUNCIONARIO { get; set; }
        public Nullable<decimal> ID_USUARIO { get; set; }
    }
}
