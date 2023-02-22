using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SIM.Areas.General.Models;
using SIM.Areas.Seguridad.Models;
using SIM.Data;
using Newtonsoft.Json;
using System.Data.Entity.SqlServer;
using System.Security.Claims;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using SIM.Data.General;

namespace SIM.Areas.General.Controllers
{
    public class DGAApiController : ApiController
    {
        public class TERCERODGA
        {
            public int ID_PERSONALDGA;
            public int ID_DGA;
            public int ID_TERCERO;
            public string S_TIPOPERSONAL;
            public string S_ESRESPONSABLE;
            public Byte? N_DEDICACION;
            public short? N_EXPERIENCIA;
            public string S_OBSERVACION;
            public int ID_TIPOPERSONAL;
            public TERCERONATURAL TERCERO;
        }

        public class TERCERONATURAL
        {
            public int? ID_TIPODOCUMENTO;
            public long? N_DOCUMENTON;
            public short? N_DIGITOVER;
            public string S_NOMBRE1;
            public string S_NOMBRE2;
            public string S_APELLIDO1;
            public string S_APELLIDO2;
            public string S_GENERO;
            public DateTime? D_NACIMIENTO;
            public int? ID_ACTIVIDADECONOMICA;
            public long? N_TELEFONO;
            public string S_CORREO;
            public decimal? ID_PROFESION;
        }

        public struct datosConsulta
        {
            public int numRegistros;
            public IEnumerable<dynamic> datos;
            public bool administrador;
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

        [Authorize(Roles = "VDGA")]
        [HttpGet, ActionName("DGAs")]
        public datosConsulta GetDGAs(string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            dynamic modelData;
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTerceroUsuario = null;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool administrador;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            administrador = false;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                administrador = claimPpal.IsInRole("XDGA");
            }

            if (claimTercero != null)
            {
                idTerceroUsuario = int.Parse(claimTercero.Value);
            }

            if (((filter == null || filter == "") && (searchValue == "" || searchValue == null) && noFilterNoRecords) || (!administrador && idTerceroUsuario == null))
            {
                datosConsulta resultado = new datosConsulta();
                resultado.numRegistros = 0;
                resultado.datos = null;

                return resultado;
            }
            else
            {
                if (administrador)
                {
                    var model = (from dga in dbSIM.DGA
                                 join tercero in dbSIM.TERCERO on dga.ID_TERCERO equals tercero.ID_TERCERO
                                 orderby dga.D_ANO, dga.ID_DGA ascending
                                 select new
                                 {
                                     ID_TERCERO = dga.ID_TERCERO,
                                     S_TIPO_DOCUMENTO = tercero.TIPO_DOCUMENTO.S_ABREVIATURA,
                                     tercero.N_DOCUMENTON,
                                     S_TERCERO = tercero.S_RSOCIAL,
                                     ID_DGA = dga.ID_DGA,
                                     D_ANO = dga.D_ANO.Year,
                                     D_FREPORTE = dga.D_FREPORTE,
                                     ID_ESTADO = dga.ID_ESTADO,
                                     S_ESTADO = dga.ESTADO.S_NOMBRE,
                                     //S_A = true // Es Administrador
                                 });
                    modelData = model;
                }
                else if (idTerceroUsuario != null)
                {
                    var model = (from dga in dbSIM.DGA
                                 join tercero in dbSIM.TERCERO on dga.ID_TERCERO equals tercero.ID_TERCERO
                                 where dga.ID_TERCERO == idTerceroUsuario
                                 orderby dga.D_ANO, dga.ID_DGA ascending
                                 select new
                                 {
                                     ID_TERCERO = dga.ID_TERCERO,
                                     S_TIPO_DOCUMENTO = tercero.TIPO_DOCUMENTO.S_ABREVIATURA,
                                     tercero.N_DOCUMENTON,
                                     S_TERCERO = tercero.S_RSOCIAL,
                                     ID_DGA = dga.ID_DGA,
                                     D_ANO = dga.D_ANO.Year,
                                     D_FREPORTE = dga.D_FREPORTE,
                                     ID_ESTADO = dga.ID_ESTADO,
                                     S_ESTADO = dga.ESTADO.S_NOMBRE,
                                     //S_A = false // NO es Administrador
                                 });
                    modelData = model;
                }
                else
                {
                    modelData = null;
                }

                if (modelData != null)
                {
                    IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelData, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

                    datosConsulta resultado = new datosConsulta();
                    resultado.numRegistros = modelFiltered.Count();
                    resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();
                    resultado.administrador = administrador;

                    return resultado;
                }
                else
                {
                    datosConsulta resultado = new datosConsulta();
                    resultado.numRegistros = 0;
                    resultado.datos = null;
                    resultado.administrador = administrador;

                    return resultado;
                }
            }
        }

        [Authorize(Roles = "VDGA")]
        [HttpGet, ActionName("DGA")]
        public object GetDGA(int id)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTercero = null;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;
            bool administrador;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            administrador = false;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                administrador = claimPpal.IsInRole("XDGA");
            }

            if (claimTercero != null)
            {
                idTercero = int.Parse(claimTercero.Value);
            }

            if (id != 0 && (administrador || idTercero != null))
            {
                if (administrador)
                {
                    var model = (from dga in dbSIM.DGA
                                 where dga.ID_DGA == id
                                 select new
                                 {
                                     dga.ID_DGA,
                                     dga.ID_TERCERO,
                                     N_ANO = dga.D_ANO.Year,
                                     dga.N_ACTIVO,
                                     dga.N_EMPLEADOS,
                                     dga.S_FILIAL,
                                     dga.S_COMPARTEDGA,
                                     dga.S_COMPARTEEMPRESA,
                                     dga.S_AGREMIACION,
                                     dga.S_AGREMIACIONASESORIA,
                                     dga.S_FUNCION,
                                     dga.S_ESSGA,
                                     dga.S_SGA,
                                     dga.S_ESSGC,
                                     dga.S_SGC,
                                     dga.S_ESECOETIQUETADO,
                                     dga.S_ECOETIQUETADO,
                                     dga.S_PRODUCCIONMASLIMPIA,
                                     dga.S_SEGUIMIENTO,
                                     dga.N_INGRESOS,
                                     dga.N_VERSION,
                                     PERMISO_AMBIENTAL = from pa in dga.PERMISO_AMBIENTAL
                                                         select pa.ID_PERMISOAMBIENTAL
                                     /*PERMISO_AMBIENTAL = from permisoAmbiental in dbSIM.DGA
                                                         where permisoAmbiental. == dga.
                                                         select permisoAmbiental.ID_PERMISOAMBIENTAL*/
                                 }).FirstOrDefault();

                    if (model != null)
                    {
                        var path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + model.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + "Organigrama_" + model.ID_TERCERO.ToString() + "_" + model.N_ANO.ToString() + "TMP";

                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    }

                    return model;
                } else if (idTercero != null)
                {
                    var model = (from dga in dbSIM.DGA
                                 where dga.ID_DGA == id && dga.ID_TERCERO == idTercero
                                 select new
                                 {
                                     dga.ID_DGA,
                                     dga.ID_TERCERO,
                                     N_ANO = dga.D_ANO.Year,
                                     dga.N_ACTIVO,
                                     dga.N_EMPLEADOS,
                                     dga.S_FILIAL,
                                     dga.S_COMPARTEDGA,
                                     dga.S_COMPARTEEMPRESA,
                                     dga.S_AGREMIACION,
                                     dga.S_AGREMIACIONASESORIA,
                                     dga.S_FUNCION,
                                     dga.S_ESSGA,
                                     dga.S_SGA,
                                     dga.S_ESSGC,
                                     dga.S_SGC,
                                     dga.S_ESECOETIQUETADO,
                                     dga.S_ECOETIQUETADO,
                                     dga.S_PRODUCCIONMASLIMPIA,
                                     dga.S_SEGUIMIENTO,
                                     dga.N_INGRESOS,
                                     dga.N_VERSION,
                                     PERMISO_AMBIENTAL = from pa in dga.PERMISO_AMBIENTAL
                                                         select pa.ID_PERMISOAMBIENTAL
                                 }).FirstOrDefault();

                    if (model != null)
                    {
                        var path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + model.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + "Organigrama_" + model.ID_TERCERO.ToString() + "_" + model.N_ANO.ToString() + "TMP";

                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    }
                    return model;
                }

                return null;
            }
            else
            {
                var model = new
                {
                    ID_DGA = 0,
                    ID_TERCERO = 0
                };

                return model;
            }
        }

        // POST api/<controller>
        [HttpPost, ActionName("DGA")]
        public object PostDGA(DGA item, string permisosAmbientales)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            Claim claimTercero;
            object dgaRetorno;
            string[] permisos = (permisosAmbientales == null ? new string[0] : permisosAmbientales.Split(','));

            bool nuevo = false;
            var model = dbSIM.DGA;
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ID_DGA == 0) // Nuevo DGA
                    {
                        claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

                        item.ID_TERCERO = int.Parse(claimTercero.Value);
                        item.ID_ESTADO = 1;

                        item.S_COMPARTEDGA = item.S_COMPARTEDGA == null ? "N" : item.S_COMPARTEDGA;
                        item.S_AGREMIACION = item.S_AGREMIACION == null ? "N" : item.S_AGREMIACION;
                        item.S_ESSGA = item.S_ESSGA == null ? "N" : item.S_ESSGA;
                        item.S_ESSGC = item.S_ESSGC == null ? "N" : item.S_ESSGC;
                        item.S_ESECOETIQUETADO = item.S_ESECOETIQUETADO == null ? "N" : item.S_ESECOETIQUETADO;
                        item.S_PRODUCCIONMASLIMPIA = item.S_PRODUCCIONMASLIMPIA == null ? "N" : item.S_PRODUCCIONMASLIMPIA;

                        if (item.S_ORGANIGRAMA != null)
                        {
                            if (item.S_ORGANIGRAMA != "")
                                item.S_ORGANIGRAMA = "Organigrama_" + item.ID_TERCERO.ToString() + "_" + item.D_ANO.Year.ToString();
                            else
                                item.S_ORGANIGRAMA = null;
                        }

                        foreach (PERMISO_AMBIENTAL permisoAmbiental in ModelsToListGeneral.GetPermisosAmbientales(dbSIM))
                        {
                            if (permisos.Contains(permisoAmbiental.ID_PERMISOAMBIENTAL.ToString()))
                            {
                                item.PERMISO_AMBIENTAL.Add(permisoAmbiental);
                            }
                        }
                        item.N_VERSION = 2;

                        nuevo = true;
                        dbSIM.Entry(item).State = EntityState.Added;
                        dbSIM.SaveChanges();

                        var path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + item.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + "Organigrama_" + item.ID_TERCERO.ToString() + "_" + item.D_ANO.Year.ToString() + "TMP";

                        if (System.IO.File.Exists(path))
                            System.IO.File.Copy(path, path.Substring(0, path.Length - 3), true);
                    }
                    else // DGA Existente
                    {
                        var modelItem = model.FirstOrDefault(it => it.ID_DGA == item.ID_DGA);
                        if (modelItem != null)
                        {
                            modelItem.D_ANO = item.D_ANO;
                            modelItem.N_ACTIVO = item.N_ACTIVO;
                            modelItem.N_EMPLEADOS = item.N_EMPLEADOS;
                            modelItem.S_FILIAL = item.S_FILIAL;
                            modelItem.S_COMPARTEDGA = item.S_COMPARTEDGA == null ? "N" : item.S_COMPARTEDGA;
                            modelItem.S_COMPARTEEMPRESA = item.S_COMPARTEEMPRESA;
                            modelItem.S_AGREMIACION = item.S_AGREMIACION == null ? "N" : item.S_AGREMIACION;
                            modelItem.S_AGREMIACIONASESORIA = item.S_AGREMIACIONASESORIA;
                            modelItem.S_FUNCION = item.S_FUNCION;
                            modelItem.S_ESSGA = item.S_ESSGA == null ? "N" : item.S_ESSGA;
                            modelItem.S_SGA = item.S_SGA;
                            modelItem.S_ESSGC = item.S_ESSGC == null ? "N" : item.S_ESSGC;
                            modelItem.S_SGC = item.S_SGC;
                            modelItem.S_ESECOETIQUETADO = item.S_ESECOETIQUETADO == null ? "N" : item.S_ESECOETIQUETADO;
                            modelItem.S_ECOETIQUETADO = item.S_ECOETIQUETADO;
                            modelItem.S_PRODUCCIONMASLIMPIA = item.S_PRODUCCIONMASLIMPIA == null ? "N" : item.S_PRODUCCIONMASLIMPIA;
                            modelItem.S_SEGUIMIENTO = item.S_SEGUIMIENTO;
                            modelItem.N_INGRESOS = item.N_INGRESOS;
                            modelItem.N_VERSION = item.N_VERSION;

                            if (item.S_ORGANIGRAMA != null)
                            {
                                if (item.S_ORGANIGRAMA != "")
                                    modelItem.S_ORGANIGRAMA = "Organigrama_" + item.ID_TERCERO.ToString() + "_" + item.D_ANO.Year.ToString();
                                else
                                    modelItem.S_ORGANIGRAMA = null;
                            }

                            modelItem.PERMISO_AMBIENTAL.Clear();

                            foreach (PERMISO_AMBIENTAL permisoAmbiental in ModelsToListGeneral.GetPermisosAmbientales(dbSIM))
                            {
                                if (permisos.Contains(permisoAmbiental.ID_PERMISOAMBIENTAL.ToString()))
                                {
                                    modelItem.PERMISO_AMBIENTAL.Add(permisoAmbiental);
                                }
                            }

                            dbSIM.Entry(modelItem).State = EntityState.Modified;
                            dbSIM.SaveChanges();

                            var path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + modelItem.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + "Organigrama_" + modelItem.ID_TERCERO.ToString() + "_" + modelItem.D_ANO.Year.ToString() + "TMP";

                            if (System.IO.File.Exists(path))
                                System.IO.File.Copy(path, path.Substring(0, path.Length - 3), true);
                        }
                    }

                    dgaRetorno = GetDGA(item.ID_DGA);
                }
                catch (Exception e)
                {
                    return new { resp = "Error", mensaje = "Error Almacenando DGA" };
                }
                return new { resp = "OK", mensaje = "DGA Almacenado Satisfactoriamente", datos = dgaRetorno };
            }
            else
                return new { resp = "Error", mensaje = "Datos Inválidos.<br/><br/>Por favor verifique que todas las preguntas estén diligenciadas." };
        }

        [HttpGet, ActionName("AnularDGA")]
        public datosRespuesta AnularDGA(int id)
        {
            datosRespuesta respuesta;

            var dga = dbSIM.DGA.FirstOrDefault(idDGA => idDGA.ID_DGA == id);

            if (dga != null)
            {
                try
                {
                    /*var estado = (from estadoDGA in dbSIM.ESTADO
                                 where estadoDGA.ID_ESTADO == 6
                                 select estadoDGA).FirstOrDefault();

                    dga.ESTADO = estado;*/
                    dga.ID_ESTADO = 6;
                    dbSIM.Entry(dga).State = EntityState.Modified;
                    dbSIM.SaveChanges();

                    var report = new DGAReport();

                    string path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var modelProfesionales = from personalDGA in dbSIM.PERSONAL_DGA
                                             join tercero in dbSIM.TERCERO on personalDGA.ID_TERCERO equals tercero.ID_TERCERO
                                             join natural in dbSIM.NATURAL on tercero.ID_TERCERO equals natural.ID_TERCERO
                                             join profesion in dbSIM.PROFESION on natural.ID_PROFESION equals profesion.ID_PROFESION into JoinedProfesion
                                             from profesion in JoinedProfesion.DefaultIfEmpty()
                                             where personalDGA.ID_DGA == id
                                             select new
                                             {
                                                 personalDGA.ID_DGA,
                                                 personalDGA.ID_PERSONALDGA,
                                                 personalDGA.ID_TERCERO,
                                                 RAZON_SOCIAL = tercero.S_RSOCIAL,
                                                 N_DOCUMENTO = tercero.N_DOCUMENTON,
                                                 personalDGA.S_TIPOPERSONAL,
                                                 S_ESRESPONSABLE = personalDGA.S_ESRESPONSABLE == "S" ? "SI" : "NO",
                                                 personalDGA.N_DEDICACION,
                                                 personalDGA.N_EXPERIENCIA,
                                                 personalDGA.S_OBSERVACION,
                                                 CORREO_ELECTRONICO = tercero.S_CORREO,
                                                 TELEFONO = tercero.N_TELEFONO,
                                                 PROFESION = profesion.S_NOMBRE
                                             };

                    report.CargarDatos(dga, (modelProfesionales == null ? null : modelProfesionales.ToList()));
                    report.Watermark.Text = "OBSOLETO";
                    report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.BackwardDiagonal;
                    report.ExportToPdf(path + "\\DGA_" + id.ToString());
                    report.Dispose();

                    respuesta = new datosRespuesta();
                    respuesta.tipoRespuesta = "OK";

                    return respuesta;
                }
                catch (Exception error)
                {
                    respuesta = new datosRespuesta();
                    respuesta.tipoRespuesta = "Error";
                    respuesta.detalleRespuesta = "No se pudo Anular el DGA Seleccionado.";

                    return respuesta;
                }
            }

            respuesta = new datosRespuesta();
            respuesta.tipoRespuesta = "Error";
            respuesta.detalleRespuesta = "El DGA Seleccionado NO Existe.";

            return respuesta;
        }

        [HttpGet, ActionName("CopiarDGA")]
        public datosRespuesta CopiarDGA(int id)
        {
            datosRespuesta respuesta;

            List<PERMISO_AMBIENTAL> permisosAmbientales;
            List<PERSONAL_DGA> personal;

            var dga = dbSIM.DGA.FirstOrDefault(idDGA => idDGA.ID_DGA == id);
            var dgaEmitido = dbSIM.DGA.FirstOrDefault(idDGA => (idDGA.ID_ESTADO == 4 || idDGA.ID_ESTADO == 1) && idDGA.D_ANO == dga.D_ANO && idDGA.ID_TERCERO == dga.ID_TERCERO);

            if (dga != null)
            {
                if (dgaEmitido != null)
                {
                    respuesta = new datosRespuesta();
                    respuesta.tipoRespuesta = "Error";
                    respuesta.detalleRespuesta = "Solamente se puede copiar un DGA si no hay Emisión para el mismo año.";

                    return respuesta;
                }
                else
                {
                    try
                    {
                        permisosAmbientales = new List<PERMISO_AMBIENTAL>();
                        foreach (PERMISO_AMBIENTAL permisoAmbiental in dga.PERMISO_AMBIENTAL)
                        {
                            permisosAmbientales.Add(permisoAmbiental);
                        }

                        dga.D_FREPORTE = null;
                        dga.ID_ESTADO = 1;
                        dbSIM.Entry(dga).State = EntityState.Added;
                        dbSIM.SaveChanges();

                        respuesta = new datosRespuesta();
                        respuesta.tipoRespuesta = "OK";

                        return respuesta;
                    }
                    catch (Exception error)
                    {
                        respuesta = new datosRespuesta();
                        respuesta.tipoRespuesta = "Error";
                        respuesta.detalleRespuesta = "No se pudo Copiar el DGA Seleccionado.";

                        return respuesta;
                    }
                }
            }

            respuesta = new datosRespuesta();
            respuesta.tipoRespuesta = "Error";
            respuesta.detalleRespuesta = "El DGA Seleccionado NO Existe.";

            return respuesta;
        }

        [HttpGet, ActionName("SendDGA")]
        public datosRespuesta SendDGA(int id)
        {
            datosRespuesta respuesta;

            var dga = dbSIM.DGA.FirstOrDefault(idDGA => idDGA.ID_DGA == id);

            if (dga != null)
            {
                try
                {
                    dga.D_FREPORTE = DateTime.Now;
                    dga.ID_ESTADO = 4;
                    dbSIM.Entry(dga).State = EntityState.Modified;
                    dbSIM.SaveChanges();
                }
                catch (Exception error)
                {
                    respuesta = new datosRespuesta();
                    respuesta.tipoRespuesta = "Error";
                    respuesta.detalleRespuesta = "No se pudo Enviar el DGA Seleccionado.";

                    return respuesta;
                }

                var report = new DGAReport();

                string path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var modelProfesionales = from personalDGA in dbSIM.PERSONAL_DGA
                                         join tercero in dbSIM.TERCERO on personalDGA.ID_TERCERO equals tercero.ID_TERCERO
                                         join natural in dbSIM.NATURAL on tercero.ID_TERCERO equals natural.ID_TERCERO
                                         join profesion in dbSIM.PROFESION on natural.ID_PROFESION equals profesion.ID_PROFESION into JoinedProfesion
                                         from profesion in JoinedProfesion.DefaultIfEmpty()
                                         where personalDGA.ID_DGA == id
                                         select new
                                         {
                                             personalDGA.ID_DGA,
                                             personalDGA.ID_PERSONALDGA,
                                             personalDGA.ID_TERCERO,
                                             RAZON_SOCIAL = tercero.S_RSOCIAL,
                                             N_DOCUMENTO = tercero.N_DOCUMENTON,
                                             personalDGA.S_TIPOPERSONAL,
                                             S_ESRESPONSABLE = personalDGA.S_ESRESPONSABLE == "S" ? "SI" : "NO",
                                             personalDGA.N_DEDICACION,
                                             personalDGA.N_EXPERIENCIA,
                                             personalDGA.S_OBSERVACION,
                                             CORREO_ELECTRONICO = tercero.S_CORREO,
                                             TELEFONO = tercero.N_TELEFONO,
                                             PROFESION = profesion.S_NOMBRE
                                         };

                report.CargarDatos(dga, (modelProfesionales == null ? null : modelProfesionales.ToList()));
                report.ExportToPdf(path + "\\DGA_" + id.ToString());
                report.Dispose();

                respuesta = new datosRespuesta();
                respuesta.tipoRespuesta = "OK";

                return respuesta;
            }

            respuesta = new datosRespuesta();
            respuesta.tipoRespuesta = "Error";
            respuesta.detalleRespuesta = "El DGA Seleccionado NO Existe.";

            return respuesta;
        }

        [HttpGet, ActionName("ProfesionalesDGA")]
        public datosConsulta ProfesionalesDGA(int id, string filter, string sort, string group, int skip, int take, string searchValue, string searchExpr, string comparation, string tipoData, bool noFilterNoRecords)
        {
            var modelProfesionales = from personalDGA in dbSIM.PERSONAL_DGA
                                     join tercero in dbSIM.TERCERO on personalDGA.ID_TERCERO equals tercero.ID_TERCERO
                                     join natural in dbSIM.NATURAL on tercero.ID_TERCERO equals natural.ID_TERCERO
                                     join profesion in dbSIM.PROFESION on natural.ID_PROFESION equals profesion.ID_PROFESION into JoinedProfesion
                                     from profesion in JoinedProfesion.DefaultIfEmpty()
                                     where personalDGA.ID_DGA == id
                                     select new
                                     {
                                         personalDGA.ID_DGA,
                                         personalDGA.ID_PERSONALDGA,
                                         personalDGA.ID_TERCERO,
                                         S_TIPO_DOCUMENTO = tercero.TIPO_DOCUMENTO.S_NOMBRE,
                                         S_RSOCIAL = (tercero.S_RSOCIAL == null ? tercero.NATURAL.S_NOMBRE1 + " " + tercero.NATURAL.S_NOMBRE2 + " " + tercero.NATURAL.S_APELLIDO1 + " " + tercero.NATURAL.S_APELLIDO2 : tercero.S_RSOCIAL),
                                         N_DOCUMENTO = tercero.N_DOCUMENTON,
                                         tercero.N_DIGITOVER,
                                         personalDGA.S_TIPOPERSONAL,
                                         S_ESRESPONSABLE = personalDGA.S_ESRESPONSABLE == "S" ? "SI" : "NO",
                                         personalDGA.N_DEDICACION,
                                         personalDGA.N_EXPERIENCIA,
                                         personalDGA.S_OBSERVACION,
                                         CORREO_ELECTRONICO = tercero.S_CORREO,
                                         TELEFONO = tercero.N_TELEFONO,
                                         PROFESION = profesion.S_NOMBRE
                                     };

            IQueryable<dynamic> modelFiltered = SIM.Utilidades.Data.ObtenerConsultaDinamica(modelProfesionales, (searchValue != null && searchValue != "" ? searchExpr + "," + comparation + "," + searchValue : filter), sort, group);

            datosConsulta resultado = new datosConsulta();
            resultado.numRegistros = modelFiltered.Count();
            resultado.datos = modelFiltered.Skip(skip).Take(take).ToList();

            return resultado;
        }

        [ActionName("TerceroDGA")]
        public object GetTerceroDGA(int idTerceroDGA)
        {
            TERCERODGA terceroDGA;

            terceroDGA = (from personalDGA in dbSIM.PERSONAL_DGA
                          where personalDGA.ID_PERSONALDGA == idTerceroDGA
                          select new TERCERODGA
                          {
                              ID_PERSONALDGA = personalDGA.ID_PERSONALDGA,
                              ID_DGA = personalDGA.ID_DGA,
                              ID_TERCERO = personalDGA.ID_TERCERO,
                              S_TIPOPERSONAL = personalDGA.S_TIPOPERSONAL,
                              S_ESRESPONSABLE = personalDGA.S_ESRESPONSABLE,
                              N_DEDICACION = personalDGA.N_DEDICACION,
                              N_EXPERIENCIA = personalDGA.N_EXPERIENCIA,
                              S_OBSERVACION = personalDGA.S_OBSERVACION,
                              ID_TIPOPERSONAL = personalDGA.ID_TIPOPERSONAL,
                              TERCERO = new TERCERONATURAL
                              {
                                  ID_TIPODOCUMENTO = personalDGA.TERCERO.ID_TIPODOCUMENTO,
                                  N_DOCUMENTON = personalDGA.TERCERO.N_DOCUMENTON,
                                  N_DIGITOVER = personalDGA.TERCERO.N_DIGITOVER,
                                  S_NOMBRE1 = personalDGA.TERCERO.NATURAL.S_NOMBRE1,
                                  S_NOMBRE2 = personalDGA.TERCERO.NATURAL.S_NOMBRE2,
                                  S_APELLIDO1 = personalDGA.TERCERO.NATURAL.S_APELLIDO1,
                                  S_APELLIDO2 = personalDGA.TERCERO.NATURAL.S_APELLIDO2,
                                  S_GENERO = personalDGA.TERCERO.NATURAL.S_GENERO,
                                  D_NACIMIENTO = personalDGA.TERCERO.NATURAL.D_NACIMIENTO,
                                  ID_ACTIVIDADECONOMICA = personalDGA.TERCERO.ID_ACTIVIDADECONOMICA,
                                  N_TELEFONO = personalDGA.TERCERO.N_TELEFONO,
                                  S_CORREO = personalDGA.TERCERO.S_CORREO,
                                  ID_PROFESION = personalDGA.TERCERO.NATURAL.ID_PROFESION
                              }
                          }).FirstOrDefault();

            return terceroDGA;
        }

        [ActionName("TerceroDGAIdentificacion")]
        public object GetTerceroDGAIdentificacion(int idDGA, int tipoDocumento, long identificacion)
        {
            TERCERODGA terceroDGA;

            terceroDGA = (from personalDGA in dbSIM.PERSONAL_DGA
                          where personalDGA.ID_DGA == idDGA && personalDGA.TERCERO.ID_TIPODOCUMENTO == tipoDocumento && personalDGA.TERCERO.N_DOCUMENTON == identificacion
                          select new TERCERODGA
                          {
                              ID_PERSONALDGA = personalDGA.ID_PERSONALDGA,
                              ID_DGA = personalDGA.ID_DGA,
                              ID_TERCERO = personalDGA.ID_TERCERO,
                              S_TIPOPERSONAL = personalDGA.S_TIPOPERSONAL,
                              S_ESRESPONSABLE = personalDGA.S_ESRESPONSABLE,
                              N_DEDICACION = personalDGA.N_DEDICACION,
                              N_EXPERIENCIA = personalDGA.N_EXPERIENCIA,
                              S_OBSERVACION = personalDGA.S_OBSERVACION,
                              ID_TIPOPERSONAL = personalDGA.ID_TIPOPERSONAL,
                              TERCERO = new TERCERONATURAL
                              {
                                  ID_TIPODOCUMENTO = personalDGA.TERCERO.ID_TIPODOCUMENTO,
                                  N_DOCUMENTON = personalDGA.TERCERO.N_DOCUMENTON,
                                  N_DIGITOVER = personalDGA.TERCERO.N_DIGITOVER,
                                  S_NOMBRE1 = personalDGA.TERCERO.NATURAL.S_NOMBRE1,
                                  S_NOMBRE2 = personalDGA.TERCERO.NATURAL.S_NOMBRE2,
                                  S_APELLIDO1 = personalDGA.TERCERO.NATURAL.S_APELLIDO1,
                                  S_APELLIDO2 = personalDGA.TERCERO.NATURAL.S_APELLIDO2,
                                  S_GENERO = personalDGA.TERCERO.NATURAL.S_GENERO,
                                  D_NACIMIENTO = personalDGA.TERCERO.NATURAL.D_NACIMIENTO,
                                  ID_ACTIVIDADECONOMICA = personalDGA.TERCERO.ID_ACTIVIDADECONOMICA,
                                  N_TELEFONO = personalDGA.TERCERO.N_TELEFONO,
                                  S_CORREO = personalDGA.TERCERO.S_CORREO,
                                  ID_PROFESION = personalDGA.TERCERO.NATURAL.ID_PROFESION
                              }
                          }).FirstOrDefault();

            if (terceroDGA == null)
            {
                terceroDGA = (from tercero in dbSIM.TERCERO
                              where tercero.ID_TIPODOCUMENTO == tipoDocumento && tercero.N_DOCUMENTON == identificacion
                              select new TERCERODGA
                              {
                                  ID_PERSONALDGA = 0,
                                  ID_DGA = idDGA,
                                  ID_TERCERO = tercero.ID_TERCERO,
                                  S_TIPOPERSONAL = "I",
                                  S_ESRESPONSABLE = "N",
                                  N_DEDICACION = 0,
                                  N_EXPERIENCIA = 0,
                                  S_OBSERVACION = "",
                                  TERCERO = new TERCERONATURAL
                                  {
                                      ID_TIPODOCUMENTO = tercero.ID_TIPODOCUMENTO,
                                      N_DOCUMENTON = tercero.N_DOCUMENTON,
                                      N_DIGITOVER = tercero.N_DIGITOVER,
                                      S_NOMBRE1 = tercero.NATURAL.S_NOMBRE1,
                                      S_NOMBRE2 = tercero.NATURAL.S_NOMBRE2,
                                      S_APELLIDO1 = tercero.NATURAL.S_APELLIDO1,
                                      S_APELLIDO2 = tercero.NATURAL.S_APELLIDO2,
                                      S_GENERO = tercero.NATURAL.S_GENERO,
                                      D_NACIMIENTO = tercero.NATURAL.D_NACIMIENTO,
                                      ID_ACTIVIDADECONOMICA = tercero.ID_ACTIVIDADECONOMICA,
                                      N_TELEFONO = tercero.N_TELEFONO,
                                      S_CORREO = tercero.S_CORREO,
                                      ID_PROFESION = tercero.NATURAL.ID_PROFESION
                                  }
                              }).FirstOrDefault();
            }

            return terceroDGA;
        }

        // POST api/<controller>
        [HttpPost, ActionName("TerceroDGA")]
        public object PostTerceroDGA(TERCERODGA item)
        {
            int idTercero = 0;
            try
            {
                // Actualizamos los datos del Tercero Natural
                var tercero = dbSIM.TERCERO.Where(t => t.ID_TERCERO == item.ID_TERCERO).FirstOrDefault();

                if (tercero != null)
                {
                    tercero.NATURAL.S_NOMBRE1 = item.TERCERO.S_NOMBRE1;
                    tercero.NATURAL.S_NOMBRE2 = item.TERCERO.S_NOMBRE2;
                    tercero.NATURAL.S_APELLIDO1 = item.TERCERO.S_APELLIDO1;
                    tercero.NATURAL.S_APELLIDO2 = item.TERCERO.S_APELLIDO2;
                    tercero.NATURAL.S_GENERO = item.TERCERO.S_GENERO;
                    tercero.NATURAL.D_NACIMIENTO = item.TERCERO.D_NACIMIENTO;
                    tercero.ID_ACTIVIDADECONOMICA = item.TERCERO.ID_ACTIVIDADECONOMICA;
                    tercero.NATURAL.ID_PROFESION = item.TERCERO.ID_PROFESION;
                    tercero.N_TELEFONO = item.TERCERO.N_TELEFONO;
                    tercero.S_CORREO = item.TERCERO.S_CORREO;

                    dbSIM.Entry(tercero).State = EntityState.Modified;

                    dbSIM.SaveChanges();
                }
                else
                {
                    tercero = new TERCERO();
                    tercero.ID_TIPODOCUMENTO = item.TERCERO.ID_TIPODOCUMENTO;
                    tercero.N_DOCUMENTO = (long)item.TERCERO.N_DOCUMENTON;
                    tercero.N_DOCUMENTON = item.TERCERO.N_DOCUMENTON;
                    tercero.N_DIGITOVER = Utilidades.Data.ObtenerDigitoVerificacion(item.TERCERO.N_DOCUMENTON.ToString());
                    tercero.NATURAL = new NATURAL();
                    tercero.NATURAL.S_NOMBRE1 = item.TERCERO.S_NOMBRE1;
                    tercero.NATURAL.S_NOMBRE2 = item.TERCERO.S_NOMBRE2;
                    tercero.NATURAL.S_APELLIDO1 = item.TERCERO.S_APELLIDO1;
                    tercero.NATURAL.S_APELLIDO2 = item.TERCERO.S_APELLIDO2;
                    tercero.NATURAL.S_GENERO = item.TERCERO.S_GENERO;
                    tercero.NATURAL.D_NACIMIENTO = item.TERCERO.D_NACIMIENTO;
                    tercero.ID_ACTIVIDADECONOMICA = item.TERCERO.ID_ACTIVIDADECONOMICA;
                    tercero.NATURAL.ID_PROFESION = item.TERCERO.ID_PROFESION;
                    tercero.N_TELEFONO = item.TERCERO.N_TELEFONO;
                    tercero.S_CORREO = item.TERCERO.S_CORREO;

                    dbSIM.Entry(tercero).State = EntityState.Added;

                    dbSIM.SaveChanges();
                }

                idTercero = tercero.ID_TERCERO;
            }
            catch (Exception e)
            {
                return new { resp = "Error", mensaje = "Error Almacenando Tercero DGA" };
            }

            // Verificamos si el tercero DGA existe y se actualiza
            if (item.ID_PERSONALDGA > 0) // Tercero DGA Existe
            {
                try
                {
                    // Actualizamos los datos del Tercero DGA
                    var terceroDGA = dbSIM.PERSONAL_DGA.Where(tdga => tdga.ID_PERSONALDGA == item.ID_PERSONALDGA).FirstOrDefault();

                    terceroDGA.S_TIPOPERSONAL = item.S_TIPOPERSONAL;
                    terceroDGA.S_ESRESPONSABLE = item.S_ESRESPONSABLE;
                    terceroDGA.N_DEDICACION = item.N_DEDICACION;
                    terceroDGA.N_EXPERIENCIA = item.N_EXPERIENCIA;
                    terceroDGA.ID_TIPOPERSONAL = item.ID_TIPOPERSONAL;
                    terceroDGA.S_OBSERVACION = item.S_OBSERVACION;

                    dbSIM.Entry(terceroDGA).State = EntityState.Modified;
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    return new { resp = "Error", mensaje = "Error Almacenando Tercero DGA" };
                }
                return new { resp = "OK", mensaje = "Tercero DGA Almacenado Satisfactoriamente" };
            }
            else
            {
                try
                {
                    // Actualizamos los datos del Tercero DGA
                    var terceroDGA = new PERSONAL_DGA();

                    terceroDGA.ID_DGA = item.ID_DGA;
                    terceroDGA.ID_TERCERO = idTercero;
                    terceroDGA.S_TIPOPERSONAL = item.S_TIPOPERSONAL;
                    terceroDGA.S_ESRESPONSABLE = item.S_ESRESPONSABLE;
                    terceroDGA.N_DEDICACION = item.N_DEDICACION;
                    terceroDGA.N_EXPERIENCIA = item.N_EXPERIENCIA;
                    terceroDGA.S_OBSERVACION = item.S_OBSERVACION;
                    terceroDGA.ID_TIPOPERSONAL = item.ID_TIPOPERSONAL;

                    dbSIM.Entry(terceroDGA).State = EntityState.Added;
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    return new { resp = "Error", mensaje = "Error Almacenando Tercero DGA" };
                }
                return new { resp = "OK", mensaje = "Tercero DGA Almacenado Satisfactoriamente" };
            }
        }

        [HttpPost, ActionName("CargarArchivo2")]
        public IHttpActionResult Post()
        {
            var request = HttpContext.Current.Request;
            if (request.Files.Count > 0)
            {
                foreach (string file in request.Files)
                {
                    var postedFile = request.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", postedFile.FileName));
                    postedFile.SaveAs(filePath);
                }
                return Ok(true);
            }
            else
                return BadRequest();
        }

        [HttpPost, ActionName("CargarArchivo")]
        public async Task<object> PostCargarArchivo()
        {
            int idDGA;
            DGA datosDGA;
            string pathImagen;
            string pathDocumentos = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"];
            string filename;

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new DGAMultipartFormDataStreamProvider(pathDocumentos);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                idDGA = Convert.ToInt32(provider.FormData["id"]);
                datosDGA = dbSIM.DGA.FirstOrDefault(dga => dga.ID_DGA == idDGA);
                pathImagen = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + datosDGA.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA";
                filename = "Organigrama_" + datosDGA.ID_TERCERO.ToString() + "_" + datosDGA.D_ANO.Year.ToString() + "TMP";// +"_" + Path.GetExtension(e.UploadedFile.FileName);

                if (!Directory.Exists(pathImagen))
                {
                    Directory.CreateDirectory(pathImagen);
                }

                if (System.IO.File.Exists(pathImagen + "\\" + filename))
                {
                    System.IO.File.Delete(pathImagen + "\\" + filename);
                }

                System.IO.File.Move(pathDocumentos + "\\" + provider.FileData[0].Headers.ContentDisposition.FileName.Replace("\"", string.Empty), pathImagen + "\\" + filename);

                //var response = new HttpResponseMessage(HttpStatusCode.OK);
                //var a = new MemoryStream()
                //var stream = new FileStream(pathImagen + "\\" + filename, FileMode.Open);
                //response.Content = new StreamContent(stream);
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //stream.Close();

                //return response;

                return Convert.ToBase64String(System.IO.File.ReadAllBytes(pathImagen + "\\" + filename));
            }
            catch (System.Exception e)
            {
                return null; // Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }

    public class DGAMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public DGAMultipartFormDataStreamProvider(string path)
            : base(path)
        {
        }

        /*public override string GetLocalFileName(HttpContentHeaders headers)
        {
            if (headers != null &&
                headers.ContentDisposition != null)
            {
                return headers
                    .ContentDisposition
                    .FileName.TrimEnd('"').TrimStart('"');
            }

            return base.GetLocalFileName(headers);
        }*/

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return name.Replace("\"", string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
        }
    }
}