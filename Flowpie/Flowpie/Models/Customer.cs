﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowpie.Models
{
    public class Customer
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string headimgurl { get; set; }
        public string subscribe { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string subscribe_time { get; set; }
        public string unionid { get; set; }
        public string remark { get; set; }
        public string groupid { get; set; }
    }
}