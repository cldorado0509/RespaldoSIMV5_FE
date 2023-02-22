namespace SIM.Data.Tramites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRAMITES.QRY_LISTADOTRAMITES")]
    public partial class QRY_LISTADOTRAMITES
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TRAMITE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string TRAMITE { get; set; }

        [StringLength(2000)]
        public string DESCRIPCION { get; set; }

        public int ID_FORMULARIO { get; set; }
    }
}
