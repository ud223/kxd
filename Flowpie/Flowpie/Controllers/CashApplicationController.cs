using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Collections;
using System.Text;

namespace Flowpie.Controllers
{
    public class CashApplicationController : ApiController
    {
        public string getCashAmount()
        {
            KxdLib.CashApplicationController cashApplicationController = new KxdLib.CashApplicationController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            Hashtable amount = cashApplicationController.getCashAmount(data["courierid"].ToString());

            if (cashApplicationController.Result)
            {
                result.data = amount["amount"].ToString();
                result.message = "获取成功!";
                result.code = "200";
            }
            else
            {
                result.code = "0";
                result.message = cashApplicationController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        public string submitCash()
        {
            KxdLib.CashApplicationController cashApplicationController = new KxdLib.CashApplicationController();
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            string application_id = serialController.getSerialNumber("cas", DateTime.Now.ToString("yyyy-MM-dd"));

            data.Add("cashapplicationid", application_id);

            cashApplicationController.add(data);

            if (cashApplicationController.Result)
            {
                courierController.updateAmount(data["courierid"].ToString(), "-" + data["amount"].ToString());

                result.code = "200";
                result.message = "申请成功!";
            }
            else
            {
                result.code = "0";
                result.message = cashApplicationController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        public string getApplication()
        {
            KxdLib.CashApplicationController cashApplicationController = new KxdLib.CashApplicationController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            List<Hashtable> list = cashApplicationController.getApplicationByCourierID(data["courierid"].ToString());

            System.Text.StringBuilder strData = new System.Text.StringBuilder();
            int index = 0;
            foreach (Hashtable item in list)
            {
                Models.CashApplication cashApplication = new Models.CashApplication();

                cashApplication.CreateAt = item["CreateAt"].ToString();
                cashApplication.state = item["state"].ToString();
                cashApplication.amount = item["amount"].ToString();

                string str_json = Newtonsoft.Json.JsonConvert.SerializeObject(cashApplication);

                if (index > 0)
                    strData.Append(",");

                strData.Append(str_json);

                index++;
            }

            if (list == null)
            {
                result.code = "0";
                result.message = "获取申请列表失败!";
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
        public string Apply()
        {
            KxdLib.CashApplicationController cashApplicationController = new KxdLib.CashApplicationController();
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            data.Add("state", "2");

            cashApplicationController.save(data);

            if (cashApplicationController.Result)
            {
                result.code = "200";
                result.message = "审批成功!";
            }
            else
            {
                result.code = "0";
                result.message = cashApplicationController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");

        }

        [HttpPost]
        public string Reject()
        {
            KxdLib.CashApplicationController cashApplicationController = new KxdLib.CashApplicationController();
            KxdLib.CourierController courierController = new KxdLib.CourierController();
            SystemConfigureLib.SerialNumberController serialController = new SystemConfigureLib.SerialNumberController();
            Models.Result result = new Models.Result();
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];

            System.Collections.Hashtable data = tools.paramToData(context.Request.Params);

            data.Add("state", "3");

            cashApplicationController.save(data);

            if (cashApplicationController.Result)
            {
                Hashtable item = cashApplicationController.load(data["cashapplicationid"].ToString());

                courierController.updateAmount(item["courierid"].ToString(), item["amount"].ToString());

                result.code = "200";
                result.message = "拒绝成功!";
            }
            else
            {
                result.code = "0";
                result.message = cashApplicationController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }

        [HttpGet]
        public string getPromptText()
        {
            KxdLib.PromptController promptController = new KxdLib.PromptController();
            Models.Result result = new Models.Result();

            Hashtable item = promptController.load("");

            if (promptController.Result)
            {
                result.code = "200";
                result.message = "获取成功!";
                result.data = item["text"].ToString();
            }
            else
            {
                result.code = "0";
                result.message = promptController.Message.Replace("'", "\"");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result).Replace("\"", "'");
        }
    }
}