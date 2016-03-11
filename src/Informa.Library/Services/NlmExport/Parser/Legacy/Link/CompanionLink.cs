using Glass.Mapper.Sc;
using Informa.Library.Article.Service;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Link
{
    public class CompanionLink : AssetLinkBase
    {
        protected readonly IArticleSearchService SearchService = ArticleSearchService.Instance;

        public override string GetLink(string linkId)
        {
            var item = SearchService.GetArticleByNumber(linkId);

            if (item == null)
            {
                return string.Empty;
            }

            return string.Format("{1}/a/{0}", linkId, BaseUrl);
        }

        public override string GetLinkText(string linkId)
        {
            string textToReturn = string.Empty;

            ArticleItem item = SearchService.GetArticleByNumber(linkId);

            if (item == null)
            {
                return string.Empty;
            }

            textToReturn += "\"" + item._Name + "\" ";

            using (var db = new SitecoreService(Constants.MasterDb))
            {
                var publication = db.GetItem<ISite_Root>(item.Publication);
                textToReturn += "\"" + publication?.Publication_Name + "\" ";
            }

            // Not sure that we have an issue anymore
            //var issue = item.GetIssue();
            //textToReturn += issue.FormattedDate;

            return textToReturn;
        }

        public override string LinkType
        {
            get { return "pii"; }
        }
    }
}
