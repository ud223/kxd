using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Collections;

namespace Flowpie.Controllers
{
    public class CourierController : ApiController
    {
        [HttpPost]
        public string setValue()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            courierController.saveValue(data);

            if (courierController.Result)
            {
                result.code = "200";
                result.message = "添加成功!";
            }
            else
            {
                result.code = "0";
                result.message = courierController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string PriceAdd()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            courierController.addPrice(data);

            if (courierController.Result)
            {
                result.code = "200";
                result.message = "添加成功!";
            }
            else
            {
                result.code = "0";
                result.message = courierController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string AddSmsTemplet()
        {
            KxdLib.CourierSmsTemplet courierSmsTempletController = new KxdLib.CourierSmsTemplet();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            Hashtable item = courierSmsTempletController.loadItem(data);

            if (item == null)
            {
                courierSmsTempletController.add(data);

                if (courierSmsTempletController.Result)
                {
                    result.code = "200";
                    result.message = "添加成功!";
                }
                else
                {
                    result.code = "0";
                    result.message = courierSmsTempletController.Message.Replace("'", "\"");
                }
            }
            else
            {
                result.code = "0";
                result.message = "已存在!";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string getSmsTemplet()
        {
            KxdLib.CourierSmsTemplet courierSmsTempletController = new KxdLib.CourierSmsTemplet();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            List<System.Collections.Hashtable> list = courierSmsTempletController.getByCourierId(data["courierid"].ToString());

            if (courierSmsTempletController.Result)
            {
                if (list == null || list.Count == 0)
                {
                    result.code = "0";
                    result.message = "没有任何模板!";
                }
                else
                {
                    int index = 0;
                    System.Text.StringBuilder strData = new System.Text.StringBuilder();

                    foreach (Hashtable item in list)
                    {
                        Models.Text text = new Models.Text();

                        text.templet = item["TempletText"].ToString();

                        string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(text);

                        if (index > 0)
                            strData.Append(",");

                        strData.Append(str_json);

                        index++;
                    }

                    result.code = "200";
                    result.message = "获取成功!";
                    result.count = list.Count.ToString();
                    result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
                }
            }
            else
            {
                result.code = "0";
                result.message = courierSmsTempletController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string GetPrice()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            List<System.Collections.Hashtable> list = courierController.getPriceByCourierId(data["courierid"].ToString());

            if (courierController.Result)
            {
                if (list == null || list.Count == 0)
                {
                    result.code = "0";
                    result.message = "没有价格信息!";
                }
                else
                {
                    int index = 0;
                    System.Text.StringBuilder strData = new System.Text.StringBuilder();

                    foreach (Hashtable item in list)
                    {
                        Models.CourierPrice price = new Models.CourierPrice();

                        price.courierid = item["courierid"].ToString();
                        price.firstprice = item["firstprice"].ToString();
                        price.stepprice = item["stepprice"].ToString();
                        price.weight = item["weight"].ToString();
                        price.local = item["local"].ToString();

                        string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(price);

                        if (index > 0)
                            strData.Append(",");

                        strData.Append(str_json);

                        index++;
                    }

                    result.code = "200";
                    result.message = "获取成功!";
                    result.count = list.Count.ToString();
                    result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
                }
            }
            else
            {
                result.code = "0";
                result.message = courierController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string StateChange()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            Hashtable item = courierController.load(data["courierid"].ToString());

            if (courierController.Result == false || item == null)
            {
                result.code = "0";
                result.message = "没有找到快递员信息!";
            }
            else
            {
                if (item["state"].ToString() == "0")
                    item["state"] = "1";
                else
                    item["state"] = "0";

                Hashtable param = new Hashtable();

                param.Add("state", item["state"].ToString());
                param.Add("courierid", item["courierid"].ToString());

                courierController.saveValue(param);

                if (courierController.Result)
                {
                    result.code = "200";
                    result.message = "保存成功!";
                }
                else
                {
                    result.code = "0";
                    result.message = courierController.Message.Replace("'", "\"");
                }
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
                return true;

            return false;
        }

        [HttpGet]
        public string getCourierOfNearby()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            Models.Result result = new Models.Result();
            System.Text.StringBuilder strData = new System.Text.StringBuilder();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            List<Hashtable> list = courierController.getAll();

            int index = 0;
            //测试请求用户坐标
            double lat = double.Parse(data["local_lat"].ToString());
            double lng = double.Parse(data["local_lng"].ToString());

            foreach (Hashtable item in list)
            {
                if (!this.isNearby(lat, lng, item))
                    continue;

                Models.CourierPrice courier = new Models.CourierPrice();

                courier.courierid = item["courierid"].ToString();
                courier.name = item["name"].ToString();
                courier.phone = item["phone"].ToString();
                courier.lat = item["lat"].ToString();
                courier.lng = item["lng"].ToString();
                courier.companytext = item["companytext"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(courier);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取快递员列表失败!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = list.Count.ToString();
                result.data =  "[" + strData.ToString().Replace("[", "{").Replace("]", "}") + "]";             
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }
    }
}