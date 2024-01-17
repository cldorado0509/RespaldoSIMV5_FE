using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIM.Data;
using SIM.Data.Tramites;
using SIM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SIM.Areas.Correspondencia.Controllers
{
    public class CodApiController : ApiController
    {

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct datosRespuesta
        {
            public string tipoRespuesta; // OK, Error
            public string detalleRespuesta;
        }

        /// <summary>
        /// Variable de Contexto de Base de Datos para acceso al esquema General y Seguridad
        /// </summary>
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        EntitiesControlOracle dbControl = new EntitiesControlOracle();

        /// <summary>
        /// Consulta de Lista de Terceros con filtros y agrupación
        /// </summary>
        /// <param name="filter">lista de campos con valores de filtros en la consulta</param>
        /// <param name="sort">Campos de ordenamiento</param>
        /// <param name="group">Campos de agrupación</param>
        /// <param name="skip">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="take">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <param name="searchValue">Valor de Búsqueda</param>
        /// <param name="searchExpr">Campo de Búsqueda</param>
        /// <param name="comparation">Tipo de comparación en la búsqueda</param>
        /// <param name="tipoData">f: Carga consulta con todos los campos, r: Consulta con campos reducidos, l: Consulta con campos para ComboBox (LookUp)</param>
        /// <param name="noFilterNoRecords">Si es verdadero y no hay filtros, no retorna registros. Si es falso y no hay filtros, retorna todos los datos de acuerdo a la paginación</param>
        /// <returns>Registros resultado de la consulta</returns>
        [System.Web.Http.Authorize(Roles = "VCORRESPONDENCIA")]
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("COD")]
        public datosConsulta GetCOD(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) // || (!administrador && idTerceroUsuario == null))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from Cod in dbSIM.QRY_INDICESDESPACHADOS
                             select new
                             {
                                 Cod.CODTRAMITE,
                                 Cod.CODDOCUMENTO,
                                 Cod.RADICADO,
                                 Cod.FECHA_RADICADO,
                                 Cod.ASUNTO,
                                 Cod.DESTINATARIO,
                                 Cod.DIRECCION,
                                 Cod.CENTROCOSTO,
                                 SERVICIOSEL = dbSIM.CORRESPONDENCIASELECCION.Where(w => w.CODTRAMITE == Cod.CODTRAMITE && w.CODDOCUMENTO == Cod.CODDOCUMENTO).Select(s => s.S_TIPOSERVICIO).FirstOrDefault(),
                                 SERVICIO = (from Ser in dbSIM.CORRESPONDENCIAENV_DET
                                             join Ord in dbSIM.CORRESPONDENCIAENVIADA on Ser.ID_COD equals Ord.ID_COD
                                             where Ser.CODTRAMITE == Cod.CODTRAMITE && Ser.CODDOCUMENTO == Cod.CODDOCUMENTO
                                             select Ord.S_TIPOSERVICIO).FirstOrDefault(),
                                 DEVOLUCION = (from Ser in dbSIM.CORRESPONDENCIAENV_DET
                                               where Ser.CODTRAMITE == Cod.CODTRAMITE && Ser.CODDOCUMENTO == Cod.CODDOCUMENTO
                                               select Ser.S_DEVOLUCION == "1" ? "Si" : "").FirstOrDefault(),
                                 PROCESO = (from Tra in dbSIM.TBTRAMITE
                                            join Pro in dbSIM.TBPROCESO on Tra.CODPROCESO equals Pro.CODPROCESO
                                            where Tra.CODTRAMITE == Cod.CODTRAMITE
                                            select Pro.NOMBRE).FirstOrDefault()
                             });

                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchExpr"></param>
        /// <param name="comparation"></param>
        /// <param name="tipoData"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Ordenes")]
        public datosConsulta Getordenes(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from Ordenes in dbSIM.CORRESPONDENCIAENVIADA
                             select Ordenes);

                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCod"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchExpr"></param>
        /// <param name="comparation"></param>
        /// <param name="tipoData"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("DetalleOrden")]
        public datosConsulta GetDetalleorden(int IdCod, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from Ordenes in dbSIM.CORRESPONDENCIAENV_DET
                             where Ordenes.ID_COD == IdCod
                             select Ordenes);

                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }
        /// <summary>
        /// Almacena los datos de los documentos que corresponden con la orden especifica
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaCOD")]
        public object Post(DISTRIBCORRESP[] objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando COD" };
            try
            {
                decimal _IdCod = -1;
                if (objData[0].ORDENSERVICIO != null) _IdCod = decimal.Parse(objData[0].ORDENSERVICIO);
                CORRESPONDENCIAENVIADA item = new CORRESPONDENCIAENVIADA();
                var DelDet = (from Cod in dbSIM.CORRESPONDENCIAENV_DET
                              where Cod.ID_COD == _IdCod
                              select Cod);
                if (DelDet.Count() > 0)
                {
                    dbSIM.CORRESPONDENCIAENV_DET.RemoveRange(DelDet);
                    dbSIM.SaveChanges();
                    _IdCod = decimal.Parse(objData[0].ORDENSERVICIO);
                }
                else
                {
                    item.D_FECHA = DateTime.Parse(objData[0].FECHATIPOSERV.ToString());
                    item.S_TIPOSERVICIO = objData[0].TIPOSERVICIO.ToString();
                    dbSIM.CORRESPONDENCIAENVIADA.Add(item);
                    dbSIM.SaveChanges();
                    _IdCod = item.ID_COD;
                }
                CORRESPONDENCIAENV_DET itemDet = new CORRESPONDENCIAENV_DET();
                foreach (DISTRIBCORRESP cod in objData)
                {
                    itemDet.CODDOCUMENTO = cod.CODTRAMITE != null ? decimal.Parse(cod.CODDOCUMENTO) : 0;
                    itemDet.CODTRAMITE = cod.CODDOCUMENTO != null ? decimal.Parse(cod.CODTRAMITE) : 0;
                    itemDet.ID_COD = _IdCod;
                    itemDet.N_PESO = decimal.Parse(cod.PESO);
                    itemDet.S_CIUDAD = cod.CIUDAD;
                    itemDet.S_CONTENIDO = cod.CONTENIDO;
                    itemDet.S_DESTINATARIO = cod.DESTINATARIO;
                    itemDet.S_DIRECCION = cod.DIRECCION;
                    itemDet.S_OBSERVACIONES = cod.OBSERVACIONES;
                    itemDet.S_REFERENCIA = cod.REFERENCIA;
                    dbSIM.CORRESPONDENCIAENV_DET.Add(itemDet);
                    dbSIM.SaveChanges();
                    itemDet = new CORRESPONDENCIAENV_DET();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Orden de servicio almacenada correctamente" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Orden"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize(Roles = "VORDENSERVICIO")]
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaOrden")]
        public object PostOrden(ORDENSERVICIO Orden)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando la Orden de Servicio" };
            try
            {
                if (Orden.IDCOD > 0)
                {
                    var modelDat = (from ModOrden in dbSIM.CORRESPONDENCIAENVIADA
                                    where ModOrden.ID_COD == Orden.IDCOD
                                    select ModOrden).FirstOrDefault();
                    if (modelDat != null)
                    {
                        modelDat.D_FECHA = Orden.FECHA;
                        modelDat.S_TIPOSERVICIO = Orden.TIPOSERVICIO;
                        modelDat.S_ORDEN = Orden.ORDEN;
                        dbSIM.Entry(modelDat).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Orden de servicio almacenada correctamente" };
        }
        /// <summary>
        /// Recibe los documentos anexos para la Orden de servicio
        /// </summary>
        /// <param name="IDOrden">Identificador de la PQRSD para relacionara</param>
        /// <returns></returns>
        [System.Web.Http.Authorize(Roles = "VORDENSERVICIO")]
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("SubirSoportes")]
        public async Task<object> SubirSoportesOrden(long IDOrden)
        {
            if (!Request.Content.IsMimeMultipartContent())
                return new { resp = "Error", mensaje = "No se encontraron archivos" };
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("RutaDocsCorrespondencia").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("RutaDocsCorrespondencia").ToString() : "";

            var provider = new CODMultipartFormDataStreamProvider(_RutaBase);
            long ID = IDOrden;
            if (ID > 0)
            {
                DateTime FechaOrden = (from ModOrden in dbSIM.CORRESPONDENCIAENVIADA
                                       where ModOrden.ID_COD == ID
                                       select ModOrden.D_FECHA).FirstOrDefault();
                if (FechaOrden.Year > 1900)
                {
                    try
                    {
                        await Request.Content.ReadAsMultipartAsync(provider);
                        string RutaArchivos = _RutaBase + FechaOrden.ToString("yyyyMM") + @"\";
                        if (!Directory.Exists(RutaArchivos)) Directory.CreateDirectory(RutaArchivos);
                        string _RutaOrigen = "";
                        string _RutaDestino = "";
                        foreach (MultipartFileData _File in provider.FileData)
                        {
                            _RutaOrigen = _RutaBase + _File.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                            string _Ext = Path.GetExtension(_File.LocalFileName);
                            byte[] _FileContent = SIM.Utilidades.Archivos.LeeArchivo(_RutaOrigen);
                            System.IO.File.Delete(_RutaOrigen);

                            _RutaDestino = RutaArchivos + ID.ToString() + ".pdf";
                            return SubeSoportePdf(_RutaDestino, _FileContent, _Ext);
                        }
                        return new { resp = "OK", mensaje = "Archivos subidos con exito" };
                    }
                    catch (Exception e)
                    {
                        return new { resp = "Error", mensaje = "Ocurrio un problema al subir uno de los archivos " + e.Message };
                    }
                }
                else return new { resp = "Error", mensaje = "Ocurrio un problema al subir uno de los archivos... No se encontró la orden de servicio" };
            }
            else return new { resp = "Error", mensaje = "Ocurrio un problema al subir uno de los archivos... No ha seleccionado la orden de servicio" };
        }
        /// <summary>
        /// Obtiene las ordenes filtradas
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchExpr"></param>
        /// <param name="comparation"></param>
        /// <param name="tipoData"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ConsultaOrden")]
        public datosConsulta ConsultaOrden(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) // || (!administrador && idTerceroUsuario == null))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from Detalle in dbSIM.CORRESPONDENCIAENV_DET
                             join Corresp in dbSIM.CORRESPONDENCIAENVIADA
                             on Detalle.ID_COD equals Corresp.ID_COD
                             select new
                             {
                                 Detalle.S_REFERENCIA,
                                 Detalle.S_DESTINATARIO,
                                 Detalle.CODTRAMITE,
                                 Detalle.CODDOCUMENTO,
                                 Corresp.ID_COD,
                                 Corresp.S_ORDEN,
                                 Corresp.D_FECHA
                             });

                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }
        /// <summary>
        /// Metodo para subir docuemnto anexo al servidor 
        /// </summary>
        /// <param name="_IdPqrsd">Identificador del lado del cliente de la PQRSD</param>
        /// <param name="Archivo">Arreglo de bytes con el archivo que se anexará a la PQRSD</param>
        /// <param name="Extension">Extension del archivo del anexo de la PQRSD (.doc,.docx,.pdf,.jpg,.png)</param>
        /// <returns></returns>
        [System.Web.Http.Authorize(Roles = "VORDENSERVICIO")]
        private object SubeSoportePdf(string _RutaDestino, byte[] Archivo, string Extension)
        {
            object _obj = null;
            try
            {
                if (Archivo.Length > 0)
                {
                    MemoryStream _SoportePdf = new MemoryStream();
                    using (RichEditDocumentServer server = new RichEditDocumentServer())
                    {
                        PdfExportOptions options = new PdfExportOptions();
                        switch (Extension.ToLower())
                        {
                            case ".doc":
                                server.LoadDocument(new MemoryStream(Archivo), DevExpress.XtraRichEdit.DocumentFormat.Doc);
                                options.Compressed = false;
                                options.ImageQuality = PdfJpegImageQuality.Highest;
                                server.ExportToPdf(_SoportePdf, options);
                                SIM.Utilidades.Archivos.GrabaMemoryStream(_SoportePdf, _RutaDestino);
                                _obj = new { resp = "OK", mensaje = "Archivos subidos con exito" };
                                break;
                            case ".docx":
                                server.LoadDocument(new MemoryStream(Archivo), DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
                                options.Compressed = false;
                                options.ImageQuality = PdfJpegImageQuality.Highest;
                                server.ExportToPdf(_SoportePdf, options);
                                SIM.Utilidades.Archivos.GrabaMemoryStream(_SoportePdf, _RutaDestino);
                                _obj = new { resp = "OK", mensaje = "Archivos subidos con exito" };
                                break;
                            case ".pdf":
                                _SoportePdf = new MemoryStream(Archivo);
                                SIM.Utilidades.Archivos.GrabaMemoryStream(_SoportePdf, _RutaDestino);
                                _obj = new { resp = "OK", mensaje = "Archivos subidos con exito" };
                                break;
                            case ".jpg":
                            case ".png":
                                DevExpress.XtraRichEdit.API.Native.DocumentImage docImage = server.Document.Images.Append(DevExpress.XtraRichEdit.API.Native.DocumentImageSource.FromStream(new MemoryStream(Archivo)));
                                server.Document.Sections[0].Page.Width = docImage.Size.Width + server.Document.Sections[0].Margins.Right + server.Document.Sections[0].Margins.Left;
                                server.Document.Sections[0].Page.Height = docImage.Size.Height + server.Document.Sections[0].Margins.Top + server.Document.Sections[0].Margins.Bottom;
                                server.ExportToPdf(_SoportePdf);
                                SIM.Utilidades.Archivos.GrabaMemoryStream(_SoportePdf, _RutaDestino);
                                _obj = new { resp = "OK", mensaje = "Archivos subidos con exito" };
                                break;
                            case ".tif":
                                _SoportePdf = SIM.Utilidades.Archivos.TifToPDFDE(new MemoryStream(Archivo));
                                SIM.Utilidades.Archivos.GrabaMemoryStream(_SoportePdf, _RutaDestino);
                                _obj = new { resp = "OK", mensaje = "Archivos subidos con exito" };
                                break;
                        }
                    }
                    _SoportePdf.Dispose();
                }
                else
                {
                    _obj = new { resp = "Error", mensaje = "No se recibió el contenido del archivo!" };
                }
            }
            catch (Exception ex)
            {
                _obj = new { resp = "Error", mensaje = "Ocurrió un error en la subida del anexo. " + ex.Message };
            }
            return _obj;
        }
        /// <summary>
        /// Consulta los documentos enviados en las diferentes ordenes de servicio
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchExpr"></param>
        /// <param name="comparation"></param>
        /// <param name="tipoData"></param>
        /// <param name="noFilterNoRecords"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("DevolucionCOD")]
        public datosConsulta ConsultaDevolucionCOD(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            datosConsulta resultado = new datosConsulta();
            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords)) // || (!administrador && idTerceroUsuario == null))
            {
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                var model = (from Detalle in dbSIM.CORRESPONDENCIAENV_DET
                             join Corresp in dbSIM.CORRESPONDENCIAENVIADA
                             on Detalle.ID_COD equals Corresp.ID_COD
                             select new
                             {
                                 Detalle.S_REFERENCIA,
                                 Detalle.S_DESTINATARIO,
                                 Detalle.CODTRAMITE,
                                 Detalle.CODDOCUMENTO,
                                 Detalle.ID_CODDET,
                                 Detalle.S_NOVEDAD,
                                 DEVOLUCION = (Detalle.S_DEVOLUCION == "1" ? "Si" : "No"),
                                 Detalle.D_FECHADEV,
                                 Corresp.ID_COD,
                                 Corresp.S_ORDEN,
                                 Corresp.D_FECHA
                             });

                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                else resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();
                return resultado;
            }
        }
        /// <summary>
        /// Guarda la devolucion del documento
        /// </summary>
        /// <param name="Devolucion"></param>
        /// <returns></returns>
        [System.Web.Http.Authorize(Roles = "VORDENSERVICIO")]
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaDevolucion")]
        public object PostDevolucion(DEVOLUCION Devolucion)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando la devolución del documento" };
            try
            {
                if (Devolucion.IDCODDET > 0)
                {
                    var modelDat = (from ModDocu in dbSIM.CORRESPONDENCIAENV_DET
                                    where ModDocu.ID_CODDET == Devolucion.IDCODDET
                                    select ModDocu).FirstOrDefault();
                    if (modelDat != null)
                    {
                        modelDat.S_NOVEDAD = Devolucion.MOTIVODEV;
                        modelDat.D_FECHADEV = Devolucion.FECHADEV;
                        modelDat.S_DEVOLUCION = "1";
                        dbSIM.Entry(modelDat).State = EntityState.Modified;
                        dbSIM.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Orden de servicio almacenada correctamente" };
        }
        /// <summary>
        /// Permite subir el documento de la devolucion al sistema de gestion documental de la entidad
        /// </summary>
        /// <param name="Codtramite">Codigo del tramite</param>
        /// <param name="Coddocumento">Codigo del documento</param>
        /// <returns></returns>
        [System.Web.Http.Authorize(Roles = "VORDENSERVICIO")]
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("SubirDevolucion")]
        //public async Task<object> SubirDocumentoDevolucion(long Codtramite, long Coddocumento, int Pag)
        public async Task<object> SubirDocumentoDevolucion(long Codtramite, long Coddocumento)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idCodDocumento;
            long idUsuario = 19522;
            long codFuncionario = 4005;
            SIM.Utilidades.Cryptografia crypt = new Utilidades.Cryptografia();
            if (!Request.Content.IsMimeMultipartContent())
                return new { resp = "Error", mensaje = "No se encontraron archivos" };
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
            string _CodSerieDev = SIM.Utilidades.Data.ObtenerValorParametro("IdSerieDevoluciones").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("IdSerieDevoluciones").ToString() : "";
            var provider = new CODMultipartFormDataStreamProvider(_RutaBase);
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
                codFuncionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(dbControl, idUsuario);
            }
            else codFuncionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(dbControl, idUsuario);

            if (Codtramite > 0)
            {
                try
                {
                    string _Ruta = "";
                    var modelPro = (from ModTra in dbSIM.TBTRAMITE
                                    where ModTra.CODTRAMITE == Codtramite
                                    select new
                                    {
                                        ModTra.CODPROCESO
                                    }).FirstOrDefault();
                    if (modelPro.CODPROCESO > 0)
                    {
                        var modelRuta = (from ModRuta in dbSIM.TBRUTAPROCESO
                                         where ModRuta.CODPROCESO == modelPro.CODPROCESO
                                         select new
                                         {
                                             ModRuta.PATH
                                         }).FirstOrDefault();
                        _Ruta = modelRuta.PATH;
                    }

                    if (_Ruta != "")
                    {
                        await Request.Content.ReadAsMultipartAsync(provider);
                        byte[] _FileContent = null;
                        string _Extension = "";
                        foreach (MultipartFileData _File in provider.FileData)
                        {
                            string _RutaOrigen = _RutaBase + _File.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                            _Extension = Path.GetExtension(_RutaOrigen);
                            _FileContent = SIM.Utilidades.Archivos.LeeArchivo(_RutaOrigen);
                            System.IO.File.Delete(_RutaOrigen);
                        }
                        if (_FileContent.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream(_FileContent);
                            if (_Extension == ".tif")
                            {
                                ms = new MemoryStream(SIM.Utilidades.Archivos.TifToPDF(ms).ToArray());
                            }
                            TBTRAMITEDOCUMENTO ultimoDocumento = dbSIM.TBTRAMITEDOCUMENTO.Where(td => td.CODTRAMITE == Codtramite).OrderByDescending(td => td.CODDOCUMENTO).FirstOrDefault();

                            if (ultimoDocumento == null)
                                idCodDocumento = 1;
                            else
                                idCodDocumento = Convert.ToInt32(ultimoDocumento.CODDOCUMENTO) + 1;

                            string rutaDocumento = _Ruta + "\\" + SIM.Utilidades.Archivos.GetRutaDocumento(Convert.ToUInt64(Codtramite), 100) + Codtramite.ToString("0") + "-" + idCodDocumento.ToString() + ".pdf";
                            FileInfo _ArchSubido = new FileInfo(rutaDocumento);
                            if (!_ArchSubido.Directory.Exists) _ArchSubido.Directory.Create();
                            if (ms.Length > 0)
                            //if (crypt.Encriptar(ms, _ArchSubido.FullName, UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ"), UnicodeEncoding.ASCII.GetBytes("ABCDEFGHIJKLMNOQ")))
                            {
                                Utilidades.Cryptografia.GrabaMemoryStream(ms, _ArchSubido.FullName);
                                TBTRAMITEDOCUMENTO documento = new TBTRAMITEDOCUMENTO();
                                TBTRAMITE_DOC relDocTra = new TBTRAMITE_DOC();
                                documento.CODDOCUMENTO = idCodDocumento;
                                documento.CODTRAMITE = Codtramite;
                                documento.TIPODOCUMENTO = 2;
                                documento.FECHACREACION = DateTime.Now;
                                documento.CODFUNCIONARIO = codFuncionario;
                                documento.ID_USUARIO = codFuncionario;
                                documento.RUTA = rutaDocumento;
                                documento.MAPAARCHIVO = "M";
                                documento.MAPABD = "M";
                                documento.PAGINAS = 1;
                                documento.CIFRADO = "0";
                                documento.CODSERIE = int.Parse(_CodSerieDev);
                                dbSIM.Entry(documento).State = System.Data.Entity.EntityState.Added;
                                dbSIM.SaveChanges();
                                relDocTra.CODTRAMITE = Codtramite;
                                relDocTra.CODDOCUMENTO = idCodDocumento;
                                relDocTra.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                dbSIM.Entry(relDocTra).State = System.Data.Entity.EntityState.Added;
                                dbSIM.SaveChanges();
                                var Indices = (from ModInd in dbSIM.TBINDICEDOCUMENTO
                                               where ModInd.CODTRAMITE == Codtramite && ModInd.CODDOCUMENTO == Coddocumento
                                               orderby ModInd.CODINDICE
                                               select ModInd).ToList();
                                TBINDICEDOCUMENTO IndDev = new TBINDICEDOCUMENTO();
                                int _SerieDevoluciones = int.Parse(_CodSerieDev);
                                var DefIndDev = (from ModIndDev in dbSIM.TBINDICESERIE
                                                 where ModIndDev.CODSERIE == _SerieDevoluciones
                                                 select ModIndDev).ToList();
                                var Devolucion = (from detCOD in dbSIM.CORRESPONDENCIAENV_DET
                                                  join COD in dbSIM.CORRESPONDENCIAENVIADA on detCOD.ID_COD equals COD.ID_COD
                                                  where detCOD.CODTRAMITE == Codtramite && detCOD.CODDOCUMENTO == Coddocumento
                                                  select detCOD).FirstOrDefault();
                                DateTime _FechaRadicado = new DateTime();
                                foreach (TBINDICESERIE Ind in DefIndDev)
                                {
                                    switch (Ind.CODINDICE)
                                    {
                                        case 506:
                                            IndDev.VALOR = Indices.Where(v => v.CODINDICE == 154).Select(v => v.VALOR).FirstOrDefault();
                                            //IndDev.FECHAREGISTRO = DateTime.Now;
                                            IndDev.CODINDICE = 506;
                                            IndDev.CODSERIE = int.Parse(_CodSerieDev);
                                            IndDev.CODTRAMITE = (int)Codtramite;
                                            IndDev.CODDOCUMENTO = idCodDocumento;
                                            IndDev.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                            dbSIM.Entry(IndDev).State = EntityState.Added;
                                            dbSIM.SaveChanges();
                                            break;
                                        case 507:
                                            IndDev.VALOR = Indices.Where(v => v.CODINDICE == 51).Select(v => v.VALOR).FirstOrDefault();
                                            //IndDev.FECHAREGISTRO = DateTime.Now;
                                            IndDev.CODINDICE = 507;
                                            IndDev.CODSERIE = int.Parse(_CodSerieDev);
                                            IndDev.CODTRAMITE = (int)Codtramite;
                                            IndDev.CODDOCUMENTO = idCodDocumento;
                                            IndDev.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                            dbSIM.Entry(IndDev).State = EntityState.Added;
                                            dbSIM.SaveChanges();
                                            break;
                                        case 508:
                                            IndDev.VALOR = Indices.Where(v => v.CODINDICE == 240).Select(v => v.VALOR).FirstOrDefault();
                                            _FechaRadicado = DateTime.Parse(IndDev.VALOR);
                                            //IndDev.FECHAREGISTRO = DateTime.Now;
                                            IndDev.CODINDICE = 508;
                                            IndDev.CODSERIE = int.Parse(_CodSerieDev);
                                            IndDev.CODTRAMITE = (int)Codtramite;
                                            IndDev.CODDOCUMENTO = idCodDocumento;
                                            IndDev.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                            dbSIM.Entry(IndDev).State = EntityState.Added;
                                            dbSIM.SaveChanges();
                                            break;
                                        case 509:
                                            IndDev.VALOR = Devolucion.D_FECHADEV.Value.ToString("dd-MM-yyyy");
                                            //IndDev.FECHAREGISTRO = DateTime.Now;
                                            IndDev.CODINDICE = 509;
                                            IndDev.CODSERIE = int.Parse(_CodSerieDev);
                                            IndDev.CODTRAMITE = (int)Codtramite;
                                            IndDev.CODDOCUMENTO = idCodDocumento;
                                            IndDev.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                            dbSIM.Entry(IndDev).State = EntityState.Added;
                                            dbSIM.SaveChanges();
                                            break;
                                        case 510:
                                            IndDev.VALOR = Indices.Where(v => v.CODINDICE == 54).Select(v => v.VALOR).FirstOrDefault();
                                            //IndDev.FECHAREGISTRO = DateTime.Now;
                                            IndDev.CODINDICE = 510;
                                            IndDev.CODSERIE = int.Parse(_CodSerieDev);
                                            IndDev.CODTRAMITE = (int)Codtramite;
                                            IndDev.CODDOCUMENTO = idCodDocumento;
                                            IndDev.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                            dbSIM.Entry(IndDev).State = EntityState.Added;
                                            dbSIM.SaveChanges();
                                            break;
                                        case 511:
                                            IndDev.VALOR = Indices.Where(v => v.CODINDICE == 53).Select(v => v.VALOR).FirstOrDefault();
                                            //IndDev.FECHAREGISTRO = DateTime.Now;
                                            IndDev.CODINDICE = 511;
                                            IndDev.CODSERIE = int.Parse(_CodSerieDev);
                                            IndDev.CODTRAMITE = (int)Codtramite;
                                            IndDev.CODDOCUMENTO = idCodDocumento;
                                            IndDev.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                            dbSIM.Entry(IndDev).State = EntityState.Added;
                                            dbSIM.SaveChanges();
                                            break;
                                        case 512:
                                            IndDev.VALOR = Indices.Where(v => v.CODINDICE == 55).Select(v => v.VALOR).FirstOrDefault();
                                            //IndDev.FECHAREGISTRO = DateTime.Now;
                                            IndDev.CODINDICE = 512;
                                            IndDev.CODSERIE = int.Parse(_CodSerieDev);
                                            IndDev.CODTRAMITE = (int)Codtramite;
                                            IndDev.CODDOCUMENTO = idCodDocumento;
                                            IndDev.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                            dbSIM.Entry(IndDev).State = EntityState.Added;
                                            dbSIM.SaveChanges();
                                            break;
                                        case 513:
                                            IndDev.VALOR = Devolucion.S_NOVEDAD;
                                            //IndDev.FECHAREGISTRO = DateTime.Now;
                                            IndDev.CODINDICE = 513;
                                            IndDev.CODSERIE = int.Parse(_CodSerieDev);
                                            IndDev.CODTRAMITE = (int)Codtramite;
                                            IndDev.CODDOCUMENTO = idCodDocumento;
                                            IndDev.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                            dbSIM.Entry(IndDev).State = EntityState.Added;
                                            dbSIM.SaveChanges();
                                            break;
                                        case 2029:
                                            IndDev.VALOR = Indices.Where(v => v.CODINDICE == 2021).Select(v => v.VALOR).FirstOrDefault();
                                            //IndDev.FECHAREGISTRO = DateTime.Now;
                                            IndDev.CODINDICE = 2029;
                                            IndDev.CODSERIE = int.Parse(_CodSerieDev);
                                            IndDev.CODTRAMITE = (int)Codtramite;
                                            IndDev.CODDOCUMENTO = idCodDocumento;
                                            IndDev.ID_DOCUMENTO = documento.ID_DOCUMENTO;
                                            dbSIM.Entry(IndDev).State = EntityState.Added;
                                            dbSIM.SaveChanges();
                                            break;
                                    }
                                }
                                var EstadoTramite = (from EstTra in dbSIM.TBTRAMITE
                                                     where EstTra.CODTRAMITE == Codtramite
                                                     select new
                                                     {
                                                         EstTra.ESTADO
                                                     }).FirstOrDefault();
                                var EstadoTarea = dbSIM.TBTRAMITETAREA.Where(tt => tt.CODTRAMITE == Codtramite).OrderByDescending(tt => tt.FECHAINI).FirstOrDefault();

                                if (EstadoTramite != null && EstadoTramite.ESTADO.Value == 0 && EstadoTarea != null && EstadoTarea.ESTADO.Value == 0)
                                {
                                    TBTRAMITES_BLOQUEADOS ttbloqueados = new TBTRAMITES_BLOQUEADOS();
                                    ttbloqueados.CODFUNCIONARIOBLOQUEO = (int)codFuncionario;
                                    ttbloqueados.CODSERIE = decimal.Parse(_CodSerieDev);
                                    ttbloqueados.CODTAREA = (int)EstadoTarea.CODTAREA;
                                    ttbloqueados.CODTRAMITE = (int)EstadoTarea.CODTRAMITE;
                                    ttbloqueados.FECHABLOQUEO = DateTime.Now;
                                    ttbloqueados.RADICADO = "N/A";
                                    ttbloqueados.FECHARADICADO = _FechaRadicado.Year > 1900 ? _FechaRadicado : DateTime.Now;
                                    ttbloqueados.OBSERVACIONES = "BLOQUEADO POR DEVOLUCION DE CORRESPONDENCIA";
                                    ttbloqueados.ORDEN = SIM.Utilidades.Data.ObtenerMaximoOrdenTramite((long)EstadoTarea.CODTRAMITE);
                                    dbSIM.Entry(ttbloqueados).State = EntityState.Added;
                                    dbSIM.SaveChanges();
                                }
                            }
                        }
                    }
                    else return new { resp = "Error", mensaje = "Ocurrio un problema al subir uno de los archivos... No se ha establecido una ruta para el tipo de trámite" };
                    return new { resp = "OK", mensaje = "Archivos subidos con exito" };
                }
                catch (Exception e)
                {
                    return new { resp = "Error", mensaje = "Ocurrio un problema al subir uno de los archivos " + e.Message };
                }
            }
            else return new { resp = "Error", mensaje = "Ocurrio un problema al subir uno de los archivos... No ha definido un código de trámite para subir el documento" };
        }
        /// <summary>
        /// Retorna lo datos de un a orden previamnete guardada a partir de un codigo de orden
        /// </summary>
        /// <param name="IdCod"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtenerDetalleOrden")]
        public JArray ObtieneDetalleOrden(long IdCod)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Cod in dbSIM.CORRESPONDENCIAENV_DET
                             join Corresp in dbSIM.CORRESPONDENCIAENVIADA
                             on Cod.ID_COD equals Corresp.ID_COD
                             where Cod.ID_COD == IdCod
                             select new
                             {
                                 Cod.CODTRAMITE,
                                 Cod.CODDOCUMENTO,
                                 DESTINATARIO = Cod.S_DESTINATARIO,
                                 DIRECCION = Cod.S_DIRECCION,
                                 CIUDAD = Cod.S_CIUDAD,
                                 PESO = Cod.N_PESO,
                                 OBSERVACIONES = Cod.S_OBSERVACIONES,
                                 REFERENCIA = Cod.S_REFERENCIA,
                                 CONTENIDO = Cod.S_CONTENIDO,
                                 FECHATIPOSERV = Corresp.D_FECHA,
                                 TIPOSERVICIO = Corresp.S_TIPOSERVICIO,
                                 ORDENSERVICIO = Corresp.ID_COD
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        /// <summary>
        /// Actualiza los datos de los documentos que corresponden con la orden especifica
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("ActualizaCOD")]
        public object ActualizaCOD(DISTRIBCORRESP[] objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando COD" };
            try
            {
                var DelDet = (from Cod in dbSIM.CORRESPONDENCIAENV_DET
                              where Cod.ID_COD == decimal.Parse(objData[0].ORDENSERVICIO)
                              select Cod);
                dbSIM.CORRESPONDENCIAENV_DET.RemoveRange(DelDet);
                dbSIM.SaveChanges();
                CORRESPONDENCIAENV_DET itemDet = new CORRESPONDENCIAENV_DET();
                foreach (DISTRIBCORRESP cod in objData)
                {
                    itemDet.CODDOCUMENTO = decimal.Parse(cod.CODDOCUMENTO);
                    itemDet.CODTRAMITE = decimal.Parse(cod.CODTRAMITE);
                    itemDet.ID_COD = decimal.Parse(cod.ORDENSERVICIO);
                    itemDet.N_PESO = decimal.Parse(cod.PESO);
                    itemDet.S_CIUDAD = cod.CIUDAD;
                    itemDet.S_CONTENIDO = cod.CONTENIDO;
                    itemDet.S_DESTINATARIO = cod.DESTINATARIO;
                    itemDet.S_DIRECCION = cod.DIRECCION;
                    itemDet.S_OBSERVACIONES = cod.OBSERVACIONES;
                    itemDet.S_REFERENCIA = cod.REFERENCIA;
                    dbSIM.CORRESPONDENCIAENV_DET.Add(itemDet);
                    dbSIM.SaveChanges();
                    itemDet = new CORRESPONDENCIAENV_DET();
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Orden de servicio almacenada correctamente" };
        }

        /// <summary>
        /// Guarda la seleccion del servicio para poder hacer la orden
        /// </summary>
        /// <param name="CodTramite"></param>
        /// <param name="CodDoc"></param>
        /// <param name="Seleccion"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaSel")]
        public object PostGuardaSel(decimal CodTramite, decimal CodDoc, string Seleccion)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando Servicio" };
            if (Seleccion != "")
            {
                try
                {
                    var seleccion = (from Sel in dbSIM.CORRESPONDENCIASELECCION
                                     where Sel.CODTRAMITE == CodTramite && Sel.CODDOCUMENTO == CodDoc
                                     select Sel).FirstOrDefault();
                    if (seleccion != null)
                    {
                        seleccion.S_TIPOSERVICIO = Seleccion;
                        dbSIM.SaveChanges();
                    }
                    else
                    {
                        CORRESPONDENCIASELECCION _Sel = new CORRESPONDENCIASELECCION();
                        _Sel.CODTRAMITE = CodTramite;
                        _Sel.CODDOCUMENTO = CodDoc;
                        _Sel.S_TIPOSERVICIO = Seleccion;
                        dbSIM.CORRESPONDENCIASELECCION.Add(_Sel);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
                }
            }
            return new { resp = "OK", mensaje = "Tipo de servicio seleccionado almacenado correctamente" };
        }
    }
    /// <summary>
    /// Estructura para recibir datos ingreso formato distribucion correspondencia
    /// </summary>
    public class DISTRIBCORRESP
    {
        public string CODTRAMITE { get; set; }
        public string CODDOCUMENTO { get; set; }
        public string DESTINATARIO { get; set; }
        public string DIRECCION { get; set; }
        public string CIUDAD { get; set; }
        public string PESO { get; set; }
        public string OBSERVACIONES { get; set; }
        public string REFERENCIA { get; set; }
        public string CONTENIDO { get; set; }
        public string FECHATIPOSERV { get; set; }
        public string TIPOSERVICIO { get; set; }
        public string ORDENSERVICIO { get; set; }
    }
    /// <summary>
    /// Estructura para recibir datos ingreso tipo de servicio
    /// </summary>
    public class ORDENSERVICIO
    {
        public int IDCOD { get; set; }
        public DateTime FECHA { get; set; }
        public string TIPOSERVICIO { get; set; }
        public string ORDEN { get; set; }
    }

    /// <summary>
    /// Estructura para recibir datos ingreso tipo de servicio
    /// </summary>
    public class DEVOLUCION
    {
        public int IDCODDET { get; set; }
        public DateTime FECHADEV { get; set; }
        public string MOTIVODEV { get; set; }
    }

    public class CODMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CODMultipartFormDataStreamProvider(string path)
            : base(path)
        {
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return name.Replace("\"", string.Empty);
        }
    }
}