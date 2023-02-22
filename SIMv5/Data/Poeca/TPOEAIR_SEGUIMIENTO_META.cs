namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POECA.TPOEAIR_SEGUIMIENTO_META")]
    public partial class TPOEAIR_SEGUIMIENTO_META
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }


        [Display(Name = "Acción del plan")]
        public int ID_INFO_ACCION { get; set; }

        [Display(Name = "Episodio")]
        public int ID_EPISODIO { get; set; }

        [Display(Name ="Meta alcanzada")]
        public int N_SEGUIMIENTO_META { get; set; }

        [Display(Name = "Fecha de actualización")]
        public DateTime D_FECHA_ACTUALIZACION { get; set; }

        [Display(Name = "Recursos invertidos")]
        public decimal? N_VALORACION_ECONOMICA { get; set; }

        [StringLength(200)]
        [Display(Name = "Observaciones")]
        public string S_OBSERVACIONES { get; set; }

        public int ID_RESPONSABLE { get; set; }

        public virtual DPOEAIR_EPISODIO DPOEAIR_EPISODIO { get; set; }

        public virtual TPOEAIR_ACCIONES_PLAN TPOEAIR_ACCIONES_PLAN { get; set; }
    }
}
