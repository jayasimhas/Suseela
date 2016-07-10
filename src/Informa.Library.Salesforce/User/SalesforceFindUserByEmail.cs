using Informa.Library.User;
using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User
{
	public class SalesforceFindUserByEmail : ISalesforceFindUserByEmail
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceFindUserByEmail(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public IUser Find(string email)
		{
		    if (string.IsNullOrEmpty(email))
		        return null;

			var response = Service.Execute(s => s.queryProfileContactInformation(email));

			if (!response.IsSuccess())
			{
				return null;
			}

			return new SalesforceUser
			{
				Username = email
			};
		}
	}
}
