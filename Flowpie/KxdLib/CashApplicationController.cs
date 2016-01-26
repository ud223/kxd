using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxdLib
{
    public class CashApplicationController : SystemConfigureLib.iController
    {
        public List<Hashtable> getApplication()
        {
            this.SqlText = "select app_cashapplication.*, name, phone, companytext from app_cashapplication left join app_courier on app_cashapplication.courierid = app_courier.courierid left join app_company on app_courier.companyid = app_company.companyid where app_cashapplication.state = 1 order by CreateAt desc";

            return base.Query(this.SqlText);
        }

        public List<Hashtable> getApplicationByCourierID(string courierid)
        {
            this.SqlText = "select * from app_cashapplication where courierid = '"+ courierid + "' order by CreateAt desc";

            return base.Query(this.SqlText);
        }

        public override string add(Hashtable data)
        {
            this.SqlText = "insert into app_cashapplication(cashapplicationid, courierid, bank, bankcode, amount, CreateAt, ModifyAt) values('@cashapplicationid@', '@courierid@', '@bank@', '@bankcode@',  @amount@, '@CreateAt@', '@ModifyAt@'); select cashapplicationid from app_cashapplication order by CreateAt desc limit 1";

            return base.add(data);
        }

        public void addDetail(Hashtable data)
        {
            this.SqlText = "insert into app_cashapplicationdetail(cashapplicationdetailid, cashapplicationid, orderid) values('"+ data["cashapplicationdetailid"].ToString() + "', '" + data["cashapplicationid"].ToString() + "', '" + data["orderid"].ToString() + "');";

            base.Execute(this.SqlText);
        }

        public override Hashtable load(string id)
        {
            this.SqlText = "select * from app_cashapplication where cashapplicationid='" + id + "'";

            return base.load("");
        }

        public override void save(Hashtable data)
        {
            this.SqlText = "update app_cashapplication set state = @state@ where cashapplicationid='@cashapplicationid@'";

            base.save(data);
        }

        public void scrapApplication(string application_id)
        {
            this.SqlText = "udpate app_cashapplication set state = 0 where cashapplication = '"+ application_id + "'";

            base.Execute(this.SqlText);
        }

        public List<Hashtable> getCashOrderIdByCourierID(string courierid)
        {
            this.SqlText = "select orderid from app_order where (state > 3 and state != 9) and isClose = 0 and courierid = '" + courierid + "'";

            return base.Query(this.SqlText);
        }
        //select ifnull(amount,0) as amount from app_courier where courierid = '2015111200007'
        public Hashtable getCashAmount(string courierid)
        {
            this.SqlText = "select ifnull(amount,0) as amount from app_courier where courierid = '" + courierid + "'";

            return base.load(" ");
        }
    }
}
