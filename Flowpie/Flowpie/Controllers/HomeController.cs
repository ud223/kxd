using System;
using System.Text;
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

            List<System.Collections.Hashtable> addresses = userController.getAddressByUserID(user_id);

            ViewData["data"] = list;
            ViewData["address"] = addresses;
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

            string urlReferrer = Request.UrlReferrer.ToString();

            if (urlReferrer.Substring(urlReferrer.Length - 5, 5).ToLower() != "/home")
            {
                return Redirect("/home");
            }

            string cur_order_id = cookie.GetCookie("cur_order_id");

            System.Collections.Hashtable order = orderController.load(cur_order_id);

            if (order != null)
            {
                return Redirect("/home");
            }

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
            data.Add("amount", 0);
            data.Add("pay_amount", 0);
            data.Add("state", "0");
            data.Add("CreateAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            data.Add("ModifyAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string id = orderController.add(data);
            int push_count = 0;
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

                    push_count++;
                }

                AppLib.Android android = new AppLib.Android();

                string msg = "有一个新的订单,请赶快抢单|" + order_id + "|" + user["name"].ToString() + "|" + user["phone"].ToString() + "|" + time.Replace(".000", "") + "|" + address + " " + addressdetail + "|" + lat + "|" + lng + "|1";

                android.pushMsg(msg, msgPush);

                ViewData["data"] = order_id;
            }
            else
            {
                return Redirect("/home/ordererror");
            }

            ViewData["push_count"] = push_count.ToString();

            return View();
        }

        public ActionResult OrderCancel(string id)
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();

            System.Collections.Hashtable item = orderController.load(id);

            if (item["state"].ToString() == "9")
            {
                return Redirect("/home");
            }

            ViewData["data"] = id;

            return View();
        }

        public ActionResult EasyOrder()
        {
            return View();
        }

        public ActionResult OrderSuccess(string id)
        {
            string urlReferrer = Request.UrlReferrer.ToString();

            if (urlReferrer.ToLower().IndexOf("easyorder") < 0)
            {
                return Redirect("/home");
            }

            KxdLib.OrderController orderController = new KxdLib.OrderController();
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            KxdLib.UserController userController = new KxdLib.UserController();

            System.Collections.Hashtable item = orderController.load(id);

            if (item == null)
            {
                return Redirect("/home");
            }

            if (item["state"].ToString() != "0")
            {
                return Redirect("/home");
            }

            System.Collections.Hashtable courier = courierController.load(item["courierid"].ToString());

            List<System.Collections.Hashtable> list = userController.getCommonCourier(item["userid"].ToString(), item["courierid"].ToString());

            AppLib.Android android = new AppLib.Android();

            string order_id = id;
            string address = item["fromaddress"].ToString();
            string addressdetail = item["fromaddressdetail"].ToString();
            string lat = item["lat"].ToString();
            string lng = item["lng"].ToString();
            string customer_name = item["fromname"].ToString();
            string customer_phone = item["fromtel"].ToString();
            string customer_runtime = item["runtime"].ToString();
            string weight = item["weight"].ToString();

            android.pushMsg("你有一个快递订单,请查看|" + order_id + "|" + customer_name + "|" + customer_phone + "|" + customer_runtime.Replace(".000", "") + "|" + address + " " + addressdetail + "|" + lat + "|" + lng + "|2|" + weight.Replace(".000", ""), courier["appid"].ToString());

            ViewData["orderid"] = id;
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

            System.Collections.Hashtable item = orderController.load(id);

            //if (item["state"].ToString() == "3")
            //{
            //    return Redirect("/home");
            //}

            //orderController.updateState(id);

            //System.Collections.Hashtable courier = courierController.load(item["courierid"].ToString());

            //courierController.updateAmount(item["courierid"].ToString(), item["amount"].ToString());

            //decimal amount1 = 0;

            //if (courier["amount"].ToString() != "")
            //    amount1 = Convert.ToDecimal(courier["amount"].ToString());

            //decimal amount2 = Convert.ToDecimal(item["amount"].ToString());

            //amount2 = amount2 + amount1;

            //orderController.logCourierAmount(id, amount1.ToString(), amount2.ToString());

            //AppLib.Android android = new AppLib.Android();

            //android.pushMsg(item["orderid"].ToString() + "|3", courier["appid"].ToString());

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

            data.Add("sendcouriername", courier["name"].ToString());
            data.Add("sendcourierphone", courier["phone"].ToString());
            data.Add("fromaddress", data["address"].ToString());
            data.Add("fromaddressdetail", data["addressdetail"].ToString());
            data.Add("companyid", courier["companyid"].ToString());
            data.Add("amount", "0");
            data.Add("fromname", user["name"].ToString());
            data.Add("fromcity", "武汉");
            data.Add("fromtel", user["phone"].ToString());
            data.Add("rundate", DateTime.Now.ToString("yyyy-MM-dd"));
            data.Add("state", "0");
            //if (data["orderTypeid"].ToString() == "1")
            //    data.Add("state", "1");
            //else
            //    data.Add("state", "2");


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

        public ActionResult MyExpress()
        {
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

        public ActionResult MyAddress()
        {
            KxdLib.UserController userController = new KxdLib.UserController();
            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string user_id = cookie.GetCookie("user_id");

            List<System.Collections.Hashtable> list = userController.getAddressByUserID(user_id);

            ViewData["list"] = list;

            return View();
        }

        public ActionResult AddressEdit(string id)
        {
            if (id == null || id == "0")
            {
                //ViewData["address"] = "";
                //ViewData["name"] = "";
                //ViewData["phone"] = "";
                //ViewData["addresstext"] = "";

                //return View();
                return Redirect("/home");
            }

            KxdLib.UserController userController = new KxdLib.UserController();

            System.Collections.Hashtable item = userController.getAddressByID(id);

            ViewData["address"] = item["address"].ToString();
            ViewData["lat"] = item["lat"].ToString();
            ViewData["lng"] = item["lng"].ToString();
            ViewData["addresstext"] = item["addresstext"].ToString();

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
            {
                string tmp_time = item["lastat"].ToString();

                if (tmp_time == "")
                    return false;

                DateTime last_time = DateTime.Parse(tmp_time);

                int tick = CommonLib.Common.DateTimeController.compareDate(DateTime.Now, last_time);

                if (tick > 5)
                    return false;

                return true;
            }

            return false;
        }

        public ActionResult ExpressQuery()
        {
            KxdLib.CompanyController companyController = new KxdLib.CompanyController();

            List<System.Collections.Hashtable> list = companyController.getAll();

            ViewData["data"] = list;

            string current_acess_takon = getacesstoken();

            string jsapi_ticket = gettiket(current_acess_takon);
            string noncestr = getnoncestr();
            string timestamp = gettimestamp();
            string url = "http://wx.playkuaidi.com/home/expressquery";//
            string signature = SHA1("jsapi_ticket=" + jsapi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url);

            ViewData["appId"] = this.app_id;
            ViewData["noncestr"] = noncestr;
            ViewData["timestamp"] = timestamp;
            ViewData["signature"] = signature.ToLower();

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

        private string getacesstoken()
        {
            DateTime now = DateTime.Now;

            Models.tokeninfo tokeninfo = new Models.tokeninfo();
            string getinfo = this.file_get_contents("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+ this.app_id +"&secret="+ this.app_secret);
            System.Web.Script.Serialization.JavaScriptSerializer k = new System.Web.Script.Serialization.JavaScriptSerializer();
            tokeninfo = k.Deserialize<Models.tokeninfo>(getinfo);
            string acess_token = tokeninfo.access_token.ToString();

            return acess_token;
        }

        private string gettiket(string acesstoken)
        {
            DateTime now = DateTime.Now;

            Models.tiketinfo tiketinfo = new Models.tiketinfo();
            string getinfo = this.file_get_contents("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + acesstoken + "&type=jsapi");
            System.Web.Script.Serialization.JavaScriptSerializer k = new System.Web.Script.Serialization.JavaScriptSerializer();
            tiketinfo = k.Deserialize<Models.tiketinfo>(getinfo);
            string acess_tiket = tiketinfo.ticket;
            //string messate = token.UpdateToken(acess_tiket, now.ToString());
            return acess_tiket;
        }

        private string getnoncestr()
        {

            string all = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,,m,m,o,p,q,r,s,t,u,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allChar = all.Split(',');
            string result = "";
            Random rand = new Random();
            for (int i = 0; i < 16; i++)
            {

                result += allChar[rand.Next(61)];
            }
            return result;

        }

        private string gettimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            //if (bflag)
            ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            //else  
            //ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;

        }

        private string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        public ActionResult ScanQRcode()
        {
            string current_acess_takon = getacesstoken();

            string jsapi_ticket = gettiket(current_acess_takon);
            string noncestr = getnoncestr();
            string timestamp = gettimestamp();
            string url = "http://wx.playkuaidi.com/home/ScanQRcode";//
            string signature = SHA1("jsapi_ticket=" + jsapi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url);

            ViewData["appId"] = this.app_id;
            ViewData["noncestr"] = noncestr;
            ViewData["timestamp"] = timestamp;
            ViewData["signature"] = signature.ToLower();

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
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            WxApiLib.lib.Log.Info(this.GetType().ToString(), "1. page load");

            string orderid = Request.QueryString["orderid"];
            string openid = Request.QueryString["openid"];
            string total_fee = Request.QueryString["total_fee"];

            System.Collections.Hashtable item = orderController.load(orderid);
            //如果当前传过来的订单id得到的状态不是支付状态 直接返回首页
            if (item["state"].ToString() != "2")
            {
                return Redirect("/home");
            }

            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(total_fee) || total_fee == "0")
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                WxApiLib.lib.Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");

                return View();
            }

            decimal amount = Convert.ToDecimal(total_fee) * 100;
            
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(Request, Response);
            jsApiPay.openid = openid;
            jsApiPay.total_fee = Convert.ToInt32(amount);
            jsApiPay.order_id = orderid;

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

            WxApiLib.lib.Log.Debug(this.GetType().ToString(), "进入回调");
            WxApiLib.lib.WxPayData notifyData = resultNotify.GetNotifyData();

            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxApiLib.lib.WxPayData res = new WxApiLib.lib.WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                WxApiLib.lib.Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                Response.Write(res.ToXml());
                Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            
            //查询订单，判断订单真实性
            if (resultNotify.QueryOrder(transaction_id))
            {
                string id = notifyData.GetValue("attach").ToString();

                KxdLib.OrderController orderController = new KxdLib.OrderController();
                KxdLib.CourierController courierController = new KxdLib.CourierController();

                WxApiLib.lib.Log.Debug(this.GetType().ToString(), "支付id:" + id);
                System.Collections.Hashtable item = orderController.load(id);

                if (item["state"].ToString() == "2")
                {
                    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "更新订单状态");
                    orderController.updateState(id);
                    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "获取快递员信息");
                    System.Collections.Hashtable courier = courierController.load(item["courierid"].ToString());
                    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "更新快递员余额");
                    courierController.updateAmount(item["courierid"].ToString(), item["amount"].ToString());

                    decimal amount1 = 0;

                    if (courier["amount"].ToString() != "")
                        amount1 = Convert.ToDecimal(courier["amount"].ToString());

                    decimal amount2 = Convert.ToDecimal(item["amount"].ToString());                        

                    amount2 = amount2 + amount1;
                    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "记录快递员余额日志");
                    orderController.logCourierAmount(id, amount1.ToString(), amount2.ToString());

                    AppLib.Android android = new AppLib.Android();
                    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "推送app消息");
                    android.pushMsg(item["orderid"].ToString() + "|3", courier["appid"].ToString());
                    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "支付成功");
                }          

                WxApiLib.lib.WxPayData res = new WxApiLib.lib.WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                WxApiLib.lib.Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                Response.Write(res.ToXml());
                Response.End();
            }
            else
            {
                WxApiLib.lib.WxPayData res = new WxApiLib.lib.WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                WxApiLib.lib.Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                Response.Write(res.ToXml());
                Response.End();
            }

            //resultNotify.ProcessNotify();

            return View();
        }

        public ActionResult QRCode()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string strParam = Request.Form.ToString();

            System.Collections.Hashtable data = tools.paramToData(strParam);

            string headpic = "";

            foreach (string upload in Request.Files)
            {
                if (!HasFiles.HasFile(Request.Files[upload]))
                    continue;

                string path = AppDomain.CurrentDomain.BaseDirectory + "photo/";
                string filename = DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".jpg";

                headpic = "/photo/" + filename;

                Request.Files[upload].SaveAs(Path.Combine(path, filename));
            }

            courierController.saveHeadPic(data["courierid"].ToString(), headpic);

            return View();
        }

        public ActionResult PayCancel()
        {
            return View();
        }
    }

    public static class HasFiles
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }
}