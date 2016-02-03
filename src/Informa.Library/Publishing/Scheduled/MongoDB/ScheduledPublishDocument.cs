using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public class ScheduledPublishDocument : ScheduledPublish, IScheduledPublish
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public DateTime Added { get; set; }
		public DateTime LastUpdated { get; set; }
	}
}
