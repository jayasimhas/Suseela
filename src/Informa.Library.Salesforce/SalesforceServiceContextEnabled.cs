using System;
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

			_lazyEnabled = new Lazy<bool>(!EnabledChecks.Any() || EnabledChecks.All(ec => ec.Enabled));
		}

		private readonly Lazy<bool> _lazyEnabled; 
		public bool Enabled => _lazyEnabled.Value;
	}
}
