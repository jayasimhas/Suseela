using Informa.Library.Publication;

namespace Informa.Library.User.Newsletter
{
	public interface ISiteNewsletterTypes
	{
		ISitePublication Publication { get; }
		string Breaking { get; }
		string Daily { get; }
		string Weekly { get; }
	}
}