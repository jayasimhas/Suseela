using System;

namespace Informa.Library.Logging
{
	public interface IErrorLogger
	{
		void Log(string message, Exception ex);
	}
}
