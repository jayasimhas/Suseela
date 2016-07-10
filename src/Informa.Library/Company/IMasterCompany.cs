namespace Informa.Library.Company
{
	public interface IMasterCompany : ICompany
	{
		bool IsExpired { get; }
	}
}
