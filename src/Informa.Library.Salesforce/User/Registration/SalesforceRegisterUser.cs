using Informa.Library.Company;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Registration;
using System.Collections.Generic;

namespace Informa.Library.Salesforce.User.Registration
{
	public class SalesforceRegisterUser : ISalesforceRegisterUser, IRegisterUser, IRegisterCompanyUser
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceRegisterUser(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public bool Register(INewUser newUser)
		{
			return Register(newUser, null);
		}

		public bool Register(INewUser newUser, ICompany company)
		{
			if (newUser == null)
			{
				return false;
			}

			var salesforceCompanies = new List<EBI_AccountData>();

			if (company != null)
			{
				salesforceCompanies.Add(new EBI_AccountData
				{
					accountId = company.Id,
					accountType = "" // TODO
				});
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
				},
				accounts = salesforceCompanies.ToArray()
			};

			var response = Service.Execute(s => s.createProfile(createProfileRequest));

			return response.IsSuccess();
		}
	}
}
