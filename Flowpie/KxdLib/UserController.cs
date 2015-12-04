using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxdLib
{
    public class UserController : SystemConfigureLib.iController
    {
        public override List<Hashtable> getAll()
        {
            this.SqlText = "SELECT * FROM app_users";

            return base.getAll();
        }
        public override Hashtable load(string id)
        {
            this.SqlText = "SELECT * FROM app_users WHERE UserID = '"+ id +"'";

            return base.load("");
        }

        public void saveValue(Hashtable data)
        {
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string sql = tools.createSqlText(data, "userid, CreateAt");

            this.SqlText = "update app_users set " + sql + " where userid = '" + data["userid"].ToString() + "'";

            base.Execute(this.SqlText);
        }

        public string addCourier(Hashtable data)
        {
            this.SqlText = "insert into app_relationcourier(relationcourierid, userid, courierid, CreateAt, ModifyAt) values('@relationcourierid@', '@userid@', '@courierid@', '@CreateAt@', '@ModifyAt@'); select relationcourierid from app_relationcourier limit CreateAt desc";

            return this.add(data);
        }

        public List<Hashtable> getCommonCourier(string userid, string courierid)
        {
            this.SqlText = "SELECT * FROM app_relationcourier where userid = '"+ userid + "' and courierid = '"+ courierid + "'";

            return base.Query(this.SqlText);
        }

        public List<Hashtable> getMyCourier(string userid)
        {
            this.SqlText = "SELECT distinct app_courier.courierid, app_courier.name, app_courier.phone, app_courier.region, ifnull(app_courier.headpic, '') as headpic, app_company.companytext FROM app_relationcourier left join app_courier on app_relationcourier.courierid = app_courier.courierid left join app_company on app_courier.companyid = app_company.companyid where userid = '" + userid + "'";

            return base.Query(this.SqlText);
        }

        public void deleteCourier(string userid, string courierid)
        {
            this.SqlText = "delete from app_relationcourier where userid = '" + userid + "' and courierid = '" + courierid + "'";

            base.delete("");
        }
    }
}
