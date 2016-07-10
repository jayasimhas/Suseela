using System.Linq;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceContextEnabled : ISalesforceServiceContextEnabled
	{
		public SalesforceServiceContextEnabled(
			ISalesforceServiceContextEnabledChecks enabledChecks)
		{
			Enabled = !enabledChecks.Any() || enabledChecks.All(ec => ec.Enabled);
		}

		public bool Enabled { get; set; }
	}
}
