using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.ConnectionParameters;
using System.Configuration;
using DashboardDesignerClass;

namespace WebBaseDashboard.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        public ActionResult Actuaciones()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult DashboardViewerPartial()
        {
            return PartialView("_DashboardViewerPartial", DashboardViewerSettings.Model);
        }

        public FileStreamResult DashboardViewerPartialExport()
        {
            return DevExpress.DashboardWeb.Mvc.DashboardViewerExtension.Export("DashboardViewer", DashboardViewerSettings.Model);
        }
    }

    class DashboardViewerSettings
    {
        public static DevExpress.DashboardWeb.Mvc.DashboardSourceModel Model
        {
            get
            {
                var model = new DevExpress.DashboardWeb.Mvc.DashboardSourceModel();
                //model.DashboardSource = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Visitas.xml");
                model.DashboardSource = System.Web.Hosting.HostingEnvironment.MapPath(@"~\PMES_CP.xml");

                model.DataLoading = (sender, e) =>
                {
                    e.Data = Consultas.ObtenerDatosFuente(e.DataSourceName, ConfigurationManager.ConnectionStrings["SIMOracle"].ConnectionString, new List<Parametro>() { new Parametro() { Nombre = ":idTercero", Valor = 414 }, new Parametro() { Nombre = ":idTercero", Valor = 414 }, new Parametro() { Nombre = ":idInstalacion", Valor = DBNull.Value }, new Parametro() { Nombre = ":idInstalacion", Valor = DBNull.Value } });
                };

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