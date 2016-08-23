using System.Web;
using System.Web.Optimization;

namespace BlogApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/BlogAppJS").Include(
                     "~/Scripts/jquery-min.js",
                     "~/Scripts/bootstrap-min.js",
                     "~/Scripts/clean-blog-min.js",
                     "~/Scripts/jCookie-min.js",
                     "~/Scripts/commonMessages.js",
                     "~/Scripts/common.js"


                     ));

            bundles.Add(new StyleBundle("~/Content/BlogAppCSS").Include("~/Content/bootstrap/css/bootstrap-min.css", "~/Content/clean-blog-min.css", "~/Content/font-awesome/css/font-awesome-min.css", "~/Content/common.css"));

        }
    }
}