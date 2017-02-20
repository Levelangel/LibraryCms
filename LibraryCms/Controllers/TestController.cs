using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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
                string savePath = "G://" + Request["filename"] + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(savePath);
                return Json("AJAX上传成功");
            }
            catch
            {
                return Json("AJAX上传失败");
            }
        }  
    }
}
