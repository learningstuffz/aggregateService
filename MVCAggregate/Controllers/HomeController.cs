using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCAggregate.Controllers
{
    public class HomeController : Controller
    {
        public string message = "Service is running..";
        public ActionResult Index()
        {
            if (Session["MESSAGE"]==null)
            {
            Session["MESSAGE"] = message;    
            }
            ViewBag.Message = Convert.ToString(Session["MESSAGE"]);
            Session["MESSAGE"] = message;
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            string newMessage = "No file to upload";
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    foreach (var item in Directory.GetFiles(Server.MapPath("~/Upload")))
                    {
                        FileInfo fData = new FileInfo(item);
                        fData.Delete();
                    }
                    var path = Path.Combine(Server.MapPath("~/Upload/"), fileName);
                    file.SaveAs(path);
                    newMessage = "File Uploaded Successfully";
                }
                
            }
            message = newMessage;
            
            Session["MESSAGE"] = message;
            return RedirectToAction("Index");
        }

    }
}
