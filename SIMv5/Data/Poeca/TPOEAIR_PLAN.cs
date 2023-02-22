namespace SIM.Data.Poeca
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;
    using System.Web.Http;

    [Table("POECA.TPOEAIR_PLAN")]
    public partial class TPOEAIR_PLAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TPOEAIR_PLAN()
        {
            TPOEAIR_ACCIONES_PLAN = new HashSet<TPOEAIR_ACCIONES_PLAN>();
            TPOEAIR_SEGUIMIENTO_GLOBAL = new HashSet<TPOEAIR_SEGUIMIENTO_GLOBAL>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DisplayName("Año de ejecución")]
        public int N_ANIO { get; set; }

        [StringLength(7)]
        [DisplayName("Número de radicado")]
        public string S_RADICADO { get; set; }

        public int? N_RADICADO { get; set; }

        [DisplayName("Año de radicación")]
        public int? N_RADICADO_ANIO { get; set; }

        [StringLength(511)]
        [DisplayName("Observaciones")]
        [DataType(DataType.MultilineText)]
        public string S_OBSERVACIONES { get; set; }

        [StringLength(150)]
        [DisplayName("Nombre del Remitente o Encargado")]
        public string S_REMITENTE { get; set; }

        [StringLength(100)]
        [DisplayName("Cargo del Remitente o Encargado")]
        public string S_CARGO_REMITENTE { get; set; }

        [StringLength(255)]
        [DisplayName("Archivo de anexos (PDF)")]
        public string S_URL_ANEXO { get; set; }

        public int? ID_RESPONSABLE { get; set; }

        public decimal ID_TRAMITE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_ACCIONES_PLAN> TPOEAIR_ACCIONES_PLAN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TPOEAIR_SEGUIMIENTO_GLOBAL> TPOEAIR_SEGUIMIENTO_GLOBAL { get; set; }
    }
}
