using System;
using Sitecore.PrintStudio.PublishingEngine.Helpers;
using Sitecore.PrintStudio.PublishingEngine.Text;
using Sitecore.Reflection;

namespace Informa.Library.PXM.Helpers
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