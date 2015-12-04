using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowpie.Models
{
    public class OrderDetail
    {
        public string orderdetailid { get; set; }
        public string expressid { get; set; }
        public string amount { get; set; }
        public string companycode { get; set; }
    }
}