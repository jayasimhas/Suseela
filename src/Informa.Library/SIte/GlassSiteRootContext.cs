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

		public GlassSiteRootContext(
            ISitecoreContext sitecoreContext,
            ICacheProvider cacheProvider)
		{
			Item = sitecoreContext.GetRootItem<ISite_Root>();
		    CacheProvider = cacheProvider;

		}
		public ISite_Root Item { get; set; }

        private string CreateCacheKey(string suffix) {
            return $"{nameof(GlassSiteRootContext)}-{suffix}";
        }
        
        public string GetBodyCssClass() {
            string cacheKey = CreateCacheKey($"GetBodyCssClass-{Item?._Id}");
            return CacheProvider.GetFromCache(cacheKey, BuildBodyCssClass);
        }

        public string BuildBodyCssClass() {
            return string.IsNullOrEmpty(Item?.Publication_Theme)
                ? string.Empty
                : $"class={Item.Publication_Theme}";
        }

        public HtmlString GetPrintHeaderMessage() {
            string cacheKey = CreateCacheKey($"GetPrintHeaderMessage-{Item?._Id}");
            return CacheProvider.GetFromCache(cacheKey, BuildPrintHeaderMessage);
        }

        public HtmlString BuildPrintHeaderMessage() {
            return new HtmlString(Item?.Print_Message ?? string.Empty);
        }
    }
}
