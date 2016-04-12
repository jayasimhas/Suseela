using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Informa.Models.FactoryInterface;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;
using Informa.Library.Utilities.StringUtils;

namespace Informa.Web.ViewModels
{
    public class ArticleListItemModel : IListableViewModel
	{       
		public bool DisplayImage { get; set; }

        public IEnumerable<ILinkable> ListableAuthors { get; set; }
        public DateTime ListableDate { get; set; }
		public string ListableImage { get; set; }
		public string ListableSummary { get; set; }
		public string ListableTitle { get; set; }
		public string ListableByline { get; set; }
		public virtual IEnumerable<ILinkable> ListableTopics { get; set; }
		public string ListableType { get; set; }
		public virtual Link ListableUrl { get; set; }
        public string By { get; set; }

		#region Implementation of ILinkable

		public string LinkableText { get; set; }
		public string LinkableUrl { get; set; }
		public string Publication { get; set; }

        #endregion
       public string SourceArticleID { get { return Sitecore.Context.Item.ID.ToString(); } }

    }
}