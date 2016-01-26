using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowpie.Models
{
    public class SendOrder
    {
        public string sendorderid { get; set; }
        public string sendname { get; set; }
        public string expresscode { get; set; }
        public string rundate { get; set; }
        public string runtime { get; set; }
        public string CreateAt { get; set; }
        public string ModifyAt { get; set; }
    }
}