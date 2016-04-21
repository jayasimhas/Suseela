namespace Informa.Library.Company
{
	public interface IUserCompanyContext
	{
		ICompany Company { get; set; }
		void Clear();
	}
}