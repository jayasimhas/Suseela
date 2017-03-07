using Informa.Library.User.Newsletter;

namespace Informa.Library.User.Offer
{
	public interface IOfferUserOptedInContext
	{
        OffersOptIn OptedIn { get; }
		void Clear();
	}
}
