namespace Informa.Library.User.Offer
{
	public interface IOfferUserOptedInContext
	{
		bool OptedIn { get; }
		void Clear();
	}
}
