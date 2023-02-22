namespace SIM.Data.FuentesFijas
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FTEMOVOP.TMOVPEM_MONITOREO")]
    public partial  class TMOVPEM_MONITOREO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        public decimal? ID_ETIQUETA { get; set; }

        public string S_PLACA { get; set; }

        public int N_KILOMETRAJE { get; set; }

        public DateTime D_MONITOREO { get; set; }

        public float N_COORDENADA_X { get; set; }

        public float N_COORDENADA_Y { get; set; }

        public string S_FOTO1 { get; set; }

        public string S_FOTO2 { get; set; }

        public string S_FOTO3 { get; set; }

        public float N_VALOR_MONITOREO { get; set; }

        public string S_EMPRESA { get; set; }

        public string S_OBSERVACIONES { get; set; }

        public string S_USUARIO { get; set; }

        public string S_ESTADO { get; set; }

        public long N_RPM_RALENTI { get; set; }

    }
}