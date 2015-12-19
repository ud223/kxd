using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowpie.Models
{
    public class CourierPrice
    {
        public string courierid { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string headpic { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string weight { get; set; }
        public string firstprice { get; set; }
        public string stepprice { get; set; }
        public string amount { get; set; }
        public string companycode { get; set; }
        public string companytext { get; set; }
        public string local { get; set; }
        public string state { get; set; }
    }
}