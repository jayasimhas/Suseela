using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Informa.Library.Utilities.UserMembership
{
    public class CustomSqlUserUnlocker
    {
        public List<string> GetLockedUsers()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["core"].ConnectionString);
            connection.Open();
            List<string> unlockUsers = new List<string>();
            try
            {
                var cmd = new SqlCommand("select [UserId],[IsLockedOut],[LastLockoutDate] from [aspnet_Membership] where [IsLockedOut] = 1", connection);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string UserId = rdr["UserId"].ToString();
                    DateTime LastLockoutDate = DateTime.Parse(rdr["LastLockoutDate"].ToString());
                    double gab = (DateTime.Now - LastLockoutDate.ToLocalTime()).TotalMinutes;
                    if (gab > Convert.ToInt32( WebConfigurationManager.AppSettings["unlocktimer"].ToString()))
                    {
                        unlockUsers.Add(UserId);
                    }

                }

                return unlockUsers;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("While attempting to get remaining password attempts, an exception was thrown.", ex);
            }
            finally
            {
                connection.Close();

            }
            return new List<string>();
        }

        public void UnLockUsers(List<string> users)
        {
            if (users.Count() != 0)
            {
                string template = "UPDATE [aspnet_Membership] SET [IsLockedOut] = 0 where [UserId]='@UserId';";

                StringBuilder executionQuery = new StringBuilder();
                int done = 0;
                foreach (string id in users)
                {
                    executionQuery.AppendLine(template.Replace("@UserId", id));
                }
                var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["core"].ConnectionString);
                connection.Open();
                List<string> unlockUsers = new List<string>();
                try
                {
                    var cmd = new SqlCommand(executionQuery.ToString(), connection);
                    done = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("While attempting to get remaining password attempts, an exception was thrown.", ex);
                }
                finally
                {
                    connection.Close();

                }

            }

        }
    }
}
