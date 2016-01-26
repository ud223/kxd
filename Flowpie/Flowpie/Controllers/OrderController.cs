using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Collections;

namespace Flowpie.Controllers
{
    public class OrderController : ApiController
    {
        [HttpGet]
        public string GetOrder()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);
            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            List<Hashtable> list = orderController.getOrderByCourierId(data["courierid"].ToString());

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.Order order = new Models.Order();

                order.address = item["fromaddress"].ToString();
                order.headpic = item["headpic"].ToString();
                order.name = item["fromname"].ToString();
                order.orderid = item["orderid"].ToString();
                order.phone = item["fromtel"].ToString();
                order.amount = item["amount"].ToString().Replace(".00", "");

                if (order.amount == "0")
                    order.amount = item["pay_amount"].ToString();

                order.rundate = item["rundate"].ToString();
                order.runtime = item["runtime"].ToString().Replace(".000", "");
                order.state = item["state"].ToString();
                order.lat = item["lat"].ToString();
                order.lng = item["lng"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(order);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取订单列表失败!";
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
        public string GetOrderDetail()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            Hashtable item = orderController.load(data["orderid"].ToString());

            if (item != null)
            {
                Models.Order order = new Models.Order();

                order.sendcouriername = item["sendcouriername"].ToString();
                order.sendcourierphone = item["sendcourierphone"].ToString();
                order.address = item["fromaddress"].ToString();
                order.name = item["fromname"].ToString();
                order.orderid = item["orderid"].ToString();
                order.phone = item["fromtel"].ToString();
                order.amount = item["amount"].ToString().Replace(".00", "");

                if (order.amount == "0")
                    order.amount = item["pay_amount"].ToString();

                order.rundate = item["rundate"].ToString();
                order.runtime = item["runtime"].ToString().Replace(".000", "");
                order.state = item["state"].ToString();
                order.lat = item["lat"].ToString();
                order.lng = item["lng"].ToString();
                order.rejectmessage = item["rejectmessage"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(order);

                result.code = "200";
                result.message = "获取成功!";
                result.data = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            }
            else
            {
                result.code = "0";
                result.message = "获取订单信息失败!";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string ExpressAdd()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            List<System.Collections.Hashtable> list = orderController.isexistExpress(data["expressid"].ToString());

            if (list == null || list.Count == 0)
            {
                string orderdetailid = serialController.getSerialNumber("odl", DateTime.Now.ToString("yyyy-MM-dd"));

                data.Add("orderdetailid", orderdetailid);

                orderController.addDetail(data);

                orderController.updateAmount(data);

                if (orderController.Result)
                {
                    result.code = "200";
                    result.message = "添加成功!";
                }
                else
                {
                    result.code = "0";
                    result.message = orderController.Message.Replace("'", "\"");
                }
            }
            else
            {
                result.code = "0";
                result.message = "该快递单已经添加!";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string ExpressQuery()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            List<System.Collections.Hashtable> list = orderController.isExistSendExpress(data["expresscode"].ToString());

            if (list == null || list.Count == 0)
            {
                string sendorderid = serialController.getSerialNumber("odl", DateTime.Now.ToString("yyyy-MM-dd"));

                data.Add("sendorderid", sendorderid);

                orderController.queryAddDetail(data);

                if (orderController.Result)
                {
                    result.code = "200";
                    result.message = "添加成功!";
                }
                else
                {
                    result.code = "0";
                    result.message = orderController.Message.Replace("'", "\"");
                }
            }
            else
            {
                result.code = "0";
                result.message = "该快递单已经添加!";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string ExpressDelete()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            Hashtable orderDetail = orderController.getDetail(data["orderdetailid"].ToString());

            orderController.deleteDetail(data["orderdetailid"].ToString());

            Hashtable info = new Hashtable();

            info.Add("orderid", orderDetail["orderid"].ToString());
            info.Add("amount", "-" + orderDetail["amount"].ToString());

            orderController.updateAmount(info);

            if (orderController.Result)
            {
                result.code = "200";
                result.message = "删除成功!";
            }
            else
            {
                result.code = "0";
                result.message = orderController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string getExpress()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            Models.Result result = new Models.Result();

            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            List<Hashtable> list = orderController.getOrderDetailByOrderId(data["orderid"].ToString());

            int index = 0;

            decimal amount = 0;

            foreach (Hashtable item in list)
            {
                Models.OrderDetail orderdetail = new Models.OrderDetail();

                orderdetail.orderdetailid = item["orderdetailid"].ToString();
                orderdetail.expressid = item["expressid"].ToString();
                orderdetail.amount = item["amount"].ToString();
                orderdetail.companycode = item["companycode"].ToString();

                amount = amount + Convert.ToDecimal(orderdetail.amount);

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(orderdetail);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取订单详情失败!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = list.Count.ToString();
                result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
                result.index = amount.ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string Take()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            Hashtable order = orderController.load(data["orderid"].ToString());

            if (order["state"].ToString() != "0")
            {
                result.code = "0";
                result.message = "该订单已经被抢或取消!";
            }
            else
            {
                Hashtable item = courierController.load(data["courierid"].ToString());

                data.Add("sendcouriername", item["name"].ToString());
                data.Add("sendcourierphone", item["phone"].ToString());
                data.Add("companyid", item["companyid"].ToString());
                //data.Add("ModifyAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                orderController.take(data);

                if (orderController.Result)
                {
                    result.code = "200";
                    result.message = "抢单成功!";
                }
                else
                {
                    result.code = "0";
                    result.message = orderController.Message.Replace("'", "\"");
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string Reject()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            orderController.reject(data["orderid"].ToString(), data["msg"].ToString());

            if (orderController.Result)
            {
                result.code = "200";
                result.message = "订单取消成功!";
            }
            else
            {
                result.code = "0";
                result.message = orderController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string UnLinePay()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            orderController.setUnlinepay(data["orderid"].ToString());
            //第一次修改状态说明是已经收完件待付款
            orderController.updateState(data["orderid"].ToString());
            //第二次修改状态说明是线下已付款
            orderController.updateState(data["orderid"].ToString());

            if (orderController.Result)
            {
                result.code = "200";
                result.message = "线下支付成功!";
            }
            else
            {
                result.code = "0";
                result.message = orderController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string getOrderState()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            Hashtable item = orderController.load(data["orderid"].ToString());

            if (orderController.Result)
            {
                result.code = "200";

                if (item["sendcouriername"].ToString() != "" && item["sendcouriername"].ToString().ToLower() != "null")
                    result.message = item["rejectmessage"].ToString();

                result.data = item["state"].ToString();
            }
            else
            {
                result.code = "0";
                result.message = orderController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string orderMove()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            Hashtable order = orderController.load(data["orderid"].ToString());
            //如果是揽收状态就更新订单包含的快递单总价到订单价格
            if (order["state"].ToString() == "1")
            {
                List<Hashtable> express_list = orderController.getOrderDetailByOrderId(data["orderid"].ToString());
                double amount = 0.00;

                foreach (Hashtable express in express_list)
                {
                    string tmp_amount = CommonLib.Common.Validate.IsNullString(express["amount"], "0");

                    amount = amount + double.Parse(tmp_amount);
                }

                data.Add("amount", amount.ToString());
                
                //orderController.updateAmount(data);
            }

            orderController.updateState(data["orderid"].ToString());

            if (orderController.Result)
            {
                result.code = "200";
                result.message = "请求成功!";
            }
            else
            {
                result.code = "0";
                result.message = orderController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string getExpressStatus()
        {
            CommonLib.Web.WebAccess webAccess = new CommonLib.Web.WebAccess();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            string url = "http://www.kuaidi100.com/applyurl?key=" + data["key"] + "&com=" + data["com"] + "&nu=" + data["nu"];

            string result_url = webAccess.file_get_contents(url);

            if (CommonLib.Common.Validate.IsNullString(result_url) == "")
            {
                result.code = "0";
                result.message = "获取快递状态失败,请重试!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.data = result_url;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpPost]
        public string addSendExpress()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Form);

            List<System.Collections.Hashtable> list = orderController.isExistSendExpress(data["expresscode"].ToString());

            if (list == null || list.Count == 0)
            {
                string sendorder_id = serialController.getSerialNumber("sod", DateTime.Now.ToString("yyyy-MM-dd"));

                string courier_id = data["courierid"].ToString();

                Hashtable courier = courierController.load(courier_id);

                data.Add("sendorderid", sendorder_id);
                data.Add("couriername", courier["name"].ToString());
                data.Add("courierphone", courier["phone"].ToString());
                data.Add("companyid", courier["companyid"].ToString());
                data.Add("rundate", DateTime.Now.ToString("yyyy-MM-dd"));
                data.Add("runtime", DateTime.Now.ToString("HH:mm:ss"));

                orderController.addSendExpress(data);

                if (orderController.Result)
                {
                    result.code = "200";
                    result.message = "添加成功!";
                    result.data = sendorder_id;
                }
                else
                {
                    result.code = "0";
                    result.message = orderController.Message.Replace("'", "\"");
                }
            }
            else
            {
                result.code = "0";
                result.message = "添加失败：该快递单已经派送完毕!";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string getReceiveExpress()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);
            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            List<Hashtable> list = orderController.getReceiveOrderByCourierId(data["courierid"].ToString());

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.SendOrder send_order = new Models.SendOrder();

                send_order.CreateAt = item["CreateAt"].ToString();
                send_order.expresscode = item["expressid"].ToString();
                send_order.ModifyAt = item["ModifyAt"].ToString();
                send_order.rundate = item["rundate"].ToString();
                send_order.runtime = item["runtime"].ToString();
                send_order.sendorderid = item["orderid"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(send_order);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取订单列表失败!";
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
        public string getSendExpress()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);
            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            List<Hashtable> list = orderController.getSendOrderByCourierId(data["courierid"].ToString());

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.SendOrder send_order = new Models.SendOrder();

                send_order.CreateAt = item["CreateAt"].ToString();
                send_order.expresscode = item["expresscode"].ToString();
                send_order.ModifyAt = item["ModifyAt"].ToString();
                send_order.rundate = item["rundate"].ToString();
                send_order.runtime = item["runtime"].ToString();
                send_order.sendorderid = item["sendorderid"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(send_order);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取订单列表失败!";
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
        public string getCourierReceiveExpress()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);
            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            List<Hashtable> list = orderController.getCourierReceiveExpress(data["courierid"].ToString(), data["startdate"].ToString(), data["enddate"].ToString(), Convert.ToInt32(data["page"].ToString()), Convert.ToInt32(data["size"].ToString()));

            Hashtable count = orderController.getCourierReceiveExpressCount(data["courierid"].ToString(), data["startdate"].ToString(), data["enddate"].ToString());

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.SendOrder send_order = new Models.SendOrder();

                send_order.sendname = item["fromname"].ToString();
                send_order.expresscode = item["expressid"].ToString();
                send_order.rundate = item["rundate"].ToString();
                send_order.runtime = item["runtime"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(send_order);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取收件列表失败!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = count["num"].ToString();
                result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string getCourierSendExpress()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);
            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            List<Hashtable> list = orderController.getCourierSendExpress(data["courierid"].ToString(), data["startdate"].ToString(), data["enddate"].ToString(), Convert.ToInt32(data["page"].ToString()), Convert.ToInt32(data["size"].ToString()));

            Hashtable count = orderController.getCourierSendExpressCount(data["courierid"].ToString(), data["startdate"].ToString(), data["enddate"].ToString());

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.SendOrder send_order = new Models.SendOrder();

                send_order.expresscode = item["expresscode"].ToString();
                send_order.rundate = item["rundate"].ToString();
                send_order.runtime = item["runtime"].ToString().Replace(".000", "");

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(send_order);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取发件列表失败!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = count["num"].ToString();
                result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        //----------------------------------------------------------------------------------

        [HttpGet]
        public string getUserReceiveExpress()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);
            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            List<Hashtable> list = orderController.getUserReceiveExpress(data["userid"].ToString(), data["startdate"].ToString(), data["enddate"].ToString(), Convert.ToInt32(data["page"].ToString()), Convert.ToInt32(data["size"].ToString()));

            Hashtable count = orderController.getUserReceiveExpressCount(data["userid"].ToString(), data["startdate"].ToString(), data["enddate"].ToString());

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.SendOrder send_order = new Models.SendOrder();

                send_order.sendname = item["fromname"].ToString();
                send_order.expresscode = item["expressid"].ToString();
                send_order.rundate = item["rundate"].ToString();
                send_order.runtime = item["runtime"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(send_order);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取收件列表失败!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = count["num"].ToString();
                result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string getUserSendExpress()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            Models.Result result = new Models.Result();

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);
            System.Text.StringBuilder strData = new System.Text.StringBuilder();

            List<Hashtable> list = orderController.getUserSendExpress(data["userid"].ToString(), data["startdate"].ToString(), data["enddate"].ToString(), Convert.ToInt32(data["page"].ToString()), Convert.ToInt32(data["size"].ToString()));

            Hashtable count = orderController.getUserSendExpressCount(data["userid"].ToString(), data["startdate"].ToString(), data["enddate"].ToString());

            int index = 0;

            foreach (Hashtable item in list)
            {
                Models.SendOrder send_order = new Models.SendOrder();

                send_order.expresscode = item["expresscode"].ToString();
                send_order.rundate = item["rundate"].ToString();
                send_order.runtime = item["runtime"].ToString().Replace(".000", "");

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(send_order);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取发件列表失败!";
            }
            else
            {
                result.code = "200";
                result.message = "获取成功!";
                result.count = count["num"].ToString();
                result.data = strData.ToString().Replace("[", "{").Replace("]", "}");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        //[HttpPost]
        //public string orderPay()
        //{
        //    DatabaseLib.Tools tools = new DatabaseLib.Tools();
        //    KxdLib.OrderController orderController = new KxdLib.OrderController();
        //    KxdLib.CourierController courierController = new KxdLib.CourierController();

        //    HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

        //    Models.Result result = new Models.Result();

        //    System.Collections.Hashtable data = tools.paramToData(context.Request.Params);
        //    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "开始调用支付");
        //    string id = data["id"].ToString();
        //    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "支付id:"+ id);
        //    System.Collections.Hashtable item = orderController.load(id);
        //    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "读取订单信息");
        //    if (item["state"].ToString() == "3")
        //    {
        //        result.code = "0";
        //        result.message = "请不要重复支付!";

        //        return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        //    }
        //    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "更新订单状态");
        //    orderController.updateState(id);
        //    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "获取快递员信息");
        //    System.Collections.Hashtable courier = courierController.load(item["courierid"].ToString());
        //    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "更新快递员余额");
        //    courierController.updateAmount(item["courierid"].ToString(), item["amount"].ToString());

        //    decimal amount1 = 0;

        //    if (courier["amount"].ToString() != "")
        //        amount1 = Convert.ToDecimal(courier["amount"].ToString());

        //    decimal amount2 = Convert.ToDecimal(item["amount"].ToString());

        //    amount2 = amount2 + amount1;
        //    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "记录快递员余额日志");
        //    orderController.logCourierAmount(id, amount1.ToString(), amount2.ToString());

        //    AppLib.Android android = new AppLib.Android();
        //    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "推送app消息");
        //    android.pushMsg(item["orderid"].ToString() + "|3", courier["appid"].ToString());
        //    WxApiLib.lib.Log.Debug(this.GetType().ToString(), "支付成功");
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        //}
    }
}