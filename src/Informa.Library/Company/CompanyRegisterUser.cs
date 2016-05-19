﻿using Informa.Library.User.Registration;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Company
{
	[AutowireService]
	public class CompanyRegisterUser : IRegisterUser
	{
		protected readonly IUserCompanyContext UserCompanyContext;
		protected readonly IRegisterCompanyUser RegisterCompanyUser;
		protected readonly IAllowedRegisterUserCompanyTypes AllowedCompanyTypes;
		protected readonly IFindCompanyByMasterId FindCompanyByMasterId;

		public CompanyRegisterUser(
			IUserCompanyContext userCompanyContext,
			IRegisterCompanyUser registerCompanyUser,
			IAllowedRegisterUserCompanyTypes allowedCompanyTypes,
			IFindCompanyByMasterId findCompanyByMasterId)
		{
			UserCompanyContext = userCompanyContext;
			RegisterCompanyUser = registerCompanyUser;
			AllowedCompanyTypes = allowedCompanyTypes;
			FindCompanyByMasterId = findCompanyByMasterId;
		}

		public IRegisterUserResult Register(INewUser newUser)
		{
			var company = UserCompanyContext.Company;

			if (company == null && !string.IsNullOrEmpty(newUser.MasterId))
			{
				company = FindCompanyByMasterId.Find(newUser.MasterId, newUser.MasterPassword);
			}

			if (company != null && !AllowedCompanyTypes.Types.Contains(company.Type))
			{
				company = null;
			}

			return RegisterCompanyUser.Register(newUser, company);
		}

		public CompanyRegisterUserResult CreateResult(bool success, IEnumerable<string> errors)
		{
			return new CompanyRegisterUserResult
			{
				Errors = errors,
				Success = success
			};
		}
	}
}
