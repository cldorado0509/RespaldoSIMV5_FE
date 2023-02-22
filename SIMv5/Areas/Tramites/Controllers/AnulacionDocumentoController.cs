using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using SIM.Areas.Tramites.Models;
using SIM.Areas.Models;
using SIM.Utilidades;
using System.IO;
using System.Text;
using BaxterSoft.Graphics;
using System.Security.Claims;
using SIM.Data;
using System.Configuration;

namespace SIM.Areas.Tramites.Controllers
{
    public class AnulacionDocumentoController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize]
        public ActionResult Index(int? codigo)
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();
            
            /*var dependienciaFuncionario = dbSIM.FUNCIONARIO_DEPENDENCIA.Where(df => df.CODFUNCIONARIO == codFuncionario).FirstOrDefault();

            ViewBag.Anulacion = "1";
            if (dependienciaFuncionario != null)
            {
                var dependenciasNoAnulacion = ConfigurationManager.AppSettings["DependenciasNoAnulacion"];

                var dependencias = dependenciasNoAnulacion.Split(',');
                if (dependencias.Contains(dependienciaFuncionario.ID_DEPENDENCIA.ToString()))
                {
                    ViewBag.Anulacion = "0";
                }
            }*/

            ViewBag.Codigo = codigo ?? 0;

            var usuarioAutoriza = (from tr in dbSIM.TBTAREARESPONSABLE
                                   where tr.CODTAREA == 4929 && tr.CODFUNCIONARIO == codFuncionario
                                   //******&&&&where tr.CODTAREA == 4468 && tr.CODFUNCIONARIO == codFuncionario // Pruebas
                                   select tr).FirstOrDefault();

            var usuarioApruebaATU = (from tr in dbSIM.TBTAREARESPONSABLE
                                    where tr.CODTAREA == 4928 && tr.CODFUNCIONARIO == codFuncionario
                                     //*******&&&&where tr.CODTAREA == 4469 && tr.CODFUNCIONARIO == codFuncionario // Pruebas
                                     select tr).FirstOrDefault();

            ViewBag.UsuarioAutoriza = usuarioAutoriza != null;
            ViewBag.UsuarioApruebaATU = usuarioApruebaATU != null;

            return View();
        }

        [Authorize]
        public ActionResult Documento(int id, int idP)
        {
            int idUsuario = 0;
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                idUsuario = Convert.ToInt32(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            int codFuncionario = dbSIM.USUARIO_FUNCIONARIO.Where(uf => uf.ID_USUARIO == idUsuario).Select(uf => uf.CODFUNCIONARIO).FirstOrDefault();

            ViewBag.id = id;
            ViewBag.idP = idP;
            ViewBag.SoloLectura = "N";

            if (id > 0)
            {
                var anulaciondocumento = (from ad in dbSIM.ANULACION_DOC
                                          where ad.ID_ANULACION_DOC == id
                                          select ad).FirstOrDefault();

                ViewBag.Tipo = (anulaciondocumento.S_FORMULARIO == "51" ? 2 : (anulaciondocumento.S_FORMULARIO == "52" ? 3 : (anulaciondocumento.S_FORMULARIO == "53" ? 4 : 4)));

                ViewBag.SoloLectura = (anulaciondocumento.CODFUNCIONARIO_ACTUAL != codFuncionario || anulaciondocumento.S_FORMULARIO == "54" ? "S" : "N");
            } else
            {
                ViewBag.Tipo = 1;
            }


            return View();
        }
    }
}
