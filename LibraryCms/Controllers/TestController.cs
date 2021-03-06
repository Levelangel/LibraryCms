﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using LibraryCms.Models;
using System.Web.Script.Serialization;

namespace LibraryCms.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AjaxUpload()
        {
            try
            {
                var postedFile = Request.Files[0];//只示范上传一个文件  
                if (postedFile == null || postedFile.ContentLength <= 0) return Json("请选择要上传的文件");
                //string savePath = Server.MapPath("/Upload/Books/") + Request["filename"] + Path.GetExtension(postedFile.FileName);
                string savePath = Server.MapPath("/Upload/test/") + postedFile.FileName + ".tmp";
                //postedFile.SaveAs(savePath);
                //int filelength = postedFile.ContentLength;
                Cache cache = HttpRuntime.Cache;
                cache.Insert("uploadStatus", 0);
                //clsSaveFile clsTmp = new clsSaveFile();
                //clsTmp.savePath = savePath;
                //clsTmp.file = postedFile;
                //clsTmp.Server = Server;
                //Thread th = new Thread(new ThreadStart(clsTmp.SaveFile));
                //th.Start();
                //SaveFile(savePath, postedFile);
                //string md5 = MD5.GetMD5HashFromFile(savePath);
                //string fileName = Server.MapPath("/Upload/Books/") + md5 + Path.GetExtension(postedFile.FileName);
                //System.IO.File.Move(savePath, fileName);
                return Json(Request["bookName"]);
            }
            catch
            {
                return Json("AJAX上传失败");
            }
        }

        public ActionResult AjaxUploadPersent()
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
            //objCache["uploadStatus"] = 0;
            Cache cache = HttpRuntime.Cache;
            FileStream fs = new FileStream(savePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            BinaryReader br = new BinaryReader(file.InputStream);

            int readCount = 0;//单次读取的字节数
            int saveCount = 0;
            byte[] buf = new byte[20 * 1024];
            int filelength = file.ContentLength;
            while ((readCount = br.Read(buf, 0, buf.Length)) > 0)
            {
                bw.Write(buf, 0, readCount);//写入字节到文件流
                bw.Flush();
                saveCount += readCount;//已经上传的进度
                cache.Insert("uploadStatus", saveCount * 100.0f / filelength);
                Thread.Sleep(2);
                //Cache = saveCount * 100.0f / filelength;
            }
            fs.Close();
            bw.Close();
            br.Close();
        }

        public JsonResult SendMessage()
        {
            Models.User user = new Models.User();
            user.Mail = "levelangel@live.com";
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
            htmlBody += "<h3>您的信息即将修改！以下是您的最终信息：</h3>";
            htmlBody += "<table>";
            htmlBody += "<tr>";
            htmlBody += string.Format("<td>邮箱地址：</td><td><span>{0}</span></td>", user.Mail);
            htmlBody += "</tr>";
            htmlBody += "<tr>";
            htmlBody += string.Format("<td>手机号码：</td><td><span>{0}</span></td>", user.Phone);
            htmlBody += "</tr>";
            htmlBody += "<tr>";
            htmlBody += string.Format("<td>QQ号码：</td><td><span>{0}</span></td>", user.QQ);
            htmlBody += "</tr>";
            htmlBody += "</table>";
            htmlBody += "<h4>请单击以下链接确认您的修改：</h4>";
            string url = Request.Url.ToString();
            string action = RouteData.Values["action"].ToString();
            string handleAction = "HandleMail/";
            url = url.Substring(0, url.Length - action.Length);
            url += handleAction;
            htmlBody += string.Format("<a href='{0}'>确认修改</a>", url + user.ToString());
            htmlBody += "<p></p>";
            htmlBody += string.Format("<p>{0}</p>", DateTime.Now);
            htmlBody += "<p></p>";
            htmlBody += "</body>";
            htmlBody += "</html>";
            //htmlBody = string.Format(htmlBody, user.Mail, user.Phone, user.QQ, Request.Url, DateTime.Now);
            Mail.SnedMsgTo(user.Mail, "信息修改确认", htmlBody, true);
            //Response.Write(htmlBody);
            return Json("ok");
        }


        public ActionResult xxhf()
        {
            DAL.DeletePrivateMessage("00000000", "00000001");
            List<Message> m = DAL.GetPrivateMessage("00000000");
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string tmp = "";
            foreach (Message d in m)  //序列化List泛型为JSON字符串
            {
                string JsonStr = jss.Serialize(d);
                tmp += JsonStr + "|";
            }
            if (m.Count > 0)
            {
                tmp = tmp.Substring(0, tmp.Length - 1);
            }
            Response.Write(tmp);
            return null;
        }

        public ActionResult xxhf1()
        {
            Question q = new Question
            {
                Content = "qqq",
                answerA = "xxx1",
                answerB = "",
                answerC = "",
                answerD = "",
                answerE = "",
                Correct = "10000",
                Type = 0
            };
            DAL.InsertQuestion(q,"1");
            List<Question> m = DAL.GetQuestion("1");
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string tmp = "";
            foreach (Question d in m)  //序列化List泛型为JSON字符串
            {
                string JsonStr = jss.Serialize(d);
                tmp += JsonStr + "|";
            }
            if (m.Count > 0)
            {
                tmp = tmp.Substring(0, tmp.Length - 1);
            }
            Response.Write(tmp);
            return null;
        }

        public ActionResult xxhf2() {
            DAL.DeleteQuestion("1","1");
            List<Question> m = DAL.GetQuestion("1");
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string tmp = "";
            foreach (Question d in m)  //序列化List泛型为JSON字符串
            {
                string JsonStr = jss.Serialize(d);
                tmp += JsonStr + "|";
            }
            if (m.Count > 0)
            {
                tmp = tmp.Substring(0, tmp.Length - 1);
            }
            Response.Write(tmp);
            return null;
        }

        public JsonResult GetUserNotReadMessage()
        {
            User user = new User()
            {
                UserID = "1"
            };
            if (user == null)
            {
                return Json("need login");
            }
            List<Message> msgs = DAL.GetPrivateMessage(user.UserID);
            List<object> objs = new List<object>();
            var obj = new { from = "test", data = msgs[0] };
            objs.Add(obj);
            objs.Add(obj); objs.Add(obj);
            JsonResult res = new JsonResult();
            res.Data = objs;
            return res;

        }
    }

    //public class clsSaveFile
    //{
    //    public string savePath;
    //    public HttpPostedFileBase file;
    //    public HttpServerUtilityBase Server;
    //    public void SaveFile()
    //    {
    //        Cache cache = HttpRuntime.Cache;
    //        FileStream fs = new FileStream(savePath, FileMode.Create);
    //        BinaryWriter bw = new BinaryWriter(fs);
    //        BinaryReader br = new BinaryReader(file.InputStream);

    //        int readCount = 0;
    //        int saveCount = 0;
    //        byte[] buf = new byte[20 * 1024];
    //        int filelength = file.ContentLength;
    //        while ((readCount = br.Read(buf, 0, buf.Length)) > 0)
    //        {
    //            bw.Write(buf, 0, readCount);
    //            bw.Flush();
    //            saveCount += readCount;
    //            cache.Insert("uploadStatus", saveCount * 100.0f / filelength); //计算上传完成度
    //        }
    //        fs.Close();
    //        bw.Close();
    //        br.Close();
    //        string md5 = MD5.GetMD5HashFromFile(savePath);
    //        string fileName = Server.MapPath("/Upload/Books/") + md5 + Path.GetExtension(file.FileName);
    //        if (System.IO.File.Exists(fileName)) //文件存在时
    //        {
    //            System.IO.File.Delete(savePath);//删除临时文件
    //        }
    //        System.IO.File.Move(savePath, fileName); //书籍改名
    //    }
    //}
}
