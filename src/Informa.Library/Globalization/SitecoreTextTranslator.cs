using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Globalization
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SitecoreTextTranslator : ITextTranslator
	{
		public string Translate(string key)
		{
			return Sitecore.Globalization.Translate.Text(key);
		}
	}
}
