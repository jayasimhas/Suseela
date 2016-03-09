using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User;
using Informa.Library.User.Profile;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Authentication;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceQueryOfferUserOptIn : IQueryOfferUserOptIn
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceQueryOfferUserOptIn(
            ISalesforceServiceContext service)
        {
            Service = service;
        }

        public IQueryOfferUserOptInResult Query(IAuthenticatedUser user)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                return ErrorResult;
            }

            var response = Service.Execute(s => s.queryInformationAndOfferOptins(user.Email));

            if (!response.IsSuccess())
            {
                return ErrorResult;
            }

            return new SalesforceQueryOfferUserOptInResult
            {
                Success = true,
                DoNotSendOffers = (response.doNotSendInformationAndOffersSpecified && response.doNotSendInformationAndOffers.Value)
            };
        }

        public IQueryOfferUserOptInResult ErrorResult => new SalesforceQueryOfferUserOptInResult
        {
            Success = false,
            DoNotSendOffers = true
        };
    }
}
