using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxdLib
{
    public class CourierController :SystemConfigureLib.iController
    {
        public override List<Hashtable> getAll()
        {
            this.SqlText = "SELECT app_courier.*, app_company.companytext, app_sites.sitetext FROM app_courier left join app_company on app_courier.companyid = app_company.companyid left join app_sites on app_courier.siteid = app_sites.siteid ";

            return base.getAll();
        }

        public List<Hashtable> getPriceCofnigByCouridID(string courierid)
        {
            this.SqlText = "SELECT app_prices.* FROM app_prices where courierid = '"+ courierid +"' ";

            return base.getAll();
        }

        public override Hashtable load(string id)
        {
            this.SqlText = "SELECT app_courier.*, companytext FROM app_courier left join app_company on app_courier.companyid = app_company.companyid WHERE courierid ='" + id + "'";

            return base.load("");
        }

        public override string add(Hashtable data)//region, code'@code@','@region@',
        {
            this.SqlText = "INSERT INTO app_courier(courierid, loginname, password, name, phone, certificatecode, certificate, companyid, siteid, remark, appid, CreateAt, ModifyAt) VALUES('@courierid@', '@loginname@', '@password@', '@name@', '@phone@',  '@certificatecode@', '', @companyid@, @siteid@,  '@remark@', '@appid@', '@CreateAt@', '@ModifyAt@'); SELECT * FROM app_courier ORDER BY CreateAt DESC";

            return base.add(data);
        }

        /// <summary>
        /// 获取报价列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Hashtable> getPrices(Hashtable data)
        {
            this.SqlText = "select distinct app_courier.courierid, name, phone, lat, lng, state, lastat, app_company.companytext, ifnull((select firstprice from app_prices where app_prices.courierid = app_courier.courierid and local like '%" + data["local"] +"%' limit 1), 0) as firstprice, ifnull((select weight from app_prices where app_prices.courierid = app_courier.courierid and local like '%"+ data["local"] +"%' limit 1), 0) as weight, ifnull((select stepprice from app_prices where app_prices.courierid = app_courier.courierid and local like '%"+ data["local"] + "%' limit 1), 0) as stepprice from app_courier left join app_company on app_courier.companyid = app_company.companyid group by app_courier.courierid order by firstprice desc";

            return base.getAll();
        }

        public List<Hashtable> getRelationCourier(string userId, Hashtable data)
        {
            this.SqlText = "select distinct app_courier.courierid, name, phone, lat, lng, state, lastat, app_company.companytext, ifnull((select firstprice from app_prices where app_prices.courierid = app_courier.courierid and local like '%" + data["local"] + "%' limit 1), 0) as firstprice, ifnull((select weight from app_prices where app_prices.courierid = app_courier.courierid and local like '%" + data["local"] + "%' limit 1), 0) as weight, ifnull((select stepprice from app_prices where app_prices.courierid = app_courier.courierid and local like '%" + data["local"] + "%' limit 1), 0) as stepprice from app_courier left join app_company on app_courier.companyid = app_company.companyid left join app_relationcourier on app_courier.courierid = app_relationcourier.courierid where app_relationcourier.userid = '" + userId +"'";

            return base.getAll();
        }

        public List<Hashtable> getPriceByCourierId(string courierid)
        {
            this.SqlText = "select * from app_prices where courierid = '"+ courierid +"'";

            return base.Query(this.SqlText);
        }

        public void saveValue(Hashtable data)
        {
            DatabaseLib.Tools tools = new DatabaseLib.Tools();

            string sql = tools.createUpdateSqlText(data, "courierid, CreateAt");

            this.SqlText = "update app_courier set "+ sql + " where courierid = '"+ data["courierid"].ToString() + "'";

            base.Execute(this.SqlText);
        }

        public void initAppId(string appid)
        {
            this.SqlText = "update app_courier set appid = '' where appid = '" + appid + "'";

            base.Execute(this.SqlText);
        }

        public string addPrice(Hashtable data)
        {
            this.SqlText = "INSERT INTO app_prices(courierid, local, firstprice, weight, stepprice, CreateAt, ModifyAt) VALUES('@courierid@', '@local@', @firstprice@, @weight@, @stepprice@, '@CreateAt@', '@ModifyAt@'); SELECT * FROM app_prices ORDER BY CreateAt DESC";

            return base.add(data);
        }

        /// <summary>
        /// 如果存在就返回快递员对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Hashtable IsExist(Hashtable data)
        {
            this.SqlText = "SELECT app_courier.*, companycode, companytext FROM app_courier left join app_company on app_courier.companyid = app_company.companyid WHERE phone ='" + data["phone"].ToString() +"' OR loginname ='"+ data["loginname"].ToString() +"' Limit 1";

            NetLog.WriteTextLog("判断用户或手机是否已注册", this.SqlText, DateTime.Now);

            Hashtable item = base.load("");

            return item;
        }

        public void saveHeadPic(string courierid, string filename)
        {
            this.SqlText = "update app_courier set headpic='@headpic@' where courierid = '" + courierid + "'";

            base.Execute(this.SqlText);
        }

        public void updateAmount(string courierid, string amount)
        {
            this.SqlText = "update app_courier set amount=amount + "+ amount +" where courierid = '" + courierid + "'";

            base.Execute(this.SqlText);
        }

        public void updateHeadPic(string courierid, string headpic)
        {
            this.SqlText = "update app_courier set headpic='" + headpic + "' where courierid = '" + courierid + "'";

            base.Execute(this.SqlText);
        }

        public void Logout(string courierid)
        {
            this.SqlText = "update app_courier set appid='' where courierid = '" + courierid + "'";

            base.Execute(this.SqlText);
        }

        //public Hashtable getCurAppID(string courierid)
        //{
        //    this.SqlText = "select appid from app_courier  where courierid = '" + courierid + "'";

        //    return this.load("");
        //}
    }
}
