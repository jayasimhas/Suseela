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
				FirstName = profile.name.firstName,
				LastName = profile.name.lastName
			};
		}

		IUserProfile IFindUserProfileByUsername.Find(string username)
		{
			return Find(username);
		}
	}
}
