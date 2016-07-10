using System.Collections.Generic;

namespace Informa.Library.User.Registration
{
	public class RegisterUserResult : IRegisterUserResult
	{
		public bool Success { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}
