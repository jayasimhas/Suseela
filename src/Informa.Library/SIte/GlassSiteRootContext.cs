using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Glass.Mapper.Sc;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.PerScope)]
	public class GlassSiteRootContext : ISiteRootContext
	{
		protected readonly ICacheProvider CacheProvider;
		protected readonly ISitecoreContext SitecoreContext;

		public GlassSiteRootContext(
						ISitecoreContext sitecoreContext,
						ICacheProvider cacheProvider)
		{
			SitecoreContext = sitecoreContext;
			CacheProvider = cacheProvider;
		}

	    private ISite_Root _item;
		public ISite_Root Item => _item ?? 
            (_item = SitecoreContext?.GetRootItem<ISite_Root>());

		private string CreateCacheKey(string suffix)
		{
			return $"{nameof(GlassSiteRootContext)}-{suffix}";
		}

		public string GetBodyCssClass()
		{
			string cacheKey = CreateCacheKey($"GetBodyCssClass-{Item?._Id}");
			return CacheProvider.GetFromCache(cacheKey, BuildBodyCssClass);
		}

		public string BuildBodyCssClass()
		{
			return string.IsNullOrEmpty(Item?.Publication_Theme)
					? string.Empty
					: $"class={Item.Publication_Theme}";
		}

		public HtmlString GetPrintHeaderMessage()
		{
			string cacheKey = CreateCacheKey($"GetPrintHeaderMessage-{Item?._Id}");
			return CacheProvider.GetFromCache(cacheKey, BuildPrintHeaderMessage);
		}

		public HtmlString BuildPrintHeaderMessage()
		{
			return new HtmlString(Item?.Print_Message ?? string.Empty);
		}
	}
}
