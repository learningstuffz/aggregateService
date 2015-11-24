using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAggregate.Models
{
    public class UploadData
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        //private string _title;

        //public string Title
        //{
        //    get { return _title; }
        //    set { _title = value; }
        //}
        private string _store;

        public string Store
        {
            get { return _store; }
            set { _store = value; }
        }
        private double _price;

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        private string _top_level;

        public string Top_Level
        {
            get { return _top_level; }
            set { _top_level = value; }
        }
        private string _sub_category;

        public string Sub_Category
        {
            get { return _sub_category; }
            set { _sub_category = value; }
        }
        
        
    }
}