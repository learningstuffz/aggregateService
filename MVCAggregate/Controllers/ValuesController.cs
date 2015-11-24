using MVCAggregate.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;


namespace MVCAggregate.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [OutputCache(Location = OutputCacheLocation.None)]
        public DataTable Get()
        {
            string file = @"C:\Debuzz\Indix Requirement\SampleData.csv";
            BusinessLogic bLogic = new BusinessLogic();
            //List<DataRow> li=bLogic.FetchData(file);
            DataTable dt = bLogic.FetchData(file);
            dt.TableName = "SampleData";
            return dt;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        
        

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}