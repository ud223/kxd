using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Authentication;

namespace Flowpie.Controllers
{
    public class AppController : Controller
    {
        public FilePathResult GetApp()
        {
            string filename = Request.QueryString["app"];
            string path = AppDomain.CurrentDomain.BaseDirectory + "down/";

            return File(path  + filename, "text/plain", filename);
        }

    }
}