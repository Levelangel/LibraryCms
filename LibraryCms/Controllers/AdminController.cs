using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryCms.Models;
using System.Web.Script.Serialization;
using System.Web.Caching;
using System.Threading;

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

        public ActionResult PersonalCenter() //个人中心
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
            
            switch (id) //根据id显示页面
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
            if (right[5] != '1') //检查用户权限
            {
                return Json("No Rights");
            }
            string strSearch = Request["strSearch"];
            List<Department> Departments = DAL.GetDepartment(strSearch);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string tmp = "";
            foreach (Department department in Departments)  //序列化List泛型为JSON字符串
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

        public ActionResult BookUpload() //书籍上传
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

            if (book.Format == "pdf")
            {
                FileStream fs =
                    System.IO.File.Open(Server.MapPath("/Upload/Books/" + book.BookPath + "." + book.Format),
                        FileMode.Open);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close(); 
                System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "filename=pdf");
                System.Web.HttpContext.Current.Response.AddHeader("content-length", buffer.Length.ToString());
                System.Web.HttpContext.Current.Response.BinaryWrite(buffer);
            }

            if (book.Format == "txt")
            {
                StreamReader fs = new StreamReader(Server.MapPath("/Upload/Books/" + book.BookPath + "." + book.Format), System.Text.Encoding.Default);
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

        [HttpPost]
        public ActionResult AjaxUpload() //书籍上传Ajax接受地址
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return null;
            }
            try
            {
                var postedFile = Request.Files[0];
                if (postedFile == null || postedFile.ContentLength <= 0) return Json("no file");
                string savePath = Server.MapPath("/Upload/temp/") + postedFile.FileName + ".tmp"; //存储到临时位置
                Cache cache = HttpRuntime.Cache;
                cache.Insert("uploadStatus", 0);
                SaveFile(savePath, postedFile);
                string md5 = MD5.GetMD5HashFromFile(savePath); //计算书籍MD5
                string fileName = Server.MapPath("/Upload/Books/") + md5 + Path.GetExtension(postedFile.FileName);
                if (System.IO.File.Exists(fileName)) //文件存在时
                {
                    System.IO.File.Delete(savePath);//删除临时文件
                    return Json("success");
                }
                System.IO.File.Move(savePath, fileName); //书籍改名
                User user = (User)Session["User"];
                Book book = new Book()
                {
                    BookName = Request["bookName"],
                    Author = Request["author"],
                    Publisher = Request["publisher"],
                    PublicTime = Request["publicTime"],
                    Format = Request["format"],
                    BookPath = md5,
                    DownloadNumber = 0,
                    Point = 0,
                    Pages = int.Parse(Request["pages"]),
                    DepartmentId = user.Role.Department.DepartmentId
                };
                DAL.InsetrBook(book); //在数据库中插入书籍信息
                return Json("success");
            }
            catch
            {
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult AjaxUploadPersent() //读取书籍上传进度响应地址
        {
            Cache cache = HttpRuntime.Cache;
            if(cache["uploadStatus"] == null)
            {
                cache.Insert("uploadStatus", 0);
            }
            if (cache["uploadStatus"].ToString() == "100")
            {
                cache["uploadStatus"] = 0;
                return Json("100");
            }
            return Json(cache["uploadStatus"]);
        }

        private void SaveFile(string savePath, HttpPostedFileBase file)
        {
            Cache cache = HttpRuntime.Cache;
            FileStream fs = new FileStream(savePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            BinaryReader br = new BinaryReader(file.InputStream);

            int readCount = 0;
            int saveCount = 0;
            byte[] buf = new byte[20 * 1024];
            int filelength = file.ContentLength;
            while ((readCount = br.Read(buf, 0, buf.Length)) > 0)
            {
                bw.Write(buf, 0, readCount);
                bw.Flush();
                saveCount += readCount;
                cache.Insert("uploadStatus", saveCount * 100.0f / filelength); //计算上传完成度
                Thread.Sleep(1);
            }
            fs.Close();
            bw.Close();
            br.Close();
        }

        [HttpPost]
        public ActionResult GetRights()
        {
            User user = (User)Session["user"];
            if( user == null )
            {
                return Json("need login");
            }
            string rights = user.Role.Rights;
            string tmp = "";
            for (int i = 0; i < rights.Length; i++ )
            {
                tmp += rights[i] + "|";
            }
            tmp = tmp.Substring(0, tmp.Length - 1);
            return Json(tmp);
        }

        [HttpPost]
        public ActionResult AddDepartment()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return Json("need login");
            }
            User user = (User)Session["User"];
            string right = user.Role.Rights;
            if (right[5] != '1')
            {
                return Json("No Rights");
            }
            string departmentName = Request["DepartmentName"];
            string departmentType = Request["DepartmentType"];

            Department dept = new Department()
            {
                DepartmentName = departmentName
            };
            switch (departmentType)
            {
                case "A":
                    dept.DepartmentType = DepartmentType.A;
                    break;

                case "B":
                    dept.DepartmentType = DepartmentType.B;
                    break;

                case "X":
                    dept.DepartmentType = DepartmentType.X;
                    break;

                default:
                    return Json("error");
            }
            int t = DAL.InsertDepartment(dept);
            return (t == 0)?Json("error"):Json("success");
        }

        [HttpPost]
        public ActionResult DeleteDepartment()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return Json("need login");
            }
            User user = (User)Session["User"];
            string right = user.Role.Rights;
            if (right[5] != '1')
            {
                return Json("No Rights");
            }
            string departmentName = Request["DepartmentName"];
            string departmentType = Request["DepartmentType"];
            Department dept = new Department()
            {
                DepartmentName = departmentName
            };
            switch (departmentType)
            {
                case "A":
                    dept.DepartmentType = DepartmentType.A;
                    break;

                case "B":
                    dept.DepartmentType = DepartmentType.B;
                    break;

                case "X":
                    dept.DepartmentType = DepartmentType.X;
                    break;
                default:
                    return Json("department type incorrect");
            }
            int t = DAL.DeleteDepartment(dept);
            return (t == 0) ? Json("sql error") : Json("success");
        }

        public ActionResult ModifyDepartment()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                return Json("need login");
            }
            User user = (User)Session["User"];
            string right = user.Role.Rights;
            if (right[5] != '1')
            {
                return Json("No Rights");
            }
            string deptName = Request["deptName"];
            string deptType = Request["deptType"];
            string oriDeptName = Request["oriDeptName"];
            string oriDeptType = Request["oriDeptType"];
            Department dept = new Department()
            {
                DepartmentName = deptName
            };
            switch (deptType)
            {
                case "A":
                    dept.DepartmentType = DepartmentType.A;
                    break;

                case "B":
                    dept.DepartmentType = DepartmentType.B;
                    break;

                case "X":
                    dept.DepartmentType = DepartmentType.X;
                    break;
                default:
                    return Json("department type incorrect");
            }
            Department oriDept = new Department()
            {
                DepartmentName = oriDeptName
            };
            switch (oriDeptType)
            {
                case "A":
                    oriDept.DepartmentType = DepartmentType.A;
                    break;

                case "B":
                    oriDept.DepartmentType = DepartmentType.B;
                    break;

                case "X":
                    oriDept.DepartmentType = DepartmentType.X;
                    break;
                default:
                    return Json("department type incorrect");
            }

            int t = DAL.DeleteDepartment(oriDept);
            if (t == 0)
            {
                return Json("sql error");
            }
            t = DAL.InsertDepartment(dept);
            if (t == 0)
            {
                return Json("sql error");
            }
            return Json("success");
        }

        public ActionResult AddGroup()
        {
            if (Session["isLogin"] == null || Session["isLogin"].ToString() == "False")
            {
                //return RedirectToAction("Index", "Account");
            }
            return View();
        }
    }
}
