namespace SIM.Data.Documental
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENTAL.UNIDAD_DOCUMENTAL")]
    public partial class UNIDAD_DOCUMENTAL
    {
        public UNIDAD_DOCUMENTAL()
        {
            this.EXPEDIENTE = new HashSet<EXPEDIENTE>();
            this.PERMISO_UNIDADDOCUMENTAL = new HashSet<PERMISO_UNIDADDOCUMENTAL>();
            this.RADICADO_UNIDADDOCUMENTAL = new HashSet<RADICADO_UNIDADDOCUMENTAL>();
            this.UNIDAD_TIPO = new HashSet<UNIDAD_TIPO>();
            this.DOCUMENTOS = new HashSet<DOCUMENTOS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_UNIDADDOCUMENTAL { get; set; }

        [StringLength(250)]
        public string S_NOMBRE { get; set; }

        [StringLength(1000)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(250)]
        public string S_RUTA { get; set; }

        public DateTime? D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [StringLength(1)]
        public string S_TIPO { get; set; }

        public int ID_CLASIFICACION { get; set; }

        public int ID_SUBSERIE { get; set; }

        public virtual ICollection<EXPEDIENTE> EXPEDIENTE { get; set; }
        public virtual ICollection<PERMISO_UNIDADDOCUMENTAL> PERMISO_UNIDADDOCUMENTAL { get; set; }
        public virtual ICollection<RADICADO_UNIDADDOCUMENTAL> RADICADO_UNIDADDOCUMENTAL { get; set; }

        [ForeignKey("ID_SUBSERIE")]
        public virtual SUBSERIE SUBSERIE { get; set; }
        public virtual ICollection<UNIDAD_TIPO> UNIDAD_TIPO { get; set; }
        public virtual ICollection<DOCUMENTOS> DOCUMENTOS { get; set; }
    }
}
