using Informa.Library.Utilities.UserMembership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Publishing.Scheduled.Tasks
{
    public class SchedulingUnlockUser
    {
        private CustomSqlUserUnlocker sql = new CustomSqlUserUnlocker();

        public void Process()
        {
            sql.UnLockUsers(sql.GetLockedUsers()); 
        }
    }
}
