using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.Pdf;
using SIM.Areas.General.Models;
using SIM.Data;
using SIM.Areas.Seguridad.Models;
using System.Security.Claims;
using System.IO;
using System.Data.Entity;
using SIM.Data.General;

namespace SIM.Areas.General.Controllers
{
    public class DGAMVCController : Controller
    {
        //
        // GET: /DGA/

        public ActionResult Index()
        {
            return View();
        }

        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [ValidateInput(false)]
        public ActionResult gvwAdministrarDGA()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero;
            int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            ViewBag.Administrador = false;
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                ViewBag.Administrador = claimPpal.IsInRole("XDGA");
            }

            if (claimTercero != null)
            {
                idTercero = int.Parse(claimTercero.Value);
            }
            else
            {
                return PartialView("_gvwAdministrarDGA", null);
            }

            if (ViewBag.Administrador)
            {
                var model = from dga in dbSIM.DGA
                            join tercero in dbSIM.TERCERO on dga.ID_TERCERO equals tercero.ID_TERCERO
                            orderby dga.D_ANO, dga.ID_DGA ascending
                            select new { ID_TERCERO = dga.ID_TERCERO, S_TERCERO = tercero.S_RSOCIAL, ID_DGA = dga.ID_DGA, D_ANO = dga.D_ANO, D_FREPORTE = dga.D_FREPORTE, ID_ESTADO = dga.ID_ESTADO, S_ESTADO = dga.ESTADO.S_NOMBRE };

                return PartialView("_gvwAdministrarDGA", model.ToList());
            }
            else if (claimTercero != null)
            {
                var model = from dga in dbSIM.DGA
                            join tercero in dbSIM.TERCERO on dga.ID_TERCERO equals tercero.ID_TERCERO
                            where dga.ID_TERCERO == idTercero
                            orderby dga.D_ANO, dga.ID_DGA ascending
                            select new { ID_TERCERO = dga.ID_TERCERO, S_TERCERO = tercero.S_RSOCIAL, ID_DGA = dga.ID_DGA, D_ANO = dga.D_ANO, D_FREPORTE = dga.D_FREPORTE, ID_ESTADO = dga.ID_ESTADO, S_ESTADO = dga.ESTADO.S_NOMBRE };

                return PartialView("_gvwAdministrarDGA", model.ToList());
            }
            else
            {
                return PartialView("_gvwAdministrarDGA", null);
            }
        }

        /// <summary>
        /// Carga modelo del DGA Asociado al año seleccionado
        /// </summary>
        /// <param name="id">ID del DGA</param>
        /// <returns>Vista del detalle de DGA</returns>
        //[ValidateInput(false)]
        //[Authorize(Roles = "VDGA")]
        public ActionResult LoadDGA(int? id)
        {
            if (id != null)
            {
                var model = from dga in dbSIM.DGA
                            where dga.ID_DGA == id
                            select dga;

                DGA registroDGA = model.FirstOrDefault();

                //ViewBag.Profesionales = modelProfesionales;
                ViewBag.ReadOnly = !(registroDGA.D_FREPORTE == null);

                var path = System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"] + "\\" + registroDGA.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + "Organigrama_" + registroDGA.ID_TERCERO.ToString() + "_" + registroDGA.D_ANO.Year.ToString() + "TMP";

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                return View("DGA", registroDGA);
            }
            else
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                Claim claimTercero;
                var model = new DGA();
                model.ID_DGA = 0;
                ViewBag.ReadOnly = false;

                claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

                if (claimTercero != null)
                {
                    ViewBag.IdTercero = int.Parse(claimTercero.Value);
                }

                return View("DGA", model);
            }
        }

        public ActionResult DGAUpdate(DGA item)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            Claim claimTercero;

            var model = dbSIM.DGA;

            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ID_DGA > 0)
                    {
                        // Busca si el DGA existe
                        var modelItem = model.FirstOrDefault(dga => dga.ID_DGA == item.ID_DGA);
                        if (modelItem != null)
                        {
                            this.UpdateModel(modelItem);

                            if (modelItem.S_ORGANIGRAMA != null)
                            {
                                //modelItem.S_ORGANIGRAMA = modelItem.S_ORGANIGRAMA.Replace("/Content", "~");
                                if (modelItem.S_ORGANIGRAMA != "")
                                    modelItem.S_ORGANIGRAMA = "Organigrama_" + modelItem.ID_TERCERO.ToString() + "_" + modelItem.D_ANO.Year.ToString();
                                else
                                    modelItem.S_ORGANIGRAMA = null;
                            }

                            var path = System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"] + "\\" + modelItem.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + "Organigrama_" + modelItem.ID_TERCERO.ToString() + "_" + modelItem.D_ANO.Year.ToString() + "TMP";

                            if (System.IO.File.Exists(path))
                                System.IO.File.Copy(path, path.Substring(0, path.Length - 3), true);

                            modelItem.PERMISO_AMBIENTAL.Clear();

                            foreach (PERMISO_AMBIENTAL permisoAmbiental in ModelsToListGeneral.GetPermisosAmbientales(dbSIM))
                            {
                                if ((Request.Params["chkpa_" + permisoAmbiental.ID_PERMISOAMBIENTAL] != null) && (Request.Params["chkpa_" + permisoAmbiental.ID_PERMISOAMBIENTAL] == "C"))
                                {
                                    modelItem.PERMISO_AMBIENTAL.Add(permisoAmbiental);
                                }
                            }
                        }
                    }
                    else
                    {
                        claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

                        item.ID_TERCERO = int.Parse(claimTercero.Value);
                        item.ID_ESTADO = 1;

                        if (item.S_ORGANIGRAMA != null)
                            item.S_ORGANIGRAMA = item.S_ORGANIGRAMA.Replace("/Content", "~");
                        /*
                        foreach (PERMISO_AMBIENTAL permisoAmbiental in ModelsToListGeneral.GetPermisosAmbientales())
                        {
                            if ((Request.Params["chkpa_" + permisoAmbiental.ID_PERMISOAMBIENTAL] != null) && (Request.Params["chkpa_" + permisoAmbiental.ID_PERMISOAMBIENTAL] == "C"))
                            {
                                var modelPA = new DGA_PERMISOAMBIENTAL();
                                modelPA.ID_DGA = item.ID_DGA;
                                modelPA.ID_PERMISOAMBIENTAL = permisoAmbiental.ID_PERMISOAMBIENTAL;

                                dbSIM.Entry(modelPA).State = EntityState.Added;
                            }
                            else
                            {
                                var modelPA = new DGA_PERMISOAMBIENTAL();
                                modelPA.ID_DGA = item.ID_DGA;
                                modelPA.ID_PERMISOAMBIENTAL = permisoAmbiental.ID_PERMISOAMBIENTAL;

                                dbSIM.Entry(modelPA).State = EntityState.Deleted;
                            }
                        }
                         * */

                        foreach (PERMISO_AMBIENTAL permisoAmbiental in ModelsToListGeneral.GetPermisosAmbientales(dbSIM))
                        {
                            if ((Request.Params["chkpa_" + permisoAmbiental.ID_PERMISOAMBIENTAL] != null) && (Request.Params["chkpa_" + permisoAmbiental.ID_PERMISOAMBIENTAL] == "C"))
                            {
                                //item.PERMISO_AMBIENTAL.Add(new PERMISO_AMBIENTAL() { ID_PERMISOAMBIENTAL = permisoAmbiental.ID_PERMISOAMBIENTAL });
                                item.PERMISO_AMBIENTAL.Add(permisoAmbiental);
                            }
                        }

                        item.S_COMPARTEDGA = (item.S_COMPARTEDGA == null ? "N" : item.S_COMPARTEDGA);
                        item.S_AGREMIACION = (item.S_AGREMIACION == null ? "N" : item.S_AGREMIACION);
                        item.S_ESSGA = (item.S_ESSGA == null ? "N" : item.S_ESSGA);
                        item.S_ESSGC = (item.S_ESSGC == null ? "N" : item.S_ESSGC);
                        item.S_PRODUCCIONMASLIMPIA = (item.S_PRODUCCIONMASLIMPIA == null ? "N" : item.S_PRODUCCIONMASLIMPIA);
                        item.S_ESECOETIQUETADO = (item.S_ESECOETIQUETADO == null ? "N" : item.S_ESECOETIQUETADO);

                        model.Add(item);
                    }

                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            // Después de actualizar el DGA se carga con los datos actualizados

            return RedirectToAction("LoadDGA", new { id = item.ID_DGA });
        }

        public ActionResult SendDGA(int id)
        {
            var dga = dbSIM.DGA.FirstOrDefault(idDGA => idDGA.ID_DGA == id);

            if (dga != null)
            {
                dga.D_FREPORTE = DateTime.Now;
                dga.ID_ESTADO = 4;
                dbSIM.Entry(dga).State = EntityState.Modified;
                dbSIM.SaveChanges();

                var report = new DGAReport();

                string path = System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA";

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

                return PrintDGA(id);
            }

            return null;
        }

        public ActionResult AnularDGA(int id)
        {
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

                    string path = System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA";

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

                    return PrintDGA(id);
                }
                catch (Exception error)
                {
                    int i = 0;

                    i++;
                    return null;
                }
            }

            return null;
        }

        public string CopiarDGA(int id)
        {
            List<PERMISO_AMBIENTAL> permisosAmbientales;
            List<PERSONAL_DGA> personal;

            var dga = dbSIM.DGA.FirstOrDefault(idDGA => idDGA.ID_DGA == id);
            var dgaEmitido = dbSIM.DGA.FirstOrDefault(idDGA => (idDGA.ID_ESTADO == 4 || idDGA.ID_ESTADO == 1) && idDGA.D_ANO == dga.D_ANO && idDGA.ID_TERCERO == dga.ID_TERCERO);

            if (dga != null)
            {
                if (dgaEmitido != null)
                {
                    return ("Solamente se puede copiar un DGA si no hay Emisión para el mismo año.");
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
                        /*
                        personal = new List<PERSONAL_DGA>();
                        foreach (PERSONAL_DGA personalDGA in dga.PERSONAL_DGA)
                        {
                            personal.Add(personalDGA);
                        }*/

                        dga.D_FREPORTE = null;
                        dga.ID_ESTADO = 1;
                        dbSIM.Entry(dga).State = EntityState.Added;
                        dbSIM.SaveChanges();
                        /*
                        foreach (PERMISO_AMBIENTAL permiso in permisosAmbientales)
                        {
                            dga.PERMISO_AMBIENTAL.Add(permiso);
                        }

                        foreach (PERSONAL_DGA persona in personal)
                        {
                            dga.PERSONAL_DGA.Add(persona);
                        }

                        dbSIM.SaveChanges();
                         */
                    }
                    catch (Exception error)
                    {
                        return (error.Message);
                    }
                }
            }
            else
            {
                return ("Registro Inválido");
            }

            return "OK";
        }

        public ActionResult PrintDGA(int id)
        {
            var dga = dbSIM.DGA.FirstOrDefault(idDGA => idDGA.ID_DGA == id);

            string path = System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\DGA_" + id.ToString();

            if (System.IO.File.Exists(path))
            {
                try
                {
                    return File(path, "application/pdf");
                }
                catch (Exception error)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult gvwAdministrarDGAAddNew(ACTIVIDAD_ECONOMICA item)
        {
            var model = dbSIM.ACTIVIDAD_ECONOMICA;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_gvwDGAPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult gvwAdministrarDGAUpdate(ACTIVIDAD_ECONOMICA item)
        {
            return null;
            /*var model = dbSIM.ACTIVIDAD_ECONOMICA;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_DGA == item.ID_DGA);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        dbSIM.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_gvwDGAPartial", model.ToList());*/
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult gvwAdministrarDGADelete(System.Int32 ID_DGA)
        {
            /*var model = dbSIM.ACTIVIDAD_ECONOMICA;
            if (ID_DGA != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_DGA == ID_DGA);
                    if (item != null)
                        model.Remove(item);
                    dbSIM.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_gvwDGAPartial", model.ToList());*/
            return null;
        }

        [ValidateInput(false)]
        public ActionResult gvwSeleccionarDGA()
        {
            /*
            bool lcbolFiltro = false;
            var model = dbSIM.ACTIVIDAD_ECONOMICA;

            for (int lcintCont = 0; lcintCont < 2; lcintCont++)
            {
                if (Request.Params["gvwActividadesEconomicas$DXFREditorcol" + lcintCont.ToString()] != null && Request.Params["gvwActividadesEconomicas$DXFREditorcol" + lcintCont.ToString()].Trim() != "")
                {
                    lcbolFiltro = true;
                    break;
                }
            }

            if (lcbolFiltro)
                return PartialView("_gvwSeleccionarDGA", model);
            else
                return PartialView("_gvwSeleccionarDGA", model.Where(p => p.ID_DGA == -10000));
             * */
            return null;
        }


        public ActionResult upcOrganigramaUpload(int idDGA)
        {
            DGA datosDGA = dbSIM.DGA.FirstOrDefault(dga => dga.ID_DGA == idDGA);

            DGAControllerupcOrganigramaSettings organigramaSettings = new DGAControllerupcOrganigramaSettings();
            organigramaSettings.idDGA = datosDGA.ID_DGA;
            organigramaSettings.idTercero = datosDGA.ID_TERCERO;
            organigramaSettings.ano = datosDGA.D_ANO.Year;
            organigramaSettings.path = System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"] + "\\" + datosDGA.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA";


            UploadControlExtension.GetUploadedFiles("upcOrganigrama", organigramaSettings.ValidationSettings, organigramaSettings.FileUploadComplete);
            return null;
        }

        public ActionResult LoadOrganigrama(int id, int? temporal)
        {
            string path;

            var dga = dbSIM.DGA.FirstOrDefault(idDGA => idDGA.ID_DGA == id);

            if (temporal != null)
                path = System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + "Organigrama_" + dga.ID_TERCERO.ToString() + "_" + dga.D_ANO.Year.ToString() + "TMP";
            else
                path = System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + dga.S_ORGANIGRAMA;

            return File(path, "image/*");
        }
    }

    public class DGAControllerupcOrganigramaSettings
    {
        public int idTercero;
        public int ano;
        public int idDGA;
        public string path;

        public DevExpress.Web.UploadControlValidationSettings ValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png" },
            MaxFileSize = Convert.ToInt64(ConfigurationManager.AppSettings["FileSize"] == null ? "90000000" : ConfigurationManager.AppSettings["FileSize"])
        };
        public void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                string filename = "Organigrama_" + idTercero.ToString() + "_" + ano.ToString() + "TMP";// +"_" + Path.GetExtension(e.UploadedFile.FileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                e.UploadedFile.SaveAs(path + "\\" + filename);
                var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                //e.CallbackData = url.Content("~/Content/LoadFiles/DGA/" + filename);
                e.CallbackData = url.Content("~/General/DGA/LoadOrganigrama/" + idDGA.ToString() + "?temporal=1");
            }
        }
    }
}
