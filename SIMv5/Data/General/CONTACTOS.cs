namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.CONTACTOS")]
    public partial class CONTACTOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_CONTACTO { get; set; }

        public int ID_TERCERO_NATURAL { get; set; }

        public decimal ID_JURIDICO { get; set; }

        public DateTime? D_INICIO { get; set; }

        [StringLength(2)]
        public string TIPO { get; set; }

        public DateTime? D_FIN { get; set; }

        [ForeignKey("ID_TERCERO_NATURAL")]
        public virtual TERCERO TERCERO { get; set; }
    }
}
