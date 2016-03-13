using Informa.Library.User.Profile;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User;

namespace Informa.Library.Salesforce.User.Profile
{
	public class SalesforceFindUserProfile : ISalesforceFindUserProfile, IUserProfileFactory, IFindUserProfileByUsername
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceFindUserProfile(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public IUserProfile Create(IUser user)
		{
			return Find(user?.Username ?? string.Empty);
		}

		public ISalesforceUserProfile Find(string username)
		{
			if (string.IsNullOrWhiteSpace(username))
			{
				return null;
			}

			var profileResponse = Service.Execute(s => s.queryProfileContactInformation(username));

			if (!profileResponse.IsSuccess())
			{
				return null;
			}

			var profile = profileResponse.profile;

			return new SalesforceUserProfile
			{
				FirstName = profile?.name?.firstName ?? string.Empty,
				LastName = profile?.name?.lastName ?? string.Empty,
                MiddleInitial = profile?.name?.middleInitial ?? string.Empty,
                NameSuffix = profile?.name?.nameSuffix ?? string.Empty,
                Salutation = profile?.name?.salutation ?? string.Empty,
                BillCountry = profile?.billingAddress?.country ?? string.Empty,
                BillAddress1 = profile?.billingAddress?.addressLine1 ?? string.Empty,
                BillAddress2 = profile?.billingAddress?.addressLine2 ?? string.Empty,
                BillCity = profile?.billingAddress?.city ?? string.Empty,
                BillPostalCode = profile?.billingAddress?.postalCode ?? string.Empty,
                BillState = profile?.billingAddress?.stateProvince ?? string.Empty,
                ShipCountry = profile?.shippingAddress?.country ?? string.Empty,
                ShipAddress1 = profile?.shippingAddress?.addressLine1 ?? string.Empty,
                ShipAddress2 = profile?.shippingAddress?.addressLine2 ?? string.Empty,
                ShipCity = profile?.shippingAddress?.city ?? string.Empty,
                ShipPostalCode = profile?.shippingAddress?.postalCode ?? string.Empty,
                ShipState = profile?.shippingAddress?.stateProvince ?? string.Empty,
                CountryCode = profile?.phoneFax?.countryCode ?? string.Empty,
                Fax = profile?.phoneFax?.fax ?? string.Empty,
                PhoneExtension = profile?.phoneFax?.phoneExtension ?? string.Empty,
                Phone = profile?.phoneFax?.phoneNumber ?? string.Empty,
                PhoneType = profile?.phoneFax?.phoneType ?? string.Empty,
                Company = profile?.companyJob?.company ?? string.Empty,
                JobFunction = profile?.companyJob?.function ?? string.Empty,
                JobIndustry = profile?.companyJob?.industry ?? string.Empty,
                JobTitle = profile?.companyJob?.title ?? string.Empty
            };
		}

		IUserProfile IFindUserProfileByUsername.Find(string username)
		{
			return Find(username);
		}
	}
}
