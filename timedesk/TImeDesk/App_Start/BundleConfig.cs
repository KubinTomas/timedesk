using System.Web;
using System.Web.Optimization;

namespace TImeDesk
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/jquery.ba-throttle-debounce.js",
                        "~/Scripts/bootstrap-datetimepicker.js",
                        "~/Scripts/bootbox.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/datatables/datatables.bootstrap.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/homepage.css",
                      "~/Content/sidebar.css",
                      "~/Content/timer.css",
                      "~/Content/project.css",
                      "~/Content/icons.css",
                      "~/Content/dropDownContainer.css",
                      "~/Content/timeEntry.css",
                      "~/Content/spectrum.css",
                      "~/Content/dashboard.css",
                      "~/Content/font-awesome-4.7.0/css/font-awesome.css",
                      "~/Content/datatables/css/datatables.bootstrap.css",
                      "~/Content/daterangepicker.css",
                      "~/Content/customElements.css",
                      "~/Content/todolist.css",
                      "~/Content/workspace.css",
                      "~/Content/workspaceDetail.css",
                      "~/Content/createworkspace.css",
                      "~/Content/bootstrap-datepicker/bootstrap-datepicker3.css"
                   
                      ));


        }
    }
}
