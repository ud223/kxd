using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace Flowpie.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            CacheLib.Cookie cookie = new CacheLib.Cookie();

            string user_id = cookie.GetCookie("user_id");

            List<System.Collections.Hashtable> list = orderController.getOrderAddressByUserId(user_id);

            ViewData["data"] = list;

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

                string msg = "有一个新的订单,请赶快抢单|" + order_id + "|" + address +"|"+ lat + "|"+ lng;

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

            android.pushMsg("你有一个快递订单,请查看|" + order_id + "|" + address + "|" + lat + "|" + lng, courier["appid"].ToString());

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

            orderController.updateState(id);

            System.Collections.Hashtable item = orderController.load(id);

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
            string user_id = cookie.GetCookie("user_id");

            System.Collections.Hashtable user = userController.load(user_id);
            System.Collections.Hashtable courier = courierController.load(data["courierid"].ToString());

            data.Add("orderid", order_id);
            data.Add("userid", user_id);
            data.Add("sendcouriername", courier["name"].ToString());
            data.Add("sendcourierphone", courier["phone"].ToString());
            data.Add("fromaddress", data["address"].ToString());
            data.Add("fromaddressdetail", data["addressdetail"].ToString());
            data.Add("fromname", user["name"].ToString());
            data.Add("fromcity", "武汉");
            data.Add("fromtel", user["phone"].ToString());
            data.Add("companyid", courier["companyid"].ToString());
            data.Add("rundate", DateTime.Now.ToString("yyyy-MM-dd"));
            data.Add("state", "1");

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

            System.Collections.Hashtable item = orderController.load(id);
            List<System.Collections.Hashtable> list = orderController.getOrderDetailByOrderId(id);

            ViewData["data"] = item;
            ViewData["list"] = list;

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

        public ActionResult apitest()
        {
            return View();
        }
    }
}