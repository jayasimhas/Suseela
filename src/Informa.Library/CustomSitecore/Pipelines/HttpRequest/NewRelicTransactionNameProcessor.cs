using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;

namespace Informa.Library.CustomSitecore.Pipelines.HttpRequest
{
	public class NewRelicTransactionNameProcessor : IProcessor<HttpRequestArgs>
	{
		public void Process(HttpRequestArgs args)
		{
			Item currentItem = Sitecore.Context.Item;
			if (!string.IsNullOrEmpty(currentItem?.TemplateName))
			{
				NewRelic.Api.Agent.NewRelic.SetTransactionName("Webpage", currentItem.TemplateName);
			}
		}
	}
}
