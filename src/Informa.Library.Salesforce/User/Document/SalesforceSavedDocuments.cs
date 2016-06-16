using Informa.Library.User.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Services.Article;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceSavedDocuments : IFindSavedDocuments, ISaveDocument, IRemoveDocument
	{
        protected readonly ISalesforceServiceContext Service;
        protected readonly ITextTranslator TextTranslator;
	    protected readonly IArticleService ArticleService;
	    protected readonly ISitecoreService SitecoreService;

        protected string BadIDKey => TextTranslator.Translate("SavedDocument.BadID");
        protected string NullUserKey => TextTranslator.Translate("SavedDocument.NullUser");
        protected string RequestFailedKey => TextTranslator.Translate("SavedDocument.RequestFailed");

        public SalesforceSavedDocuments(
            ISalesforceServiceContext service,
            ITextTranslator textTranslator, 
			IArticleService articleService, 
			ISitecoreService sitecoreService)
        {
            Service = service;
            TextTranslator = textTranslator;
	        ArticleService = articleService;
	        SitecoreService = sitecoreService;
        }

		public IEnumerable<ISavedDocument> Find(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				return Enumerable.Empty<ISavedDocument>();
			}

			var response = Service.Execute(s => s.querySavedDocuments(username));

			if (!response.IsSuccess() || response.savedDocuments == null)
			{
				return Enumerable.Empty<ISavedDocument>();
			}

			var savedDocuments = response.savedDocuments.Select(sd => new SavedDocument()
			{
				SaveDate = (sd.saveDateSpecified) ? sd.saveDate.Value : DateTime.Now,
				DocumentId = sd.documentId,
				Name = sd.name,
				Description = GetArticleDescription(sd.documentId)
			});

			return savedDocuments;
		}

	    private string GetArticleDescription(string documentId)
	    {
		    Guid id;
		    Guid.TryParse(documentId, out id);
		    var article = SitecoreService.GetItem<IArticle>(id);
		    return ArticleService.GetArticlePublicationName(article);
	    }

        public ISavedDocumentWriteResult Remove(string username, string documentId)
        {
            if (string.IsNullOrEmpty(username))
            {
                return WriteErrorResult(NullUserKey);
            }

            Guid newDID;
            if (!Guid.TryParse(documentId, out newDID)) { 
                return WriteErrorResult(BadIDKey);
            }

            var response = Service.Execute(s => s.deleteSavedDocument(username, newDID.ToString("D")));

            if (!response.IsSuccess())
            {
                return WriteErrorResult(response.errors?.First()?.message ?? RequestFailedKey);
            }

            return new SavedDocumentWriteResult
            {
                Success = true,
                Message = string.Empty
            };
        }

        public ISavedDocumentWriteResult Save(string username, string documentName, string documentDescription, string documentId)
        {
            if (string.IsNullOrEmpty(username))
            {
                return WriteErrorResult(NullUserKey);
            }

            Guid newDID;
            if (!Guid.TryParse(documentId, out newDID))
            {
                return WriteErrorResult(BadIDKey);
            }

            EBI_SavedDocument d = new EBI_SavedDocument();
            d.name = documentName;
            d.description = documentDescription;
            d.documentId = newDID.ToString("D");
            
            var response = Service.Execute(s => s.createSavedDocument(d, username));

            if (!response.IsSuccess())
            {
                return WriteErrorResult(response.errors?.First()?.message ?? RequestFailedKey);
            }

            return new SavedDocumentWriteResult
            {
                Success = true,
                Message = string.Empty
            };
        }

        public ISavedDocumentWriteResult WriteErrorResult(string message)
        {
            return new SavedDocumentWriteResult
            {
                Success = false,
                Message = message
            };
        }
	}
}
