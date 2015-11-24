using MVCAggregate.Models; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft;
using System.IO;
namespace MVCAggregate.Controllers
{
    public class SampleController : Controller
    {
        
        //
        // GET: /Sample/
        
        public ActionResult Index()
        {
            return View();
        }


        // GET: /Sample/GetData
        [HttpGet]
        [AllowCrossSiteJson]
        [OutputCache(Location = OutputCacheLocation.None)]
        public string GetData()
        {
            
            
            string[] files = Directory.GetFiles(Server.MapPath("~/Upload"));//Gets the recent file
            string file = files[0];
            BusinessLogic bLogic = new BusinessLogic();
            //List<DataRow> li=bLogic.FetchData(file);
            DataTable dt = bLogic.FetchData(file);
            string JSONString = string.Empty;
            JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dt);

            return JSONString;
            
        }

        
        //GET: /Sample/GetCSVData
        [AllowCrossSiteJson]
        [OutputCache(Location = OutputCacheLocation.None)]
        public string GetCSVData()
        {

            //if (luc.ClearLuceneIndex())
            //{
            //    luc.AddUpdateLuceneIndex(li);
            //    luc.Optimize();
            //}

            string[] files = Directory.GetFiles(Server.MapPath("~/Upload"));//Gets the recent file
            string file = files[0];
            BusinessLogic bLogic = new BusinessLogic();
            //List<DataRow> li=bLogic.FetchData(file);
            DataTable dt = bLogic.FetchCSVData(file);
            string JSONString = string.Empty;
            JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dt);

            return JSONString;

        }
        //


       
        // GET: /Sample/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Sample/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Sample/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Sample/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Sample/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Sample/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Sample/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
