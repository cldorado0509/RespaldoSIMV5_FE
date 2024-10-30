using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using SIM.Areas.REDRIO.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace SIM.Areas.REDRIO.Controllers
{
[RoutePrefix("api/REDRIOApi")]
    public class REDRIOApiController : ApiController
    {
        public REDRIOApiController()
        {
            System.Diagnostics.Debug.WriteLine("REDRIOApiController inicializado");
        }

        private readonly string apiUrl = "http://localhost:5078";

        [HttpGet, ActionName("ObtenerMunicipios")]
        public async Task<IHttpActionResult> ObtenerMunicipios()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"{apiUrl}/api/Municipio/ObtenerMunicipios");
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                    
                    var result = JsonConvert.DeserializeObject<ResultadoMunicipios>(jsonResponse);

                    if (result == null || result.result == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Error al deserializar: El resultado es nulo.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Municipios obtenidos: {result.result.Count}");
                    }

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }


        [HttpGet, ActionName("ObtenerCampañas")]
        public async Task<IHttpActionResult> ObtenerCampañas()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"{apiUrl}/api/Campaña/ObtenerCampañas");
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                    
                    var result = JsonConvert.DeserializeObject<ResultadoCampañas>(jsonResponse);

                    if (result == null || result.result == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Error al deserializar: El resultado es nulo.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Campañas obtenidos: {result.result.Count}");
                    }

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }
        
        [HttpPost, ActionName("AgregarCampaña")]
        public async Task<IHttpActionResult> AgregarCampaña([FromBody] Campaña nuevaCampaña)
                {
                    if (nuevaCampaña == null)
                    {
                        return BadRequest("La campaña no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonCampaña = JsonConvert.SerializeObject(nuevaCampaña);
                            var content = new StringContent(jsonCampaña, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/Campaña/AgregarCampaña", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);

                            return Ok(new { isSuccess = true, message = "Campaña agregada exitosamente." });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
            
         
        [HttpPut]
        [Route("EditarCampaña/{id}")]
        public async Task<IHttpActionResult> EditarCampaña(int id, [FromBody] Campaña campanaEditada)
        {
            if (campanaEditada == null)
            {
                return BadRequest("La campaña no puede ser nula.");
            }

            if (id <= 0)
            {
                return BadRequest("El ID de la campaña no es válido.");
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(campanaEditada);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"{apiUrl}/api/Campaña/ActualizarCampaña/{id}", content);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResultadoCampañas>(jsonResponse);

                    return Ok(new { isSuccess = true, message = "Campaña editada exitosamente.", result });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al editar campaña: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("EliminarCampana/{id}")]
        public async Task<IHttpActionResult> EliminarCampana(int id)

        {
            if (id <= 0)
            {
                return BadRequest("El ID de la campaña no es válido.");
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.DeleteAsync($"{apiUrl}/api/Campaña/EliminarCampaña/{id}");
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);

                    return Ok(new { isSuccess = true, message = "Campaña eliminada exitosamente." });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al eliminar campaña: " + ex.Message);
                return InternalServerError(ex);
            }
        }

         [HttpGet, ActionName("ObtenerFases")]
        public async Task<IHttpActionResult> ObtenerFases()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"{apiUrl}/api/Fase/ObtenerFases");
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                    
                    var result = JsonConvert.DeserializeObject<ResultadoFases>(jsonResponse);

                    if (result == null || result.result == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Error al deserializar: El resultado es nulo.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Fases obtenidos: {result.result.Count}");
                    }

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en ObtenerFases: " + ex.ToString());
                return InternalServerError(ex);
                System.Diagnostics.Debug.WriteLine($"Llamando a la URL: {apiUrl}/api/Fase/ObtenerFases");

            }

        }
    
        [HttpGet]
        [ActionName("ObtenerEstacionPorCodigo")]
        public async Task<IHttpActionResult> ObtenerEstacionPorCodigo(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                return BadRequest("El código de la estación no puede ser nulo o vacío.");
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"Llamando a la URL: {apiUrl}/api/Estacion/ObtenerEstacionCodigo/{codigo}");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"{apiUrl}/api/Estacion/ObtenerEstacionCodigo/{codigo}");
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);

                    var result = JsonConvert.DeserializeObject<Estacion>(jsonResponse);

                    if (result == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Error al deserializar: El resultado es nulo.");
                        return NotFound();
                    }

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

            [HttpPost, ActionName("AgregarInsitu")]
        public async Task<IHttpActionResult> AgregarInsitu([FromBody] Insitu nuevaInsitu)
        {
            if (nuevaInsitu == null)
            {
                return BadRequest("La Insitu no puede ser nula.");
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsonInsitu = JsonConvert.SerializeObject(nuevaInsitu);
                    var content = new StringContent(jsonInsitu, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/Insitu/AgregarInsitu", content);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);

                    var resultadoBackend = JsonConvert.DeserializeObject<ResponseInsitu>(jsonResponse);

                    if (resultadoBackend == null || resultadoBackend.result == null)
                    {
                        return NotFound(); 
                    }

                    return Ok(new 
                    { 
                        isSuccess = true, 
                        message = "Insitu agregada exitosamente.", 
                        result = resultadoBackend.result 
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpPost, ActionName("AgregarFisico")]
                public async Task<IHttpActionResult> AgregarFisico([FromBody] Fisico nuevaFisico)
                {
                    if (nuevaFisico == null)
                    {
                        return BadRequest("La fisico no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonFisico = JsonConvert.SerializeObject(nuevaFisico);
                            var content = new StringContent(jsonFisico, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/fisico/AgregarFisico", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                            var resultadoBackend = JsonConvert.DeserializeObject<ResponseFisico>(jsonResponse);

                            if (resultadoBackend == null || resultadoBackend.result == null)
                            {
                                return NotFound(); 
                            }


                            return Ok(new { isSuccess = true, message = "fisico agregada exitosamente.",  result = resultadoBackend.result });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
      
        [HttpPost, ActionName("AgregarQuimico")]
                public async Task<IHttpActionResult> AgregarQuimico([FromBody] Quimico nuevaQuimico)
                {
                    if (nuevaQuimico == null)
                    {
                        return BadRequest("La Quimico no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonQuimico = JsonConvert.SerializeObject(nuevaQuimico);
                            var content = new StringContent(jsonQuimico, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/Quimico/AgregarQuimico", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                            var resultadoBackend = JsonConvert.DeserializeObject<ResponseQuimico>(jsonResponse);

                            if (resultadoBackend == null || resultadoBackend.result == null)
                            {
                                return NotFound(); 
                            }


                            return Ok(new { isSuccess = true, message = "Quimico agregada exitosamente." , result = resultadoBackend.result});
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }

        [HttpPost, ActionName("AgregarNutriente")]
                public async Task<IHttpActionResult> AgregarNutriente([FromBody] Nutriente nuevaNutriente)
                {
                    if (nuevaNutriente == null)
                    {
                        return BadRequest("La Nutriente no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonNutriente = JsonConvert.SerializeObject(nuevaNutriente);
                            var content = new StringContent(jsonNutriente, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/Nutriente/AgregarNutriente", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                            var resultadoBackend = JsonConvert.DeserializeObject<ResponseNutriente>(jsonResponse);

                            if (resultadoBackend == null || resultadoBackend.result == null)
                            {
                                return NotFound(); 
                            }


                            return Ok(new { isSuccess = true, message = "Nutriente agregada exitosamente.", result = resultadoBackend.result});
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
        
        [HttpPost, ActionName("AgregarMetal")]
                public async Task<IHttpActionResult> AgregarMetal([FromBody] MetalAgua nuevaMetalAgua)
                {
                    if (nuevaMetalAgua == null)
                    {
                        return BadRequest("La MetalAgua no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonMetalAgua = JsonConvert.SerializeObject(nuevaMetalAgua);
                            var content = new StringContent(jsonMetalAgua, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/MetalAgua/AgregarMetal", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                            var resultadoBackend = JsonConvert.DeserializeObject<ResponseMetalAgua>(jsonResponse);

                            if (resultadoBackend == null || resultadoBackend.result == null)
                            {
                                return NotFound(); 
                            }


                            return Ok(new { isSuccess = true, message = "MetalAgua agregada exitosamente.", result = resultadoBackend.result });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
        
        [HttpPost, ActionName("AgregarMetalSedimental")]
                public async Task<IHttpActionResult> AgregarMetalSedimental([FromBody] MetalSedimental nuevaMetalSedimental)
                {
                    if (nuevaMetalSedimental == null)
                    {
                        return BadRequest("La MetalSedimental no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonMetalSedimental = JsonConvert.SerializeObject(nuevaMetalSedimental);
                            var content = new StringContent(jsonMetalSedimental, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/MetalSedimental/AgregarMetalSedimental", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                            var resultadoBackend = JsonConvert.DeserializeObject<ResponseMetalSedimental>(jsonResponse);

                            if (resultadoBackend == null || resultadoBackend.result == null)
                            {
                                return NotFound(); 
                            }

                            return Ok(new { isSuccess = true, message = "MetalSedimental agregada exitosamente.", result = resultadoBackend.result });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
        
        [HttpPost, ActionName("AgregarBiologico")]
                public async Task<IHttpActionResult> AgregarBiologico([FromBody] Biologico nuevaBiologico)
                {
                    if (nuevaBiologico == null)
                    {
                        return BadRequest("La Biologico no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonBiologico = JsonConvert.SerializeObject(nuevaBiologico);
                            var content = new StringContent(jsonBiologico, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/Biologico/AgregarBiologico", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);

                            var resultadoBackend = JsonConvert.DeserializeObject<ResponseBiologico>(jsonResponse);

                            if (resultadoBackend == null || resultadoBackend.result == null)
                            {
                                return NotFound(); 
                            }

                            return Ok(new { isSuccess = true, message = "Biologico agregada exitosamente.", result = resultadoBackend.result });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
        
        [HttpPost, ActionName("AgregarResultadoCampo")]
                public async Task<IHttpActionResult> AgregarResultadoCampo([FromBody] ResultadoCampo nuevaResultadoCampo)
                {
                    if (nuevaResultadoCampo == null)
                    {
                        return BadRequest("La ResultadoCampo no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonResultadoCampo = JsonConvert.SerializeObject(nuevaResultadoCampo);
                            var content = new StringContent(jsonResultadoCampo, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/ResultadoCampo/AgregarResultadoLaboratorio", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                            var resultadoBackend = JsonConvert.DeserializeObject<ResponseCampo>(jsonResponse);

                            if (resultadoBackend == null || resultadoBackend.result == null)
                            {
                                return NotFound(); 
                            }

                            return Ok(new { isSuccess = true, message = "ResultadoCampo agregada exitosamente.", result = resultadoBackend.result });
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
        
        [HttpPost, ActionName("AgregarReporteLaboratorio")]
                public async Task<IHttpActionResult> AgregarReporteLaboratorio([FromBody] ReporteLaboratorio nuevaReporteLaboratorio)
                {
                    if (nuevaReporteLaboratorio == null)
                    {
                        return BadRequest("La ReportesLaboratorio no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonReporteLaboratorio = JsonConvert.SerializeObject(nuevaReporteLaboratorio);
                            var content = new StringContent(jsonReporteLaboratorio, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/ReportesLaboratorio/AgregarReporteLaboratorio", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                            var resultadoBackend = JsonConvert.DeserializeObject<ResponseReporteLaboratorio>(jsonResponse);

                            if (resultadoBackend == null || resultadoBackend.result == null)
                            {
                                return NotFound(); 
                            }

                            return Ok(new { isSuccess = true, message = "ReportesLaboratorio agregada exitosamente.", result = resultadoBackend.result });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
        
        [HttpPost, ActionName("AgregarMuestraCompuesta")]
                public async Task<IHttpActionResult> AgregarMuestraCompuesta([FromBody] MuestraCompuesta nuevaMuestraCompuesta)
                {
                    if (nuevaMuestraCompuesta == null)
                    {
                        return BadRequest("La MuestraCompuesta no puede ser nula.");
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var jsonMuestraCompuesta = JsonConvert.SerializeObject(nuevaMuestraCompuesta);
                            var content = new StringContent(jsonMuestraCompuesta, System.Text.Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync($"{apiUrl}/api/MuestraCompuesta/AgregarMuestraCompuesta", content);
                            response.EnsureSuccessStatusCode();

                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);
                            var resultadoBackend = JsonConvert.DeserializeObject<ResponseMuestraCompuesta>(jsonResponse);

                            if (resultadoBackend == null || resultadoBackend.result == null)
                            {
                                return NotFound(); 
                            }


                            return Ok(new { isSuccess = true, message = "MuestraCompuesta agregada exitosamente.", 
                            result = resultadoBackend.result
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
        
        
         
         [HttpGet, ActionName("ObtenerReporteLaboratorioPorCampaña")]
        [Route("ObtenerReporteLaboratorioPorCampaña/{id:int}")]
        public async Task<IHttpActionResult> ObtenerReporteLaboratorioPorCampaña(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El id de la campaña no es válido.");
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{apiUrl}/api/ReportesLaboratorio/ObtenerReporteLaboratorioPorCampaña/{id}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Respuesta JSON: " + jsonResponse);

                    var result = JsonConvert.DeserializeObject<ResultadoReporteLaboratorio>(jsonResponse);

                    if (result == null || result.result == null || !result.result.Any())
                    {
                        System.Diagnostics.Debug.WriteLine("No se encontraron reportes para la campaña.");
                        return NotFound();
                    }

                    System.Diagnostics.Debug.WriteLine($"Reportes de laboratorio obtenidos: {result.result.Count}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al obtener reportes de laboratorio: " + ex.Message);
                return InternalServerError(ex);
            }
        }
        
 [HttpPost, ActionName("AgregarHistorialExcel")]
        public async Task<IHttpActionResult> AgregarHistorialExcel()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("El tipo de contenido debe ser multipart/form-data.");
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                // Verifica que hay contenido recibido
                if (provider.Contents == null || !provider.Contents.Any())
                {
                    return BadRequest("No se recibieron datos.");
                }

                var formData = new MultipartFormDataContent();
                bool fileReceived = false;

                foreach (var content in provider.Contents)
                {
                    if (content.Headers.ContentDisposition.FileName != null) 
                    {
                        fileReceived = true; // Marcamos que hemos recibido un archivo

                        var filename = content.Headers.ContentDisposition.FileName.Trim('\"');
                        var fileBytes = await content.ReadAsByteArrayAsync();
                        var byteArrayContent = new ByteArrayContent(fileBytes);
                        byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = filename
                        };
                        formData.Add(byteArrayContent, "file", filename); // Asegúrate de que el nombre del campo sea correcto
                    }
                    else 
                    {
                        var formField = await content.ReadAsStringAsync();
                        var fieldName = content.Headers.ContentDisposition.Name.Trim('\"');
                        formData.Add(new StringContent(formField), fieldName);
                    }
                }

                // Verifica si se recibió al menos un archivo
                if (!fileReceived)
                {
                    return BadRequest("El archivo no se recibió.");
                }

                // Log del contenido que se va a enviar
                System.Diagnostics.Debug.WriteLine("Enviando al backend:");
                foreach (var item in formData)
                {
                    var fileName = item.Headers.ContentDisposition.FileName ?? "No file";
                    System.Diagnostics.Debug.WriteLine($"{item.Headers.ContentDisposition.Name}: {fileName}");
                }

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync($"{apiUrl}/api/HistorialExcel/AgregarHistorialExcel", formData);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(new { isSuccess = true, message = "HistorialExcel reenviado y agregado exitosamente." });
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine($"Error de backend: {errorContent}"); // Log de error
                        return BadRequest($"Error al reenviar los datos al backend: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }
            }

}
