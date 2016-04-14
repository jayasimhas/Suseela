using System.Linq;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceContextEnabled : ISalesforceServiceContextEnabled
	{
		protected readonly ISalesforceServiceContextEnabledChecks EnabledChecks;

		public SalesforceServiceContextEnabled(
			ISalesforceServiceContextEnabledChecks enabledChecks)
		{
			EnabledChecks = enabledChecks;
		}

		public bool Enabled => !EnabledChecks.Any() || EnabledChecks.All(ec => ec.Enabled);
	}
}
