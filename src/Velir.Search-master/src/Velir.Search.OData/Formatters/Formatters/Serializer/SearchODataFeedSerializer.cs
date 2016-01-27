using System.Collections;
using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Core;
using Microsoft.OData.Edm;

namespace Velir.Search.OData.Formatters.Formatters.Serializer
{
	public class SearchODataFeedSerializer : ODataFeedSerializer
	{
		public SearchODataFeedSerializer(ODataSerializerProvider serializerProvider)
			: base(serializerProvider)
		{
		}

		public override ODataFeed CreateODataFeed(IEnumerable feedInstance, IEdmCollectionTypeReference feedType, ODataSerializerContext writeContext)
		{
			ODataFeed feed = base.CreateODataFeed(feedInstance, feedType, writeContext);
			//feed.InstanceAnnotations.Add(new ODataInstanceAnnotation("ns.previous", new ODataPrimitiveValue("http://previouspagelink")));
			return feed;
		}
	}
}
