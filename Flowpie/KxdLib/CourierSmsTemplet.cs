using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxdLib
{
    public class CourierSmsTemplet : SystemConfigureLib.iController
    {
        public List<Hashtable> getByCourierId(string courierid)
        {
            this.SqlText = "select * from app_smstemplet where courierid = '" + courierid + "'";

            return base.Query(this.SqlText);
        }

        public Hashtable loadItem(Hashtable data)
        {
            this.SqlText = "select * from app_smstemplet where TempletText = '" + data["templettext"].ToString() + "' and courierid = '" + data["courierid"].ToString() +"'";

            return this.load("");
        }

        public override string add(Hashtable data)
        {
            this.SqlText = "insert into app_smstemplet(TempletText, courierid) values('@templettext@', '@courierid@'); select SmsTempletID from app_smstemplet order by SmsTempletID desc limit 1";

            return base.add(data);
        }
    }
}
