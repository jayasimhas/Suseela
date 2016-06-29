using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Publication
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SitePublicationFactory : ISitePublicationFactory
	{
		protected readonly ISitePublicationCodeFactory CodeFactory;
		protected readonly ISitePublicationNameFactory NameFactory;

		public SitePublicationFactory(
			ISitePublicationCodeFactory codeFactory,
			ISitePublicationNameFactory nameFactory)
		{
			CodeFactory = codeFactory;
			NameFactory = nameFactory;
		}

		public ISitePublication Create(ISite_Root siteRoot)
		{
			if (siteRoot == null)
			{
				return null;
			}

			return new SitePublication
			{
				Code = CodeFactory.Create(siteRoot),
				Name = NameFactory.Create(siteRoot)
			};
		}
	}
}
