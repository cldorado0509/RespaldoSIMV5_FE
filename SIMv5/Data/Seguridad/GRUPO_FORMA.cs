namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.GRUPO_FORMA")]
    public partial class GRUPO_FORMA
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_GRUPO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FORMA { get; set; }

        [StringLength(1)]
        public string S_NUEVO { get; set; }

        [StringLength(1)]
        public string S_EDITAR { get; set; }

        [StringLength(1)]
        public string S_ELIMINAR { get; set; }

        [StringLength(1)]
        public string S_BUSCAR { get; set; }

        [StringLength(1)]
        public string S_IMPRIMIR { get; set; }

        [StringLength(1)]
        public string S_ADJUNTAR { get; set; }

        [ForeignKey("ID_FORMA")]
        public virtual FORMA FORMA { get; set; }

        [ForeignKey("ID_GRUPO")]
        public virtual GRUPO GRUPO { get; set; }
    }
}
