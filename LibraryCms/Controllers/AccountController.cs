using System.Web.Mvc;
using LibraryCms.Models;

namespace LibraryCms.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            if (Session["isLogin"] != null && Session["isLogin"].ToString() == "True")
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        public void Logout()
        {
            Session["isLogin"] = "False";
            Response.Redirect("/");
        }

        [HttpPost]
        public JsonResult Check()
        {
            string strAccount = Request["account"];
            string strPassword = Request["password"];
            User user = DAL.CheckLogin(strAccount, strPassword);

            JsonResult res = new JsonResult();

            if (user != null)
            {
                res.Data = new {status = "1", message = "登陆成功"};
                Session["isLogin"] = "True";
                Session["User"] = user;
            }
            else
            {
                res.Data = new {status = "0", message = "账号或者密码不正确"};
                Session["isLogin"] = "False";
                Session["User"] = null;
            }
            
            return res;
        }

        [HttpPost]
        public JsonResult UpdateUserInfo()
        {
            string number = Request["number"];
            string name = Request["name"];
            string qq = Request["qq"];
            string sex = Request["sex"];
            User user = DAL.GetUser(number);
            if (user == null)
            {
                return Json("Account does not exists.");
            }
            user.Name = name;
            user.Sex = sex;
            user.QQ = qq;
            int i = DAL.UpdateUserInfo(user);
            if (i == 0)
            {
                return Json("sql error");
            }
            Session["User"] = user;
            return Json("success");
        }

    }
}
