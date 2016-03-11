using Jabberwocky.Glass.Autofac.Attributes;
using System.IO;
using System.Web;

namespace Informa.Library.Mail
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class HtmlEmailTemplateFactory : IHtmlEmailTemplateFactory
	{
		public IHtmlEmailTemplate Create(string template)
		{
			try
			{
				var path = string.Format("{0}email\\{1}.html", RootPath, template);
				var html = File.ReadAllText(path);

				return new HtmlEmailTemplate
				{
					Html = html
				};
			}
			catch
			{
				return null;
			}
		}

		public string RootPath => HttpContext.Current.Request.PhysicalApplicationPath;
	}
}
