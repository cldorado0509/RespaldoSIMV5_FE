namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.OPERADOR")]
    public partial class OPERADOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_OPERADOR { get; set; }

        public int ID_TERCERO { get; set; }

        public int ID_ESTRATEGIA { get; set; }

        public DateTime D_INICIO { get; set; }

        public DateTime? D_FIN { get; set; }

        [StringLength(100)]
        public string S_DESCRIPCION { get; set; }

        [ForeignKey("ID_ESTRATEGIA")]
        public virtual ESTRATEGIA ESTRATEGIA { get; set; }
    }
}
