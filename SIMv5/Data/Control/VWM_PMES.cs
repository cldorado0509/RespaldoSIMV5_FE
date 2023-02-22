namespace SIM.Data.Control
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VWM_PMES", Schema = "CONTROL")]
    public partial class VWM_PMES
    {
        public int ID_TERCERO { get; set; }
        public string TERCERO { get; set; }
        public int ID_INSTALACION { get; set; }
        public string INSTALACION { get; set; }
        public string VIGENCIA { get; set; }
        [Key]
        public decimal ID_ESTADO { get; set; }
        public Nullable<int> ID_MODOP { get; set; }
        public string MODOP { get; set; }
        public Nullable<int> ID_MODOCAMBIO { get; set; }
        public string MODOCAMBIO { get; set; }
        public Nullable<decimal> VIAJES_LABORALES { get; set; }
        public Nullable<int> ID_MODOL { get; set; }
        public string MODOL { get; set; }
        public Nullable<int> ID_SEXO { get; set; }
        public string SEXO { get; set; }
        public Nullable<int> ID_EDAD { get; set; }
        public string EDAD { get; set; }
        public Nullable<int> ID_ESTRATO { get; set; }
        public string ESTRATO { get; set; }
        public Nullable<decimal> N_HORAINGRESO { get; set; }
        public Nullable<decimal> N_HORASALIDA { get; set; }
        public Nullable<int> ID_TIPOTRABAJADOR { get; set; }
        public string TIPOTRABAJADOR { get; set; }
        public string AREA { get; set; }
        public Nullable<int> ID_TELETRABAJO { get; set; }
        public string TELETRABAJO { get; set; }
        public Nullable<decimal> TT_LUNES { get; set; }
        public Nullable<decimal> TT_MARTES { get; set; }
        public Nullable<decimal> TT_MIERCOLES { get; set; }
        public Nullable<decimal> TT_JUEVES { get; set; }
        public Nullable<decimal> TT_VIERNES { get; set; }
        public Nullable<decimal> TT_SABADO { get; set; }
        public Nullable<decimal> TT_DOMINGO { get; set; }
        public Nullable<decimal> TT_TOTALDIAS { get; set; }
        public Nullable<decimal> N_TIEMPOVM { get; set; }
        public Nullable<decimal> N_TIEMPOM { get; set; }
        public Nullable<decimal> N_DISTANCIA { get; set; }
        public string S_DISTANCIA { get; set; }
        public Nullable<decimal> N_CO2 { get; set; }
        public Nullable<decimal> N_CO2_TT { get; set; }
        public Nullable<decimal> N_PM25 { get; set; }
        public Nullable<decimal> N_CO2P { get; set; }
        public Nullable<decimal> N_PM25P { get; set; }
        public Nullable<decimal> N_POBLACION { get; set; }
        public Nullable<decimal> N_MUESTRA { get; set; }
        public Nullable<decimal> N_FACTORMUESTRA { get; set; }
        public Nullable<decimal> N_CARRO { get; set; }
        public Nullable<decimal> N_MOTO { get; set; }
        public Nullable<decimal> N_BICICLETA { get; set; }
        public Nullable<decimal> N_DUCHAS { get; set; }
        public string S_DIRECCION { get; set; }
        public string S_MUNICIPIO { get; set; }
        public Nullable<decimal> N_LATITUD { get; set; }
        public Nullable<decimal> N_LONGITUD { get; set; }
        public string S_DIRECCION_INSTALACION { get; set; }
        public string S_MUNICIPIO_INSTALACION { get; set; }
        public Nullable<decimal> N_LATITUD_INSTALACION { get; set; }
        public Nullable<decimal> N_LONGITUD_INSTALACION { get; set; }
        public Nullable<decimal> N_CANTIDADP { get; set; }
        public Nullable<int> ID_ENCUESTA { get; set; }
    }
}
