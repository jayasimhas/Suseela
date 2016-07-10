using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;

namespace Informa.Library.Publication
{
	public interface ISitePublicationFactory
	{
		ISitePublication Create(ISite_Root siteRoot);
	}
}