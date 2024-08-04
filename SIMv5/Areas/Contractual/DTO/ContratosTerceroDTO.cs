using System;

namespace SIM.Areas.Contractual.DTO
{
    public class ContratosTerceroDTO
    {
        public int ID_CONTRATO { get; set; }
        public string TIPOCONTRATO { get; set; } = string.Empty;
        public DateTime FECHA { get; set; }
        public int ANIO { get; set; }
        public int NUMERO { get; set; }
        public int VALOR { get; set; }
        public string PLAZO { get; set; }
    }
}