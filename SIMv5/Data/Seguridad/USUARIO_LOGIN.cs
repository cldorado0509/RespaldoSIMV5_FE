namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIO_LOGIN")]
    public partial class USUARIO_LOGIN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_USUARIO_LOGIN { get; set; }

        public int ID_USUARIO { get; set; }

        [StringLength(100)]
        public string LOGINPROVIDER { get; set; }

        [StringLength(128)]
        public string PROVIDERKEY { get; set; }
    }
}
