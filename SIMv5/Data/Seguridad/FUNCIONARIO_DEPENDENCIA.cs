namespace SIM.Data.Seguridad
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.FUNCIONARIO_DEPENDENCIA")]
    public partial class FUNCIONARIO_DEPENDENCIA
    {
        [Key]
        public decimal ID_FUNCIONARIO_DEPENDENCIA { get; set; }

        public decimal CODFUNCIONARIO { get; set; }

        public decimal ID_DEPENDENCIA { get; set; }

        public DateTime D_INGRESO { get; set; }

        public DateTime? D_SALIDA { get; set; }

        [ForeignKey("ID_DEPENDENCIA")]
        public virtual DEPENDENCIA DEPENDENCIA { get; set; }
    }
}
