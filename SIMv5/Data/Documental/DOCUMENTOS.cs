namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.DOCUMENTOS")]
    public partial class DOCUMENTOS
    {
        public DOCUMENTOS()
        {
            this.ANEXOS = new HashSet<ANEXOS>();
            this.DOCUMENTO_INDICES = new HashSet<DOCUMENTO_INDICES>();
            this.DOCUMENTOS_TOMO = new HashSet<DOCUMENTOS_TOMO>();
            this.LOG_DOCUMENTO = new HashSet<LOG_DOCUMENTO>();
            this.PRESTAMO_DETALLE = new HashSet<PRESTAMO_DETALLE>();
            this.TIPODOCUMENTAL_DATO = new HashSet<TIPODOCUMENTAL_DATO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_DOCUMENTO { get; set; }

        [StringLength(250)]
        public string S_DOCUMENTO { get; set; }

        public DateTime? D_CREACION { get; set; }

        [StringLength(250)]
        public string S_HASH { get; set; }

        [StringLength(250)]
        public string S_NOMBRESERVIDOR { get; set; }

        [StringLength(250)]
        public string S_RUTA { get; set; }

        public int? N_PAGINAS { get; set; }

        public DateTime? D_VIGENCIA { get; set; }

        public int ID_UNIDADDOCUMENTAL { get; set; }

        public int ID_UNIDADTIPO { get; set; }

        public int ID_RADICADO { get; set; }

        public int? ID_TIPODOCUMENTAL { get; set; }

        public int? N_FOLIOS { get; set; }

        public virtual ICollection<ANEXOS> ANEXOS { get; set; }
        public virtual ICollection<DOCUMENTO_INDICES> DOCUMENTO_INDICES { get; set; }
        public virtual ICollection<DOCUMENTOS_TOMO> DOCUMENTOS_TOMO { get; set; }
        public virtual ICollection<LOG_DOCUMENTO> LOG_DOCUMENTO { get; set; }
        public virtual ICollection<PRESTAMO_DETALLE> PRESTAMO_DETALLE { get; set; }

        [ForeignKey("ID_RADICADO")]
        public virtual RADICADOS_ETIQUETAS RADICADOS { get; set; }
        public virtual ICollection<TIPODOCUMENTAL_DATO> TIPODOCUMENTAL_DATO { get; set; }

        [ForeignKey("ID_UNIDADTIPO")]
        public virtual UNIDAD_TIPO UNIDAD_TIPO { get; set; }

        [ForeignKey("ID_UNIDADDOCUMENTAL")]
        public virtual UNIDAD_DOCUMENTAL UNIDAD_DOCUMENTAL { get; set; }
    }
}
