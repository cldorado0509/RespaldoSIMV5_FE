﻿namespace SIM.Areas.Contratos.Controllers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using O2S.Components.PDF4NET;
    using SIM.Data;
    using SIM.Data.Contrato;
    using SIM.Data.Tramites;
    using SIM.Utilidades;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    [Authorize]
    public class PropuestaApiController : ApiController
    {
        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
        }

        public struct RptaSubir
        {
            public string SubirCorrecto;
            public string Mensaje;
        }

        public struct RptaRegistro
        {
            public bool RegistroCorrecto;
            public string Mensaje;
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();


        /// <summary>
        /// Obtiene los procesos para cargar el grid de la ventana principal
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
        /// <param name="CodFuncionario"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Procesos")]
        public datosConsulta GetProcesos(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, long CodFuncionario)
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
                var model = (from Pro in dbSIM.PRECONTRATO_PROCESO
                             join Mod in dbSIM.TIPO_PROCESO on Pro.N_MODALIDAD equals Mod.ID_TIPOPROCESO
                             join Fun in dbSIM.QRY_FUNCIONARIO_ALL on Pro.N_FUNCIONARIO_CONTRATO equals Fun.CODFUNCIONARIO
                             orderby Pro.D_REGISTRO
                             select new
                             {
                                 Pro.ID_PROCESO,
                                 MODALIDAD = Mod.S_NOMBRE,
                                 Pro.S_NOMBRE,
                                 FUNCIONARIO = Fun.NOMBRES,
                                 B_SOBRE_SELLADO = (Pro.B_SOBRESELLADO == "1" ? "SI" : "NO"),
                                 Pro.D_CIERREPROPUESTAS,
                                 Pro.D_REGISTRO,
                                 Fun.CODFUNCIONARIO
                             });
                modelData = model;
                IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                resultado.numRegistros = modelFiltered.Count();
                resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                return resultado;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Modalidad")]
        public JArray GetModalidades()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Mod in dbSIM.TIPO_PROCESO
                             orderby Mod.S_NOMBRE
                             select new
                             {
                                 IdModalidad = (int)Mod.ID_TIPOPROCESO,
                                 Modalidad = Mod.S_NOMBRE
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        //[System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaProceso")]
        [System.Web.Http.HttpPost, System.Web.Http.ActionName("GuardaProceso")]
        public object GuardaProceso(DatosProceso objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error Almacenando el proceso" };
            try
            {
                decimal IdProceso = -1;
                if (objData.IdProceso != null && objData.IdProceso != "") IdProceso = decimal.Parse(objData.IdProceso);
                string _SobreSellado = objData.SobreSellado != null ? objData.SobreSellado : "0";
                string _PptaEco = "";
                if (_SobreSellado == "1") _PptaEco = objData.PptaEconomica != null ? objData.PptaEconomica : "0";
                else _PptaEco = "0";
                if (IdProceso > 0)
                {
                    var Proceso = (from Pro in dbSIM.PRECONTRATO_PROCESO
                                   where Pro.ID_PROCESO == IdProceso
                                   select Pro).FirstOrDefault();
                    if (Proceso != null)
                    {
                        if (Proceso.B_SOBRESELLADO != _SobreSellado && objData.Propuestas == "0") Proceso.B_SOBRESELLADO = _SobreSellado;
                        if (Proceso.B_PROPECO != _PptaEco && objData.Propuestas == "0") Proceso.B_PROPECO = _PptaEco;
                        if (objData.FechaApertura != "" && objData.FechaApertura != null) Proceso.D_APERTURA = DateTime.Parse(objData.FechaApertura);
                        if (objData.FechaInicio != "" && objData.FechaInicio != null) Proceso.D_INICIAPROPUESTAS = DateTime.Parse(objData.FechaInicio);
                        if (objData.FechaCierre != "" && objData.FechaCierre != null) Proceso.D_CIERREPROPUESTAS = DateTime.Parse(objData.FechaCierre);
                        if (objData.FechaApeEco != "" && objData.FechaApeEco != null) Proceso.D_PROPECONOMICA = DateTime.Parse(objData.FechaApeEco);
                        Proceso.N_FUNCIONARIO_CONTRATO = decimal.Parse(objData.Funcionario);
                        Proceso.N_MODALIDAD = decimal.Parse(objData.Modalidad);
                        Proceso.S_NOMBRE = objData.Nombre;
                        Proceso.S_OBJETO = objData.Objeto;
                        dbSIM.SaveChanges();
                        Utilidades.LogEventos.GuardaEvento("Registro de Procesos para Propuestas", (long)Proceso.N_FUNCIONARIO_CONTRATO, "Se modifica el proceso " + Proceso.S_NOMBRE + " con identificador " + Proceso.ID_PROCESO + " por parte del funcionario " + Proceso.N_FUNCIONARIO_CONTRATO);
                    }
                }
                else if (IdProceso <= 0)
                {
                    PRECONTRATO_PROCESO Proceso = new PRECONTRATO_PROCESO();
                    Proceso.B_SOBRESELLADO = _SobreSellado;
                    Proceso.B_PROPECO = _PptaEco;
                    if (objData.FechaApertura != "" && objData.FechaApertura != null) Proceso.D_APERTURA = DateTime.Parse(objData.FechaApertura);
                    if (objData.FechaInicio != "" && objData.FechaInicio != null) Proceso.D_INICIAPROPUESTAS = DateTime.Parse(objData.FechaInicio);
                    if (objData.FechaCierre != "" && objData.FechaCierre != null) Proceso.D_CIERREPROPUESTAS = DateTime.Parse(objData.FechaCierre);
                    if (objData.FechaApeEco != "" && objData.FechaApeEco != null) Proceso.D_PROPECONOMICA = DateTime.Parse(objData.FechaApeEco);
                    Proceso.D_REGISTRO = DateTime.Now;
                    Proceso.N_FUNCIONARIO_CONTRATO = decimal.Parse(objData.Funcionario);
                    Proceso.N_MODALIDAD = decimal.Parse(objData.Modalidad);
                    Proceso.S_NOMBRE = objData.Nombre;
                    Proceso.S_OBJETO = objData.Objeto;
                    dbSIM.PRECONTRATO_PROCESO.Add(Proceso);
                    dbSIM.SaveChanges();
                    Utilidades.LogEventos.GuardaEvento("Registro de Procesos para Propuestas", (long)Proceso.N_FUNCIONARIO_CONTRATO, "Se crea el proceso " + Proceso.S_NOMBRE + " por parte del funcionario " + Proceso.N_FUNCIONARIO_CONTRATO);
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando el proceso: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Proceso almacenado correctamente" };
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("EliminaProceso")]
        public object EliminaProceso(string objData)
        {
            if (!ModelState.IsValid) return new { resp = "Error", mensaje = "Error eliminando el proceso" };
            try
            {
                decimal IdProceso = -1;
                if (objData != null && objData != "") IdProceso = decimal.Parse(objData);
                if (IdProceso > 0)
                {
                    var Proceso = (from Pro in dbSIM.PRECONTRATO_PROCESO
                                   where Pro.ID_PROCESO == IdProceso
                                   select Pro).FirstOrDefault();
                    if (Proceso != null)
                    {
                        var Propuestas = (from Pro in dbSIM.PRECONTRATO_PROCPROPUESTAS
                                          where Pro.ID_PROCESO == IdProceso
                                          select Pro).Count();
                        if (Propuestas == 0)
                        {
                            dbSIM.PRECONTRATO_PROCESO.Remove(Proceso);
                            dbSIM.SaveChanges();
                            Utilidades.LogEventos.GuardaEvento("Registro de Procesos para Propuestas", (long)Proceso.N_FUNCIONARIO_CONTRATO, "Se eliminó el proceso " + Proceso.S_NOMBRE + " por parte del funcionario " + Proceso.N_FUNCIONARIO_CONTRATO);
                        }
                        else return new { resp = "Error", mensaje = "El proceso no se puede elimiar ya que posee propuestas" };
                    }
                    else return new { resp = "Error", mensaje = "El proceso no se puede elimiar ya que no se encontró en la base de datos" };
                }
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando COD: " + e.Message };
            }
            return new { resp = "OK", mensaje = "Proceso eliminado correctamente!!" };
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtenerProceso")]
        public JObject GetProceso(string IdProceso)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                decimal _IdProceso = -1;
                if (IdProceso != null && IdProceso != "") _IdProceso = decimal.Parse(IdProceso);

                var Proceso = (from Pro in dbSIM.PRECONTRATO_PROCESO
                               join Mod in dbSIM.TIPO_PROCESO on Pro.N_MODALIDAD equals Mod.ID_TIPOPROCESO
                               join Fun in dbSIM.QRY_FUNCIONARIO_ALL on Pro.N_FUNCIONARIO_CONTRATO equals Fun.CODFUNCIONARIO
                               where Pro.ID_PROCESO == _IdProceso
                               select new
                               {
                                   Pro.ID_PROCESO,
                                   Nombre = Pro.S_NOMBRE,
                                   Pro.S_OBJETO,
                                   Modalidad = Mod.S_NOMBRE,
                                   IdModalidad = Mod.ID_TIPOPROCESO,
                                   Pro.D_INICIAPROPUESTAS,
                                   Pro.D_CIERREPROPUESTAS,
                                   Pro.D_APERTURA,
                                   Pro.D_PROPECONOMICA,
                                   Pro.B_SOBRESELLADO,
                                   Funcionario = Fun.NOMBRES,
                                   Pro.B_PROPECO,
                                   Pro.D_PASOSIM
                               }).FirstOrDefault();
                var Propuestas = (from Pro in dbSIM.PRECONTRATO_PROCPROPUESTAS
                                  where Pro.ID_PROCESO == _IdProceso
                                  select Pro).Count();
                var PropSubidas = (from Pro in dbSIM.PRECONTRATO_PROCPROPUESTAS
                                   where Pro.ID_PROCESO == _IdProceso && Pro.S_ABIERTA_PPTA == "1"
                                   select Pro).Count();
                DatosProceso DatosProceso = new DatosProceso();
                DatosProceso.IdProceso = Proceso.ID_PROCESO.ToString();
                DatosProceso.Nombre = Proceso.Nombre;
                DatosProceso.Objeto = Proceso.S_OBJETO;
                DatosProceso.Modalidad = Proceso.IdModalidad + ";" + Proceso.Modalidad;
                DatosProceso.FechaInicio = Proceso.D_INICIAPROPUESTAS.Value.Year > 1900 ? Proceso.D_INICIAPROPUESTAS.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                DatosProceso.FechaCierre = Proceso.D_CIERREPROPUESTAS.Year > 1990 ? Proceso.D_CIERREPROPUESTAS.ToString("yyyy-MM-dd HH:mm:ss") : "";
                DatosProceso.FechaApertura = Proceso.B_SOBRESELLADO == "1" ? Proceso.D_APERTURA != null ? Proceso.D_APERTURA.Value.Year > 1990 ? Proceso.D_APERTURA.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" : "" : "";
                DatosProceso.FechaApeEco = Proceso.B_PROPECO == "1" ? Proceso.D_PROPECONOMICA != null ? Proceso.D_PROPECONOMICA.Value.Year > 1990 ? Proceso.D_PROPECONOMICA.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" : "" : "";
                DatosProceso.SobreSellado = Proceso.B_SOBRESELLADO;
                DatosProceso.PptaEconomica = Proceso.B_PROPECO;
                DatosProceso.Funcionario = Proceso.Funcionario;
                DatosProceso.Propuestas = Propuestas.ToString();
                DatosProceso.Subidas = PropSubidas.ToString();
                DatosProceso.FechaSIM = Proceso.D_PASOSIM != null ? Proceso.D_PASOSIM.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                return JObject.FromObject(DatosProceso, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ListaProcesos")]
        public JArray GetListaProcesos()
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                var model = (from Pro in dbSIM.PRECONTRATO_PROCESO
                             where Pro.D_CIERREPROPUESTAS <= DateTime.Now && Pro.D_PASOSIM == null
                             orderby Pro.S_NOMBRE
                             select new
                             {
                                 IdProceso = (int)Pro.ID_PROCESO,
                                 Nombre = Pro.S_NOMBRE,
                                 Objeto = Pro.S_OBJETO,
                                 Sellado = Pro.B_SOBRESELLADO == "1" ? "SI" : "NO"
                             });
                return JArray.FromObject(model, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Obtiene los procesos para cargar el grid de la ventana principal
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
        /// <param name="CodFuncionario"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Propuestas")]
        public datosConsulta GetPropuestas(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords, long Proceso)
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
                if (Proceso > 0)
                {
                    var model = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                                 join Pro in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Pro.ID_PROCESO
                                 join Rad in dbSIM.RADICADO_DOCUMENTO on Ppta.ID_RADICADO equals Rad.ID_RADICADODOC
                                 where Ppta.ID_PROCESO == Proceso
                                 orderby Ppta.D_REGISTRO
                                 select new
                                 {
                                     IDPROPUESTA = Ppta.ID_PROPUESTA,
                                     Ppta.D_REGISTRO,
                                     PROPONENTE = Ppta.S_PROPONENTE,
                                     DIRECCION = Ppta.S_DIRECCION,
                                     TELEFONO = Ppta.S_TELEFONO,
                                     EMAIL = Ppta.S_CORREO,
                                     REPONSABLE = Ppta.S_RESPONSABLE,
                                     CODFUNCIONARIO = Pro.N_FUNCIONARIO_CONTRATO,
                                     SOBRESELLADO = Pro.B_SOBRESELLADO,
                                     ABIERTO = Pro.B_SOBRESELLADO == "1" ? Ppta.S_ABIERTA_PPTA == "1" ? "SI" : "NO" : "SI",
                                     ABIERTOECO = Pro.B_SOBRESELLADO == "1" ? Ppta.S_ABIERTA_ECO == "1" ? "SI" : "NO" : "NO",
                                     TIENEECO = Pro.B_SOBRESELLADO == "1" ? Pro.B_PROPECO == "1" ? "SI" : "NO" : "NO",
                                     RADICADO = Rad.S_RADICADO + "-" + Rad.D_RADICADO.Year.ToString()
                                 });
                    modelData = model;
                    IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);
                    resultado.numRegistros = modelFiltered.Count();
                    if (skip == 0 && take == 0) resultado.datos = modelFiltered.ToList();
                    else resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

                    return resultado;
                }
                else
                {
                    resultado.numRegistros = 0;
                    resultado.datos = null;
                    return resultado;
                }
            }
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("ObtenerPropuesta")]
        public JObject GetPropuesta(string IdPropuesta)
        {
            JsonSerializer Js = new JsonSerializer();
            Js = JsonSerializer.CreateDefault();
            try
            {
                decimal _IdPropuesta = -1;
                if (IdPropuesta != null && IdPropuesta != "") _IdPropuesta = decimal.Parse(IdPropuesta);
                var Propuesta = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                                 join Proc in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Proc.ID_PROCESO
                                 join Fun in dbSIM.QRY_FUNCIONARIO_ALL on Proc.N_FUNCIONARIO_CONTRATO equals Fun.CODFUNCIONARIO
                                 where Ppta.ID_PROPUESTA == _IdPropuesta
                                 select new
                                 {
                                     Proc.S_NOMBRE,
                                     Ppta.ID_PROPUESTA,
                                     Ppta.S_PROPONENTE,
                                     Ppta.S_DIRECCION,
                                     Ppta.S_TELEFONO,
                                     Ppta.S_CORREO,
                                     Ppta.S_RESPONSABLE,
                                     Ppta.S_CORREORESP,
                                     Proc.B_SOBRESELLADO,
                                     Funcionario = Fun.NOMBRES,
                                     Ppta.ID_RADICADO,
                                     Ppta.CODTRAMITE,
                                     Fun.CODFUNCIONARIO,
                                     Ppta.S_ABIERTA_PPTA,
                                     Ppta.D_REGISTRO,
                                     Ppta.S_NIT
                                 }).FirstOrDefault();
                DatosPropuesta propuesta = new DatosPropuesta();
                if (Propuesta != null)
                {
                    propuesta.Proceso = Propuesta.S_NOMBRE;
                    propuesta.IdPropuesta = Propuesta.ID_PROPUESTA.ToString();
                    propuesta.Proponente = Propuesta.S_PROPONENTE;
                    propuesta.Direccion = Propuesta.S_DIRECCION;
                    propuesta.Telefono = Propuesta.S_TELEFONO;
                    propuesta.Correo = Propuesta.S_CORREO;
                    propuesta.Responsable = Propuesta.S_RESPONSABLE;
                    propuesta.CorreoResp = Propuesta.S_CORREORESP;
                    propuesta.Funcionario = Propuesta.Funcionario;
                    propuesta.SobreSellado = Propuesta.B_SOBRESELLADO == "1" ? "SI" : "NO";
                    propuesta.Tramite = Propuesta.CODTRAMITE.ToString();
                    propuesta.Fecha = Propuesta.D_REGISTRO.ToString("yyyy-MM-dd HH:mm:ss");
                    propuesta.Documento = Propuesta.S_NIT != null ? Propuesta.S_NIT : "";
                    if (Propuesta.S_ABIERTA_PPTA == "1") propuesta.ArchivoPropuesta = "Documento ya abierto";
                    else propuesta.ArchivoPropuesta = "Documento no ha sido abierto";
                    if (Propuesta.ID_RADICADO > 0)
                    {
                        var Radicado = (from Rad in dbSIM.RADICADO_DOCUMENTO
                                        where Rad.ID_RADICADODOC == Propuesta.ID_RADICADO
                                        select new
                                        {
                                            Rad.S_RADICADO,
                                            Rad.D_RADICADO
                                        }).FirstOrDefault();
                        if (Radicado != null)
                            propuesta.Radicado = Radicado.S_RADICADO + "-" + Radicado.D_RADICADO.Year.ToString();
                    }
                }
                return JObject.FromObject(propuesta, Js);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Marca la propuesta como abierta antes verificando el correcto formato del documento
        /// </summary>
        /// <param name="IdPropuesta">Identificador de la propuesta</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("AbrePropuesta")]
        public RptaSubir AbrePropuesta(long IdPropuesta)
        {
            RptaSubir Rpta = new RptaSubir();
            if (IdPropuesta == 0)
            {
                Rpta.SubirCorrecto = "Error";
                Rpta.Mensaje = "No se ingresó un codigo de propuesta";
                return Rpta;
            }
            var Propuesta = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                             join Pro in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Pro.ID_PROCESO
                             where Ppta.ID_PROPUESTA == IdPropuesta && Ppta.S_ABIERTA_PPTA == "0"
                             select new
                             {
                                 Pro.B_SOBRESELLADO,
                                 Pro.D_APERTURA,
                                 Ppta.ARCHIVOPROPUESTA
                             }).FirstOrDefault();
            if (Propuesta != null)
            {
                if (Propuesta.B_SOBRESELLADO == "1")
                {
                    if (Propuesta.D_APERTURA < DateTime.Now)
                    {
                        if (Propuesta.ARCHIVOPROPUESTA.Length > 0)
                        {
                            try
                            {
                                PDFDocument Origen = new PDFDocument(new MemoryStream(Propuesta.ARCHIVOPROPUESTA), Encoding.ASCII.GetBytes("Cl4V353gur4Amv4"));
                                Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                                Origen.Dispose();
                                var Prop = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS where Ppta.ID_PROPUESTA == IdPropuesta select Ppta).FirstOrDefault();
                                Prop.S_ABIERTA_PPTA = "1";
                                dbSIM.SaveChanges();
                                Rpta.SubirCorrecto = "Ok";
                                Rpta.Mensaje = "Documento abierto correctamente!!";
                            }
                            catch
                            {
                                Rpta.SubirCorrecto = "Error";
                                Rpta.Mensaje = "Ocurrió un problema al abrir el documento";
                            }
                        }
                        else
                        {
                            Rpta.SubirCorrecto = "Error";
                            Rpta.Mensaje = "Ocurrió un problema leyendo el documento de la propuesta";
                        }
                    }
                    else
                    {
                        Rpta.SubirCorrecto = "Error";
                        Rpta.Mensaje = "Aún no es la fecha establecida para la apertura de la propuesta";
                    }
                }
                else
                {
                    Rpta.SubirCorrecto = "Error";
                    Rpta.Mensaje = "El proceso no es de sobre sellado";
                }
            }
            else
            {
                Rpta.SubirCorrecto = "Error";
                Rpta.Mensaje = "No se encontró la propuesta en el sistema";
            }
            return Rpta;
        }

        /// <summary>
        /// Realiza el proceso de apertura de la propuesta economica del proponente
        /// </summary>
        /// <param name="IdPropuesta"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("AbrePropEco")]
        public RptaSubir AbrePropuestaEco(long IdPropuesta)
        {
            RptaSubir Rpta = new RptaSubir();
            if (IdPropuesta == 0)
            {
                Rpta.SubirCorrecto = "Error";
                Rpta.Mensaje = "No se ingresó un codigo de propuesta";
                return Rpta;
            }
            var Propuesta = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                             join Pro in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Pro.ID_PROCESO
                             where Ppta.ID_PROPUESTA == IdPropuesta && Ppta.S_ABIERTA_ECO == "0"
                             select new
                             {
                                 Pro.B_SOBRESELLADO,
                                 Pro.D_PROPECONOMICA,
                                 Ppta.ARCHICOECONOMICA
                             }).FirstOrDefault();
            if (Propuesta != null)
            {
                if (Propuesta.B_SOBRESELLADO == "1")
                {
                    if (Propuesta.D_PROPECONOMICA < DateTime.Now)
                    {
                        if (Propuesta.ARCHICOECONOMICA.Length > 0)
                        {
                            try
                            {
                                PDFDocument Origen = new PDFDocument(new MemoryStream(Propuesta.ARCHICOECONOMICA), Encoding.ASCII.GetBytes("Cl4V353gur4Amv4"));
                                Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                                Origen.Dispose();
                                var Prop = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS where Ppta.ID_PROPUESTA == IdPropuesta select Ppta).FirstOrDefault();
                                Prop.S_ABIERTA_ECO = "1";
                                dbSIM.SaveChanges();
                                Rpta.SubirCorrecto = "Ok";
                                Rpta.Mensaje = "La propuesta económica se abrió correctamente!!";
                            }
                            catch
                            {
                                Rpta.SubirCorrecto = "Error";
                                Rpta.Mensaje = "Ocurrió un problema al abrir el documento";
                            }
                        }
                        else
                        {
                            Rpta.SubirCorrecto = "Error";
                            Rpta.Mensaje = "Ocurrió un problema leyendo el documento de la propuesta";
                        }
                    }
                    else
                    {
                        Rpta.SubirCorrecto = "Error";
                        Rpta.Mensaje = "Aún no es la fecha establecida para la apertura de la propuesta";
                    }
                }
                else
                {
                    Rpta.SubirCorrecto = "Error";
                    Rpta.Mensaje = "El proceso no es de sobre sellado";
                }
            }
            else
            {
                Rpta.SubirCorrecto = "Error";
                Rpta.Mensaje = "No se encontró la propuesta en el sistema";
            }
            return Rpta;
        }

        /// <summary>
        /// Sube al SIM el documento correspondiente a la propuesta
        /// </summary>
        /// <param name="IdPropuesta"></param>
        /// <returns></returns>
        private RptaSubir SubePropuesta(decimal IdPropuesta)
        {
            RptaSubir Rpta = new RptaSubir();
            Utilidades.Radicador _CtrRadDoc = new Utilidades.Radicador();
            if (IdPropuesta == 0)
            {
                Rpta.SubirCorrecto = "Error";
                Rpta.Mensaje = "No se ingresó un codigo de propuesta";
                return Rpta;
            }
            var Propuesta = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                             join Pro in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Pro.ID_PROCESO
                             where Ppta.ID_PROPUESTA == IdPropuesta && Ppta.S_ABIERTA_PPTA == "1"
                             select new
                             {
                                 Pro.B_SOBRESELLADO,
                                 Pro.D_APERTURA,
                                 Ppta.CODTRAMITE,
                                 Ppta.ID_RADICADO,
                                 Ppta.ARCHIVOPROPUESTA,
                                 Ppta.S_PROPONENTE,
                                 Ppta.S_CORREO,
                                 Pro.S_NOMBRE,
                                 Pro.S_OBJETO,
                                 Pro.ID_PROCESO
                             }).FirstOrDefault();
            if (Propuesta != null)
            {
                var Radicado = (from Rad in dbSIM.RADICADO_DOCUMENTO where Rad.ID_RADICADODOC == Propuesta.ID_RADICADO select new { Rad.CODTRAMITE, Rad.CODDOCUMENTO }).FirstOrDefault();
                if (Radicado != null)
                {
                    if (Radicado.CODTRAMITE == null && Radicado.CODDOCUMENTO == null)
                    {
                        var _IdTareaPropuestas = (from Tar in dbSIM.TBTRAMITETAREA where Tar.CODTRAMITE == Propuesta.CODTRAMITE orderby Tar.ORDEN select Tar.CODTAREA).Max();
                        var _UnidadDocCOR = (from Uni in dbSIM.RADICADO_DOCUMENTO where Uni.ID_RADICADODOC == Propuesta.ID_RADICADO select new { Uni.CODSERIE, Uni.CODFUNCIONARIO }).FirstOrDefault();
                        var _Asunto = "PROPUESTA AL PROCESO " + Propuesta.S_NOMBRE.Trim().ToUpper() + " - " + Propuesta.S_OBJETO.Trim().ToUpper();
                        if (Propuesta.ARCHIVOPROPUESTA.Length > 0)
                        {
                            PDFDocument Origen = new PDFDocument(new MemoryStream(Propuesta.ARCHIVOPROPUESTA), Encoding.ASCII.GetBytes("Cl4V353gur4Amv4"));
                            Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                            Origen.SecurityManager = null;
                            MemoryStream imagenEtiqueta = new MemoryStream();
                            var imagenRadicado = _CtrRadDoc.ObtenerImagenRadicadoArea((int)Propuesta.ID_RADICADO);
                            if (imagenRadicado != null)
                            {
                                PDFPage _pag = Origen.Pages[0];
                                _pag.Canvas.DrawImage((Bitmap)imagenRadicado, 300, 30, 288, 72);
                                MemoryStream _Out = new MemoryStream();
                                int _Paginas = Origen.Pages.Count;
                                var _RadDoc = (from Rad in dbSIM.RADICADO_DOCUMENTO where Rad.ID_RADICADODOC == Propuesta.ID_RADICADO select Rad).FirstOrDefault();
                                if (_RadDoc != null)
                                {
                                    Origen.Save(_Out);
                                    _Out.Seek(0, SeekOrigin.Begin);
                                    _RadDoc.CODDOCUMENTO = SubirDocumento(_Out, Propuesta.CODTRAMITE, _IdTareaPropuestas.ToString(), (int)_UnidadDocCOR.CODSERIE, _Paginas, _UnidadDocCOR.CODFUNCIONARIO);
                                    _RadDoc.CODTRAMITE = Propuesta.CODTRAMITE;
                                    dbSIM.SaveChanges();
                                    IngresaIndicesPropuesta(Propuesta.CODTRAMITE, (long)_RadDoc.CODDOCUMENTO, _RadDoc.S_RADICADO, _RadDoc.D_RADICADO, _Asunto, Propuesta.S_PROPONENTE, Propuesta.S_CORREO, (long)_UnidadDocCOR.CODSERIE);
                                    Rpta.SubirCorrecto = "Ok";
                                    Rpta.Mensaje = "El documento fué subido correctamente al servidor con el Radicado " + _RadDoc.S_RADICADO + ". Código de trámite " + Propuesta.CODTRAMITE;
                                    var Proceso = (from Pro in dbSIM.PRECONTRATO_PROCESO where Pro.ID_PROCESO == Propuesta.ID_PROCESO select Pro).FirstOrDefault();
                                    Utilidades.LogEventos.GuardaEvento("Administracion de Propuestas", (long)Proceso.N_FUNCIONARIO_CONTRATO, "Se subió el documento de la propuesta del proponente " + Propuesta.S_PROPONENTE.Trim() + " del proceso " + Proceso.S_NOMBRE + " por parte del funcionario " + Proceso.N_FUNCIONARIO_CONTRATO + " con trámite " + Propuesta.CODTRAMITE + " y radicado " + _RadDoc.S_RADICADO);
                                }
                                else
                                {
                                    Rpta.SubirCorrecto = "Error";
                                    Rpta.Mensaje = "Ocurrió un problema con el Radicado ";
                                }
                            }
                        }
                        else
                        {
                            Rpta.SubirCorrecto = "Error";
                            Rpta.Mensaje = "Ocurrió un problema leyendo el documento de la propuesta";
                        }
                    }
                    else
                    {
                        Rpta.SubirCorrecto = "Ok";
                    }
                }
                else
                {
                    Rpta.SubirCorrecto = "Error";
                    Rpta.Mensaje = "Ocurrió un problema con el radicado del documento de la propuesta";
                }

            }
            else
            {
                Rpta.SubirCorrecto = "Error";
                Rpta.Mensaje = "No se encontró la propuesta en el sistema";
            }
            return Rpta;
        }

        /// <summary>
        /// Sube a gestión documental del SIM el documento correspondiente a la propuesta
        /// </summary>
        /// <param name="IdPropuesta"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("RegistraPropuestas")]
        public RptaRegistro RegistraPropuestas(long IdProceso)
        {
            RptaRegistro Rpta = new RptaRegistro();
            Rpta.RegistroCorrecto = true;
            if (IdProceso == 0)
            {
                Rpta.RegistroCorrecto = false;
                Rpta.Mensaje = "No se ingresó un codigo de proceso";
                return Rpta;
            }
            var Propuestas = (from Proc in dbSIM.PRECONTRATO_PROCESO
                              join Props in dbSIM.PRECONTRATO_PROCPROPUESTAS on Proc.ID_PROCESO equals Props.ID_PROCESO
                              where Props.S_ABIERTA_PPTA == "1" && Proc.ID_PROCESO == IdProceso
                              select new { Props.ID_PROPUESTA, Props.S_ABIERTA_ECO }).ToList();
            if (Propuestas != null)
            {
                try
                {
                    RptaSubir RptaSubir = new RptaSubir();
                    foreach (var propuesta in Propuestas)
                    {
                        RptaSubir = SubePropuesta(propuesta.ID_PROPUESTA);
                        if (RptaSubir.SubirCorrecto != "Ok")
                        {
                            Rpta.RegistroCorrecto = false;
                            Rpta.Mensaje = RptaSubir.Mensaje;
                            break;
                        }
                        if (propuesta.S_ABIERTA_ECO == "1")
                        {
                            RptaSubir = SubeEconomica(propuesta.ID_PROPUESTA);
                            if (RptaSubir.SubirCorrecto != "Ok")
                            {
                                Rpta.RegistroCorrecto = false;
                                Rpta.Mensaje = RptaSubir.Mensaje;
                                break;
                            }
                        }
                    }
                    if (Rpta.RegistroCorrecto)
                    {
                        var Proceso = (from Proc in dbSIM.PRECONTRATO_PROCESO where Proc.ID_PROCESO == IdProceso select Proc).FirstOrDefault();
                        Proceso.D_PASOSIM = DateTime.Now;
                        dbSIM.SaveChanges();
                        Rpta.Mensaje = "Los documentos del proceso fueron pasados al sistema de Gestión Documental del SIM!!";
                    }
                }
                catch (Exception ex)
                {
                    Rpta.RegistroCorrecto = false;
                    Rpta.Mensaje = ex.Message;
                }
            }
            return Rpta;
        }
        /// <summary>
        /// Sube a gestión documental del SIM el documento correspondiente a la propuesta económica
        /// </summary>
        /// <param name="IdPropuesta"></param>
        /// <returns></returns>
        private RptaSubir SubeEconomica(decimal IdPropuesta)
        {
            RptaSubir Rpta = new RptaSubir();
            Utilidades.Radicador _CtrRadDoc = new Utilidades.Radicador();
            if (IdPropuesta == 0)
            {
                Rpta.SubirCorrecto = "Error";
                Rpta.Mensaje = "No se ingresó un codigo de propuesta";
                return Rpta;
            }
            var Propuesta = (from Ppta in dbSIM.PRECONTRATO_PROCPROPUESTAS
                             join Pro in dbSIM.PRECONTRATO_PROCESO on Ppta.ID_PROCESO equals Pro.ID_PROCESO
                             where Ppta.ID_PROPUESTA == IdPropuesta && Ppta.S_ABIERTA_ECO == "1"
                             select new
                             {
                                 Pro.B_SOBRESELLADO,
                                 Pro.D_PROPECONOMICA,
                                 Ppta.CODTRAMITE,
                                 Ppta.ID_RADECO,
                                 Ppta.ARCHICOECONOMICA,
                                 Ppta.S_PROPONENTE,
                                 Ppta.S_CORREO,
                                 Pro.N_FUNCIONARIO_CONTRATO,
                                 Pro.S_NOMBRE,
                                 Pro.S_OBJETO,
                                 Pro.ID_PROCESO
                             }).FirstOrDefault();
            if (Propuesta != null)
            {
                var Radicado = (from Rad in dbSIM.RADICADO_DOCUMENTO where Rad.ID_RADICADODOC == Propuesta.ID_RADECO select new { Rad.CODTRAMITE, Rad.CODDOCUMENTO }).FirstOrDefault();
                if (Radicado != null)
                {
                    if (Radicado.CODTRAMITE == null && Radicado.CODDOCUMENTO == null)
                    {
                        int _UnidadDoc = int.Parse(SIM.Utilidades.Data.ObtenerValorParametro("UnidadDocPQRSD").ToString());
                        var _IdTareaPropuestas = (from Tar in dbSIM.TBTRAMITETAREA where Tar.CODTRAMITE == Propuesta.CODTRAMITE orderby Tar.ORDEN select Tar.CODTAREA).Max();
                        var _Asunto = "PROPUESTA AL PROCESO " + Propuesta.S_NOMBRE.Trim().ToUpper();
                        var _IdUsuario = (from Usu in dbSIM.USUARIO_FUNCIONARIO where Usu.CODFUNCIONARIO == Propuesta.N_FUNCIONARIO_CONTRATO select Usu.ID_USUARIO).FirstOrDefault();
                        if (Propuesta.ARCHICOECONOMICA.Length > 0)
                        {
                            PDFDocument Origen = new PDFDocument(new MemoryStream(Propuesta.ARCHICOECONOMICA), Encoding.ASCII.GetBytes("Cl4V353gur4Amv4"));
                            Origen.SerialNumber = "PDF4NET-ACT46-D7HHE-OYPAB-ILSOD-TMYDA";
                            Origen.SecurityManager = null;
                            MemoryStream imagenEtiqueta = new MemoryStream();
                            DateTime fechaCreacion = DateTime.Now;
                            Radicador radicador = new Radicador();
                            DatosRadicado radicadoGenerado = radicador.GenerarRadicado(dbSIM, _UnidadDoc, _IdUsuario, fechaCreacion);
                            int _IdRadicado = radicadoGenerado.IdRadicado;
                            var imagenRadicado = _CtrRadDoc.ObtenerImagenRadicadoArea(_IdRadicado);
                            if (imagenRadicado != null)
                            {
                                PDFPage _pag = Origen.Pages[0];
                                _pag.Canvas.DrawImage((Bitmap)imagenRadicado, 300, 30, 288, 72);
                                MemoryStream _Out = new MemoryStream();
                                int _Paginas = Origen.Pages.Count;
                                var _RadDoc = (from Rad in dbSIM.RADICADO_DOCUMENTO where Rad.ID_RADICADODOC == radicadoGenerado.IdRadicado select Rad).FirstOrDefault();
                                if (_RadDoc != null)
                                {
                                    Origen.Save(_Out);
                                    _RadDoc.CODTRAMITE = Propuesta.CODTRAMITE;
                                    _RadDoc.CODDOCUMENTO = SubirDocumento(_Out, Propuesta.CODTRAMITE, _IdTareaPropuestas.ToString(), _UnidadDoc, _Paginas, Propuesta.N_FUNCIONARIO_CONTRATO);
                                    dbSIM.Entry(_RadDoc).State = System.Data.Entity.EntityState.Modified;
                                    dbSIM.SaveChanges();
                                    IngresaIndicesPropuesta(Propuesta.CODTRAMITE, (long)_RadDoc.CODDOCUMENTO, _RadDoc.S_RADICADO, _RadDoc.D_RADICADO, _Asunto, Propuesta.S_PROPONENTE, Propuesta.S_CORREO, _UnidadDoc);
                                    Rpta.SubirCorrecto = "Ok";
                                    Rpta.Mensaje = "El documento fué subido correctamente con el Radicado " + _RadDoc.S_RADICADO + ". Código de trámite " + Propuesta.CODTRAMITE;
                                    Utilidades.LogEventos.GuardaEvento("Administracion de Propuestas", (long)Propuesta.N_FUNCIONARIO_CONTRATO, "Se subió el documento de la propuesta económica del proponente " + Propuesta.S_PROPONENTE.Trim() + " del proceso " + Propuesta.S_NOMBRE + " por parte del funcionario " + Propuesta.N_FUNCIONARIO_CONTRATO + " con trámite " + Propuesta.CODTRAMITE + " y radicado " + _RadDoc.S_RADICADO);
                                }
                                else
                                {
                                    Rpta.SubirCorrecto = "Error";
                                    Rpta.Mensaje = "Ocurrió un problema generando el Radicado ";
                                }
                            }
                        }
                        else
                        {
                            Rpta.SubirCorrecto = "Error";
                            Rpta.Mensaje = "Ocurrió un problema leyendo el documento de la propuesta económica";
                        }
                    }
                    else
                    {
                        Rpta.SubirCorrecto = "Ok";
                    }
                }
                else
                {
                    Rpta.SubirCorrecto = "Error";
                    Rpta.Mensaje = "Ocurrió un problema con el radicado del documento de la propuesta económica";
                }
            }
            else
            {
                Rpta.SubirCorrecto = "Error";
                Rpta.Mensaje = "No se encontró la propuesta en el sistema";
            }
            return Rpta;
        }

        [System.Web.Http.HttpGet, System.Web.Http.ActionName("LeePropuesta")]
        public async Task<byte[]> GetArchivo(long Propuesta)
        {
            if (Propuesta == 0) return null;
            MemoryStream oStream = null;
            var Prop = (from Pro in dbSIM.PRECONTRATO_PROCPROPUESTAS
                        join Rad in dbSIM.RADICADO_DOCUMENTO on Pro.ID_RADICADO equals Rad.ID_RADICADODOC
                        where Pro.ID_PROPUESTA == Propuesta
                        select new
                        {
                            Rad.CODTRAMITE,
                            Rad.CODDOCUMENTO
                        }).FirstOrDefault();
            if (Prop != null) oStream = await SIM.Utilidades.Archivos.AbrirDocumento((long)Prop.CODTRAMITE, (long)Prop.CODDOCUMENTO);
            if (oStream != null && oStream.Length > 0)
            {
                oStream.Position = 0;
                return oStream.ToArray();
            }
            return null;
        }

        private decimal SubirDocumento(MemoryStream _Doc, decimal _Codtramite, string IdActividad, int _TipoDoc, int _Paginas, decimal CodFuncionario)
        {
            var Proceso = (from Pro in dbSIM.TBTRAMITE where Pro.CODTRAMITE == _Codtramite select Pro.CODPROCESO).FirstOrDefault();
            TBTRAMITEDOCUMENTO tramiteDocumento = new TBTRAMITEDOCUMENTO();
            tramiteDocumento.CODTRAMITE = _Codtramite;
            tramiteDocumento.CODDOCUMENTO = ObtenerIdDocumento(_Codtramite);
            tramiteDocumento.TIPODOCUMENTO = 1;
            tramiteDocumento.CODFUNCIONARIO = CodFuncionario;
            tramiteDocumento.CODSERIE = _TipoDoc;
            tramiteDocumento.NOMBRE = "00000000" + tramiteDocumento.CODDOCUMENTO.ToString();
            tramiteDocumento.MAPAARCHIVO = "M";
            tramiteDocumento.PAGINAS = _Paginas;
            if (_Doc.Length > 0)
            {
                _Doc.Seek(0, SeekOrigin.Begin);
                tramiteDocumento.RUTA = Utilidades.Archivos.SubirDocumentoServidorSinCifrar(_Doc, "pdf", _Codtramite.ToString(), (long)Proceso, (int)tramiteDocumento.CODDOCUMENTO);
            }
            tramiteDocumento.CIFRADO = "0";
            tramiteDocumento.ACTIVIDAD = IdActividad;
            dbSIM.TBTRAMITEDOCUMENTO.Add(tramiteDocumento);
            dbSIM.SaveChanges();
            TBTRAMITE_DOC relaDocTra = new TBTRAMITE_DOC();
            relaDocTra.CODDOCUMENTO = tramiteDocumento.CODDOCUMENTO;
            relaDocTra.CODTRAMITE = _Codtramite;
            relaDocTra.ID_DOCUMENTO = tramiteDocumento.ID_DOCUMENTO;
            dbSIM.TBTRAMITE_DOC.Add(relaDocTra);
            dbSIM.SaveChanges();
            return tramiteDocumento.CODDOCUMENTO;
        }

        public decimal ObtenerIdDocumento(decimal IdTramite)
        {
            try
            {
                var MaxDoc = (from Doc in dbSIM.TBTRAMITEDOCUMENTO
                              where Doc.CODTRAMITE == IdTramite
                              select Doc.CODDOCUMENTO).DefaultIfEmpty(0).Max();
                if (MaxDoc > 0) return MaxDoc + 1;
                else return 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        private void IngresaIndicesPropuesta(decimal _Codtramite, long _CodDocumento, string _Radicado, DateTime _Fecha, string _Asunto, string Proponente, string Correo, long UnidadDoc)
        {
            try
            {
                decimal _IndiceRadicado = decimal.Parse(SIM.Utilidades.Data.ObtenerValorParametro("IdIndiceRadPQRSD").ToString());
                decimal _IndiceFecha = decimal.Parse(SIM.Utilidades.Data.ObtenerValorParametro("IdIndiceFechaPQRSD").ToString());
                decimal _IndiceAsunto = decimal.Parse(SIM.Utilidades.Data.ObtenerValorParametro("IdIndiceAsuntoPQRSD").ToString());
                decimal _IndiceRemite = decimal.Parse(SIM.Utilidades.Data.ObtenerValorParametro("IdIndiceRemitePQRSD").ToString());
                decimal _IdIndiceHora = decimal.Parse(SIM.Utilidades.Data.ObtenerValorParametro("IdIndiceHoraPQRSD").ToString());
                decimal _IdIndiceMail = decimal.Parse(SIM.Utilidades.Data.ObtenerValorParametro("IdIndiceMailPQRSD").ToString());
                TBINDICEDOCUMENTO indiceDocumento = new TBINDICEDOCUMENTO();

                if (_IndiceRadicado > 0 && _Radicado.Length > 0)
                {
                    indiceDocumento.CODTRAMITE = (int)_Codtramite;
                    indiceDocumento.CODDOCUMENTO = (int)_CodDocumento;
                    indiceDocumento.CODINDICE = (int)_IndiceRadicado;
                    indiceDocumento.CODSERIE = (int)UnidadDoc;
                    indiceDocumento.VALOR = _Radicado;
                    indiceDocumento.ID_DOCUMENTO = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.CODTRAMITE.Equals(_Codtramite) && w.CODDOCUMENTO.Equals(_CodDocumento)).Select(s => s.ID_DOCUMENTO).FirstOrDefault();
                    dbSIM.TBINDICEDOCUMENTO.Add(indiceDocumento);
                }
                if (_IndiceFecha > 0 && _Fecha.Year > 1900)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODTRAMITE = (int)_Codtramite;
                    indiceDocumento.CODDOCUMENTO = (int)_CodDocumento;
                    indiceDocumento.CODINDICE = (int)_IndiceFecha;
                    indiceDocumento.VALOR = _Fecha.ToString("dd-MM-yyyy"); ;
                    indiceDocumento.CODSERIE = (int)UnidadDoc;
                    indiceDocumento.ID_DOCUMENTO = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.CODTRAMITE.Equals(_Codtramite) && w.CODDOCUMENTO.Equals(_CodDocumento)).Select(s => s.ID_DOCUMENTO).FirstOrDefault();
                    dbSIM.TBINDICEDOCUMENTO.Add(indiceDocumento);
                }
                if (_IdIndiceHora > 0 && _Fecha.Year > 1900)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODTRAMITE = (int)_Codtramite;
                    indiceDocumento.CODDOCUMENTO = (int)_CodDocumento;
                    indiceDocumento.CODINDICE = (int)_IdIndiceHora;
                    indiceDocumento.VALOR = _Fecha.ToString("HH:mm"); ;
                    indiceDocumento.CODSERIE = (int)UnidadDoc;
                    indiceDocumento.ID_DOCUMENTO = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.CODTRAMITE.Equals(_Codtramite) && w.CODDOCUMENTO.Equals(_CodDocumento)).Select(s => s.ID_DOCUMENTO).FirstOrDefault();
                    dbSIM.TBINDICEDOCUMENTO.Add(indiceDocumento);
                }
                if (_IndiceAsunto > 0 && _Asunto.Length > 0)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODTRAMITE = (int)_Codtramite;
                    indiceDocumento.CODDOCUMENTO = (int)_CodDocumento;
                    indiceDocumento.CODINDICE = (int)_IndiceAsunto;
                    indiceDocumento.CODSERIE = (int)UnidadDoc;
                    indiceDocumento.VALOR = _Asunto.ToUpper();
                    indiceDocumento.ID_DOCUMENTO = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.CODTRAMITE.Equals(_Codtramite) && w.CODDOCUMENTO.Equals(_CodDocumento)).Select(s => s.ID_DOCUMENTO).FirstOrDefault();
                    dbSIM.TBINDICEDOCUMENTO.Add(indiceDocumento);
                }
                if (_IndiceRemite > 0 && Proponente.Length > 0)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODTRAMITE = (int)_Codtramite;
                    indiceDocumento.CODDOCUMENTO = (int)_CodDocumento;
                    indiceDocumento.CODINDICE = (int)_IndiceRemite;
                    indiceDocumento.CODSERIE = (int)UnidadDoc;
                    indiceDocumento.VALOR = Proponente.ToUpper();
                    indiceDocumento.ID_DOCUMENTO = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.CODTRAMITE.Equals(_Codtramite) && w.CODDOCUMENTO.Equals(_CodDocumento)).Select(s => s.ID_DOCUMENTO).FirstOrDefault();
                    dbSIM.TBINDICEDOCUMENTO.Add(indiceDocumento);
                }
                if (_IdIndiceMail > 0 && Correo.Length > 0)
                {
                    indiceDocumento = new TBINDICEDOCUMENTO();
                    indiceDocumento.CODTRAMITE = (int)_Codtramite;
                    indiceDocumento.CODDOCUMENTO = (int)_CodDocumento;
                    indiceDocumento.CODINDICE = (int)_IdIndiceMail;
                    indiceDocumento.CODSERIE = (int)UnidadDoc;
                    indiceDocumento.VALOR = Correo.ToUpper();
                    indiceDocumento.ID_DOCUMENTO = dbSIM.TBTRAMITEDOCUMENTO.Where(w => w.CODTRAMITE.Equals(_Codtramite) && w.CODDOCUMENTO.Equals(_CodDocumento)).Select(s => s.ID_DOCUMENTO).FirstOrDefault();
                    dbSIM.TBINDICEDOCUMENTO.Add(indiceDocumento);
                }
                dbSIM.SaveChanges();
            }
            catch { }
        }
    }
    /// <summary>
    /// Objeto para recibir los datos de la propeusta
    /// </summary>
    public class DatosProceso
    {
        public string IdProceso { get; set; }
        public string Nombre { get; set; }
        public string Objeto { get; set; }
        public string FechaInicio { get; set; }
        public string FechaCierre { get; set; }
        public string Modalidad { get; set; }
        public string SobreSellado { get; set; }
        public string PptaEconomica { get; set; }
        public string FechaApertura { get; set; }
        public string FechaApeEco { get; set; }
        public string Funcionario { get; set; }
        public string Propuestas { get; set; }
        public string Subidas { get; set; }
        public string FechaSIM { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DatosPropuesta
    {
        public string Proceso { get; set; }
        public string IdPropuesta { get; set; }
        public string Proponente { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Responsable { get; set; }
        public string CorreoResp { get; set; }
        public string ArchivoPropuesta { get; set; }
        public string ArchivoEconomica { get; set; }
        public string Radicado { get; set; }
        public string Tramite { get; set; }
        public string Funcionario { get; set; }
        public string SobreSellado { get; set; }
        public string Fecha { get; set; }
        public string Documento { get; set; }
    }
}
