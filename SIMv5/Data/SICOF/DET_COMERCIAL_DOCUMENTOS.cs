namespace SIM.Data.SICOF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SFCOMERCIAL.DET_COMERCIAL_DOCUMENTOS")]
    public partial class DET_COMERCIAL_DOCUMENTOS
    {
        public int CODIGO_COMERCIAL { get; set; }

        public int CODIGO_DOCUMENTO { get; set; }

        public int CODIGO_INTERNO_DOCUMENTO { get; set; }

        public int CODIGO_APLICACION { get; set; }

        public int CODIGO_ASIENTO { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CODIGO_DET_COM_DOCUMENTO { get; set; }

        public int CONSECUTIVO { get; set; }

        [StringLength(50)]
        public string CODIGO_MEMPRESA { get; set; }

        [StringLength(30)]
        public string USUARIO_EMPRESA { get; set; }

        public DateTime? FECHA_REGISTRO { get; set; }

        [StringLength(1)]
        public string GENERE_DOC_REF { get; set; }
    }
}
