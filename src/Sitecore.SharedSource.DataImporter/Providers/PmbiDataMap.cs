using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Items;
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

		public void SetArticleNumber()
		{
			using (new EditContext(ImportItem, true, false))
			{
				ImportItem.Fields["Last Article Number"].Value = ArticleNumber.ToString();
			}
		}
	}
}
