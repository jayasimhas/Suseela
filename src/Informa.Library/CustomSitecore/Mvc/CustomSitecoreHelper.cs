using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Helpers;
using Sitecore.Mvc.Presentation;
using JabberwockySitecoreHelper = Jabberwocky.Glass.Autofac.Mvc.Util.CustomSitecoreHelper;

namespace Informa.Library.CustomSitecore.Mvc
{
	public class CustomSitecoreHelper : JabberwockySitecoreHelper
	{
		public CustomSitecoreHelper(HtmlHelper htmlHelper) : base(htmlHelper)
		{
		}

		protected override Rendering GetRendering(string renderingType, object parameters, params string[] defaultValues)
		{
			Rendering rendering = new Rendering { RenderingType = renderingType };
			int index = 0;
			while (index < defaultValues.Length - 1)
			{
				rendering[defaultValues[index]] = defaultValues[index + 1];
				index += 2;
			}

			// Setup existing parameters from Rendering object in Sitecore (if we can grab the rendering by path or ID)
			if (rendering.RenderingItemPath != null)
			{
				var renderingItem = rendering.RenderingItem;
				if (renderingItem != null)
				{
					rendering.Caching.Cacheable = renderingItem.Caching.Cacheable;
					rendering.Caching.VaryByData = renderingItem.Caching.VaryByData;
					rendering.Caching.VaryByDevice = renderingItem.Caching.VaryByDevice;
					rendering.Caching.VaryByLogin = renderingItem.Caching.VaryByLogin;
					rendering.Caching.VaryByParameters = renderingItem.Caching.VaryByParm;
					rendering.Caching.VaryByQueryString = renderingItem.Caching.VaryByQueryString;
					rendering.Caching.VaryByUser = renderingItem.Caching.VaryByUser;
				}
			}
			
			// Override any default/existing parameters with those explicitly passed in
			if (parameters != null)
			{
				TypeHelper.GetProperties(parameters).Each(pair => rendering.Properties[pair.Key] = pair.Value.ValueOrDefault(o => o.ToString()));
			}

			return rendering;
		}

		public virtual HtmlString Placeholder(string placeholderName, Func<HelperResult> helperResultFallback)
		{
			return base.Placeholder(placeholderName);
		}

		public virtual HtmlString Placeholder(string placeholderName, Func<HtmlString> htmlResultFallback)
		{
			return base.Placeholder(placeholderName);
		}

		public virtual HtmlString DynamicPlaceholder(string dynamicKey)
		{
			var currentRenderingId = RenderingContext.Current.Rendering.UniqueId;
			return Placeholder(string.Format("{0}_{1}", dynamicKey, currentRenderingId));
		}
	}
}
