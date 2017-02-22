using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

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
