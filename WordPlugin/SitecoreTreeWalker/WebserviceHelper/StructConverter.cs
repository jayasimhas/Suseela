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
			articleDetails.Title = articleStruct.Title;

			articleDetails.Label = articleStruct.Label;
			articleDetails.WebPublicationDate = articleStruct.WebPublicationDate;
			articleDetails.PrintPublicationDate = articleStruct.PrintPublicationDate;
			articleDetails.Embargoed = articleStruct.Embargoed;
			articleDetails.MediaType = articleStruct.MediaType;
			articleDetails.Authors = articleStruct.Authors.Select(r => new WordPluginModel.StaffStruct { ID = r.ID, Name = r.Name, Publications = r.Publications }).ToList();
			articleDetails.NotesToEditorial = articleStruct.NotesToEditorial;

			articleDetails.RelatedInlineArticles = articleStruct.RelatedInlineArticles;

			//TODO - Workflow

			articleDetails.FeaturedImage = articleStruct.FeaturedImage;
			articleDetails.FeaturedImageCaption = articleStruct.FeaturedImageCaption;
			articleDetails.FeaturedImageSource = articleStruct.FeaturedImageSource;
			articleDetails.Taxonomoy = articleStruct.Taxonomoy;
			articleDetails.RelatedArticles = articleStruct.RelatedArticles;
			articleDetails.ReferencedDeals = articleStruct.ReferencedDeals;
			articleDetails.Subtitle = articleStruct.Subtitle;
			articleDetails.Summary = articleStruct.Summary;
			articleDetails.CommandID = articleStruct.CommandID;
			articleDetails.WordCount = articleStruct.WordCount;
			articleDetails.SupportingDocumentPaths = articleStruct.SupportingDocumentPaths;
			//articleDetails.ArticleSpecificNotifications = articleStruct.ArticleSpecificNotifications.Select(n => new StaffStruct{Name = n.Name,ID = n.ID,Publications = n.Publications,}).ToArray();

			return articleDetails;
		}
	}
}
