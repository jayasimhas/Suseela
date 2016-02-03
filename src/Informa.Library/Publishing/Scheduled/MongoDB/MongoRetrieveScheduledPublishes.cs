using System;
using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public class MongoRetrieveScheduledPublishes : IRetrieveScheduledPublishes
	{
		public IEnumerable<IScheduledPublish> All
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
