namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.REPORTE")]
    public partial class REPORTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REPORTE()
        {
            REPORTE1 = new HashSet<REPORTE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_REPORTE { get; set; }

        public int? ID_REPORTEPADRE { get; set; }

        [Required]
        [StringLength(150)]
        public string S_REPORTE { get; set; }

        [StringLength(4000)]
        public string S_QRYGENERAL { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(1000)]
        public string S_MAPA { get; set; }

        [StringLength(3000)]
        public string S_QRYVARIABLE { get; set; }

        [StringLength(4000)]
        public string S_TIPOGRAFICO { get; set; }

        [StringLength(2500)]
        public string S_QRYGRAFICO { get; set; }

        [StringLength(1)]
        public string S_OCULTO { get; set; }

        public int? N_VINCULO { get; set; }

        [StringLength(100)]
        public string S_VARIABLE { get; set; }

        [StringLength(2000)]
        public string S_IMAGEN { get; set; }

        [StringLength(150)]
        public string S_GRAFICO { get; set; }

        [StringLength(4000)]
        public string S_QRYDETALLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REPORTE> REPORTE1 { get; set; }

        [ForeignKey("ID_REPORTEPADRE")]
        public virtual REPORTE REPORTE2 { get; set; }
    }
}
