using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace SIM
{
    public class DevExtremeBundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            var scriptBundle = new ScriptBundle("~/Scripts/ScriptsV5/DevExtremeBundle");
            var styleBundle = new StyleBundle("~/Content/ContentV5/ContentV5DevExtremeBundle");

            // CLDR scripts
            scriptBundle
                .Include("~/Scripts/ScriptsV5/cldr.js")
                .Include("~/Scripts/ScriptsV5/cldr/event.js")
                .Include("~/Scripts/ScriptsV5/cldr/unresolved.js");


            // NOTE: jQuery may already be included in the default script bundle. Check the BundleConfig.cs file​​​
            scriptBundle
                .Include("~/Scripts/ScriptsV5/jquery-3.4.1.js")
                .Include("~/Scripts/ScriptsV5/jquery-ui-1.12.1/jquery-ui.min.js");

            // JSZip for client side export
            scriptBundle
                .Include("~/Scripts/ScriptsV5/jszip.js");

            // DevExtreme + extensions
            scriptBundle
                .Include("~/Scripts/ScriptsV5/dx.all.js")
                .Include("~/Scripts/ScriptsV5/aspnet/dx.aspnet.data.js")
                .Include("~/Scripts/ScriptsV5/aspnet/dx.aspnet.mvc.js");

            // DevExtreme + Español
            scriptBundle
                //.Include("~/Scripts/ScriptsV5/localization/dx.messages.es.js");
                .Include("~/Scripts/ScriptsV5/globalize/dx.messages.es.js")
                .Include("~/Scripts/ScriptsV5/cldr/supplemental.js");

             // Globalize 1.x
             scriptBundle
                .Include("~/Scripts/ScriptsV5/globalize.js")
                .Include("~/Scripts/ScriptsV5/globalize/message.js")
                .Include("~/Scripts/ScriptsV5/globalize/number.js")
                .Include("~/Scripts/ScriptsV5/globalize/currency.js")
                .Include("~/Scripts/ScriptsV5/globalize/date.js");

            // dxVectorMap data (http://js.devexpress.com/Documentation/Guide/Data_Visualization/VectorMap/Providing_Data/#Data_for_Areas)
            //scriptBundle
            //    .Include("~/Scripts/ScriptsV5/vectormap-data/africa.js")
            //    .Include("~/Scripts/ScriptsV5/vectormap-data/canada.js")
            //    .Include("~/Scripts/ScriptsV5/vectormap-data/eurasia.js")
            //    .Include("~/Scripts/ScriptsV5/vectormap-data/europe.js")
            //    .Include("~/Scripts/ScriptsV5/vectormap-data/usa.js")
            //    .Include("~/Scripts/ScriptsV5/vectormap-data/world.js");


            // DevExtreme
            // NOTE: see the available theme list here: http://js.devexpress.com/Documentation/Guide/Themes/Predefined_Themes/                    
            styleBundle
                .Include("~/Content/ContentV5/dx.common.css")
                .Include("~/Content/ContentV5/dx.light.css")
                .Include("~/Scripts/ScriptsV5/jquery-ui-1.12.1/themes-1.12.1/themes/sim/jquery-ui.css");


            bundles.Add(scriptBundle);
            bundles.Add(styleBundle);

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}