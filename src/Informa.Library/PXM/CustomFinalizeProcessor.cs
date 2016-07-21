using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Informa.Library.Utilities;
using Sitecore.PrintStudio.PublishingEngine.Pipelines.ConvertHtmlToXml;

namespace Informa.Library.PXM
{
	public class CustomFinalizeProcessor
	{
		public void Process(HtmlConvertPipelineArgs args)
		{
			using (var scope = Jabberwocky.Glass.Autofac.Util.AutofacConfig.ServiceLocator.BeginLifetimeScope())
			{
				var helper = scope.Resolve<IPxmXmlHelper>();
				args.OutputText = helper.AddSidebarStyles(args.OutputXml.ToString());
			}
		}
	}
}
