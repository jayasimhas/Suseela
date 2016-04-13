using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Presentation
{
	[AutowireService]
	public class DuplicateRenderingsFactory : IDuplicateRenderingsFactory
	{
		protected readonly IDuplicateRenderingsFactoryConfiguration Configuration;
		protected readonly ISitecoreService SitecoreService;

		public DuplicateRenderingsFactory(
			IDuplicateRenderingsFactoryConfiguration configuration,
			ISitecoreService sitecoreService)
		{
			Configuration = configuration;
			SitecoreService = sitecoreService;
		}

		public IEnumerable<IRendering> Create()
		{
			var contentCurationItem = SitecoreService.GetItem<IContent_Curation>(Configuration.ContentCurationItemId);

			return new List<Rendering>(contentCurationItem.Manually_Curated_Renderings.Select(i => new Rendering
			{
				RenderingItemId = i._Id
			}));
		}
	}
}
