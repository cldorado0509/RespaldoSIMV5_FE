namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.GAE")]
    public partial class GAE
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ACTIVIDADECONOMICA { get; set; }

        public int? ID_ACTIVIDADECONOMICAPADRE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string S_NOMBRE { get; set; }

        [StringLength(50)]
        public string S_CODIGO { get; set; }

        [StringLength(2000)]
        public string S_DESCRIPCION { get; set; }
    }
}
