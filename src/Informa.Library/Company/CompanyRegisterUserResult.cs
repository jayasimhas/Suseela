using Informa.Library.User.Registration;
using System.Collections.Generic;

namespace Informa.Library.Company
{
	public class CompanyRegisterUserResult : IRegisterUserResult
	{
		public bool Success { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}
