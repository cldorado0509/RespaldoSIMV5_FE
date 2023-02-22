namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.BUSQUEDA")]
    public partial class BUSQUEDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_BUSQUEDA { get; set; }

        public byte ID_TIPO { get; set; }

        public int ID_ELEMENTO_TIPO { get; set; }

        [Required]
        [StringLength(256)]
        public string S_DESCRIPCION { get; set; }

        [StringLength(4000)]
        public string S_TEXTO { get; set; }
    }
}
