namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.TPOEAIR_SEGUIMIENTO_GLOBAL")]
    public partial class TPOEAIR_SEGUIMIENTO_GLOBAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Plan de acción")]
        public int ID_PLAN { get; set; }

        [Display(Name = "Episodio")]
        public int ID_EPISODIO { get; set; }

        [Display(Name = "Trámite")]
        public int ID_TRAMITE { get; set; }

        [Display(Name = "Id Radicado")]
        public int? N_RADICADO { get; set; }

        public int? ID_RESPONSABLE { get; set; }

        [StringLength(511)]
        [Display(Name = "Observaciones")]
        [DataType(DataType.MultilineText)]
        public string S_OBSERVACIONES { get; set; }

        [StringLength(150)]
        [Display(Name="Nombre del Remitente o Encargado")]
        public string S_REMITENTE { get; set; }

        [StringLength(100)]
        [Display(Name = "Cargo del Remitente o Encargado")]
        public string S_CARGO_REMITENTE { get; set; }

        [StringLength(255)]
        [Display(Name = "Documento de Evidencias (PDF)")]
        public string S_URL_EVIDENCIA { get; set; }

        [StringLength(20)]
        [Display(Name = "Radicado")]
        public string S_RADICADO { get; set; }

        public virtual TPOEAIR_PLAN TPOEAIR_PLAN { get; set; }

        public virtual DPOEAIR_EPISODIO DPOEAIR_EPISODIO { get; internal set; }
    }
}
