namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.GRUPO_PANEL")]
    public partial class GRUPO_PANEL
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_GRUPO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_PANEL { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FORMA { get; set; }

        [StringLength(1)]
        public string S_VISIBLE { get; set; }

        [StringLength(1)]
        public string S_HABILITADO { get; set; }

        [ForeignKey("ID_GRUPO")]
        public virtual GRUPO GRUPO { get; set; }

        [ForeignKey("ID_PANEL")]
        public virtual PANEL PANEL { get; set; }
    }
}
