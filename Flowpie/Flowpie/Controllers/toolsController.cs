using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Collections;
using System.IO;
using System.Text;

namespace Flowpie.Controllers
{
    public class toolsController : ApiController
    {
        public string PostUrl = "http://222.73.117.158/msg/HttpBatchSendSM";
        public string QueryUrl = "http://222.73.117.158/msg/QueryBalance";

        [HttpPost]
        public string getRandomNum()
        {
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            Random rad = new Random();
            int mobile_code = rad.Next(1000, 10000);

            string account = "yanqiuping_ces";
            string password = "Tch123456";
            string mobile = data["mobile"].ToString();
            string content = "您的验证码是：" + mobile_code + " 。请不要把验证码泄露给其他人。";

            string postStrTpl = "account={0}&pswd={1}&mobile={2}&msg={3}&needstatus=true&product=&extno=";

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(string.Format(postStrTpl, account, password, mobile, content));

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;

            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string return_text = reader.ReadToEnd();
                //反序列化upfileMmsMsg.Text
                //实现自己的逻辑
                result.code = "200";
                result.message = "发送成功！";
                result.data = mobile_code.ToString();
            }
            else
            {
                result.code = "0";
                result.message = "短信发送失败！";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }
    }
}