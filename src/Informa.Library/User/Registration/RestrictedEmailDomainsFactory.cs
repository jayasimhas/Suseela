using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Validation.Email;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Registration
{
	[AutowireService(LifetimeScope.Default)]
	public class RestrictedEmailDomainsFactory : IRestrictedEmailDomainsFactory
	{
		public IEnumerable<string> Create(IRestricted_Email_Domain_Folder folder)
		{
			if (folder == null)
			{
				return Enumerable.Empty<string>();
			}

			return folder._ChildrenWithInferType
				.Cast<IRestricted_Email_Domain>()
				.Where(red => red != null)
				.Select(red => red.Restricted_Domain)
				.ToList();
		}
	}
}
