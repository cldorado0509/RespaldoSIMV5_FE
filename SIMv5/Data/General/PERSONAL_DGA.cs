namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.PERSONAL_DGA")]
    public partial class PERSONAL_DGA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PERSONALDGA { get; set; }

        public int ID_DGA { get; set; }

        public int ID_TERCERO { get; set; }

        [Required]
        [StringLength(1)]
        public string S_TIPOPERSONAL { get; set; }

        [Required]
        [StringLength(1)]
        public string S_ESRESPONSABLE { get; set; }

        public byte? N_DEDICACION { get; set; }

        public short? N_EXPERIENCIA { get; set; }

        [StringLength(2000)]
        public string S_OBSERVACION { get; set; }

        public int ID_TIPOPERSONAL { get; set; }

        [ForeignKey("ID_DGA")]
        public virtual DGA DGA { get; set; }

        [ForeignKey("ID_TERCERO")]
        public virtual TERCERO TERCERO { get; set; }

        [ForeignKey("ID_TIPOPERSONAL")]
        public virtual TIPO_PERSONAL_DGA TIPO_PERSONAL_DGA { get; set; }
    }
}
