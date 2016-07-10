using Glass.Mapper.Sc;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Globalization
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SitecoreTextTranslator : ITextTranslator
	{
	    protected readonly ISiteRootContext SiteRoot;

        public SitecoreTextTranslator(ISiteRootContext siteRoot)
        {
            SiteRoot = siteRoot;
        }

		public string Translate(string key)
		{
            //Removed auto-generator for dictionary keys
            //return Sitecore.Globalization.Translate.TextByLanguage(key, Sitecore.Context.Language, null, new[] { "GenerateIfDictionaryKeyNotFound" });

            string value = Sitecore.Globalization.Translate.Text(key)
                ?? string.Empty;

		    return value
                .Replace("$publicationname", SiteRoot.Item.Publication_Name);
		}
	}
}
