namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.USUARIO_FUNCIONARIO")]
    public partial class USUARIO_FUNCIONARIO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_USUARIO_FUNCIONARIO { get; set; }

        public int CODFUNCIONARIO { get; set; }

        public int ID_USUARIO { get; set; }

        [ForeignKey("ID_USUARIO")]
        public virtual USUARIO USUARIO { get; set; }
    }
}
