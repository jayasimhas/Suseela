﻿using System;
using System.Collections.Generic;
using Elsevier.Library.CustomItems.Global.ArticleCategory;
using Elsevier.Library.CustomItems.Global.ArticleSizes;
using Elsevier.Library.CustomItems.Global.Staff;
using System.Linq;
using Elsevier.Library.CustomItems.Publication.General;
using Elsevier.Library.Metadata;
using Elsevier.Library.Reference;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Utilities.Extensions.System.Collections.Generic;
using ArticleItem = Elsevier.Library.CustomItems.Publication.General.ArticleItem;

namespace Elsevier.Web.VWB.Report
{
	public class ArticleItemWrapper
	{
		/// <summary>
		/// For usage in VWB when organizing articles by issue
		/// </summary>
		public bool IsFirstArticleInIssue;
		/// <summary>
		/// For usage in VWB when organizing articles by issue
		/// to get the count of articles. Only the "first article
		/// in issue" needs to have this value set
		/// </summary>
		public int ArticleCountInIssue;
		/// <summary>
		/// Value populated if IsFirstArticleInIssue
		/// </summary>
		public int NumArticlesInIssue;
		public string ArticleNumber;
		public Guid IssueGuid = Guid.Empty;
		public string Title;
		public DateTime ArticleCreation;
		public DateTime IssueDate;
		public IEnumerable<string> Authors;
		public ArticleItem InnerItem;
		public IEnumerable<string> Editors;
		public string IssueDateValue;
		public string ArticleSize;
		public string PreviewUrl;
		public DateTime SAPDateTime;
		public DateTime WebPublicationDateTime;
		public string HomePageCategory;

	    private string _articleLengthEstimate;
	    public string ArticleLengthEstimate
	    {
	        get
	        {
	            if (string.IsNullOrEmpty(_articleLengthEstimate))
	            {
                    _articleLengthEstimate = _estimator.GetArticleLengthEstimate(InnerItem.ID.ToGuid()).ToString();
	            }
	            return _articleLengthEstimate;
	        }
	    }

	    public string EmailPriority;

	    public string MediaType;

	    public string ContentType;

	    public string TaxonomyString;

	    public List<string> SidebarArticleNumbers = new List<string>();
		public string NotesToEditorial;
		public string NotesToProduction;
		public DateTime EmbargoDate;
		public Boolean Embargoed;
		public bool IsFirstArticleInCategory;
		public bool ArticlesInCategoryShouldBeGrouped;
		public Guid ArticleCategoryGuid;
		public int NumArticlesInCategory;

		public ArticleItemWrapper(ArticleItem	articleItem)
			: this(articleItem, new ArticleLengthEstimator())
		{}

		public ArticleItemWrapper(ArticleItem articleItem, ArticleLengthEstimator estimator)
		{
			_estimator = estimator; 

            IArticle article = new SitecoreContext().GetItem<IArticle>(articleItem.ID.ToString());
		    Item articleBaseItem = Sitecore.Context.Database.GetItem(articleItem.ID.ToString());

		    InnerItem = articleBaseItem;

            //TODO
            PreviewUrl = "/";
			ArticleNumber = article.Article_Number;
		    if (ArticleNumber == null)
		    {
		        ArticleNumber = "";

		    }
			//Title = article.Title+":"+ article._Path;
			Title = article.Title;

		    if (article.Authors != null)
		    {
                Authors = article.Authors.Select(a => a.First_Name + " " + a.Last_Name);
            }

			ArticleCreation = article.Created_Date;
			if(ArticleCreation == DateTime.MinValue)
			{
				ArticleCreation = articleBaseItem.Statistics.Created;
			}

            IssueDate = article.Actual_Publish_Date;

			IssueDateValue = IssueDate.ToString(Constants.VwbDateTimeFormatWithDashes);
			
            ArticleSize = article.Word_Count;

            //TODO
			//lArticleItem.ChildArticles.ListItems.Select(i => (ArticleItem)i).ForEach(a => SidebarArticleNumbers.Add(a.ArticleNumber.Text));
			SidebarArticleNumbers.Sort();

		    TaxonomyString = "";
		    string sep = "";
            foreach (var taxonomy in article.Taxonomies)
            {
                TaxonomyString += sep + taxonomy.Item_Name;
                sep = ",";
            }

		    ContentType = "";

            if (article.Content_Type != null)
		    {
		        ContentType = article.Content_Type.Item_Name;
		    }

		    MediaType = "";
		    if (article.Media_Type != null)
		    {
                MediaType = article.Media_Type.Item_Name;
		    }

		    EmailPriority = "";
            EmailPriority = article.Sort_Order.ToString();
		    
            NotesToEditorial = article.Editorial_Notes;


            Embargoed = article.Embargoed;
            EmbargoDate = articleBaseItem.Publishing.PublishDate;      

		    SAPDateTime = article.Planned_Publish_Date;
		    WebPublicationDateTime = article.Actual_Publish_Date;
		}

		public const string NE = "N/E";
		public const string NA = "N/A";

		public static IEnumerable<ArticleItemWrapper> GetArticleItemProxies(IEnumerable<ArticleItem> articles)
		{
			return articles.Select(a => new ArticleItemWrapper(a));
		}

		protected ArticleLengthEstimator _estimator;
	}
}