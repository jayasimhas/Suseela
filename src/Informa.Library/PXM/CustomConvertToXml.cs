using Autofac;
using Informa.Library.PXM.Parsers;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Layouts;
using Sitecore.PrintStudio.PublishingEngine.Pipelines.ConvertHtmlToXml;

namespace Informa.Library.PXM
{
    public class CustomConvertToXml
	{
		public void Process(HtmlConvertPipelineArgs args)
		{
			if (string.IsNullOrEmpty(args.InputText))
				return;
		    using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
		    {
		        var charMap = scope.Resolve<ISpecialCharacterMapper>();
		        var parser = new CustomHtmlNodeParser {SpecialCharacterMapper = charMap};

		        parser.ParseNode(XHtml.LoadHtmlDocument(args.InputText, false).DocumentNode, args.OutputXml, args.ParseContext,
		            null);
		    }
		}
	}
}