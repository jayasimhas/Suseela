using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Salesforce.V2.User.Authentication
{
    public class UserInfoResult
    {
        public string preferred_username { get; set; }
        public string name { get; set; }
        public string family_name { get; set; }
        public string email { get; set; }
        public string given_name { get; set; }
        public string nickname { get; set; }
    }
}
