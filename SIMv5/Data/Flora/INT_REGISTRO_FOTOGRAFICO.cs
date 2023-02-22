namespace SIM.Data.Flora
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FLORA.INT_REGISTRO_FOTOGRAFICO")]
    public partial class INT_REGISTRO_FOTOGRAFICO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int ID_INTERVENCION { get; set; }

        [StringLength(100)]
        public string S_FOTO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_FOTO { get; set; }

        public int? ID_FOTO_ASPECTO { get; set; }

        [ForeignKey("ID_INTERVENCION")]
        public virtual INT_INTERVENCION INT_INTERVENCION { get; set; }
    }
}
