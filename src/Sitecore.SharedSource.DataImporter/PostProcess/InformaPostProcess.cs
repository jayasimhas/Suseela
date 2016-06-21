using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Informa.Models.DCD;
using Sitecore.Data.Items;
using Sitecore.Jobs;
using Sitecore.SecurityModel;

namespace Sitecore.SharedSource.DataImporter.PostProcess {
    public class InformaPostProcess {


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

        public IEnumerable<Item> GetAllArticles() { 

            List<Item> l = new List<Item>();

            var db = Sitecore.Context.ContentDatabase;

            //get all sites
            var siteInfos = Sitecore.Configuration.Factory.GetSiteInfoList();
            foreach (SiteInfo si in siteInfos)
            {
                var articleNode = db.GetItem($"{si.ContentStartItem}/articles");
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

            string pattern1 = @"<a>\[A#(\d)*\]<\/a>";
            string pattern2 = string.Empty; 
            //temporary pattern 2: @"\(<a( href=\"[a-zA-Z0-9%&=_?/:\-]*\")*( target=\"[a-z_]*\")*>\[A#(\d)*\]<\/a>\)";
            //pattern 2 should match this: "(<a href="http://auth.pharmamedtechbi.com/?sc_itemid=%7b2f6f19c3-cf09-4c7b-80ec-bde580d43efa%7d&sc_mode=preview&sc_lang=en" target="_new">[A#00150511012]</a>)";
            
            body = ReplacePattern(pattern1, body);
            body = ReplacePattern(pattern2, body);
            summary = ReplacePattern(pattern1, summary);
            summary = ReplacePattern(pattern2, summary);

            using (new SecurityDisabler()) {
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
            foreach (Match match in regex.Matches(text)) {
                var newArticleNumber = GetNewArticleNumber();
                if (!string.IsNullOrEmpty(newArticleNumber))
                    text = text.Replace(match.Value, $"(<a>[A#{newArticleNumber}]</a>)");
            }
            return text;
        }

        public string GetNewArticleNumber()
        {
            return string.Empty;
        }
    }
}
