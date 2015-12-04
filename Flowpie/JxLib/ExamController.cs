using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxLib
{
    public class ExamController : SystemConfigureLib.iController
    {
        /// <summary>
        /// 获取所有考题
        /// </summary>
        /// <returns></returns>
        public override List<System.Collections.Hashtable> getAll()
        {
            this.SqlText = "SELECT * FROM app_exams ORDER BY ExamID";

            return base.getAll();
        }

        public override System.Collections.Hashtable load(string id)
        {
            this.SqlText = "SELECT * FROM app_exams WHERE ExamID = ";

            return base.load(id);
        }

        /// <summary>
        /// 新增考题
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回新增考题ID</returns>
        public override string add(System.Collections.Hashtable data)
        {
            throw new Exception("没有实现新增考题!");
            //this.SqlText = "INSERT INTO app_examtypes(ExamTypeText, Scale) VALUES('@ExamTypeText@', @Scale@); SELECT ExamTypeID FROM app_examtypes ORDER BY ExamTypeID DESC LIMIT 0, 1";

            //return base.add(data);
        }

        /// <summary>
        /// 保存考题信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public override void save(System.Collections.Hashtable data)
        {
            throw new Exception("没有实现保存考题!");
            //this.SqlText = "UPDATE app_examtypes SET ExamTypeText = '@ExamTypeText@', Scale = @Scale@ WHERE ExamTypeID = @ExamTypeID@";

            //base.save(data);
        }

        public override void delete(string id)
        {
            this.SqlText = "DELETE FROM app_exams WHERE ExamID = ";

            base.delete(id);
        }
    }
}
