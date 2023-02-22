namespace SIM.Areas.ParqueAguas.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class ReservaMasiva
    {
        [Range(1, int.MaxValue, ErrorMessage = "El tipo de documento es requerido!")]
        public int TipoDocumentoId { get; set; }

        [Required(ErrorMessage = "El número de documento es requerido!")]
        [Range(10000000, int.MaxValue, ErrorMessage = "El número de documento no es válido!")]
        public long NumeroDocumento { get; set; }

        public IEnumerable<SelectListItem> TiposDocumentos { get; set; }

        [Required(ErrorMessage = "Los nombres del responsable son requeridos")]
        [MaxLength(40, ErrorMessage = "Los nombres no pueden tener más de 40 caracteres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos del responsable son requeridos")]
        [MaxLength(40, ErrorMessage = "Los apellidos no pueden tener más de 40 caracteres")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El correo electrónico del responsable es requerido")]
        [MaxLength(50, ErrorMessage = "El correo electrónico no puede tener más de 50 caracteres")]
        [EmailAddress(ErrorMessage = "Correo electrónico no válido!")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        [Required(ErrorMessage = "El número telefónico del responsable es requerido")]
        [MaxLength(20, ErrorMessage = "El número telefónico no puede tener más de 20 caracteres")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La dirección del responsable es requerida")]
        [MaxLength(20, ErrorMessage = "La dirección no puede tener más de 200 caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El número de personas es requerido")]
        [Range(41, int.MaxValue, ErrorMessage = "El número de personas debe ser mayor de 40!")]
        public int NumeroPersonas { get; set; }

        [Required(ErrorMessage = "La Fecha de la reserva es requerida")]
        public DateTime Fecha { get; set; }

        public string Comprobante { get; set; }


    }
}