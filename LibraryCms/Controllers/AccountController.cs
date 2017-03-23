using System.Web.Mvc;
using LibraryCms.Models;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

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

        public ActionResult UpdateMail()
        {
            if (Request["newMail"] == null)
            {
                return View();
            }
            string newMail = Request["newMail"];
            User user = (User)Session["User"];
            user.Mail = newMail;
            SendMessage(user);
            return Json("success");
        }

        private void SendMessage(User user)
        {
            string htmlBody = "<!DOCTYPE html>";
            htmlBody += "<html>";
            htmlBody += "<head>";
            htmlBody += "<meta charset='utf-8'>";
            htmlBody += "<meta http-equiv='X-UA-Compatible' content='IE=edge'>";
            htmlBody += "<title></title>";
            htmlBody += "<style>";
            htmlBody += @"td span{color:#f16524;}";
            htmlBody += "</style>";
            htmlBody += "</head>";
            htmlBody += "<body>";
            htmlBody += string.Format("<h3>您的邮箱即将被修改为{0}，如不是本人操作请忽略该邮件</h3>", user.Mail);
            htmlBody += "<h4>请单击以下链接或者复制链接到浏览器中以确认您的修改：</h4>";
            string url = Request.Url.ToString();
            string action = RouteData.Values["action"].ToString();
            string handleAction = "HandleMail/";
            url = url.Substring(0, url.Length - action.Length);
            url += handleAction;
            url = url + user.ToString();
            htmlBody += string.Format("<a href='{0}'>确认修改</a>", url);
            htmlBody += "<p></p>";
            htmlBody += string.Format("<p>{0}</p>", url);
            htmlBody += "<p></p>";
            htmlBody += string.Format("<p>{0}</p>", DateTime.Now);
            htmlBody += "<p></p>";
            htmlBody += "<p>本邮件由系统自动发送，请勿回复</p>";
            htmlBody += "</body>";
            htmlBody += "</html>";
            Mail.SnedMsgTo(user.Mail, "信息修改确认", htmlBody, true);
        }

        public ActionResult HandleMail()
        {
            string id = "";
            if (ControllerContext.RouteData.Values["ID"] == null)
            {
                Response.Write("地址非法");
                return null;
            }
            id = ControllerContext.RouteData.Values["ID"].ToString();
            User user = new User(id);
            if (user.UserID == "Incorrect Data")
            {
                return View("WrongId");
            }
            if (user.UserID == "Time Out")
            {
                Response.Write("超时");
                return null;
            }
            return View("CheckInfo");
        }

        public ActionResult UpdatePassword()
        {
            if (Request["newPassword"] == null)
            {
                return View();
            }
            string newPassword = Request["newPassword"];
            User user = (User)Session["User"];
            user.Password = MD5.MD5_Encode(newPassword);
            int i = DAL.UpdateUserInfo(user);
            if (i == 0)
            {
                return Json("sql error");
            }
            Session["User"] = user;
            return Json("success");
        }

        [HttpPost]
        public JsonResult GetUserMessage()
        {
            User user = (User)Session["User"];//获取当前登录的用户
            if (user == null)
            {
                return Json("need login");
            }
            int msgType = int.Parse(Request["msgType"].ToString());
            List<Message> msgs = DAL.GetPrivateMessage(user.UserID);
            if (msgs == null)
            {
                return Json("");
            }
            List<object> objs = new List<object>();
            foreach (Message msg in msgs) 
            {
                if (msg.Status != msgType)
                {
                    continue;
                }
                string number = DAL.GetUserById(msg.From.ToString()).Number;
                object obj = new
                {
                    from = number,
                    msg = msg
                };
                objs.Add(obj);
            }
            JsonResult ret = new JsonResult()
            {
                Data = objs
            };
            return ret;
        }

        public ActionResult SendMessage() //发送私信
        {
            if (Session["isLogin"] != null && Session["isLogin"].ToString() == "True")
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
    }
}
