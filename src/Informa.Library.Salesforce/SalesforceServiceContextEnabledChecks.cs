using System.Collections;
using System.Collections.Generic;

namespace Informa.Library.Salesforce
{
	public class SalesforceServiceContextEnabledChecks : ISalesforceServiceContextEnabledChecks
	{
		protected readonly IEnumerable<ISalesforceServiceContextEnabledCheck> EnabledChecks;

		public SalesforceServiceContextEnabledChecks(
			IEnumerable<ISalesforceServiceContextEnabledCheck> enabledChecks)
		{
			EnabledChecks = enabledChecks;
		}

		public IEnumerator<ISalesforceServiceContextEnabledCheck> GetEnumerator()
		{
			return EnabledChecks.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
