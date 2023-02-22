namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CONTROL.EST_TIPO_DATO")]
    public partial class EST_TIPO_DATO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TIPO_DATO { get; set; }

        [StringLength(30)]
        public string S_NOMBRE { get; set; }

        [StringLength(20)]
        public string S_CAMPO { get; set; }
    }
}
