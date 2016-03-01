using Informa.Library.User;
using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User
{
	public class SalesforceUpdateUserPassword : IUpdateUserPassword
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceUpdateUserPassword(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public bool Update(IUser user, string newPassword)
		{
			if (string.IsNullOrWhiteSpace(user?.Username) || string.IsNullOrWhiteSpace(newPassword))
			{
				return false;
			}

			var response = Service.Execute(s => s.updatePassword(user.Username, string.Empty, false, newPassword));

			return response.IsSuccess();
		}
	}
}
