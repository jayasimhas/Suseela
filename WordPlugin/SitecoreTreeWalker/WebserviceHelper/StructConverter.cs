using System.Linq;
using Informa.Web.Areas.Account.Models;
using SitecoreTreeWalker.SitecoreServer;

namespace SitecoreTreeWalker.WebserviceHelper
{
	public class StructConverter
	{
		
		public ArticleStruct GetServerStruct(SitecoreTree.ArticleStruct articleStruct)
		{
			var articleDetails = new ArticleStruct();
			articleDetails.Publication = articleStruct.Publication;
			articleDetails.ArticleNumber = articleStruct.ArticleNumber;
			articleDetails.Authors = articleStruct.Authors.Select(r => new StaffStruct { ID = r.ID, Name = r.Name, Publications = r.Publications }).ToArray();
			articleDetails.RelatedInlineArticles = articleStruct.RelatedInlineArticles;
			articleDetails.WebPublicationDate = articleStruct.WebPublicationDate;
			articleDetails.PrintPublicationDate = articleStruct.PrintPublicationDate;
			articleDetails.NotesToEditorial = articleStruct.NotesToEditorial;
			articleDetails.Taxonomy = articleStruct.Taxonomy.Select(t => new TaxonomyStruct { Name = t.Name, ID = t.ID, Section = t.Section }).ToArray();
			articleDetails.Title = articleStruct.Title;
			articleDetails.RelatedArticles = articleStruct.RelatedArticles;
			articleDetails.ReferencedDeals = articleStruct.ReferencedDeals;
			articleDetails.Subtitle = articleStruct.Subtitle;
			articleDetails.Summary = articleStruct.Summary;
			articleDetails.CommandID = articleStruct.CommandID;
			articleDetails.WordCount = articleStruct.WordCount;
			articleDetails.SupportingDocumentPaths = articleStruct.SupportingDocumentPaths;
			//articleDetails.ArticleSpecificNotifications = articleStruct.ArticleSpecificNotifications.Select(n => new StaffStruct{Name = n.Name,ID = n.ID,Publications = n.Publications,}).ToArray();

			articleDetails.Embargoed = articleStruct.Embargoed;

			return articleDetails;
		}
		

		//TODO - Need to fix this 
		/*
		public ArticleStruct GetServerStruct(SitecoreTree.ArticleStruct articleStruct)
		{
			var articleDetails = new ArticleStruct();
			articleDetails.Publication = articleStruct.Publication;
			articleDetails.ArticleNumber = articleStruct.ArticleNumber;
			articleDetails.Authors = articleStruct.Authors.Select(r => new StaffStruct {ID = r.ID,Name = r.Name,Publications = r.Publications}).ToArray();
			articleDetails.RelatedInlineArticles = articleStruct.RelatedInlineArticles;
			articleDetails.WebPublicationDate = articleStruct.WebPublicationDate;
			articleDetails.PrintPublicationDate = articleStruct.PrintPublicationDate;
			articleDetails.NotesToEditorial = articleStruct.NotesToEditorial;			
			articleDetails.Taxonomy = articleStruct.Taxonomy.Select(t => new TaxonomyStruct { Name = t.Name,ID = t.ID,Section = t.Section}).ToArray();
			articleDetails.Title = articleStruct.Title;			
			articleDetails.RelatedArticles = articleStruct.RelatedArticles;
			articleDetails.ReferencedDeals = articleStruct.ReferencedDeals;
			articleDetails.Subtitle = articleStruct.Subtitle;
			articleDetails.Summary = articleStruct.Summary;
			articleDetails.CommandID = articleStruct.CommandID;
			articleDetails.WordCount = articleStruct.WordCount;
			articleDetails.SupportingDocumentPaths = articleStruct.SupportingDocumentPaths;			
			//articleDetails.ArticleSpecificNotifications = articleStruct.ArticleSpecificNotifications.Select(n => new StaffStruct{Name = n.Name,ID = n.ID,Publications = n.Publications,}).ToArray();

			articleDetails.Embargoed = articleStruct.Embargoed;

			return articleDetails;
		}
		*/
	}
}
