
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.ProtocolBuffers;
using com.gexin.rp.sdk.dto;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using System.Net;

/// <summary>
///GetuiServerApiSDK 的摘要说明
/// </summary>
namespace com.kuaixiaodi.demo
{
    public class GetuiServerApiSDK
    {
        //参数设置 <-----参数需要重新设置----->
        //您应用的AppId
        private static String APPID = "QV1CVOjQUg51Mhp33mY8a1";
        //您应用的AppKey
        private static String APPKEY = "z7lNk40mHD7JzxVVxxbzX1";

        private static String APPSECRET = "NEb0K0qiLG8yu6V6iKbD61";
        //您应用的MasterSecret
        private static String MASTERSECRET = "hMZbaQJLP07qIHl7Yso8yA";
        //您获取的clientID
        private String CLIENTID;// = "b9984262621a24c6fe417ae945062e61";//"c439be6525dd4540d1760d05a2f4b50a";
        public string clientid { get; set; }
        //HOST：OpenService接口地址
        private static String HOST = "http://sdk.open.api.igexin.com/apiex.htm";

        public string title { get; set; }
        public string contentText { get; set; }
        public string contentC { get; set; }



        public GetuiServerApiSDK()
        {

        }

        static void Main(string[] args)
        {
            //toList接口每个用户状态返回是否开启，可选
            Console.OutputEncoding = Encoding.GetEncoding(936);
            Environment.SetEnvironmentVariable("needDetails", "true");

            //2.PushMessageToList接口
            ////PushMessageToList();
        }


        public void PushMessageToSingle(string content)
        {
            // 推送主类
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

            /*消息模版：
                1.TransmissionTemplate:透传模板
                2.LinkTemplate:通知链接模板
                3.NotificationTemplate：通知透传模板
                4.NotyPopLoadTemplate：通知弹框下载模板
             */

            TransmissionTemplate template = transmissionTemplateDemo(content);
            //NotificationTemplate template =  NotificationTemplateDemo();
            //LinkTemplate template = LinkTemplateDemo();
            //NotyPopLoadTemplate template = NotyPopLoadTemplateDemo();


            // 单推消息模型
            SingleMessage message = new SingleMessage();
            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;
            //message.PushNetWorkType = 1;   //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为非WIFI环境

            com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
            target.appId = APPID;
            target.clientId = clientid;
            //target.alias = ALIAS;

            try
            {
                String pushResult = push.pushMessageToSingle(message, target);

                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("----------------服务端返回结果：" + pushResult);
            }
            catch (RequestException e)
            {
                String requestId = e.RequestId;
                String pushResult = push.pushMessageToSingle(message, target, requestId);
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("----------------服务端返回结果：" + pushResult);
            }
        }

        public static TransmissionTemplate transmissionTemplateDemo(string content)
        {

            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            template.TransmissionContent = content;
            template.TransmissionType = "1";
            return template;
        }

        //PushMessageToList接口测试代码
        public void PushMessageToList(string content, List<System.Collections.Hashtable> list)
        {
            // 推送主类（方式1，不可与方式2共存）
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
            // 推送主类（方式2，不可与方式1共存）此方式可通过获取服务端地址列表判断最快域名后进行消息推送，每10分钟检查一次最快域名
            //IGtPush push = new IGtPush("",APPKEY,MASTERSECRET);
            ListMessage message = new ListMessage();

            TransmissionTemplate template = transmissionTemplateDemo(content);
            // 用户当前不在线时，是否离线存储,可选
            message.IsOffline = false;
            // 离线有效时间，单位为毫秒，可选
            message.OfflineExpireTime = 1000 * 3600 * 12;
            message.Data = template;
            message.PushNetWorkType = 0;        //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为不限制网络环境。
            //设置接收者
            List<com.igetui.api.openservice.igetui.Target> targetList = new List<com.igetui.api.openservice.igetui.Target>();

            foreach (System.Collections.Hashtable item in list)
            {
                com.igetui.api.openservice.igetui.Target target1 = new com.igetui.api.openservice.igetui.Target();

                target1.appId = APPID;
                target1.clientId = item["appid"].ToString();

                targetList.Add(target1);
            }        

            String contentId = push.getContentId(message);
            String pushResult = push.pushMessageToList(contentId, targetList);
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("服务端返回结果:" + pushResult);
        }


        public static NotificationTemplate NotificationTemplateDemo()
        {
            NotificationTemplate template = new NotificationTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            //通知栏标题
            template.Title = "请填写通知标题";
            //通知栏内容     
            template.Text = "请填写通知内容";
            //通知栏显示本地图片
            template.Logo = "";
            //通知栏显示网络图标
            template.LogoURL = "";
            //应用启动类型，1：强制应用启动  2：等待应用启动
            template.TransmissionType = "1";
            //透传内容  
            template.TransmissionContent = "请填写透传内容";
            //接收到消息是否响铃，true：响铃 false：不响铃   
            template.IsRing = true;
            //接收到消息是否震动，true：震动 false：不震动   
            template.IsVibrate = true;
            //接收到消息是否可清除，true：可清除 false：不可清除    
            template.IsClearable = true;
            //设置通知定时展示时间，结束时间与开始时间相差需大于6分钟，消息推送后，客户端将在指定时间差内展示消息（误差6分钟）
            String begin = "2015-03-06 14:36:10";
            String end = "2015-03-06 14:46:20";
            template.setDuration(begin, end);

            return template;
        }
        //通知透传模板动作内容
        public static NotificationTemplate NotificationTemplateDemo(string t1, string t2, string t3)
        {
            NotificationTemplate template = new NotificationTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            //通知栏标题
            template.Title = t1;// "请填写通知标题";
            //通知栏内容     
            template.Text = t2;// "请填写通知内容";
            //通知栏显示本地图片
            template.Logo = "";
            //通知栏显示网络图标
            template.LogoURL = "";
            //应用启动类型，1：强制应用启动  2：等待应用启动
            template.TransmissionType = "1";
            //透传内容  
            template.TransmissionContent = t3;// "请填写透传内容";
            //接收到消息是否响铃，true：响铃 false：不响铃   
            template.IsRing = true;
            //接收到消息是否震动，true：震动 false：不震动   
            template.IsVibrate = true;
            //接收到消息是否可清除，true：可清除 false：不可清除    
            template.IsClearable = true;
            //设置通知定时展示时间，结束时间与开始时间相差需大于6分钟，消息推送后，客户端将在指定时间差内展示消息（误差6分钟）
            String begin = "2015-03-06 14:36:10";
            String end = "2015-03-06 14:46:20";
            template.setDuration(begin, end);

            return template;
        }

    }

}