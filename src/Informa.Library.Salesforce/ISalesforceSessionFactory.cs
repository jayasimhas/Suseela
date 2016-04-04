using System.Runtime.CompilerServices;

namespace Informa.Library.Salesforce
{
	public interface ISalesforceSessionFactory
	{
		ISalesforceSession Create([CallerMemberName] string CallerMemberName = "", [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0);
	}
}
