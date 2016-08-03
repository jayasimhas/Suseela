﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Informa.Library.PXM.Parsers;
using Sitecore.Layouts;
using Sitecore.PrintStudio.PublishingEngine.Pipelines.ConvertHtmlToXml;
using Sitecore.PrintStudio.PublishingEngine.Text.Parsers.Html;

namespace Informa.Library.PXM
{
	public class CustomConvertToXml
	{
		public void Process(HtmlConvertPipelineArgs args)
		{
			if (string.IsNullOrEmpty(args.InputText))
				return;
			new CustomHtmlNodeParser().ParseNode(XHtml.LoadHtmlDocument(args.InputText, false).DocumentNode, args.OutputXml, args.ParseContext, (XElement)null);
		}
	}
}