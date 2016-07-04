namespace Informa.Library.Salesforce.EBIWebServices
{
	public static class IEbiResponseExtensions
	{
		public static bool IsSuccess(this IEbiResponse source)
		{
			return source?.success != null && source.success.Value;
		}
	}
}
