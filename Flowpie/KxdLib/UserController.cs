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

        public Hashtable getUserByOpenId(string openid)
        {
            this.SqlText = "SELECT * FROM app_users WHERE openid = '" + openid + "'";

            return base.load("");
        }

        public override string add(Hashtable data)
        {
            string strSql = "INSERT INTO app_users(openid, nickname, headpic, CreateAt, ModifyAt) VALUES('@openid@', '@nickname@', '@headpic@', '@CreateAt@', '@ModifyAt@'); SELECT userid FROM app_users order by userid desc LIMIT 1";

            this.SqlText = strSql;

            return base.add(data);
        }

        public void saveValue(Hashtable data)
        {
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string sql = tools.createUpdateSqlText(data, "userid, CreateAt");

            this.SqlText = "update app_users set " + sql + " where userid = '" + data["userid"].ToString() + "'";

            base.Execute(this.SqlText);
        }

        public string addCourier(Hashtable data)
        {
            this.SqlText = "insert into app_relationcourier(relationcourierid, userid, courierid, CreateAt, ModifyAt) values('@relationcourierid@', '@userid@', '@courierid@', '@CreateAt@', '@ModifyAt@'); select relationcourierid from app_relationcourier  order by CreateAt desc limit 1";

            return base.add(data);
        }

        public void delCourier(Hashtable data)
        {
            this.SqlText = "delete from app_relationcourier where userid = '"+ data["userid"].ToString() +"' and courierid = '"+ data["courierid"].ToString() +"'";

            this.Execute(this.SqlText);
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

        public List<Hashtable> getAddressByUserID(string userid)
        {
            this.SqlText = "select * from app_address where userid = " + userid;

            return base.Query(this.SqlText);
        }

        public Hashtable getAddressByID(string addressid)
        {
            this.SqlText = "select * from app_address where addressid = " + addressid;

            return base.load("");
        }

        public string addAddress(Hashtable data)
        {
            this.SqlText = "insert into app_address(userid, address, addresstext, lat, lng) values(@userid@, '@address@', '@addresstext@', '@lat@', '@lng@'); select addressid from app_address  order by addressid desc limit 1";

            return base.add(data);
        }

        public void saveAddress(Hashtable data)
        {
            this.SqlText = "update app_address set addresstext='@addresstext@' where addressid = @addressid@";

            base.save(data);
        }

        public List<Hashtable> getAddressByAddress(string address)
        {
            this.SqlText = "select * from app_address  where address = '" + address + "'";

            return base.Query(this.SqlText);
        }
    }
}
