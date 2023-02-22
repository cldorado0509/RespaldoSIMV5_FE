namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.CAMPO")]
    public partial class CAMPO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CAMPO { get; set; }

        public int? ID_FORMA { get; set; }

        [Required]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [StringLength(1000)]
        public string S_DESCRIPCION { get; set; }

        [ForeignKey("ID_FORMA")]
        public virtual FORMA FORMA { get; set; }
    }
}
