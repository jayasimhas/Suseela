using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Globalization
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SitecoreTextTranslator : ITextTranslator
	{
		public string Translate(string key)
		{
            //Removed auto-generator for dictionary keys
            //return Sitecore.Globalization.Translate.TextByLanguage(key, Sitecore.Context.Language, null, new[] { "GenerateIfDictionaryKeyNotFound" });

            return Sitecore.Globalization.Translate.Text(key);
		}
	}
}
