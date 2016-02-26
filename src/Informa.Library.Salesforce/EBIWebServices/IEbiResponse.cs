namespace Informa.Library.Salesforce.EBIWebServices
{
	public interface IEbiResponse
	{
		EBI_Error[] errors { get; }
		bool? success { get; }
	}
}
