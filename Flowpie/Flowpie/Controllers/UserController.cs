using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;


using System.Data;
using System.Collections;
namespace Flowpie.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        public string getCompany()
        {
            KxdLib.CompanyController companyController = new KxdLib.CompanyController();
            Models.Result result = new Models.Result();
            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            List<Hashtable> list = companyController.getAll();

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.Company company = new Models.Company();

                company.companyid = item["companyid"].ToString();
                company.companytext = item["companytext"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(company);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取公司列表失败!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = list.Count.ToString();
                result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string getSite(string id)
        {
            KxdLib.SiteController siteController = new KxdLib.SiteController();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            Models.Result result = new Models.Result();

            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            List<Hashtable> list = siteController.getByCompanyId(id);

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.Site site = new Models.Site();

                site.siteid = item["siteid"].ToString();
                site.sitetext = item["sitetext"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(site);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取站点列表失败!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = list.Count.ToString();
                result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string saveCourier()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            if (courierController.IsExist(data) != null)
            {
                result.code = "0";
                result.message = "当前用户名或手机号码已经被注册!";
            }
            else
            {
                string courierid = serialController.getSerialNumber("cur", DateTime.Now.ToString("yyyy-MM-dd"));

                data.Add("courierid", courierid);

                string strValue = data["password"].ToString();

                data["password"] = CommonLib.Common.Function.toMD5String(strValue);

                courierController.add(data);                

                if (courierController.Result)
                {
                    result.code = "200";
                    result.message = "添加成功!";
                    result.data = courierid;
                }
                else
                {
                    result.code = "0";
                    result.message = courierController.Message.Replace("'", "\"");
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        /// <summary>
        /// 支持用户名和手机号码登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string loginCourier()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            data.Add("phone", data["loginname"].ToString());

            Hashtable item = courierController.IsExist(data);

            if (item == null)
            {
                result.code = "0";
                result.message = "没有找到使用该用户名或手机的快递员用户!";
            }
            else
            {
                string strValue = data["password"].ToString();

                strValue = CommonLib.Common.Function.toMD5String(strValue);

                if (item["password"].ToString() != strValue)
                {
                    result.code = "0";
                    result.message = "登录密码错误!";
                }
                else
                {
                    result.code = "200";
                    result.message = "登录成功!";

                    Models.CourierPrice courier = new Models.CourierPrice();

                    courier.courierid = item["courierid"].ToString();
                    courier.name = item["name"].ToString();
                    courier.phone = item["phone"].ToString();
                    courier.companycode = item["companycode"].ToString();
                    courier.companytext = item["companytext"].ToString();

                    if (item["headpic"].ToString() == "")
                        courier.headpic = "/img/user.jpg";
                    else
                        courier.headpic = item["headpic"].ToString();

                    result.data = Newtonsoft.Json.JsonConvert.SerializeObject(courier);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string getRelationCourier()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            CacheLib.Cookie cookie = new CacheLib.Cookie();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            string user_id = cookie.GetCookie("user_id");

            List<Hashtable> list = courierController.getRelationCourier(user_id, data);

            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.CourierPrice courierPrice = new Models.CourierPrice();

                courierPrice.courierid = item["courierid"].ToString();
                courierPrice.name = item["name"].ToString();
                courierPrice.phone = item["phone"].ToString();

                data["weight"] = CommonLib.Common.Validate.IsNullString(data["weight"].ToString(), "0");

                courierPrice.weight = item["weight"].ToString();
                courierPrice.firstprice = item["firstprice"].ToString();
                courierPrice.stepprice = item["stepprice"].ToString();
                courierPrice.companytext = item["companytext"].ToString();
                courierPrice.lat = item["lat"].ToString();
                courierPrice.lng = item["lng"].ToString();
                courierPrice.state = item["state"].ToString();

                if (courierPrice.state == "1")
                {
                    string tmp_time = item["lastat"].ToString();

                    if (tmp_time == "")
                    {
                        courierPrice.state = "0";
                    }
                    else
                    {
                        DateTime last_time = DateTime.Parse(tmp_time);

                        int tick = CommonLib.Common.DateTimeController.compareDate(DateTime.Now, last_time);

                        if (tick > 5)
                            courierPrice.state = "0";
                    }
                }

                int weight = Int32.Parse(data["weight"].ToString());
                int first_weight = Int32.Parse(courierPrice.weight);
                decimal first_price = decimal.Parse(courierPrice.firstprice);
                decimal stepprice = decimal.Parse(courierPrice.stepprice);

                decimal amount = (weight - first_weight) * stepprice;

                if (amount < 0)
                {
                    amount = first_price;
                }
                else
                {
                    amount = amount + first_price;
                }

                courierPrice.amount = amount.ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(courierPrice);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null || list.Count == 0)
            {
                result.code = "0";
                result.message = "没有获取到常用快递员!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = list.Count.ToString();
                result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string getPriceByLocal()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            double lat = double.Parse(data["lat"].ToString().Split(',')[0]);
            double lng = double.Parse(data["lng"].ToString().Split(',')[0]);

            List<Hashtable> list = courierController.getPrices(data);

            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            int index = 0;

            foreach (Hashtable item in list)
            {
                if (!this.isNearby(lat, lng, item))
                    continue;

                Models.CourierPrice courierPrice = new Models.CourierPrice();

                courierPrice.courierid = item["courierid"].ToString();
                courierPrice.name = item["name"].ToString();
                courierPrice.phone = item["phone"].ToString();

                CommonLib.Common.Validate.IsNullString(item["weight"].ToString(), "0");

                courierPrice.weight = item["weight"].ToString();
                courierPrice.firstprice = item["firstprice"].ToString();
                courierPrice.stepprice = item["stepprice"].ToString();
                courierPrice.companytext = item["companytext"].ToString();
                courierPrice.lat = item["lat"].ToString();
                courierPrice.lng = item["lng"].ToString();
                courierPrice.state = item["state"].ToString();

                if (courierPrice.state == "1")
                {
                    string tmp_time = item["lastat"].ToString();

                    if (tmp_time == "")
                    {
                        courierPrice.state = "0";
                    }
                    else
                    {
                        DateTime last_time = DateTime.Parse(tmp_time);

                        int tick = CommonLib.Common.DateTimeController.compareDate(DateTime.Now, last_time);

                        if (tick > 5)
                            courierPrice.state = "0";
                    }
                }

                data["weight"] = CommonLib.Common.Validate.IsNullString(data["weight"].ToString(), "0");

                int weight = Int32.Parse(data["weight"].ToString());
                int first_weight = Int32.Parse(courierPrice.weight);
                decimal first_price = decimal.Parse(courierPrice.firstprice);
                decimal stepprice = decimal.Parse(courierPrice.stepprice);

                decimal amount = (weight - first_weight) * stepprice;

                if (amount < 0)
                {
                    amount = first_price;
                }
                else
                {
                    amount = amount + first_price;
                }

                courierPrice.amount = amount.ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(courierPrice);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null || list.Count == 0)
            {
                result.code = "0";
                result.message = "没有获取到派送区域的快递员!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = list.Count.ToString();
                result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string saveValue()
        {
            KxdLib.UserController userController = new KxdLib.UserController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            userController.saveValue(data);

            if (userController.Result)
            {
                result.code = "200";
                result.message = "添加成功!";
            }
            else
            {
                result.code = "0";
                result.message = userController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string addCourier()
        {
            NetLog.WriteTextLog("addCourier", "新增常用快递", DateTime.Now);
            KxdLib.UserController userController = new KxdLib.UserController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            string serialid = serialController.getSerialNumber("uci", DateTime.Now.ToString("yyyy-MM-dd"));

            data.Add("relationcourierid", serialid);
            NetLog.WriteTextLog("addCourier", "开始新增", DateTime.Now);
            userController.addCourier(data);

            if (userController.Result)
            {
                result.code = "200";
                result.message = "添加成功!";
            }
            else
            {
                result.code = "0";
                result.message = userController.Message.Replace("'", "\"");
                NetLog.WriteTextLog("addCourier", "异常:"+ result.message, DateTime.Now);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }


        [HttpPost]
        public string delCourier()
        {
            KxdLib.UserController userController = new KxdLib.UserController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            userController.delCourier(data);

            if (userController.Result)
            {
                result.code = "200";
                result.message = "删除成功!";
            }
            else
            {
                result.code = "0";
                result.message = userController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        private bool isNearby(double lat, double lng, Hashtable item)
        {
            if (item["lat"].ToString() == "")
                return false;

            double lat1 = double.Parse(item["lat"].ToString());
            double lng1 = double.Parse(item["lng"].ToString());

            double range = CommonLib.Common.Pos.getDistance(lat, lng, lat1, lng1);

            if (range < 3)
            {
                //string tmp_time = item["lastat"].ToString();

                //if (tmp_time == "")
                //    return false;

                //DateTime last_time = DateTime.Parse(tmp_time);

                //int tick = CommonLib.Common.DateTimeController.compareDate(DateTime.Now, last_time);

                //if (tick > 5)
                //    return false;

                return true;
            }

            return false;
        }

        [HttpPost]
        public string Sign()
        {
            KxdLib.ScoreController scoreController = new KxdLib.ScoreController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            Hashtable item = scoreController.load(data["userid"].ToString());

            if (item == null)
            {
                scoreController.add(data);
            }
            else
            {
                string date = item["ModifyAt"].ToString();

                if (DateTime.Parse(date).ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    result.code = "200";
                    result.message = "今天已经签过了, 请明天再来!";

                    return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
                }

                scoreController.save(data);
            }

            if (scoreController.Result)
            {
                result.code = "200";
                result.message = "签到成功!";
            }
            else
            {
                result.code = "0";
                result.message = scoreController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string AddAddress()
        {
            KxdLib.UserController userController = new KxdLib.UserController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            List<Hashtable> list = userController.getAddressByAddress(data["address"].ToString());

            if (list == null || list.Count == 0)
            {
                userController.addAddress(data);
            }
            else
            {
                Hashtable itm = list[0];

                data.Add("addressid", itm["addressid"].ToString());

                userController.saveAddress(data);
            }

            if (userController.Result)
            {
                result.code = "200";
                result.message = "保存成功!";
            }
            else
            {
                result.code = "0";
                result.message = userController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }
    }
}