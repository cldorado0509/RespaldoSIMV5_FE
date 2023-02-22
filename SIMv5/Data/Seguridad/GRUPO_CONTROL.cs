namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.GRUPO_CONTROL")]
    public partial class GRUPO_CONTROL
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CODIGO_GRUPO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_CONTROL { get; set; }

        [StringLength(1)]
        public string S_VISIBLE { get; set; }

        [StringLength(1)]
        public string S_HABILITADO { get; set; }

        [ForeignKey("ID_CONTROL")]
        public virtual CONTROL CONTROL { get; set; }

        [ForeignKey("ID_CODIGO_GRUPO")]
        public virtual GRUPO GRUPO { get; set; }
    }
}
