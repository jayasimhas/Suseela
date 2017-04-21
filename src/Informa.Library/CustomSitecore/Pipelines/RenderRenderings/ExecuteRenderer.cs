using System;
using System.IO;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;
using StackExchange.Profiling;

namespace Informa.Library.CustomSitecore.Pipelines.RenderRenderings
{
	public class ExecuteRenderer : Sitecore.Mvc.Pipelines.Response.RenderRendering.ExecuteRenderer
	{
		protected override bool Render(Renderer renderer, TextWriter writer, RenderRenderingArgs args)
		{
			bool success;
            Sitecore.Diagnostics.Log.Info("Started ExecuteRenderer", " ExecuteRenderer ");
            try
			{
				using (MiniProfiler.Current.Step($"Rendering:{args.Rendering.RenderingItem.DisplayName}"))
				{
					success = base.Render(renderer, writer, args);
				}
			}
			catch (Exception e)
			{
				success = false;

				if (Context.PageMode.IsExperienceEditor || Context.PageMode.IsPreview)
				{
					writer.Write("<p>Error rendering the following component: {0}</p><p>{1}</p>", args.Rendering.RenderingItem.Name, e);
				}

				Log.Error(e.Message, e, typeof(ExecuteRenderer));
			}
            Sitecore.Diagnostics.Log.Info("Ended ExecuteRenderer", " ExecuteRenderer");
            return success;
		}
	}
}
