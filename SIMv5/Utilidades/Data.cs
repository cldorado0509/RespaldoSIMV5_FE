using DevExpress.Pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using SIM.Areas.Models;
using SIM.Areas.Tramites;
using SIM.Data;
using SIM.Data.Control;
using SIM.Data.Tramites;
using SIM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Xceed.Words.NET;

namespace SIM.Utilidades
{
    public class INDICE
    {
        public int CODINDICE { get; set; }
        public string VALOR { get; set; }
        public int TIPO { get; set; }
    }

    public class Data
    {
        public static string EliminarEspaciosExtras(string texto)
        {
            var newString = new StringBuilder();
            bool previousIsWhitespace = false;

            texto = texto.Replace("\t", " ").Trim();

            for (int i = 0; i < texto.Length; i++)
            {
                if (texto[i] == '\t')
                {
                    continue;
                }
                else if (Char.IsWhiteSpace(texto[i]))
                {
                    if (previousIsWhitespace)
                    {
                        continue;
                    }

                    previousIsWhitespace = true;
                }
                else
                {
                    previousIsWhitespace = false;
                }

                newString.Append(texto[i]);
            }

            return newString.ToString();
        }

        public static string ObtenerError(Exception rfobjError)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            string lcstrMensajeError;
            Exception lcobjErrorInterno;

            lcstrMensajeError = rfobjError.Message;

            lcobjErrorInterno = rfobjError.InnerException;
            while (lcobjErrorInterno != null)
            {
                lcstrMensajeError += "\r\n\r\n" + lcobjErrorInterno.Message;
                lcobjErrorInterno = lcobjErrorInterno.InnerException;
            }

            return lcstrMensajeError;
        }

        public static IQueryable<dynamic> ObtenerConsultaDinamica(IQueryable<dynamic> consulta, string filter, string sort, string group)
        {
            return Data.ObtenerConsultaDinamica(consulta, filter, sort, group, false);
        }

        public static IQueryable<dynamic> ObtenerConsultaDinamica(IQueryable<dynamic> consulta, string filter, string sort, string group, bool distinct)
        {
            ArrayList condicion = new ArrayList();
            ArrayList parametros = new ArrayList();
            string orderby = string.Empty;
            bool _And = true;

            if (filter != null && filter.Trim() != "")
            {
                //if (filter.Contains("\\"))
                filter = filter.Replace("\\", "").Replace("\"", "");
                _And = filter.ToLower().Contains(",or,") ? false : true;
                string[] filtros = filter.Split(',');
                for (int contFiltro = 0; contFiltro < filtros.Length; contFiltro += 4)
                {
                    filtros[contFiltro] = filtros[contFiltro].Contains(@"\") ? filtros[contFiltro].Replace(@"\", "") : filtros[contFiltro];
                    switch (filtros[contFiltro + 1])
                    {
                        case "contains":
                            switch (consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.FullName)
                            {
                                case "System.String":
                                    condicion.Add(filtros[contFiltro] + ".ToLower().Contains(@" + (contFiltro / 4).ToString() + ")");
                                    break;
                                default:
                                    condicion.Add(filtros[contFiltro] + ".ToString().ToLower().Contains(@" + (contFiltro / 4).ToString() + ")");
                                    break;
                            }
                            parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                            break;
                        case "notcontains":
                            condicion.Add("!" + filtros[contFiltro] + ".ToLower().Contains(@" + (contFiltro / 4).ToString() + ")");
                            parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                            break;
                        case "startswith":
                            condicion.Add(filtros[contFiltro] + ".ToLower().StartsWith(@" + (contFiltro / 4).ToString() + ")");
                            parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                            break;
                        case "endswith":
                            condicion.Add(filtros[contFiltro] + ".ToLower().EndsWith(@" + (contFiltro / 4).ToString() + ")");
                            parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                            break;
                        case "=":
                            switch (consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.FullName)
                            {
                                case "System.String":
                                    condicion.Add(filtros[contFiltro] + ".ToLower() = @" + (contFiltro / 4).ToString());
                                    parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                                    break;
                                case "System.DateTime":
                                    break;
                                default:
                                    condicion.Add(filtros[contFiltro] + " = @" + (contFiltro / 4).ToString());
                                    parametros.Add(Convert.ToDecimal(filtros[contFiltro + 2]));
                                    break;
                            }
                            break;
                        case "<>":
                            switch (consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.FullName)
                            {
                                case "System.String":
                                    condicion.Add(filtros[contFiltro] + ".ToLower() <> @" + (contFiltro / 4).ToString());
                                    parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                                    break;
                                case "System.DateTime":
                                    break;
                                default:
                                    condicion.Add(filtros[contFiltro] + " <> @" + (contFiltro / 4).ToString());
                                    parametros.Add(Convert.ToDecimal(filtros[contFiltro + 2]));
                                    break;
                            }
                            break;
                        case "<":
                        case "<=":
                        case ">":
                        case ">=":
                            string propertyType = "";
                            if (consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.IsGenericType && consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                propertyType = consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.GetGenericArguments()[0].ToString();
                                switch (propertyType)
                                {
                                    case "System.DateTime":
                                        condicion.Add(filtros[contFiltro] + " " + filtros[contFiltro + 1] + " @" + (contFiltro / 4).ToString());
                                        string _Fecha = filtros[contFiltro + 2];
                                        _Fecha = Regex.Replace(_Fecha, " \\(.*\\)$", "");
                                        DateTime result;
                                        if (_Fecha.Contains("GMT")) result = DateTime.ParseExact(_Fecha, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz", CultureInfo.InvariantCulture);
                                        else result = DateTime.Parse(_Fecha);
                                        parametros.Add(result);
                                        break;
                                    default:
                                        condicion.Add(filtros[contFiltro] + " " + filtros[contFiltro + 1] + " @" + (contFiltro / 4).ToString());
                                        parametros.Add(Convert.ToDecimal(filtros[contFiltro + 2]));
                                        break;
                                }
                            }
                            else
                            {
                                switch (consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.FullName)
                                {
                                    case "System.DateTime":
                                        condicion.Add(filtros[contFiltro] + " " + filtros[contFiltro + 1] + " @" + (contFiltro / 4).ToString());
                                        string _Fecha = filtros[contFiltro + 2];
                                        _Fecha = Regex.Replace(_Fecha, " \\(.*\\)$", "");
                                        DateTime result;
                                        if (_Fecha.Contains("GMT")) result = DateTime.ParseExact(_Fecha, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz", CultureInfo.InvariantCulture);
                                        else result = DateTime.Parse(_Fecha);
                                        parametros.Add(result);
                                        break;
                                    default:
                                        condicion.Add(filtros[contFiltro] + " " + filtros[contFiltro + 1] + " @" + (contFiltro / 4).ToString());
                                        parametros.Add(Convert.ToDecimal(filtros[contFiltro + 2]));
                                        break;
                                }
                            }
                            break;
                    }
                }
            }

            if (sort != null)
            {
                JArray sortData = (JArray)JsonConvert.DeserializeObject(sort);

                foreach (JToken ordenCampo in sortData)
                {
                    if (orderby == string.Empty)
                        orderby = ordenCampo["selector"].ToString() + (ordenCampo["desc"].ToString() == "True" ? " DESC" : "");
                    else
                        orderby += ", " + ordenCampo["selector"].ToString() + (ordenCampo["desc"].ToString() == "True" ? " DESC" : "");
                }
            }

            if (condicion.Count > 0)
            {
                string _strCondicion = " AND ";
                if (!_And) _strCondicion = " OR ";
                if (orderby != string.Empty)
                    return consulta.Where(String.Join(_strCondicion, condicion.ToArray()), parametros.ToArray()).OrderBy(orderby);
                else
                    return consulta.Where(String.Join(_strCondicion, condicion.ToArray()), parametros.ToArray());
            }
            else
            {
                if (orderby != string.Empty)
                    return consulta.OrderBy(orderby);
                else
                    return consulta;
            }
        }

        public static IEnumerable<dynamic> ObtenerConsultaDinamica(IEnumerable<DATOSEXT> consulta, string filter, string sort, string group)
        {
            ArrayList condicion = new ArrayList();
            ArrayList parametros = new ArrayList();
            string orderby = string.Empty;

            if (filter != null && filter.Trim() != "")
            {
                string[] filtros = filter.Split(',');

                for (int contFiltro = 0; contFiltro < filtros.Length; contFiltro += 4)
                {
                    switch (filtros[contFiltro + 1])
                    {
                        case "contains":
                            condicion.Add(filtros[contFiltro] + ".ToLower().Contains(@" + (contFiltro / 4).ToString() + ")");
                            parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                            break;
                        case "notcontains":
                            condicion.Add("!" + filtros[contFiltro] + ".ToLower().Contains(@" + (contFiltro / 4).ToString() + ")");
                            parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                            break;
                        case "startswith":
                            condicion.Add(filtros[contFiltro] + ".ToLower().StartsWith(@" + (contFiltro / 4).ToString() + ")");
                            parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                            break;
                        case "endswith":
                            condicion.Add(filtros[contFiltro] + ".ToLower().EndsWith(@" + (contFiltro / 4).ToString() + ")");
                            parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                            break;
                        case "=":
                            /*switch (consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.FullName)
                            {
                                case "System.String":
                                    condicion.Add(filtros[contFiltro] + ".ToLower() = @" + (contFiltro / 4).ToString());
                                    parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                                    break;
                                case "System.DateTime":
                                    break;
                                default:
                                    condicion.Add(filtros[contFiltro] + " = @" + (contFiltro / 4).ToString());
                                    parametros.Add(Convert.ToDecimal(filtros[contFiltro + 2]));
                                    break;
                            }*/
                            break;
                        case "<>":
                            /*switch (consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.FullName)
                            {
                                case "System.String":
                                    condicion.Add(filtros[contFiltro] + ".ToLower() <> @" + (contFiltro / 4).ToString());
                                    parametros.Add(filtros[contFiltro + 2].Trim().ToLowerInvariant());
                                    break;
                                case "System.DateTime":
                                    break;
                                default:
                                    condicion.Add(filtros[contFiltro] + " <> @" + (contFiltro / 4).ToString());
                                    parametros.Add(Convert.ToDecimal(filtros[contFiltro + 2]));
                                    break;
                            }*/
                            break;
                        case "<":
                        case "<=":
                        case ">":
                        case ">=":
                            /*switch (consulta.ElementType.GetProperty(filtros[contFiltro]).PropertyType.FullName)
                            {
                                case "System.DateTime":
                                    condicion.Add(filtros[contFiltro] + " " + filtros[contFiltro + 1] + " @" + (contFiltro / 4).ToString());
                                    parametros.Add(Convert.ToDateTime(filtros[contFiltro + 2]));
                                    break;
                                default:
                                    condicion.Add(filtros[contFiltro] + " " + filtros[contFiltro + 1] + " @" + (contFiltro / 4).ToString());
                                    parametros.Add(Convert.ToDecimal(filtros[contFiltro + 2]));
                                    break;
                            }*/
                            break;
                    }
                }
            }

            if (sort != null)
            {
                JArray sortData = (JArray)JsonConvert.DeserializeObject(sort);

                foreach (JToken ordenCampo in sortData)
                {
                    if (orderby == string.Empty)
                        orderby = ordenCampo["selector"].ToString() + (ordenCampo["desc"].ToString() == "True" ? " DESC" : "");
                    else
                        orderby += ", " + ordenCampo["selector"].ToString() + (ordenCampo["desc"].ToString() == "True" ? " DESC" : "");
                }
            }

            if (condicion.Count > 0)
                if (orderby != string.Empty)
                    return consulta.Where(String.Join(" AND ", condicion.ToArray()), parametros.ToArray()).OrderBy(orderby);
                else
                    return consulta.Where(String.Join(" AND ", condicion.ToArray()), parametros.ToArray());
            else
                if (orderby != string.Empty)
                return consulta.OrderBy(orderby);
            else
                return consulta;
        }

        public static IQueryable<dynamic> ObtenerConsultaDinamicaFullText(IQueryable<dynamic> consulta, string filter)
        {
            ArrayList condicion = new ArrayList();
            ArrayList parametros = new ArrayList();
            string orderby = string.Empty;

            if (filter != null && filter.Trim() != "")
            {
                string[] filtros = filter.Split(',');

                //return consulta.Where("CATSEARCH(" + filtros[0] + ", '*@0*', '') > 0", filtros[1]);
                return consulta.Where(filtros[0] + ".ToLower().Contains(@0)", filtros[1]).OrderBy(filtros[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Genera el digito de verificación de un número de documento
        /// </summary>
        /// <param name="numeroDocumento">Número de documento</param>
        /// <returns></returns>
        public static Byte ObtenerDigitoVerificacion(string numeroDocumento)
        {
            StringBuilder ceros = new StringBuilder();
            int longitud = numeroDocumento.Length;
            int[] liPeso = { 71, 67, 59, 53, 47, 43, 41, 37, 29, 23, 19, 17, 13, 7, 3 };
            string numeroDocumentoCompleto;
            int suma = 0;
            int digitoVerificacion;

            for (int i = 1; i <= (15 - longitud); i++)
            {
                ceros.Append("0");
            }

            numeroDocumentoCompleto = ceros.ToString() + numeroDocumento;

            for (int i = 0; i < 15; i++)
            {
                suma += int.Parse(numeroDocumentoCompleto.Substring(i, 1)) * liPeso[i];
            }

            digitoVerificacion = suma % 11;

            if (digitoVerificacion >= 2)
            {
                digitoVerificacion = 11 - digitoVerificacion;
            }

            return (Byte)digitoVerificacion;
        }

        public static string ObtenerValorParametro(string clave)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

            var valor = dbSIM.Database.SqlQuery<string>("SELECT VALOR FROM GENERAL.PARAMETROS WHERE CLAVE = '" + clave + "'").FirstOrDefault();

            return valor;
        }

        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;


            //if (!string.IsNullOrWhiteSpace(appUrl)) appUrl += "/";


            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
            return baseUrl;
        }

        public static bool AlmacenarDocumentosTemporalesTramite(decimal codTramite, decimal codTarea, decimal codFuncionario, string rutaDocumento, string descripcion)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            EntitiesTramitesOracle dbTramites = new EntitiesTramitesOracle();

            decimal codProceso;
            ObjectParameter respCodProceso = new ObjectParameter("respCodProceso", typeof(decimal));
            ObjectParameter rtaResultado = new ObjectParameter("rtaResultado", typeof(string));

            dbTramites.SP_OBTENER_PROCESO_TAREA(codTarea, respCodProceso, rtaResultado);

            if (rtaResultado.Value.ToString() != "OK")
                return false;

            codProceso = Convert.ToDecimal(respCodProceso.Value);
            TBRUTAPROCESO rutaProceso = dbSIM.TBRUTAPROCESO.Where(rp => rp.CODPROCESO == codProceso).FirstOrDefault();
            string pathDocumentosTemporalesTramite = rutaProceso.PATH + "\\" + Archivos.GetRutaDocumento(Convert.ToUInt64(codTramite), 100);
            string ruta;

            ruta = pathDocumentosTemporalesTramite + codTramite.ToString("0") + "_" + Path.GetFileName(rutaDocumento);

            if (!Directory.Exists(pathDocumentosTemporalesTramite))
                Directory.CreateDirectory(pathDocumentosTemporalesTramite);

            if (File.Exists(ruta))
                File.Delete(ruta);

            System.IO.File.Copy(rutaDocumento, ruta);

            dbTramites.SP_ASIGNAR_TEMPORAL_TRAMITE(codTramite, codTarea, codFuncionario, -1, -1, descripcion, ruta, rtaResultado);

            if (rtaResultado.Value.ToString() != "OK")
                return false;
            else
                return true;
        }

        public static dynamic ObtenerIndicesDocumento(int idVisita)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            List<INDICE> indices = new List<INDICE>();
            string lineaTexto;
            bool paraAsignado = false;
            bool asuntoAsignado = false;
            bool finIndices = false;

            INFORME_TECNICO informeTecnico = dbSIM.INFORME_TECNICO.Where(it => it.ID_VISITA == idVisita).FirstOrDefault();

            if (informeTecnico != null && File.Exists(informeTecnico.URL))
            {
                decimal codFuncionario = (decimal)informeTecnico.FUNCIONARIO;

                var tecnico = (from uf in dbSIM.USUARIO_FUNCIONARIO
                               join u in dbSIM.USUARIO on uf.ID_USUARIO equals u.ID_USUARIO
                               where uf.CODFUNCIONARIO == codFuncionario
                               select u.S_NOMBRES + " " + u.S_APELLIDOS).FirstOrDefault();

                if (tecnico != null)
                {
                    indices.Add(new INDICE { CODINDICE = 680, VALOR = tecnico, TIPO = 0 });
                }

                indices.Add(new INDICE { CODINDICE = 400, VALOR = "NA", TIPO = 0 });

                DocX document = DocX.Load(informeTecnico.URL);
                //DocX document = DocX.Load(@"C:\Temp\InformeTecnico2.docx");
                var texto = document.Text;

                foreach (var lineaTextoCompleta in texto.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (finIndices)
                        break;

                    lineaTexto = Data.EliminarEspaciosExtras(lineaTextoCompleta);
                    var palabras = lineaTexto.Split(' ', ':', '\t');

                    if (palabras.Length > 0)
                    {
                        if (palabras.Length > 3 &&
                                (
                                    (palabras[0].ToUpper().Trim() == "XCONTROL" && palabras[1].ToUpper().Trim() == "Y" && palabras[2].ToUpper().Trim() == "SEGUIMIENTO") ||
                                    (palabras[1].ToUpper().Trim() == "CONTROL" && palabras[2].ToUpper().Trim() == "Y" && palabras[3].ToUpper().Trim() == "SEGUIMIENTO")
                                )
                            )
                        {
                            int posActual = 0;
                            if (palabras.Contains("permiso"))
                            {
                                string valor = "";

                                for (int i = 16; i < palabras.Length; i++)
                                {
                                    if (palabras[i].Contains("Número"))
                                    {
                                        posActual = i;
                                        valor += " " + palabras[i].Replace("Número", "");
                                        break;
                                    }
                                    else
                                    {
                                        if (palabras[i] != "")
                                            valor += " " + palabras[i];
                                    }
                                }

                                indices.Add(new INDICE { CODINDICE = 3920, VALOR = valor.Trim(), TIPO = 0 });
                            }

                            if (palabras.Contains("anual") && posActual > 0)
                            {
                                string valor = "";

                                for (int i = posActual + 5; i < palabras.Length; i++)
                                {
                                    if (palabras[i].Contains("Se"))
                                    {
                                        posActual = i;
                                        valor += " " + palabras[i].Replace("Se", "");
                                        break;
                                    }
                                    else if (palabras[i - 1].Contains("Se"))
                                    {
                                        posActual = i - 1;
                                        valor += " " + palabras[i - 1].Replace("Se", "");
                                        break;
                                    }
                                    else
                                    {
                                        valor += " " + palabras[i];
                                    }
                                }

                                indices.Add(new INDICE { CODINDICE = 3921, VALOR = valor.Trim(), TIPO = 0 });
                            }

                            if ((lineaTexto.Substring(3).ToUpper().IndexOf("X") > lineaTexto.Substring(3).ToUpper().IndexOf("SI") || lineaTexto.Substring(3).ToUpper().IndexOf("X") > lineaTexto.Substring(3).ToUpper().IndexOf("SÍ")) && lineaTexto.Substring(3).ToUpper().IndexOf("X") < lineaTexto.Substring(3).ToUpper().IndexOf("NO"))
                            {
                                indices.Add(new INDICE { CODINDICE = 3922, VALOR = "S", TIPO = 4 });
                            }
                            else
                            {
                                indices.Add(new INDICE { CODINDICE = 3922, VALOR = "N", TIPO = 4 });
                            }

                            continue;
                        }

                        if (palabras[0].Length > 0 && palabras[0][0].ToString().ToUpper() == "X")
                        {
                            if (!paraAsignado)
                            {
                                // Para
                                indices.Add(new INDICE { CODINDICE = 320, VALOR = string.Join(" ", palabras, 0, palabras.Length).Substring(1).Trim(), TIPO = 0 });

                                paraAsignado = true;
                            }
                            else if (!asuntoAsignado)
                            {
                                string asunto;

                                if (palabras[1].ToUpper() == "CONTROL" && palabras.Length > 6)
                                {
                                    asunto = string.Join(" ", palabras, 0, 7).Substring(1).Trim().Replace("Número", "");
                                }
                                else
                                    asunto = string.Join(" ", palabras, 0, palabras.Length).Substring(1).Trim();

                                // Asunto
                                indices.Add(new INDICE { CODINDICE = 321, VALOR = asunto, TIPO = 0 });
                                asuntoAsignado = true;

                                switch (asunto.ToUpper().Substring(0, 3))
                                {
                                    case "EVA":
                                        indices.Add(new INDICE { CODINDICE = 2500, VALOR = "S", TIPO = 4 });
                                        break;
                                    case "CON":
                                        if (asunto.ToUpper().Substring(0, 11) == "CONTROL Y S")
                                            indices.Add(new INDICE { CODINDICE = 2501, VALOR = "S", TIPO = 4 });
                                        else if (asunto.ToUpper().Substring(0, 11) == "CONTROL Y V")
                                            indices.Add(new INDICE { CODINDICE = 2502, VALOR = "S", TIPO = 4 });
                                        break;
                                    case "PRA":
                                        indices.Add(new INDICE { CODINDICE = 2503, VALOR = "S", TIPO = 4 });
                                        break;
                                    case "TAS":
                                        indices.Add(new INDICE { CODINDICE = 2504, VALOR = "S", TIPO = 4 });
                                        break;
                                }
                            }
                            else if (palabras.Length > 1 && palabras[1].ToUpper() == "APROVECHAMIENTO")
                            {
                                indices.Add(new INDICE { CODINDICE = 3940, VALOR = string.Join(" ", palabras, 0, palabras.Length).Substring(1).Trim(), TIPO = 0 });
                            }
                            else if (palabras.Length > 1 && palabras[1].ToUpper() == "PRIMER")
                            {
                                indices.Add(new INDICE { CODINDICE = 3925, VALOR = "S", TIPO = 4 });
                            }
                            else if (palabras.Length > 1 && palabras[1].ToUpper() == "SEGUNDO")
                            {
                                indices.Add(new INDICE { CODINDICE = 3926, VALOR = "S", TIPO = 4 });
                            }
                        }
                        else
                        {
                            switch (palabras[0].ToUpper())
                            {
                                case "COMPLEMENTO":
                                    if (palabras.Length >= 4)
                                        indices.Add(new INDICE { CODINDICE = 2520, VALOR = string.Join(" ", palabras, 4, palabras.Length - 4).Trim(), TIPO = 0 });
                                    break;
                                case "INFORME":
                                    if (palabras.Length > 1 && palabras[1].Length > 10 && palabras[1].Substring(0, 7) == "TÉCNICO")
                                    {
                                        var posFinal = palabras[1].IndexOf('-');
                                        if (posFinal > 0)
                                            indices.Add(new INDICE { CODINDICE = 380, VALOR = palabras[1].Substring(7, posFinal - 7).Trim(), TIPO = 0 });
                                    }
                                    break;
                                case "DE":
                                    indices.Add(new INDICE { CODINDICE = 340, VALOR = string.Join(" ", palabras, 1, palabras.Length - 1).Trim(), TIPO = 0 });
                                    break;
                                case "PARA":
                                    if (palabras[1].Length > 0 && palabras[1][0].ToString().ToUpper() == "X")
                                    {
                                        // Para
                                        indices.Add(new INDICE { CODINDICE = 320, VALOR = string.Join(" ", palabras, 1, palabras.Length - 1).Substring(1).Trim(), TIPO = 0 });
                                        paraAsignado = true;
                                    }
                                    else if (palabras[2].Length > 0 && palabras[2][0].ToString().ToUpper() == "X")
                                    {
                                        // Para
                                        indices.Add(new INDICE { CODINDICE = 320, VALOR = string.Join(" ", palabras, 2, palabras.Length - 2).Substring(1).Trim(), TIPO = 0 });
                                        paraAsignado = true;
                                    }
                                    //indices.Add(new INDICE { CODINDICE = 320, VALOR = string.Join(" ", palabras, 1, palabras.Length - 1) });
                                    break;
                                case "":
                                case "ASUNTO":
                                    if (palabras.Length > 2 && ((palabras[1].Length > 0 && palabras[1][0].ToString().ToUpper() == "X") || (palabras[2].Length > 0 && palabras[2][0].ToString().ToUpper() == "X")))
                                    {
                                        int posIni;

                                        if (palabras[1].Length > 0 && palabras[1][0].ToString().ToUpper() == "X")
                                        {
                                            posIni = 1;
                                        }
                                        else
                                        {
                                            posIni = 3;
                                        }

                                        string asunto = string.Join(" ", palabras, posIni, palabras.Length - posIni).Trim();

                                        // Asunto
                                        indices.Add(new INDICE { CODINDICE = 321, VALOR = asunto, TIPO = 0 });
                                        asuntoAsignado = true;

                                        switch (asunto.ToUpper().Substring(0, 3))
                                        {
                                            case "EVA":
                                                indices.Add(new INDICE { CODINDICE = 2500, VALOR = "S", TIPO = 4 });
                                                break;
                                            case "CON":
                                                if (asunto.ToUpper().Substring(0, 11) == "CONTROL Y S")
                                                    indices.Add(new INDICE { CODINDICE = 2501, VALOR = "S", TIPO = 4 });
                                                else if (asunto.ToUpper().Substring(0, 11) == "CONTROL Y V")
                                                    indices.Add(new INDICE { CODINDICE = 2502, VALOR = "S", TIPO = 4 });
                                                break;
                                            case "PRA":
                                            case "PRÁ":
                                                indices.Add(new INDICE { CODINDICE = 2503, VALOR = "S", TIPO = 4 });
                                                break;
                                            case "TAS":
                                                indices.Add(new INDICE { CODINDICE = 2504, VALOR = "S", TIPO = 4 });
                                                break;
                                        }
                                    }
                                    //indices.Add(new INDICE { CODINDICE = 321, VALOR = string.Join(" ", palabras, 1, palabras.Length - 1) });
                                    break;
                                case "NOMBRE":
                                case "PROYECTO":
                                    {
                                        int posInicial = 1;

                                        if (palabras.Length > 6)
                                        {
                                            posInicial = (palabras[4].ToUpper().Contains("LOCALIZACION") || palabras[4].ToUpper().Contains("LOCALIZACIÓN") ? 5 : 7);
                                            if (posInicial < 6)
                                                palabras[posInicial] = palabras[posInicial].Substring(12);
                                        }
                                        else
                                        {
                                            posInicial = (palabras[0].ToUpper() == "PROYECTO" ? 1 : (palabras[1].ToUpper() == "PROYECTO" ? 2 : 3));
                                        }

                                        indices.Add(new INDICE { CODINDICE = 341, VALOR = string.Join(" ", palabras, posInicial, palabras.Length - posInicial).Trim(), TIPO = 0 });
                                    }
                                    break;
                                case "REPRESENTANTE":
                                    indices.Add(new INDICE { CODINDICE = 322, VALOR = string.Join(" ", palabras, 2, palabras.Length - 2).Trim(), TIPO = 0 });
                                    break;
                                case "NIT":
                                case "CEDULA":
                                case "CÉDULA":
                                    {
                                        var posInicial = (palabras[1].ToUpper() == "O" || palabras[1].ToUpper() == "Ó" ? 3 : 1);
                                        indices.Add(new INDICE { CODINDICE = 323, VALOR = string.Join(" ", palabras, posInicial, palabras.Length - posInicial).Trim(), TIPO = 0 });
                                    }
                                    break;
                                case "DIRECCION":
                                case "DIRECCIÓN":
                                    if (palabras[1].ToUpper() == "DEL")
                                    {
                                        if (palabras[4].ToUpper() == "RURAL")
                                            indices.Add(new INDICE { CODINDICE = 324, VALOR = string.Join(" ", palabras, 5, palabras.Length - 5).Trim(), TIPO = 0 });
                                        else
                                            indices.Add(new INDICE { CODINDICE = 324, VALOR = string.Join(" ", palabras, 4, palabras.Length - 4).Trim(), TIPO = 0 });
                                    }
                                    if (palabras[1].ToUpper() == "DE")
                                    {
                                        indices.Add(new INDICE { CODINDICE = 3923, VALOR = string.Join(" ", palabras, 7, palabras.Length - 7).Trim(), TIPO = 0 });
                                    }
                                    break;
                                case "TELEFONO":
                                case "TELÉFONO":
                                    indices.Add(new INDICE { CODINDICE = 325, VALOR = string.Join(" ", palabras, 1, palabras.Length - 1).Trim(), TIPO = 0 });
                                    break;
                                case "MUNICIPIO":
                                    indices.Add(new INDICE { CODINDICE = 326, VALOR = string.Join(" ", palabras, 1, palabras.Length - 1).Trim(), TIPO = 0 });
                                    break;
                                case "CORREO":
                                    indices.Add(new INDICE { CODINDICE = 3924, VALOR = string.Join(" ", palabras, 8, palabras.Length - 8).Trim(), TIPO = 0 });
                                    break;
                                case "NO":
                                case "NO.":
                                case "CM":
                                case "QUEJA":
                                    {
                                        if (palabras[0].ToUpper() == "CM")
                                        {
                                            indices.Add(new INDICE { CODINDICE = 327, VALOR = string.Join(" ", palabras, 1, palabras.Length - 1).Trim(), TIPO = 0 });
                                        }
                                        else if (palabras[0].ToUpper() == "QUEJA")
                                        {
                                            indices.Add(new INDICE { CODINDICE = 360, VALOR = string.Join(" ", palabras, 1, palabras.Length - 1).Trim(), TIPO = 0 });
                                        }
                                        else
                                        {
                                            if (palabras[1].ToUpper() == "CM")
                                            {
                                                indices.Add(new INDICE { CODINDICE = 327, VALOR = string.Join(" ", palabras, 2, palabras.Length - 2).Trim(), TIPO = 0 });
                                            }
                                            else if (palabras[1].ToUpper() == "QUEJA")
                                            {
                                                indices.Add(new INDICE { CODINDICE = 360, VALOR = string.Join(" ", palabras, 2, palabras.Length - 2).Trim(), TIPO = 0 });
                                            }
                                            else
                                            {
                                                if (palabras[2].ToUpper() == "CM")
                                                {
                                                    indices.Add(new INDICE { CODINDICE = 327, VALOR = string.Join(" ", palabras, 3, palabras.Length - 3).Trim(), TIPO = 0 });
                                                }
                                                else if (palabras[2].ToUpper() == "QUEJA")
                                                {
                                                    indices.Add(new INDICE { CODINDICE = 360, VALOR = string.Join(" ", palabras, 3, palabras.Length - 3).Trim(), TIPO = 0 });
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case "AÑO":
                                    {
                                        var posInicial = (palabras[1].ToUpper() == "QUEJA" ? 2 : (palabras[1].ToUpper() == "QUEJA" ? 3 : 4));
                                        indices.Add(new INDICE { CODINDICE = 328, VALOR = string.Join(" ", palabras, posInicial, palabras.Length - posInicial).Trim(), TIPO = 0 });
                                    }
                                    break;
                                case "ABOGADO":
                                    {
                                        int posFinal = 1;

                                        if (palabras.Length > 12)
                                        {
                                            for (int i = 2; i < 13; i++)
                                            {
                                                if (palabras[i].ToUpper().Contains("DESCRIPCI"))
                                                {
                                                    palabras[i] = palabras[i].Substring(0, palabras[i].ToUpper().IndexOf("DESCRIPCI"));
                                                    posFinal = i;
                                                }
                                            }

                                            indices.Add(new INDICE { CODINDICE = 342, VALOR = string.Join(" ", palabras, 2, posFinal - 1).Trim(), TIPO = 0 });
                                        }
                                        else
                                        {
                                            indices.Add(new INDICE { CODINDICE = 342, VALOR = string.Join(" ", palabras, 2, palabras.Length - 2).Trim(), TIPO = 0 });
                                        }

                                        finIndices = true;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                return null;
            }

            return indices;
        }

        public static dynamic GenerarDocumentoRadicado(int idVisita, int idRadicado, bool radicado, int codFuncionario1, int codFuncionario2)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            //int codFuncionario1;
            //int codFuncionario2;
            int count;
            int numPaginas = 0;
            string tipos = "15,20";
            int tipo;
            string nombre;
            PdfTextSearchResults result1 = null;
            PdfTextSearchResults result2 = null;
            PdfSharp.Pdf.PdfDocument inputDocument;
            MemoryStream stream = null;
            INFORME_TECNICO informeTecnico = dbSIM.INFORME_TECNICO.Where(it => it.ID_VISITA == idVisita).FirstOrDefault();

            VISITA visita = dbSIM.VISITA.Where(v => v.ID_VISITA == idVisita).FirstOrDefault();
            //codFuncionario1 = (int)db.VISITAESTADO.Where(visitaFunc => visitaFunc.ID_VISITA == idVisita && visitaFunc.ID_ESTADOVISITA == 2).FirstOrDefault().ID_TERCERO;
            //codFuncionario2 = Convert.ToInt32(clsGenerales.Obtener_Codigo_Funcionario(db, Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value)));

            PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument();

            if (informeTecnico != null)
            {
                var streamDocumento = new MemoryStream();

                // Obtener coordenadas de Firma
                inputDocument = PdfReader.Open(Archivos.ConvertirAPDF(informeTecnico.URL), PdfDocumentOpenMode.Import);
                //inputDocument = PdfReader.Open("C:\\Temp\\Temporal\\21774-20210713172554Radicado.pdf", PdfDocumentOpenMode.Import);
                count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                    outputDocument.AddPage(page);

                    numPaginas++;
                }
                outputDocument.Save(streamDocumento);
                outputDocument.Close();
                inputDocument.Close();
                inputDocument.Dispose();

                PdfDocumentProcessor documento = new PdfDocumentProcessor();
                documento.LoadDocument(streamDocumento);
                result1 = documento.FindText("{FIRMA1}");
                result2 = documento.FindText("{FIRMA2}");
                documento.CloseDocument();
                streamDocumento.Close();
                streamDocumento.Dispose();

                /*streamDocumento = new MemoryStream();
                // Eliminar Texto {Firma}
                DocX document = DocX.Load(informeTecnico.URL);
                document.ReplaceText("{FIRMA1}", "");
                document.ReplaceText("{FIRMA2}", "");
                document.SaveAs(streamDocumento);*/

                //inputDocument = PdfReader.Open(Archivos.ConvertirAPDF(streamDocumento, informeTecnico.URL), PdfDocumentOpenMode.Import);

                //inputDocument = PdfReader.Open(Archivos.ConvertirAPDF(informeTecnico.URL), PdfDocumentOpenMode.Import);
                //inputDocument = PdfReader.Open("D:\\Temp\\InformeTecnico.docx", PdfDocumentOpenMode.Import);
                //inputDocument = PdfReader.Open(Archivos.ConvertirAPDF("D:\\Temp\\InformeTecnico.docx"), PdfDocumentOpenMode.Import);
                //inputDocument = PdfReader.Open(informeTecnico.URL, PdfDocumentOpenMode.Import);

                //count = inputDocument.PageCount;
                //for (int idx = 0; idx < count; idx++)
                //{
                //PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                //outputDocument.AddPage(page);

                if (radicado)
                {
                    PdfSharp.Pdf.PdfPage pageRadicado = outputDocument.Pages[0];

                    /*Radicado01Report etiqueta = new Radicado01Report();
                    MemoryStream imagenEtiqueta = etiqueta.GenerarEtiqueta(idRadicado, "png");

                    XGraphics gfx = XGraphics.FromPdfPage(pageRadicado);
                    DrawImage(gfx, imagenEtiqueta, 170, 160, 239, 61); */ // ESTE ES EL ACTUAL

                    MemoryStream imagenEtiqueta = new MemoryStream();
                    TramitesLibrary tramitesLibrary = new TramitesLibrary();

                    Radicador radicador = new Radicador();
                    var imagenRadicado = radicador.ObtenerImagenRadicadoArea(idRadicado);
                    imagenRadicado.Save(imagenEtiqueta, ImageFormat.Bmp);

                    XGraphics gfx = XGraphics.FromPdfPage(pageRadicado);
                    DrawImage(gfx, imagenEtiqueta, 300, 30, 288, 72);
                }

                /*if (idx == count-1) // Última Página, se inserta la firma
                {
                    PdfPage pageRadicado = outputDocument.Pages[idx];

                    pageRadicado.

                    var found = pageRadicado.Text.Find("text for search", FindFlags.MatchWholeWord, 0);
                    if (found == null)
                        return; //nothing found
                    do
                    {
                        var textInfo = found.FindedText;
                        foreach (var rect in textInfo.Rects)
                        {
                            float x = rect.left;
                            float y = rect.top;
                            //...
                        }
                    } while (found.FindNext());

                    page.Dispose();
                }*/

                //numPaginas++;
                //}
            }

            if (result1 != null && result1.PageNumber > 0 && codFuncionario1 >= 0)
            {
                var alturaPagina = outputDocument.Pages[0].Height.Value;
                XGraphics gfxFirma = XGraphics.FromPdfPage(outputDocument.Pages[result1.PageNumber - 1]);
                //DrawImage(gfxFirma, Security.ObtenerFirmaElectronicaFuncionario(codFuncionario1), Convert.ToInt32(result1.Rectangles[0].Left), Convert.ToInt32((alturaPagina - result1.Rectangles[0].Top) - (130) / 2), Convert.ToInt32(400), Convert.ToInt32(130));
                DrawImage(gfxFirma, Security.ObtenerFirmaElectronicaFuncionario(codFuncionario1, false, ""), Convert.ToInt32(result1.Rectangles[0].Left), Convert.ToInt32((alturaPagina - result1.Rectangles[0].Top) - (130) / 2), Convert.ToInt32(300), Convert.ToInt32(98));
                gfxFirma.Dispose();
            }

            if (result2 != null && result2.PageNumber > 0 && codFuncionario2 >= 0)
            {
                var alturaPagina = outputDocument.Pages[0].Height.Value;
                XGraphics gfxFirma = XGraphics.FromPdfPage(outputDocument.Pages[result2.PageNumber - 1]);
                //DrawImage(gfxFirma, Security.ObtenerFirmaElectronicaFuncionario(codFuncionario2), Convert.ToInt32(result2.Rectangles[0].Left), Convert.ToInt32((alturaPagina - result2.Rectangles[0].Top) - (130) / 2), Convert.ToInt32(400), Convert.ToInt32(130));
                DrawImage(gfxFirma, Security.ObtenerFirmaElectronicaFuncionario(codFuncionario2, false, ""), Convert.ToInt32(result2.Rectangles[0].Left), Convert.ToInt32((alturaPagina - result2.Rectangles[0].Top) - (130) / 2), Convert.ToInt32(300), Convert.ToInt32(98));
                gfxFirma.Dispose();
            }

            if (visita != null)
            {
                bool estadosVisitaOK;

                Decimal idInstalacion = Convert.ToDecimal(visita.INSTALACION_VISITA.FirstOrDefault().ID_INSTALACION);
                Decimal idTercero = Convert.ToDecimal(visita.INSTALACION_VISITA.FirstOrDefault().ID_TERCERO);
                /*ObjectParameter jSONOUT = new ObjectParameter("jSONOUT", typeof(string));
                dbSIM.SP_GET_FORMULARIOS(idInstalacion, idTercero, Convert.ToDecimal(idVisita), jSONOUT);

                dynamic data = JsonConvert.DeserializeObject(jSONOUT.Value.ToString());*/

                var datosFormulario = (new FormularioDatos()).ObtenerJsonFormularios(Convert.ToInt32(idInstalacion), Convert.ToInt32(idTercero), idVisita);
                dynamic data = JsonConvert.DeserializeObject(datosFormulario.json);

                foreach (dynamic nodos in data)
                {
                    foreach (dynamic formulario in nodos.FORMULARIOS)
                    {
                        if (formulario.ITEMS != null && formulario.ITEMS.Count > 0)
                        {
                            estadosVisitaOK = false;

                            foreach (dynamic item in formulario.ITEMS)
                            {
                                if (item.ESTADO != 0)
                                {
                                    estadosVisitaOK = true;
                                    break;
                                }
                            }

                            //if (tipos == "")
                            //    tipos = formulario.ID.Value.ToString();
                            //else
                            if (estadosVisitaOK)
                                tipos += "," + formulario.ID.Value.ToString();
                        }
                    }
                }
            }

            foreach (string tipoSel in tipos.Split(','))
            {
                tipo = Convert.ToInt32(tipoSel);

                if (tipo != 8 && tipo != 20) // Excluimos Arbol Urbano mientras tanto.
                {
                    stream = new MemoryStream();

                    switch (tipo)
                    {
                        case 15://general
                            var report1 = new SIM.Areas.ControlVigilancia.reporte.aguas.visitas();
                            DevExpress.XtraReports.Parameters.Parameter param1 = report1.Parameters["prm_id_visita"];
                            //DevExpress.XtraReports.Parameters.Parameter url = report1.Parameters["prm_radicador"];
                            //url.Value = urlRad;
                            param1.Value = idVisita;
                            report1.ExportToPdf(stream);
                            report1.Dispose();
                            nombre = "visitas.pdf";
                            break;
                        case 20://Arbol Urbano
                            /*var report20 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                            DevExpress.XtraReports.Parameters.Parameter param20 = report20.Parameters["prm_idvisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url20 = report20.Parameters["prm_radicador"];
                            //url20.Value = urlRad;
                            param20.Value = idVisita;
                            report20.ExportToPdf(stream);
                            report20.Dispose();
                            nombre = "ArbolUrbano.pdf";*/
                            break;
                        case 13://vertimiento
                            var report2 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.reportVertimiento();
                            DevExpress.XtraReports.Parameters.Parameter param2 = report2.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url2 = report2.Parameters["prm_radicador"];
                            //url2.Value = urlRad;
                            param2.Value = idVisita;
                            report2.ExportToPdf(stream);
                            report2.Dispose();
                            nombre = "vertimiento.pdf";
                            break;
                        case 5://subterranea
                            var report3 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuterranea();
                            DevExpress.XtraReports.Parameters.Parameter param3 = report3.Parameters["prm_visita"];
                            //DevExpress.XtraReports.Parameters.Parameter url3 = report3.Parameters["prm_radicador"];
                            //url3.Value = urlRad;
                            param3.Value = idVisita;
                            report3.ExportToPdf(stream);
                            report3.Dispose();
                            nombre = "subterranea.pdf";
                            break;
                        case 4://aguas Superficiales
                            var report4 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.aguaSuperficial();
                            DevExpress.XtraReports.Parameters.Parameter param4 = report4.Parameters["prm_visita"];
                            //DevExpress.XtraReports.Parameters.Parameter url4 = report4.Parameters["prm_radicador"];
                            //url4.Value = urlRad;
                            param4.Value = idVisita;
                            report4.ExportToPdf(stream);
                            report4.Dispose();
                            nombre = "Superficiales.pdf";
                            break;
                        case 12://ocupacion de cause
                            var report5 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NocupacionCause();
                            DevExpress.XtraReports.Parameters.Parameter param5 = report5.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url5 = report5.Parameters["prm_radicador"];
                            //url5.Value = urlRad;
                            param5.Value = idVisita;
                            report5.ExportToPdf(stream);
                            report5.Dispose();
                            nombre = "ocupaciondecause.pdf";
                            break;
                        case 9://fuente fijas
                            {
                                var report6 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NfuentesFijas();
                                DevExpress.XtraReports.Parameters.Parameter param6 = report6.Parameters["prm_idVisita"];
                                //DevExpress.XtraReports.Parameters.Parameter url6 = report6.Parameters["prm_radicador"];
                                //url6.Value = urlRad;
                                param6.Value = idVisita;

                                var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)report6.DataSource;
                                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                                //query.Sql = "SELECT * FROM CONTROL.VWR_FUENTE_FIJA WHERE ID_VISITA = " + idVisita.ToString() + " AND id_formulario=9";
                                query.Sql = "SELECT CAST(ID_FUENTE_FIJA AS NUMBER(10,0)) ID_FUENTE_FIJA, NOMBRE, ID_FUENTE_FIJA_ESTADO, ID_VISITA, ID_INSTALACION, ID_TERCERO, ID_FORMULARIO, X, Y, URLMAPA FROM CONTROL.VWR_FUENTE_FIJA WHERE ID_VISITA = " + idVisita.ToString() + " AND id_formulario=9";
                                dataSource.RebuildResultSchema();

                                report6.ExportToPdf(stream);
                                report6.Dispose();
                                nombre = "fuentesFijas.pdf";
                            }
                            break;
                        case 10://CDA
                            var report7 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NCDA();
                            DevExpress.XtraReports.Parameters.Parameter param7 = report7.Parameters["prm_visita"];
                            //DevExpress.XtraReports.Parameters.Parameter url7 = report7.Parameters["prm_radicador"];
                            //url7.Value = urlRad;
                            param7.Value = idVisita;
                            report7.ExportToPdf(stream);
                            report7.Dispose();
                            nombre = "cda.pdf";
                            break;
                        case 11://quejas
                            var report11 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.Nquejas();
                            DevExpress.XtraReports.Parameters.Parameter param11 = report11.Parameters["prm_visita"];
                            //DevExpress.XtraReports.Parameters.Parameter url11 = report11.Parameters["prm_radicador"];
                            //url11.Value = urlRad;
                            param11.Value = idVisita;
                            report11.ExportToPdf(stream);
                            report11.Dispose();
                            nombre = "quejas.pdf";
                            break;
                        case 8://arbol urbano
                            var report8 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NarbolUrbano();
                            DevExpress.XtraReports.Parameters.Parameter param8 = report8.Parameters["prm_idvisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url8 = report8.Parameters["prm_radicador"];
                            //url8.Value = urlRad;
                            param8.Value = idVisita;
                            report8.ExportToPdf(stream);
                            report8.Dispose();
                            nombre = "arbolUrbano.pdf";
                            break;
                        case 6://residuos peligrosos
                            {
                                var report9 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.ResiduoPeligroso();
                                DevExpress.XtraReports.Parameters.Parameter param9 = report9.Parameters["prm_idVisita"];
                                //DevExpress.XtraReports.Parameters.Parameter url9 = report9.Parameters["prm_radicador"];
                                //url9.Value = urlRad;
                                param9.Value = idVisita;

                                /*var dataSource = (DevExpress.DataAccess.Sql.SqlDataSource)report9.DataSource;
                                var query = (DevExpress.DataAccess.Sql.CustomSqlQuery)dataSource.Queries[0];

                                //query.Sql = "SELECT * FROM CONTROL.VWR_RESIDUO WHERE ID_VISITA = " + idVisita.ToString();
                                query.Sql = "SELECT NOMBRE, CAST(ID_RESIDUO AS NUMBER(10,0)) AS ID_RESIDUO, ID_VISITA, ID_RESIDUO_ESTADO FROM CONTROL.VWR_RESIDUO WHERE ID_VISITA = " + idVisita.ToString();
                                dataSource.RebuildResultSchema();*/

                                report9.ExportToPdf(stream);
                                report9.Dispose();
                                nombre = "residuosPeligrosos.pdf";
                            }
                            break;
                        case 7://proyectos constructivos
                            var report10 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.nproyectosContructivos();
                            DevExpress.XtraReports.Parameters.Parameter param10 = report10.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url10 = report10.Parameters["prm_radicador"];
                            //url10.Value = urlRad;
                            param10.Value = idVisita;
                            report10.ExportToPdf(stream);
                            report10.Dispose();
                            nombre = "proyectosContructivos.pdf";
                            break;
                        case 1://proyectos constructivos
                            var report12 = new SIM.Areas.ControlVigilancia.reporteVisitas.certificadoVisita.NindustriaForestal();
                            DevExpress.XtraReports.Parameters.Parameter param12 = report12.Parameters["prm_idVisita"];
                            //DevExpress.XtraReports.Parameters.Parameter url12 = report12.Parameters["prm_radicador"];
                            //url12.Value = urlRad;
                            param12.Value = idVisita;
                            report12.ExportToPdf(stream);
                            report12.Dispose();
                            nombre = "industriaForestal.pdf";
                            break;
                    }

                    inputDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                    count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];

                        outputDocument.AddPage(page);
                        numPaginas++;
                    }

                    if (stream != null)
                        stream.Close();
                }
            }

            MemoryStream ms = new MemoryStream();

            outputDocument.Save(ms);

            var documentoRetorno = ms.GetBuffer();

            ms.Close();
            ms.Dispose();

            return new { documentoBinario = documentoRetorno, numPaginas = numPaginas };


            //outputDocument.Save("D:\\Temp\\Prueba.pdf");
            //
            //return File(ms.GetBuffer(), "application/pdf", "reporte.pdf");
        }

        private static void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }

        private static void DrawImage(XGraphics gfx, System.Drawing.Image imageFirma, int x, int y, int width, int height)
        {
            var stream = new System.IO.MemoryStream();
            imageFirma.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;

            XImage image = XImage.FromStream(stream);
            gfx.DrawImage(image, x, y, width, height);
        }

        private static void DrawImage(XGraphics gfx, Stream imageEtiqueta, int x, int y, int width, int height)
        {
            XImage image = XImage.FromStream(imageEtiqueta);
            //gfx.DrawImage(image, x, y, width, height);
            gfx.DrawImage(image, new System.Drawing.Point(x, y));
        }

        public static string ObtenerListaCampo(DataTable tabla, string columna)
        {
            int cont = 0;
            StringBuilder campos = new StringBuilder();

            foreach (DataRow registro in tabla.Rows)
            {
                if (cont == 0)
                    campos.Append(registro[columna].ToString());
                else
                    campos.Append("," + registro[columna].ToString());
            }

            return campos.ToString();
        }

        /// <summary>
        /// Obtiene el ultimo orden de las tareas de un tramite esopecifico
        /// </summary>
        /// <param name="CodTramite"></param>
        /// <returns></returns>
        public static int ObtenerMaximoOrdenTramite(long CodTramite)
        {
            EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
            string sql = "SELECT MAX(ORDEN) FROM TRAMITES.TBTRAMITETAREA WHERE COPIA=0 AND CODTRAMITE =" + CodTramite;
            var MaximoOrden = dbSIM.Database.SqlQuery<int>(sql).ToList<int>()[0];
            if (MaximoOrden > 0)
                return int.Parse(MaximoOrden.ToString());
            else return 0;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Retorna la Fecha con la hora en formato VITAL
        /// </summary>
        /// <returns></returns>
        public static string ObtenerFecha()
        {
            try
            {
                string _Fecha = "";
                string _Mes = DateTime.Now.Month.ToString();
                if (_Mes.Length == 1) _Mes = "0" + _Mes;

                string _Dia = DateTime.Now.Day.ToString();
                if (_Dia.Length == 1) _Dia = "0" + _Dia;

                string _Horas = DateTime.Now.Hour.ToString();
                if (_Horas.Length == 1) _Horas = "0" + _Horas;

                string _Minutos = DateTime.Now.Minute.ToString();
                if (_Minutos.Length == 1) _Minutos = "0" + _Minutos;

                string _Segundos = DateTime.Now.Second.ToString();
                if (_Segundos.Length == 1) _Segundos = "0" + _Segundos;


                _Fecha = DateTime.Now.Year.ToString();
                _Fecha = _Fecha + "-" + _Mes + "-" + _Dia + "T" + _Horas + ":" + _Minutos + ":" + _Segundos;

                return _Fecha;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna la Fecha con la hora en formato VITAL
        /// </summary>
        /// <returns></returns>
        public static string ObtenerFecha(DateTime Fecha)
        {
            try
            {
                string _Fecha = "";
                string _Mes = Fecha.Month.ToString();
                if (_Mes.Length == 1) _Mes = "0" + _Mes;

                string _Dia = Fecha.Day.ToString();
                if (_Dia.Length == 1) _Dia = "0" + _Dia;

                string _Horas = Fecha.Hour.ToString();
                if (_Horas.Length == 1) _Horas = "0" + _Horas;

                string _Minutos = Fecha.Minute.ToString();
                if (_Minutos.Length == 1) _Minutos = "0" + _Minutos;

                string _Segundos = Fecha.Second.ToString();
                if (_Segundos.Length == 1) _Segundos = "0" + _Segundos;


                _Fecha = Fecha.Year.ToString();
                _Fecha = _Fecha + "-" + _Mes + "-" + _Dia + "T" + _Horas + ":" + _Minutos + ":" + _Segundos;

                return _Fecha;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

    }
}