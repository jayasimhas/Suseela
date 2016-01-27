using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Edm;
using Velir.Search.OData.Formatters.Formatters.Serializer;

namespace Velir.Search.OData.Formatters.Formatters.Provider
{
	public class SearchODataSerializerProvider : DefaultODataSerializerProvider
	{
		private readonly SearchODataFeedSerializer _feedSerializer;

		public SearchODataSerializerProvider()
		{
			_feedSerializer = new SearchODataFeedSerializer(this);
		}
		
		public override ODataEdmTypeSerializer GetEdmTypeSerializer(IEdmTypeReference edmType)
		{
			var serializer = base.GetEdmTypeSerializer(edmType);
			var feedSerializer = serializer as ODataFeedSerializer;
			if (feedSerializer != null)
			{
				return _feedSerializer;
			}
			return serializer;
		}
	}
}
