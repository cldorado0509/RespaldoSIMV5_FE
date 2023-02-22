namespace SIM.Data.Control
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CONTROL.TIPO_MEDIDAADICIONAL")]
    public class TIPO_MEDIDAADICIONAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("NOMBRE")]
        public string NOMBRE { get; set; }
    }
}