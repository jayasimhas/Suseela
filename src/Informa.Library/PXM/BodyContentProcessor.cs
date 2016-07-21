using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Informa.Library.Utilities.TokenMatcher;
using Sitecore.PrintStudio.PublishingEngine.Pipelines.ConvertHtmlToXml;

namespace Informa.Library.PXM
{
	public class BodyContentProcessor
	{
		public void Process(HtmlConvertPipelineArgs args)
		{
			using (var scope = Jabberwocky.Glass.Autofac.Util.AutofacConfig.ServiceLocator.BeginLifetimeScope())
			{
				var tokenToHtml = scope.Resolve<ITokenToHtml>();
				args.InputText = tokenToHtml.ReplaceAllTokens(args.InputText);
			}
		}
	}
}
