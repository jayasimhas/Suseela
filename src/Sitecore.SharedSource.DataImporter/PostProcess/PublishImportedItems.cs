﻿using Sitecore.Data.Items;
using Sitecore.Publishing;
using Sitecore.SharedSource.DataImporter.Logger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.SharedSource.DataImporter.PostProcess
{
	public class PublishImportedItems
	{
		ILogger _log;
		List<Guid> _publishedParents;
		Item _importToWhereItem;
		public PublishImportedItems(ILogger l, Item importToWhereItem)
		{
			_log = l;
			_publishedParents = new List<Guid>();
			_importToWhereItem = importToWhereItem;
		}

		private void publishParentWithParents(Item parent)
		{
			//Prevents publishing of 'Articles' folder and other sub folders if already published
			if (parent.ID.Guid.Equals(_importToWhereItem.ID.Guid) || _publishedParents.Contains(parent.ID.Guid))
				return;

			//Publish folder
			PublishManager.PublishItem(parent, new Data.Database[] { Sitecore.Data.Database.GetDatabase("web") }, parent.Languages, false, false, true);
			//Add GUID of published folder to temp list so not to publish again
			_publishedParents.Add(parent.ID.Guid);

			if (parent.Parent != null)//publish parent of parent
				publishParentWithParents(parent.Parent);
		}

		public void Publish()
		{
			try
			{
				//Get all items that have been imported using the DataImporter Module
				var importedItems = _importToWhereItem.Axes.GetDescendants().Where(w => w.Fields["Notification Text"] != null && w.Fields["Notification Text"].Value == "Imported with DataImporter").ToList();//Notification Text gets filled in BaseDataMap

				if (Sitecore.Context.Job != null)
					Sitecore.Context.Job.Status.Total = importedItems.Count;

				int procCount = 0;
				foreach (var item in importedItems)
				{
					try
					{
						//Publish parents
						if (_publishedParents.Contains(item.Parent.ID.Guid) == false)
						{
							publishParentWithParents(item.Parent);
						}

						//Publish item
						//PublishManager.PublishItem(item, new Data.Database[] { Sitecore.Data.Database.GetDatabase("web") }, item.Languages, false, false, true);
						Sitecore.Publishing.PublishOptions publishOptions =
						new Sitecore.Publishing.PublishOptions(item.Database,
										 Sitecore.Data.Database.GetDatabase("web"),
										 Sitecore.Publishing.PublishMode.SingleItem,
										 item.Language,
										 System.DateTime.Now);
						Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);
						publisher.Options.RootItem = item;
						publisher.Options.Deep = false;
						publisher.Publish();
					}
					catch (Exception ex)
					{
						_log.Log(item.Name, ex.ToString(), Providers.ProcessStatus.Error);
					}

					procCount++;

					if (Sitecore.Context.Job != null)
						Sitecore.Context.Job.Status.Processed = procCount;
				}
			}
			catch (Exception ex)
			{
				_log.Log(string.Empty, ex.ToString(), Providers.ProcessStatus.Error);
			}
		}
	}
}