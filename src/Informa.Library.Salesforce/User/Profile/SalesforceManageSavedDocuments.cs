using Informa.Library.User.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Globalization;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Authentication;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceManageSavedDocuments : IManageSavedDocuments
    {
        protected readonly ISalesforceServiceContext Service;
        protected readonly ITextTranslator TextTranslator;
        
        protected string NullUserKey => TextTranslator.Translate("SavedDocument.NullUser");
        protected string RequestFailedKey => TextTranslator.Translate("SavedDocument.RequestFailed");

        public SalesforceManageSavedDocuments(
            ISalesforceServiceContext service,
            ITextTranslator textTranslator)
        {
            Service = service;
            TextTranslator = textTranslator;
        }

        public ISavedDocumentReadResult QueryItems(IAuthenticatedUser user)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                return ReadErrorResult;
            }

            var response = Service.Execute(s => s.querySavedDocuments(user.Email));

            if (!response.IsSuccess())
            {
                return ReadErrorResult;
            }

            return new SavedDocumentReadResult
            {
                Success = true,
                SavedDocuments = (response.savedDocuments != null && response.savedDocuments.Any())
                    ? response.savedDocuments.Select(a => new SavedDocument()
                    {
                        SaveDate = (a.saveDateSpecified) ? a.saveDate.Value : DateTime.Now,
                        DocumentId = a.documentId,
                        Name = a.name,
                        Description = a.description
                    })
                    : Enumerable.Empty<ISavedDocument>()
            };
        }

        public bool IsBookmarked(IAuthenticatedUser user, Guid g)
        {
            string gID = g.ToString("B").ToUpper();
            ISavedDocumentReadResult result = QueryItems(user);
            return result.Success && result.SavedDocuments.Any(b => b.DocumentId.Equals(gID));
        }

        public ISavedDocumentWriteResult RemoveItem(IAuthenticatedUser user, string documentId)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                return WriteErrorResult(NullUserKey);
            }

            var response = Service.Execute(s => s.deleteSavedDocument(user.Email, documentId));

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

        public ISavedDocumentWriteResult SaveItem(IAuthenticatedUser user, string documentName, string documentDescription, string documentId)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                return WriteErrorResult(NullUserKey);
            }

            EBI_SavedDocument d = new EBI_SavedDocument();
            d.name = documentName;
            d.description = documentDescription;
            d.documentId = documentId;
            
            var response = Service.Execute(s => s.createSavedDocument(d, user.Email));

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

        public ISavedDocumentReadResult ReadErrorResult => new SavedDocumentReadResult
        {
            Success = false,
            SavedDocuments = Enumerable.Empty<ISavedDocument>()
        };

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
