using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Publication;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Extensions;
using Sitecore.SharedSource.DataImporter.Logger;
using Sitecore.SharedSource.DataImporter.Mappings.Properties;

namespace Sitecore.SharedSource.DataImporter.Providers
{
	public class PmbiDataMap : SitecoreDataMap
	{
		public string ArticleNumberPrefix { get; set; }
		public int ArticleNumber { get; set; }
		public PmbiDataMap(Database db, string connectionString, Item importItem, ILogger l) : base(db, connectionString, importItem, l)
		{
			ArticleNumberPrefix = GetArticleNumberPrefix(importItem);
			var val = importItem.Fields["Last Article Number"].Value;
			if (string.IsNullOrWhiteSpace(val))
			{
				ArticleNumber = 0;
			}
			else
			{
				int articleNumber;
				int.TryParse(importItem.Fields["Last Article Number"].Value, out articleNumber);
				ArticleNumber = articleNumber;
			}
		}

		/// <summary>
		/// Get Import items from specified start path, template id and year
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<object> GetImportData()
		{
			var startItem = Sitecore.Data.Database.GetDatabase("pmbiContent").GetItem(StartPath);
			var articles = Enumerable.Empty<Item>();
			if (startItem != null)
			{
				var sw = new Stopwatch();
				sw.Start();
				if (!string.IsNullOrWhiteSpace(Year))
				{
					articles =
						startItem.Axes.GetDescendants()
							.Where(i => i.TemplateID.ToString() == TemplateId && i.Fields["Article Date"].Value.Contains(Year));
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
				ImportItem.Fields["Last Article Number"].Value = ArticleNumber.ToString();
			}
		}

		public void SetRelatedArticles()
		{
			var articlesToSet = GetImportedArticles();
			var repo = new ArticleMappingRepository(new ArticleMappingContext());

			foreach (var item in articlesToSet)
			{
				var oldValue = item.Fields["Related Articles"].Value;
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
							item.Fields["Related Articles"].Value = transformedValue;
						}
					}				
				}                								
			}
		}

		public void AddOrUpdateMapping(Item newItem)
		{
			var legacyId = new ID(newItem.Fields["Legacy Sitecore ID"].Value).Guid;

			var repo = new ArticleMappingRepository(new ArticleMappingContext());
			if (repo.Exist(legacyId))
			{
				repo.Update(newItem.ID.Guid, newItem.Fields["Article Number"].Value, legacyId,
					newItem.Fields["Legacy Article Number"].Value);
			}
			else
			{
				repo.Insert(newItem.ID.Guid, newItem.Fields["Article Number"].Value, legacyId,
					newItem.Fields["Legacy Article Number"].Value);
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
					.Where(i => i.TemplateID.ToString() == templateId && i.Fields["Created Date"].Value.Contains(Year));
			return articles;
		}

		#endregion
	}
}
