using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SystemConfigureLib
{
    public class App : iController
    {
        public Hashtable getByVersion(string version)
        {
            this.SqlText = "select * from appUpdateRecords where aur_appversion = '"+ version +"'";

            return base.load("");
        }

        public Hashtable getLastVersion()
        {
            this.SqlText = "select * from appUpdateRecords order by aur_id desc limit 1";

            return base.load("");
        }
    }
}
