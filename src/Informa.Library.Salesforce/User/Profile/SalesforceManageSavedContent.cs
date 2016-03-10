using Informa.Library.User.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Authentication;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceManageSavedContent : IManageSavedContent
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceManageSavedContent(ISalesforceServiceContext service)
        {
            Service = service;
        }

        public ISavedContentReadResult QueryItems(IAuthenticatedUser user)
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

            return new SavedContentReadResult
            {
                Success = true,
                SavedContentItems = (response.savedDocuments != null && response.savedDocuments.Any())
                    ? response.savedDocuments.Select(a => new SavedContent()
                    {
                        SaveDate = (a.saveDateSpecified) ? a.saveDate.Value : DateTime.Now,
                        DocumentId = a.documentId,
                        Name = a.name,
                        Description = a.description
                    })
                    : Enumerable.Empty<ISavedContent>()
            };
        }
        
        public ISavedContentWriteResult RemoveItem(IAuthenticatedUser user, string documentId)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                return WriteErrorResult("Null User");
            }

            var response = Service.Execute(s => s.deleteSavedDocument(user.Email, documentId));

            if (!response.IsSuccess())
            {
                return WriteErrorResult(response.errors?.First()?.message ?? "Request Failed");
            }

            return new SavedContentWriteResult
            {
                Success = true,
                Message = string.Empty
            };
        }

        public ISavedContentWriteResult SaveItem(IAuthenticatedUser user, string documentName, string documentDescription, string documentId, DateTime saveDate)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                return WriteErrorResult("Null User");
            }

            EBI_SavedDocument d = new EBI_SavedDocument();
            d.name = documentName;
            d.description = documentDescription;
            d.documentId = documentId;
            d.saveDate = saveDate;
            
            var response = Service.Execute(s => s.createSavedDocument(d, user.Email));

            if (!response.IsSuccess())
            {
                return WriteErrorResult(response.errors?.First()?.message ?? "Request Failed");
            }

            return new SavedContentWriteResult
            {
                Success = true,
                Message = string.Empty
            };
        }

        public ISavedContentReadResult ReadErrorResult => new SavedContentReadResult
        {
            Success = false,
            SavedContentItems = Enumerable.Empty<ISavedContent>()
        };

        public ISavedContentWriteResult WriteErrorResult(string message)
        {
            return new SavedContentWriteResult
            {
                Success = false,
                Message = message
            };
        }
    }
}
