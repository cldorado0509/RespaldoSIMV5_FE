namespace SIM.Data.ExpedienteAmbiental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPEDIENTEAMBIENTAL.TEXPAMB_FUNCIONARIOEXPEDIENTE")]
    public partial class TEXPAMB_FUNCIONARIOEXPEDIENTE
    {
        public int ID { get; set; }

        public int EXPEDIENTEAMBIENTAL_ID { get; set; }

        public DateTime D_ASIGNACION { get; set; }

        public int FUNCIONARIO_ID { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        public virtual TEXPAMB_EXPEDIENTEAMBIENTAL TEXPAMB_EXPEDIENTEAMBIENTAL { get; set; }
    }
}