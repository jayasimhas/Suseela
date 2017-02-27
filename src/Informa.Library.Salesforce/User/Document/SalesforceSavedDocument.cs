using Informa.Library.User.Document;
using System;

namespace Informa.Library.Salesforce.User.Profile
{
	public class SalesforceSavedDocument : ISavedDocument
	{
		public string Description { get; set; }
		public string DocumentId { get; set; }
		public string Name { get; set; }
		public DateTime SaveDate { get; set; }
        public string SalesforceId { get; set; }
    }
}
