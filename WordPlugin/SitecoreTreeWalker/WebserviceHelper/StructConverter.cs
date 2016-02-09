using System;
using System.Linq;
using Informa.Web.Areas.Account.Models;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls;

namespace SitecoreTreeWalker.WebserviceHelper
{
	public class StructConverter
	{
		
		public WordPluginModel.ArticleStruct GetServerStruct(WordPluginModel.ArticleStruct articleStruct)
		{
			var articleDetails = new WordPluginModel.ArticleStruct();
			articleDetails.Publication = articleStruct.Publication;
			articleDetails.ArticleNumber = articleStruct.ArticleNumber;
			articleDetails.Authors = articleStruct.Authors.Select(r => new WordPluginModel.StaffStruct { ID = r.ID, Name = r.Name, Publications = r.Publications }).ToList();
			articleDetails.RelatedInlineArticles = articleStruct.RelatedInlineArticles;
			articleDetails.WebPublicationDate = articleStruct.WebPublicationDate;
			articleDetails.PrintPublicationDate = articleStruct.PrintPublicationDate;
			articleDetails.NotesToEditorial = articleStruct.NotesToEditorial;
			//TODO - add article taxonomy to the Struct.
			//if (articleStruct.Taxonomoy.Any())
			{
				//articleDetails.Taxonomoy = articleStruct.Taxonomoy.Select(t => new WordPluginModel.TaxonomyStruct {Name = t.Name, ID = t.ID}).ToList();
			}
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

			articleDetails.Label = articleStruct.Label;
			articleDetails.MediaType = articleStruct.MediaType;
			//articleDetails.FeaturedImage = articleStruct.FeaturedImage;
			articleDetails.FeaturedImageCaption = articleStruct.FeaturedImageCaption;
			articleDetails.FeaturedImageSource = articleStruct.FeaturedImageSource;

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
