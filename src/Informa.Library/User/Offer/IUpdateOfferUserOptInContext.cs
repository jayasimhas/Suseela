using Informa.Library.User.Newsletter;

namespace Informa.Library.User.Offer
{
	public interface IUpdateOfferUserOptInContext
	{
		bool Update(bool optIn,bool isUpdate);
	}
}