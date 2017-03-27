using System.Web;
using System.Web.Optimization;

namespace LibraryCms
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

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

            ////////////////////////////////////////////////////////////////////////////////////
            bundles.Add(new StyleBundle("~/Content/css-login").Include("~/Content/css/login.css"));
            bundles.Add(new ScriptBundle("~/Scripts/js-account").Include("~/Scripts/account.js"));
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include("~/Scripts/jquery.js"));
            bundles.Add(new StyleBundle("~/Content/css-layout").Include("~/Content/css/_Layout.css"));
            bundles.Add(new ScriptBundle("~/Scripts/js-layout").Include("~/Scripts/_Layout.js"));
            bundles.Add(new StyleBundle("~/Content/css-personalcenter").Include("~/Content/css/personalcenter.css"));
            bundles.Add(new StyleBundle("~/Content/css-advancemanage").Include("~/Content/css/advancemanage.css"));
            bundles.Add(new ScriptBundle("~/Scripts/js-departmentmanage").Include("~/Scripts/DepartmentManage.js"));
            bundles.Add(new ScriptBundle("~/Scripts/js-groupmanage").Include("~/Scripts/GroupManage.js"));
            bundles.Add(new ScriptBundle("~/Scripts/js-usermanage").Include("~/Scripts/UserManage.js"));
            bundles.Add(new StyleBundle("~/Content/css-usermanage").Include("~/Content/css/UserManage.css"));
            bundles.Add(new ScriptBundle("~/Scripts/js-layer").Include("~/Content/layer/layer.js", "~/Scripts/dialog.js"));
            bundles.Add(new ScriptBundle("~/Scripts/js-bookmanage").Include("~/Scripts/BookManage.js"));
            bundles.Add(new StyleBundle("~/Content/css-bookupload").Include("~/Content/css/bookupload.css"));
            bundles.Add(new ScriptBundle("~/Scripts/js-bookupload").Include("~/Scripts/bookupload.js"));
            bundles.Add(new ScriptBundle("~/Scripts/js-updatemail").Include("~/Scripts/updatemail.js"));
            bundles.Add(new ScriptBundle("~/Scripts/js-updatpassword").Include("~/Scripts/updatepassword.js"));
            bundles.Add(new ScriptBundle("~/Scripts/js-addgroup").Include("~/Scripts/addgroup.js"));
            bundles.Add(new StyleBundle("~/Content/css-addgroup").Include("~/Content/css/addgroup.css"));
            bundles.Add(new ScriptBundle("~/Scripts/js-updategroup").Include("~/Scripts/updateGroup.js"));
            bundles.Add(new StyleBundle("~/Content/css-sendmessage").Include("~/Content/css/sendMessage.css"));
            bundles.Add(new ScriptBundle("~/Scripts/js-sendmessage").Include("~/Scripts/sendMessage.js"));
        }
    }
}