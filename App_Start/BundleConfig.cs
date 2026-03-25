using System.Web;
using System.Web.Optimization;

namespace FIlms
{
    /// <summary>
    /// Bundle configuration for scripts and styles
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Register bundles for the application
        /// </summary>
        /// <param name="bundles">Bundle collection</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // jQuery bundle
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // jQuery UI bundle
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            // Modernizr bundle
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // WebForms bundle
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                        "~/Scripts/WebForms/WebForms.js",
                        "~/Scripts/WebForms/WebUIValidation.js",
                        "~/Scripts/WebForms/MenuStandards.js",
                        "~/Scripts/WebForms/Focus.js",
                        "~/Scripts/WebForms/GridView.js",
                        "~/Scripts/WebForms/DetailsView.js",
                        "~/Scripts/WebForms/TreeView.js",
                        "~/Scripts/WebForms/WebParts.js"));

            // MSAjax bundle
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                        "~/Scripts/WebForms/MSAjax/MicrosoftAjax.js",
                        "~/Scripts/WebForms/MSAjax/MicrosoftAjaxApplicationServices.js",
                        "~/Scripts/WebForms/MSAjax/MicrosoftAjaxTimer.js",
                        "~/Scripts/WebForms/MSAjax/MicrosoftAjaxWebForms.js"));

            // CSS bundle
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/Site.css"));

            // jQuery UI CSS bundle
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}
