using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryCms.Models;
using System.Web.Script.Serialization;

namespace LibraryCms.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return RedirectToAction("Index", "Account");
            }
            ViewBag.RecoBooks = GetRecomendBook();
            return View();
        }

        public ActionResult PersonalCenter()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return RedirectToAction("Index", "Account");
            }
            string id = "";
            if (ControllerContext.RouteData.Values["ID"] != null)
            {
                id = ControllerContext.RouteData.Values["ID"].ToString();
            }
            
            switch (id)
            {
                case "Safety":
                    return View("Safety");
                case "PersonalMail":
                    return View("PersonalMail");
                default:
                    return View("UserInfo");
            }
        }

        public ActionResult AdvanceManage()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return RedirectToAction("Index", "Account");
            }
            string id = "";
            if (ControllerContext.RouteData.Values["ID"] != null)
            {
                id = ControllerContext.RouteData.Values["ID"].ToString();
            }
            User user = (User) Session["User"];
            string right = user.Role.Rights;
            switch (id)
            {
                case "BookManage":
                    if (right[0] == '1') return View("BookManage");
                    break;
                case "BookTypeManage":
                    if (right[1] == '1') return View("BookTypeManage");
                    break;
                case "BookQuestionBank":
                    if (right[2] == '1') return View("BookQuestionBank");
                    break;
                case "UserManage":
                    if (right[3] == '1') return View("UserManage");
                    break;
                case "GroupManage":
                    if (right[4] == '1') return View("GroupManage");
                    break;
                case "DepartmentManage":
                    if (right[5] == '1') return View("DepartmentManage");
                    break;
                default:
                    return View();
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetDepartment()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return Json("");
            }
            User user = (User)Session["User"];
            string right = user.Role.Rights;
            if (right[5] != '1')
            {
                return Json("No Rights");
            }
            string strSearch = Request["strSearch"];
            List<Department> Departments = DAL.GetDepartment(strSearch);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string tmp = "";
            foreach (Department department in Departments)
            {
                string JsonStr = jss.Serialize(department);
                tmp += JsonStr + "|";
            }
            if (Departments.Count > 0)
            {
                tmp = tmp.Substring(0, tmp.Length - 1);
            }
            return Json(tmp);
        }


        [HttpPost]
        public JsonResult GetRole()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return Json("");
            }
            User user = (User)Session["User"];
            string right = user.Role.Rights;
            if (right[4] != '1')
            {
                return Json("No Rights");
            }
            string strSearch = Request["strSearch"];
            List<Role> Roles = DAL.GetRole(strSearch);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string tmp = "";
            foreach (Role role in Roles)
            {
                string JsonStr = jss.Serialize(role);
                tmp += JsonStr + "|";
            }
            if (Roles.Count > 0)
            {
                tmp = tmp.Substring(0, tmp.Length - 1);
            }
            return Json(tmp);
        }

        [HttpPost]
        public JsonResult GetUser()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return Json("");
            }
            User user = (User)Session["User"];
            string right = user.Role.Rights;
            if (right[3] != '1')
            {
                return Json("No Rights");
            }
            string strSearch = Request["strSearch"];
            User tmpUser = DAL.GetUser(strSearch);
            if (tmpUser == null)
            {
                return Json("");
            }
            return Json(DAL.GetUser(strSearch));
        }

        public ActionResult BookUpload()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetBook()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return Json("");
            }
            User user = (User)Session["User"];
            string right = user.Role.Rights;
            if (right[0] != '1')
            {
                return Json("No Rights");
            }
            string strSearch = Request["strSearch"];
            if (strSearch == "")
            {
                return Json("");
            }
            List<Book> Books = DAL.GetBook(strSearch);
            if (Books == null)
            {
                return Json("");
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string tmp = "";
            foreach (Book book in Books)
            {
                string JsonStr = jss.Serialize(book);
                tmp += JsonStr + "|";
            }
            if (Books.Count > 0)
            {
                tmp = tmp.Substring(0, tmp.Length - 1);
            }
            return Json(tmp);
        }

        public ActionResult ReadBook()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                Response.Write("请先登录");
                return null;
            }
            string id;
            if (ControllerContext.RouteData.Values["ID"] != null)
            {
                id = ControllerContext.RouteData.Values["ID"].ToString();
            }
            else
            {
                Response.Write("链接格式错误");
                return null;
            }
            Book book = DAL.GetBookById(id);
            if (book == null)
            {
                Response.Write("书籍不存在");
                return null;
            }

            if (book.Formart == "pdf")
            {
                FileStream fs =
                    System.IO.File.Open(Server.MapPath("/Upload/Books/" + book.BookPath + "." + book.Formart),
                        FileMode.Open);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close(); 
                System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "filename=pdf");
                System.Web.HttpContext.Current.Response.AddHeader("content-length", buffer.Length.ToString());
                System.Web.HttpContext.Current.Response.BinaryWrite(buffer);
            }

            if (book.Formart == "txt")
            {
                StreamReader fs = new StreamReader(Server.MapPath("/Upload/Books/" + book.BookPath + "." + book.Formart), System.Text.Encoding.Default);
                List<string> lines = new List<string>();
                while (fs.Peek() > 0)
                {
                    lines.Add(fs.ReadLine());
                }
                ViewBag.Content = lines;
                ViewBag.Title = book.BookName;
                return View("ReadBook");
            }
            return null;
        }

        public List<Book> GetRecomendBook()
        {
            List<Book> books = DAL.GetRecomendBooks();
            return books;
        }
    }
}
