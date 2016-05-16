using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Informa.Library.Publication;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Proxies;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.DataImporter.Extensions;
using Sitecore.SharedSource.DataImporter.Logger;
using Sitecore.SharedSource.DataImporter.Mappings.Properties;

namespace Sitecore.SharedSource.DataImporter.Providers
{
	public class PmbiDataMap : SitecoreDataMap
	{
		public string ArticleNumberPrefix { get; set; }
		public int ArticleNumber { get; set; }

		private const string LastArticleNumber = "Last Article Number";
		private const string ArticleDate = "Article Date";
		private const string RelatedArticles = "Related Articles";
		private const string LegacySitecoreId = "Legacy Sitecore ID";
		private const string PmbiArticleNumber = "Article Number";
		private const string MediaSourcePath = "Media Source Path";
		private const string MediaDestinationPath = "Media Destination Path";
		private const string CreatedDate = "Created Date";
		private const string LegacyArticleNumber = "Legacy Article Number";
		private const string ArticleNumberPrefixStr = "Article Number Prefix";

		public PmbiDataMap(Database db, string connectionString, Item importItem, ILogger l) : base(db, connectionString, importItem, l)
		{
			ArticleNumberPrefix = importItem.Fields[ArticleNumberPrefixStr].Value;
			var val = importItem.Fields[LastArticleNumber].Value;
			if (string.IsNullOrWhiteSpace(val))
			{
				ArticleNumber = 0;
			}
			else
			{
				int articleNumber;
				int.TryParse(importItem.Fields[LastArticleNumber].Value, out articleNumber);
				ArticleNumber = articleNumber;
			}
		}

		/// <summary>
		/// Get Import items from specified start path, template id and year
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<object> GetImportData()
		{
			var startItem = FromDB.GetItem(StartPath);
			var articles = Enumerable.Empty<Item>();
			if (startItem != null)
			{
				var sw = new Stopwatch();
				sw.Start();
				if (!string.IsNullOrWhiteSpace(Year))
				{
					articles =
						startItem.Axes.GetDescendants()
							.Where(i => i.TemplateID.ToString() == TemplateId && i.Fields[ArticleDate].Value.Contains(Year));
				}
				else
				{
					articles =
						startItem.Axes.GetDescendants()
							.Where(i => i.TemplateID.ToString() == TemplateId);
				}
				sw.Stop();
				Logger.Log("Performance Statistic-(Sitecore.SharedSource.DataImporter.Providers.PmbiDataMap.GetImportData)", $"Used {sw.Elapsed.TotalSeconds} to Find matches");
			}

			return articles;
		}

		public void SetArticleNumber()
		{
			using (new EditContext(ImportItem, true, false))
			{
				ImportItem.Fields[LastArticleNumber].Value = ArticleNumber.ToString();
			}
		}

		public void SetRelatedArticles()
		{
			var articlesToSet = GetImportedArticles();
			var repo = new ArticleMappingRepository(new ArticleMappingContext());

			foreach (var item in articlesToSet)
			{
				var oldValue = item.Fields[RelatedArticles].Value;
				if (!string.IsNullOrWhiteSpace(oldValue))
				{
					// Get old guids and map to new guids
					var ids = oldValue.Split('|').Select(i => new Guid(i));

					var sw = new Stopwatch();
					sw.Start();
					var newIds = repo.GetMappingsByIds(ids).Select(i => i.ArticleId);
					sw.Stop();
					Logger.Log("Performance Statistic-(Informa.Library.Publication.ArticleMappingRepository.GetMappingsByIds)", $"Used {sw.Elapsed.TotalSeconds} to Find matches");

					// Transform new guids to sitecore field value
					var transformedValue = IdsToSitecoreStringValue(newIds);

					if (!string.IsNullOrWhiteSpace(transformedValue))
					{
						using (new EditContext(item, true, false))
						{
							item.Fields[RelatedArticles].Value = transformedValue;
						}
					}
				}
			}
		}

		public void AddOrUpdateMapping(Item newItem)
		{
			var legacyId = new ID(newItem.Fields[LegacySitecoreId].Value).Guid;

			var repo = new ArticleMappingRepository(new ArticleMappingContext());
			if (repo.Exist(legacyId))
			{
				repo.Update(newItem.ID.Guid, newItem.Fields[PmbiArticleNumber].Value, legacyId,
					newItem.Fields[LegacyArticleNumber].Value);
			}
			else
			{
				repo.Insert(newItem.ID.Guid, newItem.Fields[PmbiArticleNumber].Value, legacyId,
					newItem.Fields[LegacyArticleNumber].Value);
			}
		}

		public void TransferMediaLibrary()
		{
			var sourcePath = ImportItem.Fields[MediaSourcePath].Value;
			var destinationPath = ImportItem.Fields[MediaDestinationPath].Value;

			var sourceItem = FromDB.GetItem(sourcePath);
			var destinationItem = Database.GetDatabase("master").GetItem(destinationPath);

			if (sourceItem != null && destinationItem != null)
			{
				using (new ProxyDisabler())
				{
					var defaultOptions = ItemSerializerOptions.GetDefaultOptions();
					defaultOptions.AllowDefaultValues = false;
					defaultOptions.AllowStandardValues = false;
					defaultOptions.ProcessChildren = true;

					TransferMediaItem(sourceItem, destinationItem, defaultOptions);
				}
			}
		}

		#region Utility Methods

		protected string IdsToSitecoreStringValue(IEnumerable<Guid> guids)
		{
			var result = string.Empty;
			var enumerable = guids as IList<Guid> ?? guids.ToList();

			if (guids != null && enumerable.Any())
			{
				foreach (var guid in enumerable)
				{
					result = string.IsNullOrWhiteSpace(result) ? new ID(guid).ToString() : $"{result}|{new ID(guid)}";
				}
			}
			return result;
		}

		protected IEnumerable<Item> GetImportedArticles()
		{
			var templateId = ImportItem.GetItemField("Import To What Template", Logger);
			var articles =
				ImportToWhere.Axes.GetDescendants()
					.Where(i => i.TemplateID.ToString() == templateId && i.Fields[CreatedDate].Value.Contains(Year));
			return articles;
		}

		protected void TransferMediaItemBlob(Item source, Item destination, bool processChildren, ref long count)
		{
			Assert.IsNotNull(source, "source is null");
			Assert.IsNotNull(destination, "destination is null");

			count++;
			Logger.Log("Progress-", $"Processing \"{source.Paths.FullPath}\" - {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}");
			foreach (Field field in source.Fields)
			{
				if (field.IsBlobField)
				{
					var str = field.Value;
					if (str.Length > 38)
					{
						str = str.Substring(0, 38);
					}
					var guid = MainUtil.GetGuid(str, Guid.Empty);
					if (!(guid == Guid.Empty))
					{
						var blobStream = ItemManager.GetBlobStream(guid, ProxyManager.GetRealDatabase(source));
						if (blobStream != null)
						{
							using (blobStream)
							{
								ItemManager.SetBlobStream(blobStream, guid, ProxyManager.GetRealDatabase(destination));
							}
						}
					}
				}
			}

			if (Sitecore.Context.Job != null)
			{
				Sitecore.Context.Job.Status.Processed = count;
				Sitecore.Context.Job.Status.Messages.Add($"Processed media item {count}.");
			}

			if (processChildren)
			{
				foreach (Item child in source.Children)
				{
					if (child != null)
					{
						TransferMediaItemBlob(child, destination, true, ref count);
					}
				}
			}
		}

		protected void TransferMediaItem(Item sourceItem, Item destItem, ItemSerializerOptions option)
		{
			var outerXml = sourceItem.GetOuterXml(option);
			try
			{
				destItem.Paste(outerXml, false, PasteMode.Overwrite);

				if (sourceItem.Paths.IsMediaItem)
				{
					long itemCount = 0;
					TransferMediaItemBlob(sourceItem, destItem, true, ref itemCount);
				}
			}
			catch (Exception ex)
			{
				Log.Error("An error occured while importing media", ex, this);
			}
		}

		protected void ProcessNodeRecursively(XmlNode current, HashSet<string> checkSet, XmlDocument doc)
		{
			if (current.HasChildNodes)
			{
				var childNodes = current.ChildNodes.Cast<XmlNode>().Where(i => i.Name == "item").ToList();
				if (childNodes.Any())
				{
					for (int i = 0; i < current.ChildNodes.Count; i++)
					{
						ProcessNodeRecursively(current.ChildNodes[i], checkSet, doc);
					}
				}
			}

			if (current.Attributes?["id"] != null)
			{
				if (checkSet.Contains(current.Attributes?["id"].Value))
				{
					//var attr = doc.CreateAttribute("remove");
					//attr.Value = "1";
					//current.Attributes.Append(attr);
					if (!current.ChildNodes.Cast<XmlNode>().Any(i => i.Name == "item"))
					{
						current.ParentNode?.RemoveChild(current);
					}
				}
			}
		}

		protected HashSet<string> GetExistingMediaItemsInTargetDb(Item startItem)
		{
			var result = new HashSet<string>();
			var descendants = startItem?.Axes.GetDescendants();

			if (descendants != null)
			{
				foreach (var item in descendants)
				{
					result.Add(item.ID.ToString());
				}
			}

			return result;
		}

		#endregion
	}
}
