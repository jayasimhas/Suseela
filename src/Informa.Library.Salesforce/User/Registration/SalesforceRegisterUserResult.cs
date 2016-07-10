using Informa.Library.User.Registration;
using System.Collections.Generic;

namespace Informa.Library.Salesforce.User.Registration
{
	public class SalesforceRegisterUserResult : IRegisterUserResult
	{
		public bool Success { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}
