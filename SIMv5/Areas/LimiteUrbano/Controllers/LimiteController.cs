
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.ControlVigilancia.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;



namespace SIM.Areas.LimiteUrbano.Controllers
{
    public class LimiteController : Controller
    {
        // GET: LimiteUrbano/Limite
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MapaEncuesta()
        {
            String strInstalacion = Request.Params["idinstalacion"];
            String strModo = Request.Params["modo"];
            ViewBag.strInstalacion = strInstalacion;
            ViewBag.strModo = strModo;
            return View();
        }
        public ActionResult UbicacionUrbana()
        {
            String strDirecion=Request.Params["direccion"];
            String strMunicipio = Request.Params["municipio"];
            String strTipo = Request.Params["tipo"];
            String srtid=Request.Params["id"];
            String stridprenta = Request.Params["idprenta"];
            String srtidCampo = Request.Params["idCampo"];
            ViewBag.txtDireccion = strDirecion;
            ViewBag.cmbMunicipio = strMunicipio;
            ViewBag.strTipo = strTipo;
            ViewBag.srtid = srtid;
            ViewBag.stridprenta = stridprenta;
            ViewBag.srtidCampo = srtidCampo;

            return View(); 
        }

        public ActionResult UbicacionUrbanaLite()
        {
            String strMunicipio;

            String strDirecion = Request.Params["direccion"];
            var isNumeric = int.TryParse(Request.Params["municipio"], out _);
            if (isNumeric)
                strMunicipio = Request.Params["municipio"];
            else
            {
                strMunicipio = "";

                switch (Request.Params["municipio"].Trim().ToUpper())
                {
                    case "BARBOSA":
                        strMunicipio = "1";
                        break;
                    case "GIRARDOTA":
                        strMunicipio = "2";
                        break;
                    case "COPACABANA":
                        strMunicipio = "3";
                        break;
                    case "BELLO":
                        strMunicipio = "4";
                        break;
                    case "MEDELLIN":
                    case "MEDELLÍN":
                        strMunicipio = "5";
                        break;
                    case "ITAGUI":
                        strMunicipio = "6";
                        break;
                    case "LA ESTRELLA":
                        strMunicipio = "7";
                        break;
                    case "SABANETA":
                        strMunicipio = "8";
                        break;
                    case "CALDAS":
                        strMunicipio = "9";
                        break;
                    case "ENVIGADO":
                        strMunicipio = "10";
                        break;
                    case "OTRO":
                        strMunicipio = "100";
                        break;
                }
            }
                
            String strTipo = Request.Params["tipo"];
            String srtid = Request.Params["id"];
            String stridprenta = Request.Params["idprenta"];
            String srtidCampo = Request.Params["idCampo"];
            String strX = Request.Params["x"];
            String strY = Request.Params["y"];
            ViewBag.txtDireccion = strDirecion;
            ViewBag.cmbMunicipio = strMunicipio;
            ViewBag.strTipo = strTipo;
            ViewBag.srtid = srtid;
            ViewBag.stridprenta = stridprenta;
            ViewBag.srtidCampo = srtidCampo;
            ViewBag.srtX = (strX ?? "").Replace(',', '.');
            ViewBag.srtY = (strY ?? "").Replace(',', '.');

            return View();
        }

        public ActionResult geoCodificar(String valor)
        {
            string apiKey = ConfigurationManager.AppSettings["GoogleApiKey"];

            /*ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            coordenadas coorP = new coordenadas();
            coordenadas coorP2 = new coordenadas();
            //coordenadas resul = new coordenadas();
            var client = new System.Net.WebClient();
            var dato = "";
            try
            {
                dato = client.DownloadString("https://www.medellin.gov.co/MapGIS/Geocod/AjaxGeocod?accion=2&valor=" + valor + "&f=json");
                //dato = client.DownloadString("https://maps.googleapis.com/maps/api/geocode/json?address=" + valor + "&key=" + apiKey);
            }
            catch (Exception e)
            {

            }
            if (dato != "0")
            {


                coorP = JsonConvert.DeserializeObject<coordenadas>(dato);
                String xp = coorP.x;
                String yp = coorP.y;
                var dato2 = client.DownloadString("https://www.medellin.gov.co/mapas/rest/services/Utilities/Geometry/GeometryServer/project?inSR=6257&outSR=4326&geometries=" + xp + "," + yp + "&transformation=15738&transformForward=true&f=json");
                JObject o = JObject.Parse(dato2);
                Decimal x = (Decimal)o.SelectToken("geometries[0].x");
                Decimal y = (Decimal)o.SelectToken("geometries[0].y");
                resul.x = x.ToString();
                resul.y = y.ToString();
            }else
            {
                resul.x ="0";
                resul.y = "0";
            }*/

            coordenadas resul = Utilidades.Geocoding.ObtenerCoordenadas(valor);

            return Json(resul, JsonRequestBehavior.AllowGet); 
                
        }

        public ActionResult Ubicacion()
        {
            //try
            //{
            //    co.gov.medellin.www.reverseGeocodService geo = new co.gov.medellin.www.reverseGeocodService();

            //    var s = geo.wsReverseGeocod(1183128.05, 833048.089, 50, 10);


            //}
            //catch (Exception e)
            //{

            //}
            //var client = new System.Net.WebClient();
            //var dato = client.DownloadString("https://www.medellin.gov.co/servicios/GEOCOD_WEB/reverseGeocodService?coordX=1183128.053&coordY=833048.089&radioBusqueda=50&numRegistros=5");

            return View();
        }
        private CredentialCache GetCredential()
        {
            string url = @"http://www.medellin.gov.co/GEOCOD_WEB/servicios/reverseGeoCodUTF?coordX=-75.5831534&coordY=6.2392499&radioBusqueda=100&numRegistros=1";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(new System.Uri(url), " ", new NetworkCredential(ConfigurationManager.AppSettings["metropolgeorever"], ConfigurationManager.AppSettings["Metropol0504**2"]));
            return credentialCache;
        }
        public ActionResult geocoRever()
        {

            try
            {
                string autorization = "metropolgeorever" + ":" + "Metropol0504**2";
                byte[] binaryAuthorization = System.Text.Encoding.UTF8.GetBytes(autorization);
                autorization = Convert.ToBase64String(binaryAuthorization);
                
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebRequest myWebRequest = WebRequest.Create("http://www.medellin.gov.co/GEOCOD_WEB/servicios/reverseGeoCodUTF?coordX=-75.5831534&coordY=6.2392499&radioBusqueda=100&numRegistros=1");
                myWebRequest.PreAuthenticate = true;
                myWebRequest.ContentType = "application/json";
                myWebRequest.Headers.Add("AUTHORIZATION", autorization);
                var webResponse = (HttpWebResponse)myWebRequest.GetResponse();
                //NetworkCredential networkCredential = new NetworkCredential("metropolgeorever", "Metropol0504**2");
               // myWebRequest.Credentials = networkCredential;

                // Assign the response object of 'WebRequest' to a 'WebResponse' variable.
                WebResponse myWebResponse = myWebRequest.GetResponse();

                /*string url = @"http://www.medellin.gov.co/GEOCOD_WEB/servicios/reverseGeoCodUTF?coordX=-75.5831534&coordY=6.2392499&radioBusqueda=100&numRegistros=1";
                WebRequest request = WebRequest.Create(url);
                request.Credentials = GetCredential();
                request.PreAuthenticate = true;
                request.UseDefaultCredentials = true;

                request.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse resp = request.GetResponse() as HttpWebResponse;*/
            //WebRequest req = WebRequest.Create("http://www.medellin.gov.co/GEOCOD_WEB/servicios/reverseGeoCodUTF?coordX=-75.5831534&coordY=6.2392499&radioBusqueda=100&numRegistros=1");
            //req.Method = "GET";
            //req.Headers["Authorization"] = Convert.ToBase64String(Encoding.Default.GetBytes("metropolgeorever:Metropol0504**2"));
            //req.Credentials = new NetworkCredential("metropolgeorever", "Metropol0504**2");
            //HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

            }
            catch (Exception e)
            {
                WebRequest req = WebRequest.Create("http://www.medellin.gov.co/GEOCOD_WEB/servicios/reverseGeoCodUTF?coordX=-75.5831534&coordY=6.2392499&radioBusqueda=100&numRegistros=1");
                req.Method = "GET";
                req.Headers["Authorization"] =Convert.ToBase64String(Encoding.Default.GetBytes("metropolgeorever:Metropol0504**2"));
                req.Credentials = new NetworkCredential("metropolgeorever", "Metropol0504**2");
                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            }
          
            return View();
        }

        public HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://www.medellin.gov.co/GEOCOD_WEB/reverseGeocodService");
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
       
        public ActionResult abrirUbicacion()
        {

            return View();
        }

         public ActionResult abrir()
        {

            return View();
        }
        public ActionResult Reubicar()
        {
            return Content("");
        }
    }
}