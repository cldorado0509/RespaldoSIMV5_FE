using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using DevExpress.Web.Mvc;
using SIM.Areas.General.Models;
using SIM.Data;
using SIM.Areas.Seguridad.Models;
using Newtonsoft.Json;
using SIM.Data.General;

namespace SIM.Controllers
{
    public class HomeController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        public ActionResult Index()
        {
            int numPendientes = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            ViewBag.CrearTercero = false;
            ViewBag.AsignarRepresentanteLegal = false;
            ViewBag.ActualizarActividadEconomica = false;
            ViewBag.ActualizarActividadEconomicaInst = new ArrayList();

            if (Request.IsAuthenticated)
            {
                int idTercero;

                /*if (Session["_Menu"] == null)
                {
                    Session["_Menu"] = ObtenerHTMLMenu(ModelsToListSeguridad.GetOpcionesMenu()); //JsonConvert.SerializeObject(ModelsToListSeguridad.GetOpcionesMenu());
                }*/

                /*if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdRol) == null)
                {
                    //return RedirectToAction("AsignarRolUsuarioExterno", new { Controller = "UsuarioRol", Area = "Seguridad" });
                    //return RedirectToAction("Index", new { Controller = "Account", Area = "Seguridad" }); ;
                    return RedirectToAction("Login", new { Controller = "Account", Area = "Seguridad" });
                }
                else*/
                if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero) == null)
                {
                    //ViewBag.CrearTercero = true;

                    //numPendientes++;
                }
                else
                {
                    idTercero = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);
                    TERCERO tercero = dbSIM.TERCERO.Where(t => t.ID_TERCERO == idTercero).FirstOrDefault();

                    ViewBag.Usuario = User.Identity.Name;

                    ViewBag.IdTercero = idTercero;

                    if (tercero != null)
                    {
                        if (tercero.ACTIVIDAD_ECONOMICA == null || tercero.ACTIVIDAD_ECONOMICA.S_VERSION != "4")
                        {
                            if (tercero.ID_TIPODOCUMENTO == 2)
                            {
                                ViewBag.ActualizarActividadEconomica = true;
                                numPendientes++;
                            }
                        }

                        if (tercero.ID_TIPODOCUMENTO == 2) // Nit, por lo tanto valida que tenga representante legal registrado
                        {
                            var representanteLegal = (from rl in dbSIM.CONTACTOS
                                                      where rl.ID_JURIDICO == tercero.ID_TERCERO && rl.TIPO == "R" && rl.D_FIN == null
                                                      select rl).FirstOrDefault();

                            if (representanteLegal == null)
                            {
                                ViewBag.AsignarRepresentanteLegal = true;
                                numPendientes++;
                            }
                        }
                    }

                    /*var instalacionTercero = from instalacion in dbSIM.TERCERO_INSTALACION
                                             join actividadEconomica in dbSIM.ACTIVIDAD_ECONOMICA on instalacion.ID_ACTIVIDADECONOMICA equals actividadEconomica.ID_ACTIVIDADECONOMICA
                                             where instalacion.ID_TERCERO == idTercero
                                             select new { instalacion, actividadEconomica };*/

                    var instalacionTercero = from instalacion in dbSIM.TERCERO_INSTALACION
                                             join actividadEconomica in dbSIM.ACTIVIDAD_ECONOMICA on instalacion.ID_ACTIVIDADECONOMICA equals actividadEconomica.ID_ACTIVIDADECONOMICA
                                             where instalacion.ID_TERCERO == idTercero
                                             select new { instalacion.ID_INSTALACION, instalacion.INSTALACION.S_NOMBRE, instalacion.ID_ACTIVIDADECONOMICA, actividadEconomica.S_VERSION };

                    if (instalacionTercero.Count() > 0)
                    {
                        foreach (var datosInstalacion in instalacionTercero.ToList())
                        {
                            //if (datosInstalacion.actividadEconomica.S_VERSION != "4")
                            if (datosInstalacion.S_VERSION != "4")
                                ((ArrayList)ViewBag.ActualizarActividadEconomicaInst).Add(new object[] { datosInstalacion.ID_INSTALACION, datosInstalacion.S_NOMBRE, datosInstalacion.ID_ACTIVIDADECONOMICA, datosInstalacion.S_VERSION });
                        }
                    }
                }

                ViewBag.NumeroPendientes = numPendientes;

                if (numPendientes > 0)
                {
                    Session["_Menu"] = null;
                    Session["_Pendientes"] = true;
                }
                else
                {
                    Session["_Pendientes"] = false;
                }

                var rolPMES = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "45");

                ViewBag.RolPMES = (rolPMES == null || rolPMES.FirstOrDefault() == null ? false : true);

                return View();
            }
            else
            {
                return RedirectToAction("Login", new { Controller = "Account", Area = "Seguridad" });
            }
        }
    }
}