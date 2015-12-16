using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxdLib
{
    public class OrderController : SystemConfigureLib.iController
    {
        public override List<Hashtable> getAll()
        {
            this.SqlText = "select app_order.*, app_company.companytext from app_order left join app_company on app_order.companyid = app_company.companyid ORDER BY  CreateAt DESC";

            return base.getAll();
        }

        public List<Hashtable> getOrderByUserId(string user_id)
        {
            this.SqlText = "select app_order.*, app_company.companytext from app_order left join app_company on app_order.companyid = app_company.companyid where userid = '" + user_id + "' and state > 0 order by CreateAt desc";

            return base.getAll();
        }

        public List<Hashtable> getOrderAddressByUserId(string user_id)
        {
            this.SqlText = "select DISTINCT fromaddress, fromaddressdetail, lat, lng from app_order where userid = '" + user_id + "' group by fromaddress";

            return base.getAll();
        }

        public List<Hashtable> getOrderByCourierId(string courierid)
        {
            this.SqlText = "select app_order.*, app_users.headpic from app_order left join app_users on app_order.userid = app_users.userid where courierid = '" + courierid + "' and state > 0 order by CreateAt desc";

            return base.getAll();
        }

        public List<Hashtable> getOrderDetailByOrderId(string order_id)
        {
            this.SqlText = "select app_orderdetail.*, companycode from app_orderdetail left join app_order on app_orderdetail.orderid = app_order.orderid left join app_company on app_order.companyid = app_company.companyid where app_orderdetail.orderid = '" + order_id + "'";

            return base.getAll();
        }

        public override Hashtable load(string id)
        {
            this.SqlText = "select app_order.*, app_company.companytext, score, headpic from app_order left join app_company on app_order.companyid = app_company.companyid left join app_courier on app_order.courierid = app_courier.courierid where orderid = '" + id + "'";

            return base.load("");
        }

        public override string add(Hashtable data)
        {
            this.SqlText = "INSERT INTO app_order(orderid, userid, courierid, sendcouriername, sendcourierphone, weight, `long`, width, height, amount, fromname, fromcity, fromtel, orderTypeid, companyid, rundate, runtime, fromaddress, fromaddressdetail, lat, lng, state,  CreateAt, ModifyAt) VALUES('@orderid@', '@userid@', '@courierid@', '@sendcouriername@', '@sendcourierphone@', '@weight@', @long@, @width@, @height@, @amount@, '@fromname@', '@fromcity@', '@fromtel@', @orderTypeid@, @companyid@, '@rundate@', '@runtime@', '@fromaddress@', '@fromaddressdetail@', '@lat@', '@lng@', @state@, '@CreateAt@', '@ModifyAt@'); SELECT orderid FROM app_order ORDER BY orderid DESC Limit 1";

            return base.add(data);
        }

        public string addDetail(Hashtable data)
        {
            this.SqlText = "INSERT INTO app_orderdetail(orderdetailid, expressid, amount, orderid, orderdate, CreateAt, ModifyAt) VALUES('@orderdetailid@', '@expressid@', @amount@, '@orderid@', '@orderdate@', '@CreateAt@', '@ModifyAt@'); SELECT orderdetailid FROM app_orderdetail ORDER BY CreateAt DESC Limit 1";

            return base.add(data);
        }

        public void updateAmount(Hashtable data)
        {
            this.SqlText = "update app_order set amount = "+ data["amount"].ToString() + " where orderid = '"+ data["orderid"].ToString() + "'";

            base.Execute(this.SqlText);
        }

        public void updateState(string id)
        {
            this.SqlText = "update app_order set state = state + 1 where orderid = '"+ id + "'";

            base.Execute(this.SqlText);
        }

        public void reject(string id, string msg)
        {
            this.SqlText = "update app_order set rejectmessage = '"+ msg +"', state = 9 where orderid = '" + id + "'";

            base.Execute(this.SqlText);
        }

        public void setUnlinepay(string id)
        {
            this.SqlText = "update app_order set unlinepay = 1 where orderid = '" + id + "'";

            base.Execute(this.SqlText);
        }

        public void take(Hashtable data)
        {
            this.SqlText = "update app_order set courierid = '"+ data["courierid"].ToString() +"', sendcouriername='"+ data["sendcouriername"].ToString() +"', sendcourierphone='"+ data["sendcourierphone"].ToString() + "', companyid="+ data["companyid"].ToString() + ", state = 1, orderTypeid = 2, ModifyAt='"+ data["ModifyAt"].ToString() + "' where orderid = '" + data["orderid"].ToString() + "'";

            NetLog.WriteTextLog("抢单", this.SqlText, DateTime.Now);

            base.Execute(this.SqlText);
        }

        public void deleteDetail(string id)
        {
            this.SqlText = "delete from app_orderdetail where orderdetailid = '"+ id +"'";

            base.delete("");
        }
        
        public List<Hashtable> isexistExpress(string expressid)
        {
            this.SqlText = "select * from app_orderdetail where expressid = '"+ expressid +"'";

            return base.Query(this.SqlText);
        }

        public List<Hashtable> isExistSendExpress(string expresscode)
        {
            this.SqlText = "select * from app_sendorder where expresscode = '" + expresscode + "'";

            return base.Query(this.SqlText);
        }

        public void addSendExpress(Hashtable data)
        {
            this.SqlText = "INSERT INTO app_sendorder(sendorderid, expresscode, courierid, couriername, courierphone, rundate, runtime, companyid, CreateAt, ModifyAt) values('@sendorderid@', '@expresscode@', '@courierid@', '@couriername@', '@courierphone@', '@rundate@', '@runtime@', @companyid@, '@CreateAt@', '@ModifyAt@'); select sendorderid from app_sendorder  ORDER BY CreateAt DESC Limit 1";

            base.add(data);
        }

        public List<Hashtable> getReceiveOrderByCourierId(string courierid)
        {
            this.SqlText = "select app_order.orderid, expressid, rundate, runtime, app_order.CreateAt, app_order.ModifyAt from app_orderdetail left join app_order on app_orderdetail.orderid = app_order.orderid where app_order.courierid = '" + courierid + "'  order by app_order.CreateAt desc";

            return base.getAll();
        }

        public List<Hashtable> getSendOrderByCourierId(string courierid)
        {
            this.SqlText = "select * from app_sendorder where courierid = '" + courierid + "'  order by CreateAt desc";

            return base.getAll();
        }
    }
}
