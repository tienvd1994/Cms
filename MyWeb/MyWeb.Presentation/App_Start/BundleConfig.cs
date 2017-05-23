using System.Web;
using System.Web.Optimization;

namespace MyWeb.Presentation
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
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

            bundles.Add(new StyleBundle("~/style/admin").Include(
              "~/Content/bootstrap.min.css",
              "~/Content/font-awesome.min.css",
              "~/Content/ionicons.min.css",
              "~/Content/AdminLTE.css",
              "~/Content/_all-skins.min.css",
              "~/Content/main.css"
              ));

            bundles.Add(new ScriptBundle("~/scripts/admin").Include(
                "~/Scripts/jquery-3.1.1.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/jquery.slimscroll.min.js",
                "~/Scripts/fastclick.js",
                "~/Scripts/app.js",
                "~/Scripts/demo.js"
                ));
        }
    }
}
