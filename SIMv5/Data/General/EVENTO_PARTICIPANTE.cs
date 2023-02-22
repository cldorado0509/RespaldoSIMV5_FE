namespace SIM.Data.General
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GENERAL.EVENTO_PARTICIPANTE")]
    public class EVENTO_PARTICIPANTE
    {
        [Key, Column(Order = 1)]
        public int ID_EVENTO { get; set; }
        [Key, Column(Order = 2)]
        public int ID_PARTICIPANTE { get; set; }
        public EVENTO Evento { get; set; }
        public PARTICIPANTE Participante { get; set; }
    }
}