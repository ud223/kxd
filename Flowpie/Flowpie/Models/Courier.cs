using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowpie.Models
{
    public class Courier
    {
        public string courierid { get; set; }
        public string loginname { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string headpic { get; set; }
        public string code { get; set; }
        public string certificatecode { get; set; }
        public string companyid { get; set; }
        public string siteid { get; set; }
        
        public string score { get; set; }
        public string region { get; set; }
    }
}