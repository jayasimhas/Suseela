using Glass.Mapper.Sc;
using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Globalization
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SitecoreTextTranslator : ITextTranslator
	{
	    private readonly IDependencies _dependencies;

        [AutowireService(true)]
	    public interface IDependencies
	    {
            ISiteRootContext SiteRootContext { get; }
            ISitecoreService SitecoreService { get; }
	    }

	    public SitecoreTextTranslator(IDependencies dependencies)
	    {
	        _dependencies = dependencies;
	    }

		public string Translate(string key)
		{
            //Removed auto-generator for dictionary keys
            //return Sitecore.Globalization.Translate.TextByLanguage(key, Sitecore.Context.Language, null, new[] { "GenerateIfDictionaryKeyNotFound" });

            string value = Sitecore.Globalization.Translate.Text(key)
                ?? string.Empty;

		    return value
                .Replace("$publicationname", _dependencies.SiteRootContext.Item.Publication_Name);
		}
	}
}
