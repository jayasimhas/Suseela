using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Globalization;

namespace Informa.Library.Globalization
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SitecoreTextTranslator : ITextTranslator
	{
		public string Translate(string key)
		{   
            return Sitecore.Globalization.Translate.TextByLanguage(key, Sitecore.Context.Language, null, new []{ "GenerateIfDictionaryKeyNotFound" });
		}
	}                         
}
