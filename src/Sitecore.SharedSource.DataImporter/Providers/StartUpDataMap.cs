using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Informa.Library.Article.Search;
using Informa.Library.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Logger;
using Sitecore.ContentSearch.Linq;

namespace Sitecore.SharedSource.DataImporter.Providers
{
	public class StartUpDataMap : PmbiDataMap
	{
		private const string InVitroDiagnosticsId = "{901D7D50-0DE0-43BB-B23C-DFD1CF36821C}";
		private const string MedicalDevicesId = "{2ABD34FD-532B-429E-A380-C59439E16AA2}";
		private const string BiopharmaceuticalsId = "{E77953E2-B66F-419A-B042-E99872B33D7F}";
		private const string Industries = "Industries";

		public StartUpDataMap(Database db, string connectionString, Item importItem, ILogger l) : base(db, connectionString, importItem, l)
		{
			//var parent = importItem.Parent.Name == "Articles" ? importItem.Parent.Parent.Name : importItem.Parent.Name;
			//if (parent == "Scrip")
			//{
			//	ArticleNumber = (int)GetScripLastArticleNumber("{3818C47E-4B75-4305-8F01-AB994150A1B0}", importItem);
			//}
		}

		public override IEnumerable<object> GetImportData(string site, string channel)
		{
			var parent = ImportItem.Parent.Name == "Articles" ? ImportItem.Parent.Parent.Name : ImportItem.Parent.Name;
			var startItem = FromDB.GetItem(StartPath);
			var articles = Enumerable.Empty<Item>();
			if (startItem != null)
			{
				var sw = new Stopwatch();
				sw.Start();

				if (parent == "Medtech Insight")
				{
					articles =
						startItem.Axes.GetDescendants()
							.Where(i =>
								i.TemplateID.ToString() == TemplateId 
								&& (string.IsNullOrWhiteSpace(Year) || i.Fields[ArticleDate].Value.Contains(Year))
								&& !PublisherSpotlights.Contains(i.Fields[ArticleCategory].Value)
								&& GetImportToFolder(i.Fields[Industries].Value)
							);
				}
				else
				{
					articles =
						startItem.Axes.GetDescendants()
							.Where(i =>
							i.TemplateID.ToString() == TemplateId
								&& (string.IsNullOrWhiteSpace(Year) || i.Fields[ArticleDate].Value.Contains(Year))
								&& !PublisherSpotlights.Contains(i.Fields[ArticleCategory].Value)
								&& !GetImportToFolder(i.Fields[Industries].Value)
							);
				}
				sw.Stop();
				Logger.Log("Performance Statistic-(Sitecore.SharedSource.DataImporter.Providers.PmbiDataMap.GetImportData)", $"Used {sw.Elapsed.TotalSeconds} to Find matches");
			}
			return articles;
		}

		private long GetScripLastArticleNumber(string publicationGuid, Item importItem)
		{
			var searchContextFactory = new ProviderSearchContextFactory();
            using (var context = searchContextFactory.Create(Informa.Library.Utilities.References.Constants.MasterDb))
			{
				var publicationItem = importItem.Database.GetItem(new ID(publicationGuid));
				if (publicationItem != null)
				{
					var query = context.GetQueryable<ArticleSearchResultItem>()
						.Filter(i => i.TemplateId == IArticleConstants.TemplateId)
						.Filter(i => i.PublicationTitle == publicationItem.Fields["Publication Name"].Value)
						.OrderByDescending(i => i.ArticleIntegerNumber)
						.Take(1);
					var result = query.GetResults();
					return result?.Hits?.FirstOrDefault()?.Document.ArticleIntegerNumber ?? 0;
				}
				return 0;
			}
		}

		/// <summary>
		/// Determine which folder should this article move to
		/// </summary>
		/// <param name="value">"Industries" field value</param>
		/// <returns>True: Medtech Insight; False: Scrip</returns>
		private bool GetImportToFolder(string value)
		{
			var imCheckList = GetAllTaxonomy(InVitroDiagnosticsId);
			imCheckList.UnionWith(GetAllTaxonomy(MedicalDevicesId));
			var biopharmaCheckList = GetAllTaxonomy(BiopharmaceuticalsId);

			var strs = value.Split('|');
			var inIVDorMD = strs.Any(str => imCheckList.Contains(str));
			var inBioPharma = strs.Any(str => biopharmaCheckList.Contains(str));

			// Any article with either (In Vitro Diagnostics or Medical Devices) and without Biopharmaceuticals ==> move to Medtech Insight
			if (inIVDorMD && inBioPharma == false)
			{
				return true;
			}

			return false;
		}

		private HashSet<string> GetAllTaxonomy(string id)
		{
			var startItem = FromDB.GetItem(new ID(id));
			var result = new HashSet<string> { id };
			if (startItem != null)
			{
				var descendants = startItem.Axes.GetDescendants().Select(i => i.ID.ToString()).ToList();
				if (descendants.Any())
				{
					result.UnionWith(descendants);
				}
			}
			return result;
		}
	}
}
