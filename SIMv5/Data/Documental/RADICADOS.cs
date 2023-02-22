namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.RADICADOS")]
    public partial class RADICADOS_ETIQUETAS
    {
        public RADICADOS_ETIQUETAS()
        {
            this.ANEXOS = new HashSet<ANEXOS>();
            this.DOCUMENTOS = new HashSet<DOCUMENTOS>();
            this.EXPEDIENTE = new HashSet<EXPEDIENTE>();
            this.PRESTAMO_DETALLE = new HashSet<PRESTAMO_DETALLE>();
            this.TOMOS = new HashSet<TOMOS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_RADICADO { get; set; }

        public DateTime? D_CREACION { get; set; }

        [StringLength(50)]
        public string S_ETIQUETA { get; set; }

        [Required]
        [StringLength(30)]
        public string S_IDENTIFICADOR { get; set; }

        public int? ID_TERCERO { get; set; }

        [StringLength(1)]
        public string S_ESTADO { get; set; }

        public int ID_RADICADOUNIDADDOCUMENTAL { get; set; }

        public int? S_ESTAMPADO { get; set; }

        public DateTime? D_ESTAMPA { get; set; }

        public int? ID_RADICADOPADRE { get; set; }

        public int? ID_REGISTROREL { get; set; }

        public int? ID_TIPOANEXO { get; set; }

        [StringLength(255)]
        public string S_TEXTO { get; set; }

        [StringLength(1)]
        public string S_TIPO { get; set; }

        [StringLength(30)]
        public string S_UBICACION { get; set; }

        [StringLength(30)]
        public string S_CONSECUTIVOTIPO { get; set; }

        [StringLength(20)]
        public string S_FOLIOS { get; set; }

        public virtual ICollection<ANEXOS> ANEXOS { get; set; }
        public virtual ICollection<DOCUMENTOS> DOCUMENTOS { get; set; }
        public virtual ICollection<EXPEDIENTE> EXPEDIENTE { get; set; }
        public virtual ICollection<PRESTAMO_DETALLE> PRESTAMO_DETALLE { get; set; }

        [ForeignKey("ID_RADICADOUNIDADDOCUMENTAL")]
        public virtual RADICADO_UNIDADDOCUMENTAL RADICADO_UNIDADDOCUMENTAL { get; set; }
        public virtual ICollection<TOMOS> TOMOS { get; set; }
    }
}
