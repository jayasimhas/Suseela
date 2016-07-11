using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Informa.Models.FactoryInterface;

namespace Informa.Web.ViewModels.Articles
{
	public class ArticleListItemModel : IListableViewModel
	{
		public bool DisplayImage { get; set; }
		public string ListableAuthorByLine { get; set; }
		public DateTime ListableDate { get; set; }
		public string ListableImage { get; set; }
		public string ListableSummary { get; set; }
		public string ListableTitle { get; set; }
		public string ListablePublication { get; set; }
		public virtual IEnumerable<ILinkable> ListableTopics { get; set; }
		public string ListableType { get; set; }
		public virtual Link ListableUrl { get; set; }
		
		#region Implementation of ILinkable

		public string LinkableText { get; set; }
		public string LinkableUrl { get; set; }
		
		#endregion

        public bool IsUserAuthenticated { get; set; }
        public bool IsArticleBookmarked { get; set; }
        public string BookmarkText { get; set; }
        public string BookmarkedText { get; set; }
		public Guid ID { get; set; }
		public string PageTitle { get; set; }
	}
}