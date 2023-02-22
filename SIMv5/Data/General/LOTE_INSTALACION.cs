namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.LOTE_INSTALACION")]
    public partial class LOTE_INSTALACION
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_LOTEINSTALACION { get; set; }

        public int ID_INSTALACION { get; set; }

        [StringLength(19)]
        public string PK_PREDIO { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [StringLength(50)]
        public string S_CEDULACATASTRAL { get; set; }

        [StringLength(50)]
        public string S_MATRICULAINMOBILIARIA { get; set; }

        public DateTime? D_ACTUALIZACION { get; set; }

        public DateTime? D_CREACION { get; set; }

        public decimal? N_AREA { get; set; }

        [ForeignKey("ID_INSTALACION")]
        public virtual INSTALACION INSTALACION { get; set; }
    }
}
