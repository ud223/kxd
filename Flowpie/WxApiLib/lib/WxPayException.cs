﻿using System;
using System.Collections.Generic;
using System.Web;

namespace WxApiLib.lib
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg) : base(msg)
        {

        }
    }
}
