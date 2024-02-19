using System.Web;
using System.Web.Optimization;

namespace MVCBlog.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery-ui-1.12.1.js",
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/jquery.lightbox.js",
                    "~/Scripts/jquery.autocomplete.pack.js",
                    "~/Scripts/SyntaxHighlighter/shCore.js",
                    "~/Scripts/SyntaxHighlighter/shAutoloader.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/custom.js",
                    "~/Scripts/jquery-3.1.1.js",
                    "~/Scripts/jquery.unobtrusive-ajax.js"));



            bundles.Add(new StyleBundle("~/css/combined")
                .Include(
                    "~/Content/bootstrap.css",
                    "~/Content/bootstrap-datetimepicker.min.css",
                    "~/Content/shCoreDefault.css",
                    "~/Content/custom.css",
                    "~/Content/lightbox.css",
                    "~/Content/jquery.autocomplete.css"));
        }
    }
}
