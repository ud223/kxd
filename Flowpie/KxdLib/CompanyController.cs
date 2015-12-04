using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxdLib
{
    public class CompanyController : SystemConfigureLib.iController
    {
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public override List<System.Collections.Hashtable> getAll()
        {
            this.SqlText = "SELECT * FROM app_company";

            return base.getAll();
        }


        public override System.Collections.Hashtable load(string id)
        {
            this.SqlText = "SELECT * FROM app_company WHERE companyid = ";

            return base.load(id);
        }

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回新增部门ID</returns>
        public override string add(System.Collections.Hashtable data)
        {
            this.SqlText = "INSERT INTO app_company(companytext, companycode) VALUES('@companytext@', '@companycode@'); SELECT companyid FROM app_company ORDER BY companyid DESC LIMIT 0, 1";

            return base.add(data);
        }

        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public override void save(System.Collections.Hashtable data)
        {
            this.SqlText = "UPDATE app_company SET companytext = '@companytext@', companycode = '@companycode@' WHERE companyid = @companyid@";

            base.save(data);
        }

        public override void delete(string id)
        {
            this.SqlText = "DELETE FROM app_company WHERE companyid = ";

            base.delete(id);
        }
    }
}
