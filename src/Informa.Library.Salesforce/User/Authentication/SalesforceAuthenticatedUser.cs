namespace Informa.Library.Salesforce.User.Authentication
{
	public class SalesforceAuthenticatedUser : SalesforceUser, ISalesforceAuthenticatedUser
	{
		public string Name { get; set; }
		public string Email { get; set; }
	}
}
