using Sitecore.Diagnostics;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Informa.Library.CustomSitecore.Pipelines
{
	public class AddIndexUpdateCacheFlag : RenderRenderingProcessor
	{
		private const string ClearOnIndexUpdatePropertyName = "ClearOnIndexUpdate";
		
		public override void Process(RenderRenderingArgs args)
		{
            Sitecore.Diagnostics.Log.Info("Started AddIndexUpdateCacheFlag", " AddIndexUpdateCacheFlag ");
            Assert.ArgumentNotNull(args, "args");
			if (args.Rendered || !args.Cacheable || 
				args.Rendering == null || args.Rendering.RenderingItem == null || args.Rendering.RenderingItem.InnerItem == null)
				return;

			if (args.Rendering.RenderingItem.InnerItem[ClearOnIndexUpdatePropertyName].ToBool())
			{
				args.CacheKey = (args.CacheKey ?? string.Empty) + "_#index";
			}
            Sitecore.Diagnostics.Log.Info("Ended AddIndexUpdateCacheFlag", " AddIndexUpdateCacheFlag");
        }
	}
}
