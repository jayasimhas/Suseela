using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Validation.Email;

namespace Informa.Library.User.Registration
{
	public interface IRestrictedEmailDomainsFactory
	{
		IEnumerable<string> Create(IRestricted_Email_Domain_Folder folder);
	}
}