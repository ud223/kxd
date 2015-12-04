using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.kuaixiaodi.demo;

namespace AppLib
{
    public class Android
    {
        private static String APPID = "QV1CVOjQUg51Mhp33mY8a1";                     //您应用的AppId
        private static String APPKEY = "z7lNk40mHD7JzxVVxxbzX1";                    //您应用的AppKey
        private static String APPSECRET = "NEb0K0qiLG8yu6V6iKbD61";
        private static String MASTERSECRET = "hMZbaQJLP07qIHl7Yso8yA";              //您应用的MasterSecret 
        private static String CLIENTID = "";        //您获取的clientID

        public void pushMsg(string msg, string appid)
        {
            CLIENTID = appid;

            GetuiServerApiSDK gsa = new GetuiServerApiSDK();

            gsa.clientid = appid;
            gsa.PushMessageToSingle(msg);
        }

        public void pushMsg(string msg, List<System.Collections.Hashtable> list)
        {
            GetuiServerApiSDK gsa = new GetuiServerApiSDK();

            gsa.PushMessageToList(msg, list);
        }
    }
}
