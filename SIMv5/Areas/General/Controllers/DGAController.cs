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
using System.Data;
using Newtonsoft.Json;
using SIM.Data.General;

namespace SIM.Areas.General.Controllers
{
    public class DGAController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        //
        // GET: /DGA/
        [Authorize]
        public ActionResult Index()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int? idTercero = null;
            //int idRol;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

            ViewBag.Administrador = false;
            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
            {
                //idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                ViewBag.Administrador = claimPpal.IsInRole("XDGA");
            }

            if (claimTercero != null)
            {
                idTercero = int.Parse(claimTercero.Value);
            }

            return View();
        }

        [Authorize]
        public ActionResult DGA(int? id)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            Claim claimTercero;
            ClaimsPrincipal claimPpal = (ClaimsPrincipal)context.User;

            ViewBag.idDGA = id;

            ViewBag.tiposIdentificacion = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposDocumentoNatural());
            ViewBag.tiposPersonal = JsonConvert.SerializeObject(ModelsToListGeneral.GetTiposPersonal());

            if (id != null)
            {
                var model = from dga in dbSIM.DGA
                            where dga.ID_DGA == id
                            select dga;

                DGA registroDGA = model.FirstOrDefault();

                //ViewBag.Profesionales = modelProfesionales;
                ViewBag.ReadOnly = !(registroDGA.D_FREPORTE == null);

                ViewBag.Version = (registroDGA.N_VERSION ?? 1);

                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) != null)
                {
                    //idRol = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol).Value);
                    ViewBag.ReadOnly = (bool)(ViewBag.ReadOnly) || claimPpal.IsInRole("XDGA");
                }

                var path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + registroDGA.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + "Organigrama_" + registroDGA.ID_TERCERO.ToString() + "_" + registroDGA.D_ANO.Year.ToString() + "TMP";

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                ViewBag.AnosDGA = JsonConvert.SerializeObject(ModelsToListGeneral.GetAnosDGA(registroDGA.ID_TERCERO));

                return View("DGA", registroDGA);
            }
            else
            {
                var registroDGA = new DGA();
                registroDGA.ID_DGA = 0;
                ViewBag.ReadOnly = false;

                ViewBag.idDGA = 0;

                ViewBag.Version = 2;

                claimTercero = ((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero);

                if (claimTercero != null)
                {
                    ViewBag.IdTercero = int.Parse(claimTercero.Value);
                }

                ViewBag.AnosDGA = JsonConvert.SerializeObject(ModelsToListGeneral.GetAnosDGA(ViewBag.IdTercero));

                return View("DGA", registroDGA);
            }
        }

        public ActionResult PrintDGA(int id)
        {
            var dga = dbSIM.DGA.FirstOrDefault(idDGA => idDGA.ID_DGA == id);

            string path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\DGA_" + id.ToString();

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

        public ActionResult LoadOrganigrama(int id, int? temporal)
        {
            string path;

            var dga = dbSIM.DGA.FirstOrDefault(idDGA => idDGA.ID_DGA == id);

            if (dga != null)
            {
                if (temporal != null)
                    path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + "Organigrama_" + dga.ID_TERCERO.ToString() + "_" + dga.D_ANO.Year.ToString() + "TMP";
                else
                    path = System.Configuration.ConfigurationManager.AppSettings["DocumentsDGAPath"] + "\\" + dga.TERCERO.ID_TERCERO.ToString("D8").Insert(6, "\\").Insert(4, "\\").Insert(2, "\\") + "\\DGA\\" + dga.S_ORGANIGRAMA;

                if (System.IO.File.Exists(path))
                    return File(path, "image/*");
                else
                    return null;
            }
            else
                return null;
        }
    }
}
