using System;
using System.Text.RegularExpressions;
using Autofac;
using Glass.Mapper.Sc;
using Informa.Library.PXM.Helpers;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.PrintStudio.PublishingEngine.Pipelines.ConvertHtmlToXml;

namespace Informa.Library.PXM
{
    public class BodyContentProcessor
	{
		public void Process(HtmlConvertPipelineArgs args)
		{
			using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
			{
				var tokenToHtml = scope.Resolve<ITokenToHtml>();
                var sitecoreService = scope.Resolve<ISitecoreService>();
                var injector = scope.Resolve<IInjectAdditionalFields>();

			    var regex = new Regex(@"<!--(\{\w{8}-\w{4}-\w{4}-\w{4}-\w{12}\})-->");
			    var match = regex.Match(args.InputText);
			    args.InputText = regex.Replace(args.InputText, string.Empty);
			    Guid itemId;
			    if (Guid.TryParse(match.Groups[1].Value, out itemId))
			    {
                    var glassArticle = sitecoreService.GetItem<IArticle>(itemId);
                    args.InputText = injector.InjectIntoHtml(args.InputText, glassArticle);
                }

                var helper = scope.Resolve<IPxmHtmlHelper>();
				args.InputText = tokenToHtml.ReplaceAllTokens(args.InputText);
                args.InputText = helper.ProcessQandA(args.InputText);
				args.InputText = helper.ProcessIframe(args.InputText);
				args.InputText = helper.ProcessQuickFacts(args.InputText);
				args.InputText = helper.ProcessTableStyles(args.InputText);
            }
		}
	}
}
