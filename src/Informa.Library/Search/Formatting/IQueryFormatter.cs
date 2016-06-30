namespace Informa.Library.Search.Formatting
{
	public interface IQueryFormatter
	{
		bool NeedsFormatting(string rawQuery);
		string FormatQuery(string rawQuery);
	}
}
