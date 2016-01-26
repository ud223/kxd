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
    public class nodeValue
    {
        public nodeValue(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public string key { get; set; }
        public string value { get; set; }
    }

    public class ExcelController : Controller
    {
        public ActionResult Order()
        {
            KxdLib.OrderController orderController = new KxdLib.OrderController();

            List<System.Collections.Hashtable> list = orderController.getAll();

            System.Collections.ArrayList fields = new System.Collections.ArrayList();

            foreach (System.Collections.Hashtable item in list)
            {
                switch (item["state"].ToString())
                {
                    case "0":
                        {
                            item["state"] = "创建订单";

                            break;
                        }
                    case "1":
                        {
                            item["state"] = "确认订单";

                            break;
                        }
                    case "2":
                        {
                            item["state"] = "已收件";

                            break;
                        }
                    case "3":
                        {
                            item["state"] = "已评价";

                            break;
                        }
                    case "4":
                        {
                            item["state"] = "已结算";

                            break;
                        }
                    case "9":
                        {
                            item["state"] = "已取消";

                            break;

                        }
                }
            }

            fields.Add(new nodeValue("orderid", "编号"));
            fields.Add(new nodeValue("fromname", "发件人"));
            fields.Add(new nodeValue("fromtel", "发件人电话"));
            fields.Add(new nodeValue("sendcouriername", "快递员"));
            fields.Add(new nodeValue("sendcourierphone", "快递员电话"));
            fields.Add(new nodeValue("companytext", "快递公司"));
            fields.Add(new nodeValue("amount", "订单金额"));
            fields.Add(new nodeValue("CreateAt", "下单时间"));
            fields.Add(new nodeValue("state", "状态"));

            this.DataToExcel(list, fields, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

            return View();
        }

        public ActionResult Courier()
        {
            KxdLib.CourierController courierController = new KxdLib.CourierController();

            List<System.Collections.Hashtable> list = courierController.getAll();

            System.Collections.ArrayList fields = new System.Collections.ArrayList();

            fields.Add(new nodeValue("courierid", "编号"));
            fields.Add(new nodeValue("name", "姓名"));
            fields.Add(new nodeValue("code", "身份证"));
            fields.Add(new nodeValue("phone", "电话"));
            fields.Add(new nodeValue("companytext", "所属公司"));
            fields.Add(new nodeValue("sitetext", "所属站点"));

            this.DataToExcel(list, fields, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

            return View();
        }

        private void DataToExcel(List<System.Collections.Hashtable> list, System.Collections.ArrayList fields, string fileName)
        {
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "Utf-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8));
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            sbHtml.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            //写出列名
            sbHtml.AppendLine("<tr style=\"background-color: #FFE88C;font-weight: bold; white-space: nowrap;\">");

            foreach (object field in fields)
            {
                sbHtml.AppendLine("<td>" + ((nodeValue)field).value + "</td>");
            }
            sbHtml.AppendLine("</tr>");
            //写数据
            foreach (System.Collections.Hashtable row in list)
            {
                sbHtml.Append("<tr>");
         
                foreach (object tmpfield in fields)
                {
                    nodeValue field = tmpfield as nodeValue;

                    foreach (System.Collections.DictionaryEntry item in row)
                    {
                        if (item.Key.ToString().ToLower() == field.key.ToLower())
                        {
                            if (field.value == "编号" || field.value == "身份证")
                                sbHtml.AppendLine("<td>'" + item.Value.ToString().Replace("null", "") + "</td>");
                            else
                                sbHtml.AppendLine("<td>" + item.Value.ToString().Replace("null", "") + "</td>");
                        }
                    }
                }

                sbHtml.AppendLine("</tr>");
            }

            sbHtml.AppendLine("</table>");

            Response.Write(sbHtml.ToString());
            Response.End();
        }
    }
}