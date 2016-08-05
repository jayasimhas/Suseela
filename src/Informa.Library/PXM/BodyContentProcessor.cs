using Autofac;
using Informa.Library.PXM.Helpers;
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
                
				var helper = scope.Resolve<IPxmHtmlHelper>();
				args.InputText = tokenToHtml.ReplaceAllTokens(args.InputText);
				args.InputText = helper.ProcessIframe(args.InputText);
				args.InputText = helper.ProcessQuickFacts(args.InputText);
				args.InputText = helper.ProcessTableStyles(args.InputText);
			}
		}
	}
}
