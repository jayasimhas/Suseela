namespace Informa.Library.Company
{
	public interface IFindCompanyByMasterId
	{
		ICompany Find(string masterId, string password);
	}
}
