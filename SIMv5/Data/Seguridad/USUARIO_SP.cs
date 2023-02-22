namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIO_SP")]
    public partial class USUARIO_SP
    {
        [Key]
        [StringLength(255)]
        public string ID_USUARIO { get; set; }

        [Required]
        [StringLength(100)]
        public string S_LOGIN { get; set; }

        [Required]
        [StringLength(255)]
        public string S_APPLICATIONNAME { get; set; }

        [Required]
        [StringLength(128)]
        public string S_EMAIL { get; set; }

        [StringLength(255)]
        public string S_COMMENT { get; set; }

        [Required]
        [StringLength(128)]
        public string S_PASSWORD { get; set; }

        [StringLength(255)]
        public string S_PASSWORDQUESTION { get; set; }

        [StringLength(255)]
        public string S_PASSWORDANSWER { get; set; }

        [StringLength(1)]
        public string C_ISAPPROVED { get; set; }

        public DateTime? D_LASTACTIVITYDATE { get; set; }

        public DateTime? D_LASTLOGINDATE { get; set; }

        public DateTime? D_LASTPASSWORDCHANGEDDATE { get; set; }

        public DateTime? D_CREATIONDATE { get; set; }

        [StringLength(1)]
        public string C_ISONLINE { get; set; }

        [StringLength(1)]
        public string C_ISLOCKEDOUT { get; set; }

        public DateTime? D_LASTLOCKEDOUTDATE { get; set; }

        public decimal? N_FAILEDPASSATTEMCOUNT { get; set; }

        public DateTime? D_FAILEDPASSATTEMWINSTART { get; set; }

        public decimal? N_FAILEDPASSANSATTEMCOUNT { get; set; }

        public DateTime? D_FAILEDPASSANSATTEMWINSTART { get; set; }
    }
}
