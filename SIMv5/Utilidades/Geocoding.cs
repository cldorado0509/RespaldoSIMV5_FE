using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using SIM.Areas.ControlVigilancia.Controllers;

namespace SIM.Utilidades
{
    public class Geocoding
    {
        private class Coordenadas
        {
            public decimal lat { get; set; }
            public decimal lng { get; set; }
        }

        private class Geometria
        {
            public Coordenadas location { get; set; }
        }

        private class Resultado
        {
            public Geometria geometry { get; set; }
        }
        private class DatosGeocoding
        {
            public string error_message { get; set; }
            public List<Resultado> results { get; set; }
            public string status { get; set; }
        }

        public static coordenadas ObtenerCoordenadas(string direccion)
        {
            coordenadas coordenadasGoogle = new coordenadas();
            string apiKey = ConfigurationManager.AppSettings["GoogleApiKey"] ?? "";

            WebClient client = new WebClient();

            direccion = HttpUtility.UrlEncode(direccion);

            var response = client.DownloadString("https://maps.googleapis.com/maps/api/geocode/json?address=" + direccion + "&key=" + apiKey);

            DatosGeocoding datosGeocoding = JsonConvert.DeserializeObject<DatosGeocoding>(response);

            if (datosGeocoding.status == "OK")
            {
                coordenadasGoogle.x = datosGeocoding.results[0].geometry.location.lng.ToString();
                coordenadasGoogle.y = datosGeocoding.results[0].geometry.location.lat.ToString();
            }
            else
            {
                coordenadasGoogle.x = "0";
                coordenadasGoogle.y = "0";
            }

            return coordenadasGoogle;
        }
    }
}