using Informa.Library.Threading;

namespace Informa.Library.Salesforce
{
	public class SalesforceSessionContext : ThreadSafe<ISalesforceSession>, ISalesforceSessionContext
	{
		protected readonly ISalesforceSessionFactory SessionFactory;

		public SalesforceSessionContext(
			ISalesforceSessionFactory sessionFactory)
		{
			SessionFactory = sessionFactory;
		}

		public ISalesforceSession Session => SafeObject;

		protected override ISalesforceSession UnsafeObject => SessionFactory.Create();

		public ISalesforceSession Refresh()
		{
			Reload();

			return Session;
		}
	}
}
