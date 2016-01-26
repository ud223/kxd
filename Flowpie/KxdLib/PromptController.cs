using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KxdLib
{
    public class PromptController : SystemConfigureLib.iController
    {
        public override Hashtable load(string id)
        {
            this.SqlText = "select * from app_prompt limit 1";

            return base.load(id);
        }

        public override string add(Hashtable data)
        {
            this.SqlText = "insert into app_prompt(text) values('@text@'); select * from app_prompt limit 1";

            return base.add(data);
        }

        public override void save(Hashtable data)
        {
            this.SqlText = "update app_prompt set text = '@text@' where promptid = @promptid@";

            base.save(data);
        }
    }
}
