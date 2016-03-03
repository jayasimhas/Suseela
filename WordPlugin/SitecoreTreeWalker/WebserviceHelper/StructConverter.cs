using System.Linq;
using PluginModels;

namespace InformaSitecoreWord.WebserviceHelper
{
	public class StructConverter
	{

		public ArticleStruct GetServerStruct(ArticleStruct articleStruct)
		{
			var articleDetails = new ArticleStruct
			{
				Publication = articleStruct.Publication,
				ArticleNumber = articleStruct.ArticleNumber,
				Title = articleStruct.Title,
				Label = articleStruct.Label,
				WebPublicationDate = articleStruct.WebPublicationDate,
				PrintPublicationDate = articleStruct.PrintPublicationDate,
				Embargoed = articleStruct.Embargoed,
				MediaType = articleStruct.MediaType,
				Authors =
					articleStruct.Authors.Select(r => new StaffStruct { ID = r.ID, Name = r.Name, Publications = r.Publications })
						.ToList(),
				NotesToEditorial = articleStruct.NotesToEditorial,
				RelatedInlineArticles = articleStruct.RelatedInlineArticles,
				ArticleWorkflowState = articleStruct.ArticleWorkflowState,
				FeaturedImage = articleStruct.FeaturedImage,
				FeaturedImageCaption = articleStruct.FeaturedImageCaption,
				FeaturedImageSource = articleStruct.FeaturedImageSource,
				Taxonomoy = articleStruct.Taxonomoy,
				RelatedArticles = articleStruct.RelatedArticles,
				ReferencedDeals = articleStruct.ReferencedDeals,
				Subtitle = articleStruct.Subtitle,
				Summary = articleStruct.Summary,
				CommandID = articleStruct.CommandID,
				WordCount = articleStruct.WordCount,
				SupportingDocumentPaths = articleStruct.SupportingDocumentPaths,
				ArticleSpecificNotifications = articleStruct.ArticleSpecificNotifications.
					Select(n => new StaffStruct { Name = n.Name, ID = n.ID, }).ToList()
			};

			return articleDetails;
		}
	}
}
