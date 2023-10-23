using System;
using System.Collections.Generic;

namespace SIM.Areas.GestionDocumental.Models
{
    public class MasivoDTO
    {
        public string IdSolicitud { get; set; }
        public string CodTramite { get; set; } = string.Empty;
        public bool EnviarEmail { get; set; } = false;
        public string Tema { get; set; }
        public List<Indice> Indices { get; set; }
    }

    public class Indice
    {
        public int CODINDICE { get; set; }
        public string INDICE { get; set; }
        public byte TIPO { get; set; }
        public long LONGITUD { get; set; }
        public int OBLIGA { get; set; }
        public string VALORDEFECTO { get; set; }
        public string VALOR { get; set; } = string.Empty;
        public int? ID_VALOR { get; set; }
        public Nullable<int> ID_LISTA { get; set; }
        public Nullable<int> TIPO_LISTA { get; set; }
        public string CAMPO_NOMBRE { get; set; }
    }
}