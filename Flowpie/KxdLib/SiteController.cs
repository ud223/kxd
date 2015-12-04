using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxdLib
{
    public class SiteController : SystemConfigureLib.iController
    {
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public override List<System.Collections.Hashtable> getAll()
        {
            this.SqlText = "SELECT app_sites.*, app_company.companytext FROM app_sites LEFT JOIN app_company ON app_sites.companyid = app_company.companyid";

            return base.getAll();
        }

        public List<System.Collections.Hashtable> getByCompanyId(string companyid)
        {
            this.SqlText = "SELECT app_sites.*, app_company.companytext FROM app_sites LEFT JOIN app_company ON app_sites.companyid = app_company.companyid WHERE app_sites.companyid = " + companyid;

            return base.getAll();
        }

        public override System.Collections.Hashtable load(string id)
        {
            this.SqlText = "SELECT * FROM app_sites WHERE siteid = ";

            return base.load(id);
        }

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回新增部门ID</returns>
        public override string add(System.Collections.Hashtable data)
        {
            this.SqlText = "INSERT INTO app_sites(sitetext, address, phone, contactname, companyid, lat, lng, CreateAt, ModifyAt) VALUES('@sitetext@', '@address@', '@phone@', '@contactname@', @companyid@, '@lat@', '@lng@', '@CreateAt@', '@ModifyAt@'); SELECT SiteID FROM app_sites ORDER BY SiteID DESC LIMIT 0, 1";

            return base.add(data);
        }

        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public override void save(System.Collections.Hashtable data)
        {
            this.SqlText = "UPDATE app_sites SET sitetext = '@sitetext@', address = '@address@', phone = '@phone@', contactname = '@contactname@', companyid = @companyid@, lat = '@lat@', lng = '@lng@', CreateAt = '@CreateAt@', ModifyAt = '@ModifyAt@' WHERE siteid = @siteid@";

            base.save(data);
        }

        public override void delete(string id)
        {
            this.SqlText = "DELETE FROM app_sites WHERE siteid = ";

            base.delete(id);
        }
    }
}
