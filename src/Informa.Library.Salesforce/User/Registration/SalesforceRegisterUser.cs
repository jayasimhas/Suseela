using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Registration;

namespace Informa.Library.Salesforce.User.Registration
{
	public class SalesforceRegisterUser : ISalesforceRegisterUser, IRegisterUser
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceRegisterUser(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public bool Register(INewUser newUser)
		{
			if (newUser == null)
			{
				return false;
			}

			var createProfileRequest = new EBI_CreateProfileRequest
			{
				userName = newUser.Username,
				password = newUser.Password,
				profile = new EBI_Profile
				{
					name = new EBI_Name
					{
						firstName = newUser.FirstName,
						lastName = newUser.LastName
					},
					tosAccepted = true,
					tosAcceptedSpecified = true
				}
			};

			var response = Service.Execute(s => s.createProfile(createProfileRequest));

			return response.IsSuccess();
		}
	}
}
