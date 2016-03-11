using Informa.Library.User;

namespace Informa.Library.Company
{
	public interface IFindCompanyByUser
	{
		ICompany Find(IUser user);
	}
}
