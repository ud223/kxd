using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Flowpie.Controllers
{
    public class WeixinController : ApiController
    {
        string app_id = "wx78b3b4daaed7f512";
        string app_secret = "5ba8c179baf309974fac686236591d15";
        string access_token = "";

        [HttpGet]
        public string getSdkOption()
        {
            return "";
        }

        /// <summary>
        /// Will return the string contents of a
        /// regular file or the contents of a
        /// response from a URL
        /// </summary>
        /// <param name="fileName">The filename or URL</param>
        /// <returns></returns>
        protected string file_get_contents(string fileName)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(fileName);///cgi-bin/loginpage?t=wxm2-login&lang=zh_CN 
            //req.CookieContainer = cookie;
            req.Method = "GET";
            req.ProtocolVersion = HttpVersion.Version10;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader rd = new StreamReader(res.GetResponseStream());
            string theContent = rd.ReadToEnd();

            return theContent;
        }
    }
}