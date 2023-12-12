using DevExpress.Web.Mvc;
using DevExpress.XtraReports.Security;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SIM
{
    /// <summary>
    /// 
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            DevExtremeBundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DevExpress.Data.Helpers.ServerModeCore.DefaultForceCaseInsensitiveForAnySource = true;
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();

            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);

            /*var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };*/
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            DevExpressHelper.Theme = "DevEx";
        }
    }
}
