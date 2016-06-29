using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;

namespace Informa.Library.User.Newsletter
{
	public interface ISiteNewsletterTypesFactory
	{
		ISiteNewsletterTypes Create(ISite_Root siteRoot);
	}
}