namespace Informa.Library.Company
{
	public interface IFindCompanyByMasterId
	{
		IMasterCompany Find(string masterId, string password);
	}
}
