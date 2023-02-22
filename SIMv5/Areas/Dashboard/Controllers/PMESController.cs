using DashboardDesignerClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIM.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using SIM.Areas.Seguridad.Models;
using DevExpress.DashboardWeb;

namespace SIM.Areas.Dashboard.Controllers
{
    public class ModoTransporte
    {
        public int ID_MODO { get; set; }
        public string MODO { get; set; }
        public string COLOR { get; set; }
    }

    public class UbicacionesModoTransporte
    {
        public int ID_MODO { get; set; }
        public string COLOR { get; set; }
        public string LATITUD { get; set; }
        public string LONGITUD { get; set; }
    }

    public class UbicacionInstalacion
    {
        public int ID_INSTALACION { get; set; }
        public string INSTALACION { get; set; }
        public string LATITUD { get; set; }
        public string LONGITUD { get; set; }
    }

    public class PMESController : Controller
    {
        private EntitiesSIMOracle dbSIM = new EntitiesSIMOracle();

        [Authorize]
        public ActionResult PMES(int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;
            bool seleccionTercero = false;

            var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

            if (rolesUsuario != null && rolesUsuario.Count() > 0)
            {
                if (t != null)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }

                seleccionTercero = true;
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            var tercero = (from ter in dbSIM.TERCERO where ter.ID_TERCERO == idTercero select ter.S_RSOCIAL).FirstOrDefault();

            ViewBag.idTercero = idTercero;
            ViewBag.NombreTercero = tercero;
            ViewBag.seleccionTercero = seleccionTercero;

            /*var sql = "SELECT ID_RESPUESTA AS ID_MODO, TRIM(S_VALOR) AS MODO " +
                        "FROM CONTROL.ENC_OPCION_RESPUESTA " +
                        "WHERE ID_PREGUNTA = 529 " +
                        "ORDER BY TRIM(S_VALOR)";

            var modos = dbSIM.Database.SqlQuery<ModoTransporte>(sql).ToList();

            for (int i = 0; i < modos.Count; i++)
            {
                var modo = modos[i];
                switch (i)
                {
                    case 0:
                        modo.COLOR = "255,0,0"; // red
                        break;
                    case 1:
                        modo.COLOR = "0,255,0"; // lime
                        break;
                    case 2:
                        modo.COLOR = "0,0,255"; // blue
                        break;
                    case 3:
                        modo.COLOR = "255,255,0"; // yellow
                        break;
                    case 4:
                        modo.COLOR = "0,255,255"; // cyan
                        break;
                    case 5:
                        modo.COLOR = "255,0,255"; // magenta
                        break;
                    case 6:
                        modo.COLOR = "255,140,0"; // dark orange
                        break;
                    case 7:
                        modo.COLOR = "138,43,226"; // blue violet
                        break;
                    case 8:
                        modo.COLOR = "186,85,211"; // medium orchid
                        break;
                    case 9:
                        modo.COLOR = "255,20,147"; // deep pink
                        break;
                    case 10:
                        modo.COLOR = "0,0,139"; // dark blue
                        break;
                    case 11:
                        modo.COLOR = "139,69,19"; // saddle brown
                        break;
                    case 12:
                        modo.COLOR = "176,196,222"; // light steel blue
                        break;
                    case 13:
                        modo.COLOR = "245,222,179"; // wheat
                        break;
                    case 14:
                        modo.COLOR = "139,0,139"; // dark magenta
                        break;
                    case 15:
                        modo.COLOR = "30,144,255"; // dodger blue
                        break;
                    case 16:
                        modo.COLOR = "127,255,212"; // aqua marine
                        break;
                    case 17:
                        modo.COLOR = "50,205,50"; // lime green
                        break;
                    case 18:
                        modo.COLOR = "255,255,255"; // white
                        break;
                }
            }

            ViewBag.ModosTransporte = modos.ToList();*/

            return View();
        }

        public ActionResult PMESDashboard(string v, int e, int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;

            if (t != null)
            {
                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

                if (rolesUsuario != null && rolesUsuario.Count() > 0)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            ViewBag.IdTercero = idTercero;
            ViewBag.Vigencia = v;
            ViewBag.Encuesta = e;
            return View();
        }

        public ActionResult PMESDashboardS(string v, string vbase, int e, int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;

            if (t != null)
            {
                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

                if (rolesUsuario != null && rolesUsuario.Count() > 0)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            ViewBag.IdTercero = idTercero;
            ViewBag.Vigencia = v;
            ViewBag.VigenciaBase = vbase;
            ViewBag.Encuesta = e;
            return View();
        }


        [Authorize]
        public ActionResult PMESMapaContainer(string v, int e, int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;

            var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

            if (t != null)
            {
                if (rolesUsuario != null && rolesUsuario.Count() > 0)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            ViewBag.idTercero = idTercero;
            ViewBag.seleccionTercero = !terceroUsuario;

            ViewBag.IdEncuesta = e;
            ViewBag.Vigencia = v;

            string sql;

            if (e == 1)
            {
                sql = "SELECT ID_RESPUESTA AS ID_MODO, TRIM(S_VALOR) AS MODO " +
                            "FROM CONTROL.ENC_OPCION_RESPUESTA " +
                            "WHERE ID_PREGUNTA = 529 " +
                            "ORDER BY N_ORDEN";
            }
            else
            {
                sql = "SELECT ID_RESPUESTA AS ID_MODO, TRIM(S_VALOR) AS MODO " +
                            "FROM CONTROL.ENC_OPCION_RESPUESTA " +
                            "WHERE ID_PREGUNTA = 2486 " +
                            "ORDER BY N_ORDEN";
            }

            var modos = dbSIM.Database.SqlQuery<ModoTransporte>(sql).ToList();

            for (int i = 0; i < modos.Count; i++)
            {
                var modo = modos[i];
                switch (i)
                {
                    case 0:
                        modo.COLOR = "255,0,0"; // red
                        break;
                    case 1:
                        modo.COLOR = "0,255,0"; // lime
                        break;
                    case 2:
                        modo.COLOR = "0,0,255"; // blue
                        break;
                    case 3:
                        modo.COLOR = "255,255,0"; // yellow
                        break;
                    case 4:
                        modo.COLOR = "0,255,255"; // cyan
                        break;
                    case 5:
                        modo.COLOR = "255,0,255"; // magenta
                        break;
                    case 6:
                        modo.COLOR = "255,140,0"; // dark orange
                        break;
                    case 7:
                        modo.COLOR = "138,43,226"; // blue violet
                        break;
                    case 8:
                        modo.COLOR = "186,85,211"; // medium orchid
                        break;
                    case 9:
                        modo.COLOR = "255,20,147"; // deep pink
                        break;
                    case 10:
                        modo.COLOR = "0,0,139"; // dark blue
                        break;
                    case 11:
                        modo.COLOR = "139,69,19"; // saddle brown
                        break;
                    case 12:
                        modo.COLOR = "176,196,222"; // light steel blue
                        break;
                    case 13:
                        modo.COLOR = "245,222,179"; // wheat
                        break;
                    case 14:
                        modo.COLOR = "139,0,139"; // dark magenta
                        break;
                    case 15:
                        modo.COLOR = "30,144,255"; // dodger blue
                        break;
                    case 16:
                        modo.COLOR = "127,255,212"; // aqua marine
                        break;
                    case 17:
                        modo.COLOR = "50,205,50"; // lime green
                        break;
                    case 18:
                        modo.COLOR = "0,0,128"; // navy
                        break;
                    case 19:
                        modo.COLOR = "0,128,128"; // teal
                        break;
                    case 20:
                        modo.COLOR = "128,128,0"; // olive
                        break;
                    case 21:
                        modo.COLOR = "128,0,128"; // purple
                        break;
                    case 22:
                        modo.COLOR = "70,130,180"; // steel blue
                        break;
                }
            }

            ViewBag.ModosTransporte = modos.ToList();

            return View();
        }

        [Authorize]
        public ActionResult PMESMapa(string v, int e, int? t, int? i, string modos, string todos)
        {
            if ((modos != null && modos.Trim() != "") || todos == "S")
            {
                string sqlModoColor = "";
                string sqlModo = "";
                string[] modosSel = modos.Split('|');

                System.Web.HttpContext context = System.Web.HttpContext.Current;
                int idTercero = 0;
                bool terceroUsuario = true;

                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

                if (t != null)
                {
                    if (rolesUsuario != null && rolesUsuario.Count() > 0)
                    {
                        idTercero = (int)t;
                        terceroUsuario = false;
                    }
                }

                if (terceroUsuario)
                    idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

                foreach (string modo in modosSel)
                {
                    if (modo.Trim() != "")
                    {
                        var modoDato = modo.Split('-');
                        //sqlModoColor += " WHEN ID_MODOP = " + modoDato[0] + " THEN '" + modoDato[1] + "'";
                        sqlModoColor += " WHEN me.ID_MODOBASE = " + modoDato[0] + " THEN '" + modoDato[1] + "'";
                        if (sqlModo == "")
                            sqlModo = modoDato[0];
                        else
                            sqlModo += "," + modoDato[0];
                    }
                }

                var sql = "SELECT p.ID_MODOP AS ID_MODO, CASE " + sqlModoColor + " ELSE '255, 255, 255' END AS COLOR, " +
                            "REPLACE(CAST(p.N_LATITUD AS VARCHAR2(50)), ',', '.') AS LATITUD, REPLACE(CAST(p.N_LONGITUD AS VARCHAR2(50)), ',', '.') AS LONGITUD " +
                            "FROM CONTROL.VWM_PMES p LEFT OUTER JOIN " +
                            "   CONTROL.PMES_MODOS_EQUIV me ON p.ID_MODOP = me.ID_MODO " +
                            "WHERE p.ID_ENCUESTA = " + e.ToString() + " AND p.VIGENCIA = '" + v + "' AND p.ID_TERCERO = " + idTercero.ToString() + (i != null && i != -1? " AND p.ID_INSTALACION = " + i.ToString() + " " : "") + " AND NVL(p.N_LATITUD, 0) <> 0 AND NVL(p.N_LONGITUD, 0) <> 0 " + (todos != "S" ? " AND me.ID_MODOBASE IN (" + sqlModo + ")" : "");

                var ubicaciones = dbSIM.Database.SqlQuery<UbicacionesModoTransporte>(sql).ToList();

                var sqlInstalaciones = "SELECT DISTINCT ID_INSTALACION, INSTALACION, " +
                            "REPLACE(CAST(N_LATITUD_INSTALACION AS VARCHAR2(50)), ',', '.') AS LATITUD, REPLACE(CAST(N_LONGITUD_INSTALACION AS VARCHAR2(50)), ',', '.') AS LONGITUD " +
                            "FROM CONTROL.VWM_PMES " +
                            "WHERE ID_ENCUESTA = " + e.ToString() + " AND VIGENCIA = '" + v + "' AND ID_TERCERO = " + idTercero.ToString() + (i != null && i != -1 ? " AND ID_INSTALACION = " + i.ToString() + " " : "") + " AND NVL(N_LATITUD_INSTALACION, 0) <> 0 AND NVL(N_LONGITUD_INSTALACION, 0) <> 0";

                var instalaciones = dbSIM.Database.SqlQuery<UbicacionInstalacion>(sqlInstalaciones).ToList();

                ViewBag.ubicaciones = ubicaciones;
                ViewBag.instalaciones = instalaciones;
            }
            else
            {
                ViewBag.ubicaciones = new List<UbicacionesModoTransporte>();
                ViewBag.instalaciones = new List<UbicacionInstalacion>();
            }

            return View();
        }

        public ActionResult PMESDashboardPartial(string v, int e, int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;

            bool terceroUsuario = true;

            if (t != null)
            {
                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

                if (rolesUsuario != null && rolesUsuario.Count() > 0)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            ViewBag.IdTercero = idTercero;
            ViewBag.Vigencia = v;
            ViewBag.Encuesta = e;

            PMESViewerSettings.IDDashboard = e;
            PMESViewerSettings.idTercero = idTercero;
            PMESViewerSettings.vigencia = v;
            return PartialView("_PMESDashboardPartial", PMESViewerSettings.Model);
        }

        public ActionResult PMESDashboardSPartial(string v, string vbase, int e, int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;

            if (t != null)
            {
                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

                if (rolesUsuario != null && rolesUsuario.Count() > 0)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            if ((vbase ?? "").Trim() == "")
            {
                var vigenciaBase = dbSIM.VWM_PMES.Where(p => p.ID_TERCERO == idTercero).OrderBy(p => p.VIGENCIA).Select(p => p.VIGENCIA).FirstOrDefault();
                vbase = vigenciaBase;
            }

            ViewBag.IdTercero = idTercero;
            ViewBag.Vigencia = v;
            ViewBag.VigenciaBase = vbase;
            ViewBag.Encuesta = e;

            PMESViewerSettings.IDDashboard = 100;
            PMESViewerSettings.idTercero = idTercero;
            PMESViewerSettings.vigencia = v;
            PMESViewerSettings.vigenciaBase = vbase;
            return PartialView("_PMESDashboardSPartial", PMESViewerSettings.Model);
        }

        public FileStreamResult PMESDashboardPartialExport(string v, int e, int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;

            if (t != null)
            {
                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

                if (rolesUsuario != null && rolesUsuario.Count() > 0)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario) 
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            ViewBag.idTercero = idTercero;
            ViewBag.vigencia = v;

            PMESViewerSettings.IDDashboard = e;
            PMESViewerSettings.idTercero = idTercero;
            PMESViewerSettings.vigencia = v;
            return DevExpress.DashboardWeb.Mvc.DashboardViewerExtension.Export("PMESViewer", PMESViewerSettings.Model);
        }

        public FileStreamResult PMESDashboardSPartialExport(string v, string vbase, int e, int? t)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            int idTercero = 0;
            bool terceroUsuario = true;

            if (t != null)
            {
                var rolesUsuario = ((System.Security.Claims.ClaimsPrincipal)context.User).FindAll(CustomClaimTypes.IdRol).Where(r => r.Value == "402");

                if (rolesUsuario != null && rolesUsuario.Count() > 0)
                {
                    idTercero = (int)t;
                    terceroUsuario = false;
                }
            }

            if (terceroUsuario)
                idTercero = int.Parse(((System.Security.Claims.ClaimsPrincipal)context.User).FindFirst(CustomClaimTypes.IdTercero).Value);

            if ((vbase ?? "").Trim() == "")
            {
                var vigenciaBase = dbSIM.VWM_PMES.Where(p => p.ID_TERCERO == idTercero).OrderBy(p => p.VIGENCIA).Select(p => p.VIGENCIA).FirstOrDefault();
                vbase = vigenciaBase;
            }

            ViewBag.idTercero = idTercero;
            ViewBag.vigencia = v;
            ViewBag.vigenciaBase = vbase;

            PMESViewerSettings.IDDashboard = e;
            PMESViewerSettings.idTercero = idTercero;
            PMESViewerSettings.vigencia = v;
            PMESViewerSettings.vigenciaBase = vbase;
            return DevExpress.DashboardWeb.Mvc.DashboardViewerExtension.Export("PMESViewer", PMESViewerSettings.Model);
        }
    }

    class PMESViewerSettings
    {
        public static int IDDashboard;
        public static int idTercero;
        public static string vigencia;
        public static string vigenciaBase;

        public static DevExpress.DashboardWeb.Mvc.DashboardSourceModel Model
        {
            get
            {
                var model = new DevExpress.DashboardWeb.Mvc.DashboardSourceModel();

                switch (IDDashboard)
                {
                    case 1:
                        model.DashboardSource = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Dashboard/PMES.xml");
                        break;
                    case 2:
                        model.DashboardSource = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Dashboard/PMES_2021.xml");
                        break;
                    /*case 3:
                        model.DashboardSource = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Dashboard/PMES_CP.xml");
                        break;*/
                    case 3:
                        model.DashboardSource = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Dashboard/PMES_2022.xml");
                        break;
                    case 100:
                        model.DashboardSource = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Dashboard/SPMES_2021.xml");
                        break;
                }

                /*model.DashboardLoading = (sender, e) =>
                    {
                        using (DevExpress.DashboardCommon.Dashboard a = new DevExpress.DashboardCommon.Dashboard())
                        {
                            a.LoadFromXml(e.DashboardXml);
                            foreach (var item in a.Items)
                            {
                                item.Name = "Custom " + item.Name;
                            }
                            MemoryStream stream = new MemoryStream();
                            a.SaveToXml(stream);
                            stream.Position = 0;
                            using (StreamReader streamReader = new StreamReader(stream))
                                e.DashboardXml = streamReader.ReadToEnd();

                            a.Title = "PRUEBA DE TITULO";
                        }
                    };*/

 
                model.DataLoading = (object sender, DataLoadingWebEventArgs e) =>
                {
                    if (e.DataSourceName.Substring(0, 4) == "PMES")
                        e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = idTercero }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = vigencia }, new Parametro() { Nombre = ":idEncuesta", Valor = IDDashboard } });
                    else if (e.DataSourceName.Substring(0, 5) == "SPMES")
                        e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = idTercero }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = vigencia }, new Parametro() { Nombre = ":vigenciaEncuestaBase", Valor = vigenciaBase } });
                        //e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero1", Valor = 31396 }, new Parametro() { Nombre = ":idTercero2", Valor = 148285 }, new Parametro() { Nombre = ":vigenciaEncuesta1", Valor = 2018 }, new Parametro() { Nombre = ":vigenciaEncuesta2", Valor = 2020 } });
                    else
                        e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = idTercero }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = vigencia }, new Parametro() { Nombre = ":idEncuesta", Valor = IDDashboard } });

                    //e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = idTercero }, new Parametro() { Nombre = ":vigenciaEncuesta", Valor = vigencia }, new Parametro() { Nombre = ":idEncuesta", Valor = IDDashboard } });
                };

                /*model.DashboardLoaded = (object sender, DashboardLoadedWebEventArgs e) =>
                {
                    if (e.Dashboard.DataSources[0].Name.Substring(0, 5) == "SPMES")
                    {
                        e.Dashboard.
                        e.Dashboard.Parameters.Title = "PLAN DE MOVILIDAD EMPRESARIAL SOSTENIBLE - " + vigenciaBase.ToString() + " vs " + vigencia.ToString();
                    }
                };*/

                /*model.DashboardLoading = (sender, e) =>
                {

                    // Checks the identifier of the required dashboard.
                    //if (e.DashboardId == "SalesDashboard")
                    //{
                        // Writes the dashboard XML definition from a file to a string.
                        string dashboardDefinition = File.ReadAllText(System.Web.Hosting.
                            HostingEnvironment.MapPath(@"~\App_Data\SalesDashboard.xml"));

                        // Specifies the dashboard XML definition.
                        e.DashboardXml = dashboardDefinition;
                    //}
                };

                model.ConfigureDataConnection = (sender, e) =>
                {
                    DevExpress.DataAccess.ConnectionParameters.
                    if (e.DataSourceName == "SQL Data Source 1")
                    {
                        // Gets connection parameters used to establish a connection to the database.
                        Access97ConnectionParameters parameters =
                            (Access97ConnectionParameters)e.ConnectionParameters;
                        string databasePath = System.Web.Hosting.
                            HostingEnvironment.MapPath(@"~\App_Data\nwind.mdb");
                        // Specifies the path to a database file.  
                        parameters.FileName = databasePath;
                    }
                };*/

                return model;
            }
        }
    }
}