using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User
{
	public class SalesforceSetUserTemporaryPassword : ISalesforceSetUserTemporaryPassword
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceSetUserTemporaryPassword(
			ISalesforceServiceContext service)
		{
			Service = service;	
		}

		public bool Set(string username, string temporaryPassword)
		{
		    if (string.IsNullOrEmpty(username))
		        return false;

			var tempUpdatePasswordResponse = Service.Execute(s => s.updatePassword(username, string.Empty, true, temporaryPassword));

			return tempUpdatePasswordResponse.IsSuccess();
		}
	}
}
