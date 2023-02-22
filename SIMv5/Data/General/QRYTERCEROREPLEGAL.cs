namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.QRYTERCEROREPLEGAL")]
    public partial class QRYTERCEROREPLEGAL
    {
        [StringLength(10)]
        public string S_ABREVIATURA { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long N_DOCUMENTO { get; set; }

        public DateTime? D_REGISTRO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TERCERO { get; set; }

        [StringLength(100)]
        public string S_CORREO { get; set; }

        public int? ID_TERCEROREPLEGAL { get; set; }

        [StringLength(100)]
        public string S_APELLIDO1 { get; set; }

        [StringLength(100)]
        public string S_APELLIDO2 { get; set; }

        [StringLength(100)]
        public string S_NOMBRE1 { get; set; }

        [StringLength(100)]
        public string S_NOMBRE2 { get; set; }

        [StringLength(403)]
        public string NOMBRE_COMPLETO { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ORGANIZACION { get; set; }

        [StringLength(500)]
        public string S_RSOCIAL { get; set; }
    }
}
