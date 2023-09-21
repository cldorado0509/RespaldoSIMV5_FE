namespace SIM.Data.ExpedienteAmbiental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPEDIENTEAMBIENTAL.TEXPAMB_ANOTACIONESPUNTO")]
    public partial class TEXPAMB_ANOTACIONESPUNTO
    {
        public int ID { get; set; }

        public int PUNTOCONTROL_ID { get; set; }

        public int FUNCIONARIO_ID { get; set; }

        public DateTime D_REGISTRO { get; set; }

        [Required]
        [StringLength(2000)]
        public string S_ANOTACION { get; set; }

        public virtual TEXPAMB_PUNTOCONTROL TEXPAMB_PUNTOCONTROL { get; set; }
    }
}
