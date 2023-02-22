using System.Web;
using System.Web.Optimization;

namespace SIM
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
             * */

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                   "~/Scripts/bootstrap.js",
                   "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/carouFredSel").Include(
                "~/Scripts/jquery.carouFredSel-6.2.1-packed.js",
                      "~/Scripts/jquery.mousewheel.min.js",
                         "~/Scripts/jquery.touchSwipe.min.js",
                            "~/Scripts/jquery.transit.min.js",
                      "~/Scripts/jquery.ba-throttle-debounce.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/uploadfile.css",
                      //"~/Content/bootstrap.css",
                      "~/Content/main.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/forms.css?v=1.02",
                      "~/Content/grid.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/webcam").Include(
                       "~/Scripts/webcam/jquery.webcam.js"));

            bundles.Add(new ScriptBundle("~/bundles/uploadfile").Include(
                       "~/Scripts/jquery.uploadfile.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // V5
            bundles.Add(new ScriptBundle("~/bundlesv5/jquery").Include(
                        "~/Scripts/ScriptsV5/jquery-3.4.1.js"));

            bundles.Add(new ScriptBundle("~/bundlesv5/jqueryval").Include(
                        "~/Scripts/ScriptsV5/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundlesv5/modernizr").Include(
                        "~/Scripts/ScriptsV5/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundlesv5/bootstrap").Include(
                      "~/Scripts/ScriptsV5/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Contentv5/css").Include(
                      "~/Content/ContentV5/bootstrap.css"));
        }
    }
}
