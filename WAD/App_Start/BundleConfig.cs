using System.Web;
using System.Web.Optimization;

namespace WAD
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/fonts/icomoon/style.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/fonts/flaticon/font/flaticon.css",
                      "~/Content/magnific-popup.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/owl.carousel.min.css",
                      "~/Content/owl.theme.default.min.css",
                      "~/Content/aos.css",
                      "~/Content/style.css"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/pharma").Include(
                      "~/Scripts/jquery-3.3.1.min.js",
                      "~/Scripts/jquery-ui.js",
                      "~/Scripts/popper.min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/owl.carousel.min.js",
                      "~/Scripts/jquery.magnific-popup.min.js",
                      "~/Scripts/aos.js",
                      "~/Scripts/main.js"
                      ));
        }
    }
}
