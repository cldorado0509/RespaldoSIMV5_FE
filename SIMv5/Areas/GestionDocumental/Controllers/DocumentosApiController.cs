using DevExpress.Web;
using Independentsoft.Office.Odf.Fields;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2S.Components.PDF4NET;
using O2S.Components.PDF4NET.PDFFile;
using SIM.Areas.Pqrsd.Models;
using SIM.Controllers;
using SIM.Data;
using SIM.Data.Seguridad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SIM.Areas.GestionDocumental.Controllers
{
    [Authorize]
    public class DocumentosApiController : ApiController
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        /// <summary>
        /// Consulta de Lista de Terceros con filtros y agrupación
        /// </summary>
        /// <param name="IdUnidadDoc">Para paginar. Indica a partir de qué registro debe cargar</param>
        /// <param name="Buscar">Para paginar. Indica cuantos registros debe cargar (tamaño de página)</param>
        /// <returns>Registros resultado de la consulta</returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("BuscarDoc")]
        public JArray GetBuscarDoc(int IdUnidadDoc, string Buscar)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            string _Sql = "SELECT ID_DOCUMENTO,CODTRAMITE,CODDOCUMENTO,'' AS NOMBRE,FECHACREACION FROM TRAMITES.TBTRAMITEDOCUMENTO WHERE ID_DOCUMENTO <= 0";
            var DocsEncontrados = dbSIM.Database.SqlQuery<DatosDocs>(_Sql);
            if (Buscar == "" || Buscar == null) return JArray.FromObject(DocsEncontrados, Js);
            try
            {
                if (Buscar != "" && Buscar != null)
                {
                    string[] _Buscar = Buscar.Split(';');
                    if (_Buscar.Length > 0)
                    {
                        dbSIM.Database.ExecuteSqlCommandAsync("ALTER SESSION SET NLS_COMP = LINGUISTIC");
                        dbSIM.Database.ExecuteSqlCommandAsync("ALTER SESSION SET NLS_SORT = 'SPANISH_AI'");
                        switch (_Buscar[0])
                        {
                            case "T":
                                _Sql = "SELECT DISTINCT DOC.ID_DOCUMENTO,DOC.CODTRAMITE,DOC.CODDOCUMENTO,SER.NOMBRE,DOC.FECHACREACION FROM TRAMITES.TBTRAMITEDOCUMENTO DOC INNER JOIN TRAMITES.TBSERIE SER ON DOC.CODSERIE = SER.CODSERIE WHERE DOC.CODTRAMITE= " + _Buscar[1].ToString().Trim() + " ORDER BY DOC.ID_DOCUMENTO DESC";
                                DocsEncontrados = dbSIM.Database.SqlQuery<DatosDocs>(_Sql);
                                break;
                            case "F":
                                var _condicion = _Buscar[1].ToString().Replace("\r\n", string.Empty);
                                //_condicion = _condicion.Replace("\\", "").Replace("\"", "");
                                _condicion = _condicion.Replace("[", "").Replace("]", "");
                                if (_condicion != null)
                                {

                                    _condicion = ProcesaWhereOracle(_condicion);
                                    if (_condicion != "")
                                    {
                                        _Sql = ObtenerSqlDocOracle(IdUnidadDoc, _condicion);
                                        DocsEncontrados = dbSIM.Database.SqlQuery<DatosDocs>(_Sql);
                                    }
                                    else
                                    {
                                        _Sql = ObtenerSqlDocOracle(IdUnidadDoc);
                                        DocsEncontrados = dbSIM.Database.SqlQuery<DatosDocs>(_Sql);
                                    }
                                    return JArray.FromObject(DocsEncontrados, Js);
                                }
                                break;
                            case "B":
                                _Sql = "SELECT DISTINCT DOC.ID_DOCUMENTO,DOC.CODTRAMITE,DOC.CODDOCUMENTO,SER.NOMBRE,DOC.FECHACREACION FROM TRAMITES.BUSQUEDA_DOCUMENTO BUS INNER JOIN TRAMITES.TBTRAMITEDOCUMENTO DOC ON BUS.COD_TRAMITE = DOC.CODTRAMITE AND BUS.COD_DOCUMENTO = DOC.CODDOCUMENTO INNER JOIN TRAMITES.TBSERIE SER ON BUS.COD_SERIE = SER.CODSERIE WHERE CONTAINS(BUS.S_INDICE, '%" + _Buscar[1].ToString().ToUpper().Trim() + "%') > 0 ORDER BY DOC.ID_DOCUMENTO DESC";
                                DocsEncontrados = dbSIM.Database.SqlQuery<DatosDocs>(_Sql);
                                break;
                            case "D":
                                string[] _rango = _Buscar[1].Split(',');
                                _Sql = $"SELECT DISTINCT DOC.ID_DOCUMENTO,DOC.CODTRAMITE,DOC.CODDOCUMENTO,SER.NOMBRE,DOC.FECHACREACION FROM TRAMITES.TBTRAMITEDOCUMENTO DOC INNER JOIN TRAMITES.TBSERIE SER ON DOC.CODSERIE = SER.CODSERIE WHERE DOC.CODSERIE ={IdUnidadDoc} AND TO_DATE(TO_CHAR(DOC.FECHACREACION,'DD-MM-YYYY'),'DD-MM-YYYY') BETWEEN TO_DATE('{DateTime.Parse(_rango[0].ToString()).ToString("dd-MM-yyyy")}','DD-MM-YYYY') AND TO_DATE('{DateTime.Parse(_rango[1].ToString()).ToString("dd-MM-yyyy")}','DD-MM-YYYY') ORDER BY DOC.ID_DOCUMENTO DESC";
                                DocsEncontrados = dbSIM.Database.SqlQuery<DatosDocs>(_Sql);
                                break;
                        }
                    }
                }
                else
                {
                    return JArray.FromObject(DocsEncontrados, Js);
                }
                return JArray.FromObject(DocsEncontrados, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private string ProcesaWhereOracle(string _Where)
        {
            ArrayList condicion = new ArrayList();
            string orderby = string.Empty;
            bool _And = true;

            if (_Where != null && _Where.Trim() != "")
            {
                _Where = _Where.Replace("\\", "").Replace("\"", "");
                _And = _Where.ToLower().Contains(",or,") ? false : true;
                string[] filtros = _Where.Split(',');
                for (int contFiltro = 0; contFiltro < filtros.Length; contFiltro += 4)
                {
                    filtros[contFiltro] = filtros[contFiltro].Contains(@"\") ? filtros[contFiltro].Replace(@"\", "") : filtros[contFiltro];
                    var esFecha = filtros[contFiltro].ToLower().Contains("fecha");
                    switch (filtros[contFiltro + 1])
                    {
                        case "contains":
                            condicion.Add(" INSTR(LOWER(\"" + filtros[contFiltro] + "\"),'" + filtros[contFiltro + 2].Trim().ToLower() + "') > 0");
                            break;
                        case "notcontains":
                            condicion.Add(" INSTR(LOWER(\"" + filtros[contFiltro] + "\"),'" + filtros[contFiltro + 2].Trim().ToLower() + "') = 0");
                            break;
                        case "startswith":
                            condicion.Add(" LOWER(\"" + filtros[contFiltro] + "\") LIKE '" + filtros[contFiltro + 2].Trim().ToLower() + "%'");
                            break;
                        case "endswith":
                            condicion.Add(" LOWER(\"" + filtros[contFiltro] + "\") LIKE '%" + filtros[contFiltro + 2].Trim().ToLower() + "'");
                            break;
                        case "=":
                            if (esFecha) condicion.Add("TO_DATE(\"" + filtros[contFiltro] + "\",'dd-MM-yyyy') = TO_DATE('" + DateTime.Parse(filtros[contFiltro + 2].Trim().ToLower()).ToString("dd-MM-yyyy") + "','dd-MM-yyyy')");
                            else condicion.Add("LOWER(\"" + filtros[contFiltro] + "\") = '" + filtros[contFiltro + 2].Trim().ToLower() + "'");
                            break;
                        case "<>":
                            if (esFecha) condicion.Add("TO_DATE(\"" + filtros[contFiltro] + "\",'dd-MM-yyyy') <> TO_DATE('" + DateTime.Parse(filtros[contFiltro + 2].Trim().ToLower()).ToString("dd-MM-yyyy") + "','dd-MM-yyyy')");
                            else condicion.Add("LOWER(\"" + filtros[contFiltro] + "\") <> '" + filtros[contFiltro + 2].Trim().ToLower() + "'");
                            break;
                        case "<":
                        case "<=":
                        case ">":
                        case ">=":
                            if (esFecha) condicion.Add("TO_DATE(\"" + filtros[contFiltro] + "\",'dd-MM-yyyy') " + filtros[contFiltro + 1] + " TO_DATE('" + DateTime.Parse(filtros[contFiltro + 2].Trim().ToLower()).ToString("dd-MM-yyyy") + "','dd-MM-yyyy')");
                            else condicion.Add("TO_NUMBER(LOWER(\"" + filtros[contFiltro] + "\")) " + filtros[contFiltro + 1] + " " + filtros[contFiltro + 2].Trim().ToLower());
                            break;
                    }
                }
            }

            if (condicion.Count > 0)
            {
                string _strCondicion = " AND ";
                if (!_And) _strCondicion = " OR ";
                return String.Join(_strCondicion, condicion.ToArray());
            }
            else return "";
        }

        public string ObtenerSqlDocOracle(int IdUnidadDocumental, string _criterio)
        {
            string _aux = "";
            decimal CodSerie = (decimal)IdUnidadDocumental;
            if (CodSerie > 0)
            {
                _aux = "SELECT DISTINCT QRY.ID_DOCUMENTO,QRY.CODTRAMITE,QRY.CODDOCUMENTO,(SELECT SER.NOMBRE FROM TRAMITES.TBSERIE SER WHERE QRY.CODSERIE = SER.CODSERIE) AS NOMBRE, QRY.FECHACREACION ";
                if (_criterio != "" && _criterio != null)
                {
                    string _codindices = "";
                    _aux += " FROM (SELECT DISTINCT IND.CODTRAMITE,IND.CODDOCUMENTO,";
                    var IndicesSerie = (from Ind in dbSIM.TBINDICESERIE
                                        where Ind.CODSERIE == CodSerie
                                        orderby Ind.INDICE
                                        select new
                                        {
                                            Ind.CODINDICE,
                                            Ind.INDICE,
                                            Ind.TIPO
                                        }).ToList();
                    if (IndicesSerie != null)
                    {
                        string _Indice = "";
                        foreach (var fila in IndicesSerie)
                        {
                            _Indice = fila.INDICE.ToUpper().Replace(" ", "");
                            if (_criterio.Contains(_Indice))
                            {
                                _aux += "(SELECT TID.VALOR FROM TRAMITES.TBINDICEDOCUMENTO TID WHERE TID.CODTRAMITE = IND.CODTRAMITE AND TID.CODDOCUMENTO=IND.CODDOCUMENTO AND TID.CODINDICE=" + fila.CODINDICE.ToString() + ") AS \"" + _Indice + "\",";
                                _codindices += fila.CODINDICE.ToString().Trim() + ",";
                            }
                        }
                    }
                    _aux += "(SELECT DOC.CODSERIE FROM TRAMITES.TBTRAMITEDOCUMENTO DOC WHERE DOC.CODTRAMITE = ind.codtramite AND DOC.CODDOCUMENTO = ind.coddocumento) AS CODSERIE, ";
                    _aux += "(SELECT DOC.ID_DOCUMENTO FROM TRAMITES.TBTRAMITEDOCUMENTO DOC WHERE DOC.CODTRAMITE = ind.codtramite AND DOC.CODDOCUMENTO = ind.coddocumento) AS ID_DOCUMENTO, ";
                    _aux += "(SELECT DOC.FECHACREACION FROM TRAMITES.TBTRAMITEDOCUMENTO DOC WHERE DOC.CODTRAMITE = ind.codtramite AND DOC.CODDOCUMENTO = ind.coddocumento) AS FECHACREACION ";
                    _codindices = _codindices.Substring(0, _codindices.Length - 1);
                    _aux += " FROM TRAMITES.TBINDICEDOCUMENTO IND WHERE IND.CODINDICE IN (" + _codindices + ")) QRY ";
                    _aux += " WHERE " + _criterio + " ORDER BY QRY.CODTRAMITE DESC, QRY.CODDOCUMENTO DESC";
                }
                else
                {
                    _aux += "FROM TRAMITES.TBTRAMITEDOCUMENTO QRY WHERE QRY.CODSERIE=" + CodSerie + " ORDER BY QRY.ID_DOCUMENTO DESC";
                }
            }
            return _aux;
        }

        public string ObtenerSqlDocOracle(int IdUnidadDocumental)
        {
            string _aux = "";
            decimal CodSerie = (decimal)IdUnidadDocumental;
            if (CodSerie > 0)
            {
                _aux = "SELECT DISTINCT QRY.ID_DOCUMENTO,QRY.CODTRAMITE,QRY.CODDOCUMENTO,(SELECT SER.NOMBRE FROM TRAMITES.TBSERIE SER WHERE QRY.CODSERIE = SER.CODSERIE) AS NOMBRE, QRY.FECHACREACION ";
                _aux += "FROM TRAMITES.TBTRAMITEDOCUMENTO QRY WHERE QRY.CODSERIE=" + CodSerie + " ORDER BY QRY.ID_DOCUMENTO DESC";
            }
            return _aux;
        }

        /// <summary>
        /// Retorna los documentos de un tramite
        /// </summary>
        /// <param name="IdDocumento">Codigo del trámite</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtieneDocumento")]
        public JObject GetObtieneDocumento(int IdDocumento)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                int idUsuario = 0;
                decimal funcionario = 0;
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);

                    funcionario = SIM.Utilidades.Security.Obtener_Codigo_Funcionario(idUsuario);
                }


                var model = (from Doc in dbSIM.TBTRAMITEDOCUMENTO
                             join Ser in dbSIM.TBSERIE on Doc.CODSERIE equals Ser.CODSERIE
                             where Doc.ID_DOCUMENTO == IdDocumento
                             orderby Doc.CODDOCUMENTO descending
                             select new Documento
                             {
                                 ID_DOCUMENTO = Doc.ID_DOCUMENTO,
                                 CODDOC = Doc.CODDOCUMENTO,
                                 SERIE = Ser.NOMBRE,
                                 FECHA = Doc.FECHACREACION.Value,
                                 ESTADO = Doc.S_ESTADO == "N" ? "Anulado" : "",
                                 ADJUNTO = Doc.S_ADJUNTO != "1" ? "No" : "Si",
                                 CODTRAMITE = Doc.CODTRAMITE,
                                 EDIT_INDICES = (from PER in dbSIM.PERMISOSSERIE where PER.CODFUNCIONARIO == funcionario &&
                                                 PER.CODSERIE == Doc.CODSERIE select PER.PM).FirstOrDefault() == "1" ? true : false
                             }).FirstOrDefault();
                if (model != null)
                {
                    if (model.ESTADO != "Anulado")
                    {
                        var estDoc = (from A in dbSIM.ANULACION_DOC
                                      join D in dbSIM.TRAMITES_PROYECCION on A.ID_PROYECCION_DOC equals D.ID_PROYECCION_DOC
                                      where D.CODTRAMITE == model.CODTRAMITE && D.CODDOCUMENTO == model.CODDOC && A.S_ESTADO == "P"
                                      select A.ID_ANULACION_DOC).FirstOrDefault();
                        if (estDoc > 0) model.ESTADO = "En proceso";
                    }
                }
                return JObject.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("RecibeArch")]
        public string RecibeArchAsync()
        {
            string _resp = "";
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            PDFFile oSrcPDF = null;
            var httpRequest = context.Request;
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
            string _Ruta = _RutaBase + @"\" + DateTime.Now.ToString("yyyyMM");
            if (!Directory.Exists(_Ruta)) Directory.CreateDirectory(_Ruta);
            var File = httpRequest.Files[0];
            if (File != null && File.ContentLength > 0)
            {
                string[] fileExtensions = { ".pdf" };
                var fileName = File.FileName.ToLower();
                var isValidExtenstion = fileExtensions.Any(ext =>
                {
                    return fileName.LastIndexOf(ext) > -1;
                });
                if (isValidExtenstion) {
                    string filePath = _Ruta + @"\" + fileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);    
                    }
                    File.SaveAs(filePath);
                    try
                    {
                        oSrcPDF = PDFFile.FromFile(filePath);
                        _resp = "Ok;" + fileName;
                    }
                    catch (Exception ex)
                    {
                        _resp = "Error;" + ex.Message;
                    }
                    oSrcPDF.Dispose();
                }
                else
                {
                    _resp = "Error;Tipo de documento no permitido";
                }
            }
            return _resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDocumento"></param>
        /// <param name="Doc"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ReemplazaDoc")]
        public object PostReemplazaDoc(decimal IdDocumento, string Doc)
        {
            PDFFile oSrcPDF = null;
            if (IdDocumento <= 0) return new { resp = "Error", mensaje = "No se ha ingresado un identificador de documento" };
            if (Doc == null || Doc.Length == 0) return new { resp = "Error", mensaje = "No se ha ingresado un documento de reemplazo" };
            string _RutaBase = SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() != "" ? SIM.Utilidades.Data.ObtenerValorParametro("Temporales").ToString() : "";
            string _Ruta = _RutaBase + @"\" + DateTime.Now.ToString("yyyyMM");
            string filePath = _Ruta + @"\" + Doc;
            FileInfo DocNuevo = new FileInfo(filePath);   
            if (!DocNuevo.Exists) return new { resp = "Error", mensaje = "No se ha podido encontrar el documento subido al sistema" };
            var Docu = (from D in dbSIM.TBTRAMITEDOCUMENTO
                        where D.ID_DOCUMENTO == IdDocumento
                        select D).FirstOrDefault();
            if (Docu == null || Docu.RUTA.Length == 0) return new { resp = "Error", mensaje = "Hay un problema con el documento destino, no se encontró la ruta" };
            FileInfo DocAnte = new FileInfo(Docu.RUTA);
            if (!DocAnte.Exists) return new { resp = "Error", mensaje = "No se ha podido encontrar el documento cod identificador " + IdDocumento };
            try
            {
                oSrcPDF = PDFFile.FromFile(DocNuevo.FullName);
                var _bkFile = new FileInfo(DocAnte.DirectoryName + @"\"+ Path.GetFileNameWithoutExtension(DocAnte.FullName) + "_old" + DocAnte.Extension);
                if (_bkFile.Exists) _bkFile.Delete();
                DocAnte.CopyTo(_bkFile.FullName);
                if (DocAnte.Extension.ToLower() != DocNuevo.Extension.ToLower()) {
                    DocAnte.Delete();
                    Docu.RUTA = DocAnte.DirectoryName + @"\" + Path.GetFileNameWithoutExtension(DocAnte.FullName) + DocNuevo.Extension;
                }
                
                DocNuevo.CopyTo(Docu.RUTA);
                int _Pag = oSrcPDF.PagesCount;
                oSrcPDF.Dispose();
                Docu.PAGINAS = _Pag;
                Docu.CIFRADO = "0";
                dbSIM.Entry(Docu).State = System.Data.Entity.EntityState.Modified;
                dbSIM.SaveChanges();
            }
            catch (Exception ex)
            {
                return new { resp = "Error", mensaje = ex.Message };
            }
            return new { resp = "Ok", mensaje = "Documento reemplazado correctamente" };
        }
    }

        public class Documento
    {
        public decimal ID_DOCUMENTO { get; set; }
        public decimal CODDOC { get; set; }
        public string SERIE { get; set; }
        public DateTime FECHA { get; set; }
        public string ESTADO { get; set; }
        public string ADJUNTO { get; set; }
        public decimal CODTRAMITE { get; set; }
        public bool EDIT_INDICES { get; set; }   
    }
}
