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
using System.Web.Script.Serialization;
using WxApiLib;

namespace Flowpie.Controllers
{
    public class HomeController : Controller
    {
        string app_id = "wx38d79befbac723ff";
        string app_secret = "acd03afe2dbec2b0e70a9b262d26fb59";
        string access_token = "";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            KxdLib.UserController userController = new KxdLib.UserController();
            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string user_id = cookie.GetCookie("user_id");

            System.Collections.Hashtable item = userController.load(user_id);
            List<System.Collections.Hashtable> list = orderController.getOrderAddressByUserId(user_id);

            ViewData["data"] = list;
            ViewData["item"] = item;

            return View();
        }

        public ActionResult OrderTake()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            KxdLib.UserController userController = new KxdLib.UserController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string strParam = Request.Form.ToString();

            string address = cookie.GetCookie("address");
            string addressdetail = cookie.GetCookie("addressdetail");
            string lat = cookie.GetCookie("lat");
            string lng = cookie.GetCookie("lng");
            string time = cookie.GetCookie("time");

            System.Collections.Hashtable data = new System.Collections.Hashtable();

            string order_id = serialController.getSerialNumber("ord", DateTime.Now.ToString("yyyy-MM-dd"));
            string user_id = cookie.GetCookie("user_id");

            System.Collections.Hashtable user = userController.load(user_id);

            data.Add("orderid", order_id);
            data.Add("userid", user_id);
            data.Add("fromaddress", address);
            data.Add("fromaddressdetail", addressdetail);
            data.Add("fromname", user["name"].ToString());
            
            data.Add("weight", "0");
            data.Add("width", "0");
            data.Add("height", "0");

            data.Add("fromcity", "武汉");
            data.Add("fromtel", user["phone"].ToString());
            data.Add("rundate", DateTime.Now.ToString("yyyy-MM-dd"));
            data.Add("runtime", time);
            data.Add("lat", lat);
            data.Add("lng", lng);
            data.Add("state", "0");
            data.Add("CreateAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            data.Add("ModifyAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string id = orderController.add(data);

            if (orderController.Result)
            {
                List<System.Collections.Hashtable> list = courierController.getAll();
                List<System.Collections.Hashtable> msgPush = new List<System.Collections.Hashtable>();

                var n_lat = double.Parse(lat);
                var n_lng = double.Parse(lng);

                foreach (System.Collections.Hashtable item in list)
                {
                    if (!this.isNearby(n_lat, n_lng, item))
                        continue;

                    msgPush.Add(item);
                }

                AppLib.Android android = new AppLib.Android();

                string msg = "有一个新的订单,请赶快抢单|" + order_id + "|" + address +"|"+ lat + "|"+ lng +"|1";

                android.pushMsg(msg, msgPush);

                ViewData["data"] = order_id;
            }
            else
            {
                return Redirect("/home/ordererror");
            }

            return View();
        }

        public ActionResult OrderCancel(string id)
        {
            ViewData["data"] = id;

            return View();
        }

        public ActionResult EasyOrder()
        {
            return View();
        }

        public ActionResult OrderSuccess(string id)
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            KxdLib.UserController userController = new KxdLib.UserController();

            System.Collections.Hashtable item = orderController.load(id);

            System.Collections.Hashtable courier = courierController.load(item["courierid"].ToString());

            List<System.Collections.Hashtable> list = userController.getCommonCourier(item["userid"].ToString(), item["courierid"].ToString());

            AppLib.Android android = new AppLib.Android();

            string order_id = id;
            string address = item["fromaddress"].ToString();
            string lat = item["lat"].ToString();
            string lng = item["lng"].ToString();

            android.pushMsg("你有一个快递订单,请查看|" + order_id + "|" + address + "|" + lat + "|" + lng + "|2", courier["appid"].ToString());

            ViewData["data"] = item;

            if (list == null || list.Count == 0)
            {
                ViewData["iscommon"] = "0";
            }
            else
            {
                ViewData["iscommon"] = "1";
            }

            return View();
        }

        public ActionResult PaySuccess(string id)
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            KxdLib.CourierController courierController = new KxdLib.CourierController();

            orderController.updateState(id);

            System.Collections.Hashtable item = orderController.load(id);

            System.Collections.Hashtable courier = courierController.load(item["courierid"].ToString());

            AppLib.Android android = new AppLib.Android();

            android.pushMsg(item["orderid"].ToString() + "|3", courier["appid"].ToString());

            ViewData["data"] = item;

            return View();
        }

        public ActionResult MyIndex()
        {
            KxdLib.UserController userController = new KxdLib.UserController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string user_id = cookie.GetCookie("user_id");

            System.Collections.Hashtable item = userController.load(user_id);

            ViewData["data"] = item;

            return View();
        }

        [HttpPost]
        public ActionResult OrderSave()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            KxdLib.UserController userController = new KxdLib.UserController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable data = tools.paramToData(strParam);

            string order_id = serialController.getSerialNumber("ord", DateTime.Now.ToString("yyyy-MM-dd"));
            string user_id = data["userid"].ToString();// cookie.GetCookie("user_id");

            System.Collections.Hashtable user = userController.load(user_id);
            System.Collections.Hashtable courier = courierController.load(data["courierid"].ToString());

            data.Add("orderid", order_id);
            //data.Add("userid", user_id);

           
            data.Add("sendcouriername", courier["name"].ToString());
            data.Add("sendcourierphone", courier["phone"].ToString());
            data.Add("fromaddress", data["address"].ToString());
            data.Add("fromaddressdetail", data["addressdetail"].ToString());
            data.Add("companyid", courier["companyid"].ToString());

            //return Redirect("/home/ordererror?msg=" + data["courierid"].ToString());

            data.Add("fromname", user["name"].ToString());
            data.Add("fromcity", "武汉");
            data.Add("fromtel", user["phone"].ToString());
            data.Add("rundate", DateTime.Now.ToString("yyyy-MM-dd"));

            if (data["orderTypeid"].ToString() == "1")
                data.Add("state", "1");
            else
                data.Add("state", "2");


            string id = orderController.add(data);

            if (orderController.Result)
            {
                return Redirect("/home/ordersuccess/" + order_id);
            }
            else
            {
                return Redirect("/home/ordererror");
            }  
        }

        public ActionResult OrderError()
        {
            string msg = Request.QueryString["msg"];

            ViewData["error"] = msg;

            return View();
        }

        public ActionResult MyOrder()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string user_id = cookie.GetCookie("user_id");

            List<System.Collections.Hashtable> list = orderController.getOrderByUserId(user_id);

            ViewData["data"] = list;

            return View();
        }

        public ActionResult OrderDetail(string id)
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            KxdLib.UserController userController = new KxdLib.UserController();

            System.Collections.Hashtable item = orderController.load(id);
            List<System.Collections.Hashtable> list = orderController.getOrderDetailByOrderId(id);

            System.Collections.Hashtable user = userController.load(item["userid"].ToString());

            ViewData["data"] = item;
            ViewData["list"] = list;
            ViewData["amount"] = item["amount"].ToString().Replace(".00", "");
            ViewData["openid"] = user["openid"].ToString();

            return View();
        }

        public ActionResult ModifyInfo()
        {
            KxdLib.UserController userController = new KxdLib.UserController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string user_id = cookie.GetCookie("user_id");

            System.Collections.Hashtable item = userController.load(user_id);

            ViewData["data"] = item;

            return View();
        }

        public ActionResult MyCourier()
        {
            KxdLib.UserController userController = new KxdLib.UserController();
            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string user_id = cookie.GetCookie("user_id");

            List<System.Collections.Hashtable> list = userController.getMyCourier(user_id);

            ViewData["data"] = list;

            return View();
        }

        public ActionResult CourierDetail(string id)
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();

            System.Collections.Hashtable item = courierController.load(id);

            ViewData["data"] = item;

            return View();
        }

        [HttpPost]
        public ActionResult CourierDelete()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            KxdLib.UserController userController = new KxdLib.UserController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable data = tools.paramToData(strParam);

            string courierid = data["courierid"].ToString();
            string user_id = cookie.GetCookie("user_id");

            userController.deleteCourier(user_id, courierid);

            return Redirect("/home/mycourier");
        }

        private bool isNearby(double lat, double lng, System.Collections.Hashtable item)
        {
            if (item["lat"].ToString() == "")
                return false;

            double lat1 = double.Parse(item["lat"].ToString());
            double lng1 = double.Parse(item["lng"].ToString());

            double range = CommonLib.Common.Pos.getDistance(lat, lng, lat1, lng1);

            if (range < 3)
                return true;

            return false;
        }

        public ActionResult ExpressQuery()
        {
            KxdLib.CompanyController companyController = new KxdLib.CompanyController();

            List<System.Collections.Hashtable> list = companyController.getAll();

            ViewData["data"] = list;

            return View();
        }

        public ActionResult RegUser()
        {
            string code = this.HttpContext.Request.QueryString["code"];
            string tmp_web_url = this.HttpContext.Request.QueryString["web_url"];

            //string web_url = System.Web.HttpUtility.UrlDecode(tmp_web_url); //"http://www.playkuaidi.com/home";//

            //if (CommonLib.Common.Validate.IsNullString(web_url) != "")
            //{
            //    if (web_url == "http://www.playkuaidi.com/")
            //    {
            //        web_url = "/";
            //    }
            //    else
            //    {
            //        web_url = web_url.Replace("http://www.playkuaidi.com/", "/");
            //    }
            //}
            //else
            //{
            //    web_url = "/";
            //}
            string web_url = "/";
            string open_id = this.getOpenId(code);

            Models.Customer customer = this.getUserInfo(open_id);

            string customer_id = this.addCustomer(customer);

            ViewData["data"] = customer_id;
            ViewData["url"] = web_url;

            return View();
        }

        private string getOpenId(string code)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + this.app_id + "&secret=" + this.app_secret + "&code=" + code + "&grant_type=authorization_code";

            string weixin = this.file_get_contents(url);

            System.Web.Script.Serialization.JavaScriptSerializer j = new System.Web.Script.Serialization.JavaScriptSerializer();

            Models.OpenId openid_info = new Models.OpenId();

            openid_info = j.Deserialize<Models.OpenId>(weixin);

            this.access_token = openid_info.access_token;

            return openid_info.openid;
        }

        private Models.Customer getUserInfo(string open_id)
        {
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + this.access_token + "&openid=" + open_id + "&lang=zh_CN";

            string weixin = this.file_get_contents(url);

            System.Web.Script.Serialization.JavaScriptSerializer j = new System.Web.Script.Serialization.JavaScriptSerializer();

            Models.Customer customer = new Models.Customer();

            customer = j.Deserialize<Models.Customer>(weixin);

            return customer;
        }

        private string addCustomer(Models.Customer customer)
        {
            KxdLib.UserController userController = new KxdLib.UserController();

            System.Collections.Hashtable item = userController.getUserByOpenId(customer.openid);

            if (item != null)
            {
                return item["userid"].ToString();
            }

            System.Collections.Hashtable data = new System.Collections.Hashtable();

            data.Add("nickname", CommonLib.Common.Validate.filterEmoji(customer.nickname));
            data.Add("openid", customer.openid);
            data.Add("headpic", customer.headimgurl);
            data.Add("CreateAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            data.Add("ModifyAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string strText = userController.add(data);

            if (userController.Result)
                return strText;
            else
                return null;
        }

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

        public ActionResult ScanQRcode()
        {
            return View();
        }

        public ActionResult apitest()
        {
            return View();
        }

        public ActionResult Clear()
        {
            return View();
        }

        public ActionResult OrderPay()
        {
            string wxJsApiParam = "";
            //string editAddress = "";

            WxApiLib.lib.Log.Info(this.GetType().ToString(), "1. page load");

            string openid = Request.QueryString["openid"];
            string total_fee = Request.QueryString["total_fee"];
            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(total_fee) || total_fee == "0")
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                WxApiLib.lib.Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");

                return View();
            }
            
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(Request, Response);
            jsApiPay.openid = openid;
            jsApiPay.total_fee = int.Parse(total_fee);

            //JSAPI支付预处理
            try
            {
                WxApiLib.lib.WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();

                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数      
                //editAddress = jsApiPay.GetEditAddressParameters();          
                    
                WxApiLib.lib.Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);

                JavaScriptSerializer js = new JavaScriptSerializer();
                Models.PayInfo payInfo = js.Deserialize<Models.PayInfo>(wxJsApiParam);


                ViewData["appId"] = payInfo.appId;
                ViewData["nonceStr"] = payInfo.nonceStr;
                ViewData["package"] = payInfo.package;
                ViewData["paySign"] = payInfo.paySign;
                ViewData["signType"] = payInfo.signType;
                ViewData["timeStamp"] = payInfo.timeStamp;

                ViewData["orderid"] = Request.QueryString["orderid"];
                //ViewData["editAddress"] = editAddress;
                //在页面上显示订单信息
                //Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
                //Response.Write("<span style='color:#00CD00;font-size:20px'>" + unifiedOrderResult.ToPrintStr() + "</span>");
                //Response.Write("<span style='color:#00CD00;font-size:20px'>" + wxJsApiParam + "</span>");

            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试:" + ex.Message + "</span>");
            }

            return View();
        }

        public ActionResult ResultNotify()
        {
            ResultNotify resultNotify = new ResultNotify(Request, Response);
            resultNotify.ProcessNotify();

            return View();
        }
    }
}