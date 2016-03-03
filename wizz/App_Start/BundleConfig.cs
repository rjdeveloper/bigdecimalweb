using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;


namespace wizz.App_Start
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-1.11.2.min.js",
                   "~/Scripts/siteUrl.js",
                  "~/Scripts/custom.js",
                  "~/Scripts/enscroll-0.6.1.min.js",
                  "~/Scripts/bootstrap.min.js",
                  "~/Scripts/common/siteUrl.js",
                  "~/Scripts/common/customNotifications.js",
                    "~/Scripts/jquery-ui.js",
                  "~/Scripts/jquery-ui-timepicker-addon.js",
                "~/Scripts/Angular/angular.min.js",
                "~/Scripts/Angular/angular-route.js",
                "~/Scripts/Angular/ngTable/ng-table.js",
               
                  "~/Scripts/Angular/directives.js",
                  "~/Scripts/Angular/filters.js",
                  "~/Scripts/Angular/HttpLoader.js",
                           "~/Scripts/Angular/controllers.js",
                 "~/Scripts/Angular/angular-sanitize.js",
                 "~/Scripts/modernizr-2.6.2.js",                
                   "~/Scripts/ui-bootstrap-tpls-0.10.0.js",
                        "~/Scripts/angular-chart.js/Chart.js",
                     "~/Scripts/angular-chart.js/angular-chart.js",
                    "~/Scripts/Angular/app.js",
                      "~/Scripts/Angular/services.js"
                        ));

            bundles.Add(new StyleBundle("~/Content1/css").Include(
                    "~/Scripts/Angular/ngTable/ng-table.css",
                    "~/Content/cssmine/bootstrap.css",
                     "~/Content/cssmine/style.css",
                       "~/Scripts/angular-chart.js/angular-chart.css"
                    ));

            BundleTable.EnableOptimizations = true;


        }
    }
}