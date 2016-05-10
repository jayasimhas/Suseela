﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		private const string PmbiContent = "pmbiContent";
		private const string LastArticleNumber = "Last Article Number";
		private const string ArticleDate = "Article Date";
		private const string RelatedArticles = "Related Articles";
		private const string LegacySitecoreId = "Legacy Sitecore ID";
		private const string PmbiArticleNumber = "Article Number";
		private const string MediaSourcePath = "Media Source Path";
		private const string MediaDestinationPath = "Media Destination Path";
		private const string CreatedDate = "Created Date";
		private const string LegacyArticleNumber = "Legacy Article Number";

		public PmbiDataMap(Database db, string connectionString, Item importItem, ILogger l) : base(db, connectionString, importItem, l)
		{
			ArticleNumberPrefix = GetArticleNumberPrefix(importItem);
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
				Logger.Log("Performance Statistic", $"Used {sw.Elapsed.TotalSeconds} to Find matches");
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
					var newIds = repo.GetMappingsByIds(ids).Select(i => i.ArticleId);

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
					var outerXml = sourceItem.GetOuterXml(defaultOptions);

					try
					{
						destinationItem.Paste(outerXml, false, PasteMode.Overwrite);
						if (sourceItem.Paths.IsMediaItem)
						{
							TransferMediaItemBlob(sourceItem, destinationItem, true);
						}
					}
					catch (Exception exception)
					{
						Log.Error("An error occured while importing media", exception, this);
					}
				}
			}
		}

		#region Utility Methods

		protected string GetArticleNumberPrefix(Item item)
		{
			var result = string.Empty;

			switch (item.Parent.Name)
			{
				case "The Pink Sheet":
					result = "PS";
					break;
				case "Medtech Insight":
					result = "MI";
					break;
				case "In Vivo":
					result = "IV";
					break;
				case "The Rose Sheet":
					result = "RS";
					break;
			}

			return result;
		}

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

		protected void TransferMediaItemBlob(Item source, Item destination, bool processChildren)
		{
			Assert.IsNotNull(source, "source is null");
			Assert.IsNotNull(destination, "destination is null");

			foreach (Field field in source.Fields)
			{
				if (field.IsBlobField)
				{
					var str = field.Value;
					if (str.Length > 38)
						str = str.Substring(0, 38);
					var guid = MainUtil.GetGuid(str, Guid.Empty);
					if (!(guid == Guid.Empty))
					{
						var blobStream = ItemManager.GetBlobStream(guid, ProxyManager.GetRealDatabase(source));
						if (blobStream != null)
						{
							using (blobStream)
								ItemManager.SetBlobStream(blobStream, guid, ProxyManager.GetRealDatabase(destination));
						}
					}
				}
			}

			if (processChildren)
			{
				foreach (Item child in source.Children)
				{
					if (child != null)
					{
						TransferMediaItemBlob(child, destination, true);
					}
				}
			}
		}

		#endregion
	}
}
