using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Informa.Library.Search.Results;
using Informa.Models.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Jobs;
using Sitecore.SecurityModel;

namespace Sitecore.SharedSource.DataImporter.PostProcess
{
	public class InformaPostProcess
	{


		public InformaPostProcess()
		{

		}

		public void UpdateArticleReferences()
		{
			if (Sitecore.Context.Job != null)
				Sitecore.Context.Job.Options.Priority = ThreadPriority.Highest;

			IEnumerable<Item> articleItems;

			try
			{
				articleItems = GetAllArticles();
			}
			catch (Exception ex)
			{
				if (Sitecore.Context.Job != null)
					Sitecore.Context.Job.Status.State = JobState.Finished;

				return;
			}

			int totalLines = articleItems.Count();
			if (Sitecore.Context.Job != null)
				Sitecore.Context.Job.Status.Total = totalLines;

			int line = 1;
			foreach (Item a in articleItems)
			{
				ReplaceArticleRef(a);

				if (Sitecore.Context.Job != null)
				{
					Sitecore.Context.Job.Status.Processed = line;
					Sitecore.Context.Job.Status.Messages.Add(string.Format("Processed item {0} of {1}", line, totalLines));
				}
				line++;
			}

			if (Sitecore.Context.Job != null)
				Sitecore.Context.Job.Status.State = JobState.Finished;
		}

		public IEnumerable<Item> GetAllArticles()
		{

			List<Item> l = new List<Item>();

			var db = Sitecore.Context.ContentDatabase;

			//get all sites
			var siteInfos = Sitecore.Configuration.Factory.GetSiteInfoList();
			foreach (SiteInfo si in siteInfos)
			{
				var articleNode = db.GetItem($"{si.RootPath}{si.StartItem}/articles");
				if (articleNode == null)
					continue;

				var articles = articleNode.Axes.GetDescendants().Where(a => a.TemplateName.Equals("Article"));
				l.AddRange(articles);
			}

			return l;
		}

		public void ReplaceArticleRef(Item a)
		{
			string bodyFieldName = "Body";
			string summaryFieldName = "Summary";

			string body = a[bodyFieldName];
			string summary = a[summaryFieldName];

			string pattern1 = @"\(<a[^>]*?>[A#(\d*)\]</a>)";
			string pattern2 = @"\<a[^>]*?>[A#(\d*)\]</a>";
			string pattern3 = @"\[A#(\d*)\]";

			body = ReplacePattern(pattern1, body);
			body = ReplacePattern(pattern2, body);
			body = ReplacePattern(pattern3, body);
			summary = ReplacePattern(pattern1, summary);
			summary = ReplacePattern(pattern2, summary);
			summary = ReplacePattern(pattern3, summary);

			using (new SecurityDisabler())
			{
				using (new EditContext(a))
				{
					a[bodyFieldName] = body;
					a[summaryFieldName] = summary;
				}
			}
		}

		public string ReplacePattern(string pattern, string text)
		{
			Regex regex = new Regex(pattern);
			foreach (Match match in regex.Matches(text))
			{
				var newArticleNumber = GetNewArticleNumber(match);
				if (!string.IsNullOrEmpty(newArticleNumber))
					text = text.Replace(match.Value, $"(<a>[A#{newArticleNumber}]</a>)");
			}
			return text;
		}

		public string GetNewArticleNumber(Match match)
		{
			if (!match.Success || string.IsNullOrEmpty(match.Groups[1].Value)) return null;

			using (var context = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
			{
				var query = context.GetQueryable<ImportSearchResultItem>();
				query = query.Where(x => x.LegacyArticleNumber == match.Groups[1].Value);
				query.Page(1, 1);

				var results = query.GetResults();

				return results?.Hits?.Select(h => h?.Document?.NewArticleNumber).FirstOrDefault();
			}
		}
	}

	public class ImportSearchResultItem : SearchResultItem
	{
		[IndexField(IArticleConstants.Legacy_Article_NumberFieldName)]
		public string LegacyArticleNumber { get; set; }

		[IndexField(IArticleConstants.Article_NumberFieldName)]
		public string NewArticleNumber { get; set; }
	}
}
