namespace Informa.Library.Salesforce.EBIWebServices
{
	public static class IEbiResponseExtensions
	{
		public static bool IsSuccess(this IEbiResponse source)
		{
			return source != null && source.success.HasValue && source.success.Value;
		}
	}
}
