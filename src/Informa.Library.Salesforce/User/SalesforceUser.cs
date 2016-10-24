namespace Informa.Library.Salesforce.User
{
	public class SalesforceUser : ISalesforceUser
	{
		public string Username { get; set; }
        public string SalesForceSessionId { get; set; }
        public string SalesForceURL { get; set; }
    }
}
