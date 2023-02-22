namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.NATURAL")]
    public partial class NATURAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("TERCERO")]
        public int ID_TERCERO { get; set; }

        public int? ID_ESTADOCIVIL { get; set; }

        public int? ID_DIVIPOLA { get; set; }

        [StringLength(100)]
        public string S_APELLIDO1 { get; set; }

        [StringLength(100)]
        public string S_APELLIDO2 { get; set; }

        [StringLength(100)]
        public string S_NOMBRE1 { get; set; }

        [StringLength(100)]
        public string S_NOMBRE2 { get; set; }

        [StringLength(1)]
        public string S_CLASLIBRETA { get; set; }

        public int? N_NUMLIBRETA { get; set; }

        public int? N_DISTLIBRETA { get; set; }

        [StringLength(1)]
        public string S_EXTRANJERO { get; set; }

        public DateTime? D_NACIMIENTO { get; set; }

        [StringLength(1)]
        public string S_GENERO { get; set; }

        [StringLength(20)]
        public string S_MATRICULAPROFESIONAL { get; set; }

        public decimal? ID_PROFESION { get; set; }

        [ForeignKey("ID_DIVIPOLA")]
        public virtual DIVIPOLA DIVIPOLA { get; set; }

        [ForeignKey("ID_ESTADOCIVIL")]
        public virtual ESTADO_CIVIL ESTADO_CIVIL { get; set; }

        [ForeignKey("ID_PROFESION")]
        public virtual PROFESION PROFESION { get; set; }

        [ForeignKey("ID_TERCERO")]
        public virtual TERCERO TERCERO { get; set; }
    }
}
