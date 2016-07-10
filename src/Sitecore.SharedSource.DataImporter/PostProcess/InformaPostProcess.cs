using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Glass.Mapper.Sc.Configuration.Fluent;
using Informa.Library.Search.Results;
using Informa.Models.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Jobs;
using Sitecore.SecurityModel;
using Sitecore.SharedSource.DataImporter.Logger;
using Sitecore.SharedSource.DataImporter.Providers;

namespace Sitecore.SharedSource.DataImporter.PostProcess
{
	public class InformaPostProcess
	{
        public List<string> TextFields = new List<string>()
        {
            "Body",
            "Summary"
        };

        public List<string> LinkPatterns = new List<string>()
        {
            @"\(<a[^>]*?>\[A#(\d*)\]</a>\)",
            @"<a[^>]*?>\[A#(\d*)\]</a>",
            @"\[A#(\d*)\]"
        };

        public string ReferenceField = "Referenced Articles";

	    public string NewLinkFormat = "[A#{0}]";

	    protected ILogger Log;

        public InformaPostProcess(ILogger l)
        {
            Log = l;
        }

		public void UpdateArticleReferences()
		{
			if (Sitecore.Context.Job != null)
				Sitecore.Context.Job.Options.Priority = ThreadPriority.Highest;

			IEnumerable<Item> articleItems;

			try
			{
				articleItems = GetAllArticles();
            } catch (Exception ex) {
                Log.Log(string.Empty, ex.ToString(), ProcessStatus.ReferenceError, string.Empty);

                return;
            }

            int totalLines = articleItems.Count();
			if (Sitecore.Context.Job != null)
				Sitecore.Context.Job.Status.Total = totalLines;

			int line = 1;
			foreach (Item a in articleItems)
			{
                try { 
				    ReplaceArticleRef(a);
                } catch (Exception ex) {
                    Log.Log(a.Paths.FullPath, ex.ToString(), ProcessStatus.ReferenceError, string.Empty);
                }

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
            foreach (string f in TextFields)
            {
                string textValue = a[f];
                string referenceValue = a[ReferenceField];
                if (string.IsNullOrEmpty(textValue))
                    continue;

                KeyValuePair<string, string> newValues = UpdateArticleReferences(
                    LinkPatterns, 
                    textValue, 
                    string.IsNullOrEmpty(referenceValue) 
                        ? string.Empty 
                        : referenceValue, 
                    GetMatchingArticle);

                if(!string.IsNullOrEmpty(newValues.Key))
                    EditItem(a, f, newValues.Key);
                if (!string.IsNullOrEmpty(newValues.Value))
                    EditItem(a, ReferenceField, newValues.Value);
            }
        }

		public KeyValuePair<string, string> UpdateArticleReferences(
            List<string> patterns, 
            string textValue, 
            string referenceValue, 
            Func<Match, ImportSearchResultItem> findMatchingArticle)
		{
		    if (string.IsNullOrEmpty(textValue))
		        return new KeyValuePair<string, string>(string.Empty, string.Empty);

            string text = textValue;
		    string references = (string.IsNullOrEmpty(referenceValue)) 
                ? string.Empty 
                : referenceValue;

            foreach (string p in patterns) { 
			    Regex regex = new Regex(p);
			    foreach (Match match in regex.Matches(text))
			    {
                    var matchedArticle = findMatchingArticle(match);
			        if (matchedArticle == null)
			            continue;

			        var newArticleNumber = matchedArticle.NewArticleNumber;
			        if (string.IsNullOrEmpty(newArticleNumber))
			            continue;

					text = text.Replace(match.Value, string.Format(NewLinkFormat, newArticleNumber));

                    string referenceID = matchedArticle.ItemId.ToString();
                    if (references.Contains(referenceID))
                        continue;

                    references = (references.Length > 0)
                        ? $"{references}|{referenceID}"
                        : referenceID;
			    }
            }

		    return new KeyValuePair<string, string>(text, references);
		}
        
	    public void EditItem(Item a, string fieldName, string fieldValue)
	    {
            using (new SecurityDisabler()) {
                using (new EditContext(a)) {
                    a[fieldName] = fieldValue;
                }
            }
        }

		public ImportSearchResultItem GetMatchingArticle(Match match)
		{
			if (!match.Success || string.IsNullOrEmpty(match.Groups[1].Value)) return null;

			using (var context = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
			{
				var query = context.GetQueryable<ImportSearchResultItem>();
				query = query.Where(x => x.LegacyArticleNumber == match.Groups[1].Value);
				query.Page(1, 1);

				var results = query.GetResults();

				return results?.Hits?.Select(h => h?.Document).FirstOrDefault();
			}
		}
	}

	public class ImportSearchResultItem : SearchResultItem
	{
		[IndexField(IArticleConstants.Legacy_Article_NumberFieldName)]
		public string LegacyArticleNumber { get; set; }

		[IndexField(IArticleConstants.Article_NumberFieldName)]
		public string NewArticleNumber { get; set; }

        public ID SitecoreID { get; set; }
    }
}
