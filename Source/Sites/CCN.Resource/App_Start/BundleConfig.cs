using System.Web;
using System.Web.Optimization;

namespace CCN.Resource
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            //js引用
            bundles.Add(new ScriptBundle("~/bundles/original").Include(
                      "~/Scripts/jquery-1.10.2.js",
                      "~/Plugins/My97DatePicker/WdatePicker.js",
                      "~/Scripts/jquery.pagination/jquery.twbsPagination.min.js",
                      "~/Scripts/jquery.fn.extend.js",
                      "~/Scripts/common.js",
                      "~/Scripts/highcharts.js",
                      "~/Scripts/CCN_Chart.js")); 

            //模版js引用
            bundles.Add(new ScriptBundle("~/bundles/template").Include(
                      "~/Scripts/js/skins.min.js",
                      "~/Scripts/js/slimscroll/jquery.slimscroll.min.js",
                      "~/Scripts/js/beyond.js",
                      "~/Scripts/js/charts/sparkline/jquery.sparkline.js",
                      "~/Scripts/js/charts/sparkline/sparkline-init.js",
                      "~/Scripts/js/charts/easypiechart/jquery.easypiechart.js",
                      "~/Scripts/js/charts/easypiechart/easypiechart-init.js",
                      "~/Scripts/js/charts/flot/jquery.flot.js",
                      "~/Scripts/js/charts/flot/jquery.flot.resize.js",
                      "~/Scripts/js/charts/flot/jquery.flot.pie.js",
                      "~/Scripts/js/charts/flot/jquery.flot.tooltip.js",
                      "~/Scripts/js/charts/flot/jquery.flot.orderBars.js"));

                 bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //模版css引用
            bundles.Add(new StyleBundle("~/Content/template").Include(
                      "~/Content/Template/css/bootstrap.min.css",
                      "~/Content/Template/css/font-awesome.min.css",
                      "~/Content/Template/css/weather-icons.min.css",
                      "~/Content/Template/css/beyond.min.css",
                      "~/Content/Template/css/demo.min.css",
                      "~/Content/Template/css/typicons.min.css",
                      "~/Content/Template/css/animate.min.css",
                      "~/Content/Template/assets/img/favicon.png"));
        }
    }
}

