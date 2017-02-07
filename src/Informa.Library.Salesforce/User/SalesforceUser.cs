namespace Informa.Library.Salesforce.User
{
	public class SalesforceUser : ISalesforceUser
	{
		public string Username { get; set; }

        public string AccessToken { get; set; }
    }
}
