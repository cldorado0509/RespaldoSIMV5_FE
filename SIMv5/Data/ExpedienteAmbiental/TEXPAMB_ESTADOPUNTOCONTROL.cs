namespace SIM.Data.ExpedienteAmbiental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPEDIENTEAMBIENTAL.TEXPAMB_ESTADOPUNTOCONTROL")]
    public partial class TEXPAMB_ESTADOPUNTOCONTROL
    {
        public int ID { get; set; }

        public int TIPOESTADOPUNTOCONTROL_ID { get; set; }

        public int PUNTOCONTROL_ID { get; set; }

        public int FUNCIONARIO_ID { get; set; }

        public DateTime D_ESTADO { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        public int? TERCERO_ID { get; set; }

        public decimal? CODIGO_ESTADO_SOLICITUD { get; set; }

        public virtual DEXPAMB_TIPOESTADOPUNTOCONTROL DEXPAMB_TIPOESTADOPUNTOCONTROL { get; set; }

        public virtual TEXPAMB_PUNTOCONTROL TEXPAMB_PUNTOCONTROL { get; set; }
    }
}
