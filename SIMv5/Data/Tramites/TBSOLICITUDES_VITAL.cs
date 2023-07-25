namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TRAMITES.TBSOLICITUDES_VITAL")]
    public class TBSOLICITUDES_VITAL
    {
		[Key]
		public decimal CODIGO_SOLICITUD { get; set; }
		public string TIPO_TRAMITE { get; set; }
		public string IDENTIFICADOR { get; set; }
		public decimal? CODTRAMITE { get; set; }
		public DateTime FECHA { get; set; }
		public string ATENDIDA { get; set; }
		public string ID_RADICACION { get; set; }
		public string NUMERO_SILPA { get; set; }
		public string ACTO_ADMINISTRATIVO { get; set; }
		public string ID_FORMULARIO { get; set; }
		public string PATH_DOCUMENTO { get; set; }
		public string ID_AA { get; set; }
		public string NUMERO_VITAL { get; set; }
		public string NUMERO_VITAL_ASOCIADO { get; set; }
        public string OBSERVACION { get; set; }
        public decimal? ID_CAUSA_NO_ATENCION { get; set; }
    }
}