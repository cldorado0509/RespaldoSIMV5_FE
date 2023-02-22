namespace SIM.Data.EnCicla
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ENCICLA.LOG_CONSULTA")]
    public partial class LOG_CONSULTA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDLOGCONSULTA { get; set; }

        public DateTime FECHACONSULTA { get; set; }

        public int IDUSUARIO { get; set; }

        public int IDTERCERO { get; set; }

        public DateTime? FECHAINICIAL { get; set; }

        public DateTime? FECHAFINAL { get; set; }

        public DateTime? HORAINICIAL { get; set; }

        public DateTime? HORAFINAL { get; set; }

        [StringLength(2048)]
        public string ESTACIONES { get; set; }

        public int? IDDATOSCONSULTA { get; set; }
    }
}
