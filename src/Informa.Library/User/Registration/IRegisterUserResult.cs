using System.Collections.Generic;

namespace Informa.Library.User.Registration
{
	public interface IRegisterUserResult
	{
		bool Success { get; }
		IEnumerable<string> Errors { get; }
	}
}
