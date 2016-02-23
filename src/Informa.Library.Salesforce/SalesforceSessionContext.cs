using System;

namespace Informa.Library.Salesforce
{
	public class SalesforceSessionContext : ISalesforceSessionContext
	{
		protected readonly ISalesforceSessionFactory SessionFactory;

		public SalesforceSessionContext(
			ISalesforceSessionFactory sessionFactory)
		{
			SessionFactory = sessionFactory;
		}

		public ISalesforceSession Session { get; set; }

		public void Refresh()
		{
			throw new NotImplementedException();
		}
	}
}
