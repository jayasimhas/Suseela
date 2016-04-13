using System.Collections.Generic;

namespace Informa.Library.Company
{
	public interface IAllowedRegisterUserCompanyTypes
	{
		IEnumerable<CompanyType> Types { get; }
	}
}