using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowpie.Models
{
    public class Order
    {
        public string orderid { get; set; }
        public string address { get; set; }
        public string rundate { get; set; }
        public string runtime { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string headpic { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string amount { get; set; }
    }
}