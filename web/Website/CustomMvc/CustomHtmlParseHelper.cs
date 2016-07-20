using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.PrintStudio.PublishingEngine.Helpers;
using Sitecore.PrintStudio.PublishingEngine.Text;
using Sitecore.PrintStudio.PublishingEngine.Text.Parsers.Html;
using Sitecore.Reflection;

namespace Informa.Web.CustomMvc
{
	public class CustomHtmlParseHelper
	{
		public static T GetParser<T>(ParseDefinition definition)
		{
			if (!string.IsNullOrEmpty(definition.HtmlParserType))
			{
				try
				{
					Type typeInfo = ReflectionUtil.GetTypeInfo(definition.HtmlParserType);
					return typeInfo != (Type)null ? (T)ReflectionUtil.CreateObject(typeInfo) : default(T);
				}
				catch (Exception ex)
				{
					Logger.Error(ex.Message, (Exception)null);
				}
			}
			return default(T);
		}
	}
}