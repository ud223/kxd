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
using System.Drawing;
using System.Drawing.Imaging;

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

        [HttpGet]
        public string getLastVersion()
        {
            SystemConfigureLib.App app = new SystemConfigureLib.App();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            Hashtable item = app.getByVersion(data["currentVersion"].ToString());

            if (item == null)
            {
                item = app.getLastVersion();

                result.code = "200";
                result.message = "获取新版本成功";
                result.data = item["aur_downurl"].ToString();
            }
            else
            {
                result.code = "0";
                result.message = "没有新版本!";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string UploadPic()
        {
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            KxdLib.CourierController courierController = new KxdLib.CourierController();

            Models.Result result = new Models.Result();
            string headpic = "";

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            string courierid = data["courierid"].ToString();
            string inputStr = data["imgs"].ToString();

            try
            {
                NetLog.WriteTextLog("uploadPic", inputStr, DateTime.Now);

                //inputStr = inputStr.Replace("\\n", "\n");
                inputStr = inputStr.IndexOf("data:image/jpeg;base64,") > -1 ? inputStr.Replace("data:image/jpeg;base64,", "") : inputStr;
                inputStr = inputStr.Replace("data:image/png;base64,", "");

                byte[] arr = Convert.FromBase64String(inputStr);

                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);


                Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format16bppRgb555);

                Graphics g = Graphics.FromImage(bmp2);

                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

                g.Dispose();

                bmp.Dispose();

                ms.Close();

                string path = AppDomain.CurrentDomain.BaseDirectory + "photo/";
                string filename = DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".jpg";

                headpic = "/photo/" + filename;

                bmp2.Save(Path.Combine(path, filename));

                bmp2.Dispose();

                courierController.updateHeadPic(courierid, headpic);

                result.code = "200";
                result.message = "上传成功!";
                result.data = headpic;
            }
            catch (Exception ex)
            {
                NetLog.WriteTextLog("uploadPic:error", ex.Message, DateTime.Now);

                result.code = "0";
                result.message = ex.Message;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }
    }
}