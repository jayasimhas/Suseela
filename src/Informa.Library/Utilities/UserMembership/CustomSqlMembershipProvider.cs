using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Informa.Library.Utilities.UserMembership
{
    public class CustomSqlMembershipProvider : SqlMembershipProvider
    {
        public int GetRemainingPasswordAttempts(Guid userId)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["core"].ConnectionString);
            connection.Open();

            try
            {
                var cmd = new SqlCommand("select [FailedPasswordAttemptCount] from [vw_aspnet_MembershipUsers] where [vw_aspnet_MembershipUsers].UserId = @userID", connection);

                var userIDParam = new SqlParameter();
                userIDParam.ParameterName = "@userID";
                userIDParam.SqlDbType = SqlDbType.UniqueIdentifier;
                userIDParam.Value = userId;

                cmd.Parameters.Add(userIDParam);

                int output = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                //if (output == null)
                //{
                //    Sitecore.Diagnostics.Log.Warn("Attempting to get remaining password attempts for user [" + userId.ToString() + "] but got nothing from the database!", this);
                //    return 0;
                //}



                return base.MaxInvalidPasswordAttempts - (int)output;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("While attempting to get remaining password attempts, an exception was thrown.", ex);
            }
            finally
            {
                connection.Close();

            }
            return 0;
        }
    }
}
