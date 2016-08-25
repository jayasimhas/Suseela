using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Informa.Library.Utilities.UserMembership
{
    public class UserAttemp
    {
        public static int AttemptedPasswordAttemptsRemaining(MembershipUser user)
        {
            var membershipProvider = (CustomSqlMembershipProvider)Membership.Providers["sqlCustom"];
            if (membershipProvider == null)
            {
                return 0;
            }

            return membershipProvider.GetRemainingPasswordAttempts((Guid)user.ProviderUserKey);
        }
    }
}
