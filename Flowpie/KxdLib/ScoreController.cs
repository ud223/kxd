using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace KxdLib
{
    public class ScoreController : SystemConfigureLib.iController
    {
        public override Hashtable load(string id)
        {
            this.SqlText = "select * from app_score where userid = ";

            return base.load(id);
        }

        public override string add(Hashtable data)
        {
            this.SqlText = "insert into app_score(userid, score, ModifyAt) values(@userid@, @score@, '@ModifyAt@'); select scoreid from app_score order by scoreid desc limit 1";

            return base.add(data);
        }

        public override void save(Hashtable data)
        {
            this.SqlText = "update app_score set score = score + @score@, ModifyAt = '@ModifyAt@' where userid = @userid@";

            base.save(data);
        }
    }
}
