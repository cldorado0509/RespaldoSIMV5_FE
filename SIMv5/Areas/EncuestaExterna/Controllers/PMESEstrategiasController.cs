using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AspNet.Identity.Oracle;
using Microsoft.Owin.Security;
using SIM.Areas.Seguridad.Models;
using SIM.Areas.Models;
using Newtonsoft.Json;
using System.Text;
using System.Web.Hosting;
using System.Security.Cryptography;
using System.IO;
using System.Data.Entity.Core.Objects;
using SIM.Areas.ControlVigilancia.Models;
using System.Globalization;
using System.Data.Entity;
using SIM.Areas.EncuestaExterna.Reporte;
using System.Data.Entity.SqlServer;
using Xceed.Words.NET;
using Oracle.ManagedDataAccess.Client;
using SIM.Areas.Tramites.Models;
using SIM.Utilidades;
using SIM.Data;
using SIM.Data.Control;
using SIM.Data.Tramites;
using SIM.Models;

namespace SIM.Areas.EncuestaExterna.Controllers
{
    public class PMESEstrategiasController : Controller
    {
        EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        Int32 idUsuario;
        decimal codFuncionario;
        decimal idTerceroUsuario;

        [Authorize]
        public ActionResult PMESEstrategias(int e, int cr, int t)
        {
            var encuesta = (from ge in dbSIM.FRM_GENERICO_ESTADO
                            join vs in dbSIM.VIGENCIA_SOLUCION on ge.ID_ESTADO equals vs.ID_ESTADO
                            where ge.ID_ESTADO == e
                            select new
                            {
                                vs.ID_VIGENCIA,
                                ge.NOMBRE,
                                VIGENCIA = vs.VALOR

                            }).FirstOrDefault();

            if (encuesta == null)
            {
                var estrategiaTercero = new PMES_ESTRATEGIAS_TERCERO();
                estrategiaTercero.ID_ESTADO = e;
            }

            ViewBag.Vigencia = encuesta.VIGENCIA;

            return View();
        }
    }
}