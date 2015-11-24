using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace MVCAggregate.Models
{
    public class ReadCSV
    {
         private string _filepath;
        public ReadCSV(string filePath)
        {
            _filepath = filePath;
        }

        public List<UploadData> ReadData()
        {
            List<UploadData> upData = new List<UploadData>();
            using (CsvReader csv = new CsvReader(new StreamReader(_filepath), true))
            {
                int fieldCount = csv.FieldCount;
                
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    upData.Add(new UploadData{
                         Id=csv["ID"].ToString()
                        ,Price=Convert.ToDouble (csv["PRICE"])
                        ,Store = csv["STORE"].ToString()
                        ,Top_Level=csv["TOP LEVEL Category"].ToString()
                        ,Sub_Category=csv["SUB CATEGORY"].ToString()
                         
                    });
                }
            }
            return upData;
        }
    }
}