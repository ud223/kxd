﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowpie.Models
{
    public class Result
    {
        public Result()
        {
            this.code = "";
            this.message = "";
            this.data = "";
            this.count = "";
            this.page = "";
            this.index = "";
        }

        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public string count { get; set; }
        public string page { get; set; }
        public string index { get; set; }
    }
}