using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Areas.Dynamics.Data;
using System.Collections.Generic;
using System.Web.Http;

namespace SIM.Areas.Dynamics.Controllers
{
    public class FacturasApiController : ApiController
    {
        DynamicsContext dbDynamics = new DynamicsContext();

        [HttpGet]
        [ActionName("ObtenerFacturas")]
        public JArray GetFacturas(string customFilters)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            List<ListadoFacturas> listadoFacturas = new List<ListadoFacturas>();
            if (customFilters != "" && customFilters != null)
            {
                string[] _Buscar = customFilters.Split(':');
                string _Sql = "";
                if (_Buscar.Length > 0)
                {
                    string[] Buscar;
                    switch (_Buscar[0])
                    {

                    }
                }
            }
        }
    }
}
