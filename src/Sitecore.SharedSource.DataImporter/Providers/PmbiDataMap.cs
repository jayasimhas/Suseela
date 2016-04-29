using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Logger;

namespace Sitecore.SharedSource.DataImporter.Providers
{
	public class PmbiDataMap : SitecoreDataMap
	{
		public PmbiDataMap(Database db, string connectionString, Item importItem, ILogger l) : base(db, connectionString, importItem, l)
		{
		}

		/// <summary>
		/// Get 
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

		protected void GetArticles(Item startItem, IList<Item> list)
		{
			var parent = startItem;	
			// Root is null		
			if (parent == null)
			{
				return;
			}
			// Add root to list
			list.Add(parent);
			// The last node in the branch
			if (!parent.HasChildren)
			{
				return;
			}
			// Recursively get children
			var count = parent.Children.Count;
            for (int i = 0; i < count; i++)
			{
				GetArticles(parent.Children[i], list);
			}
		}
	}
}
