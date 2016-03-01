using Informa.Library.User;
using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User
{
	public class SalesforceUpdateUserPassword : ISalesforceUpdateUserPassword, IUpdateUserPassword
	{
		protected readonly ISalesforceServiceContext Service;
		protected readonly ISalesforceSetUserTemporaryPassword SetUserTemporaryPassword;

		public SalesforceUpdateUserPassword(
			ISalesforceServiceContext service,
			ISalesforceSetUserTemporaryPassword setUserTemporaryPassword)
		{
			Service = service;
			SetUserTemporaryPassword = setUserTemporaryPassword;
		}

		public bool Update(IUser user, string newPassword)
		{
			if (string.IsNullOrWhiteSpace(user?.Username) || string.IsNullOrWhiteSpace(newPassword))
			{
				return false;
			}

			var username = user.Username;

			if (!SetUserTemporaryPassword.Set(username, newPassword))
			{
				return false;
			}

			var updatePasswordResponse = Service.Execute(s => s.updatePassword(username, newPassword, false, newPassword));

			return updatePasswordResponse.IsSuccess();
		}
	}
}
