using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Validation.Email;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Collections.Generic;

namespace Informa.Library.User.Registration
{
	[AutowireService(LifetimeScope.Default)]
	public class CompetitorRestrictedEmailDomains : ICompetitorRestrictedEmailDomains
	{
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly IRestrictedEmailDomainsFactory EmailDomainsFactory;

		public CompetitorRestrictedEmailDomains(
			ISitecoreContext sitecoreContext,
			IRestrictedEmailDomainsFactory emailDomainsFactory)
		{
			EmailDomainsFactory = emailDomainsFactory;
			SitecoreContext = sitecoreContext;
		}

		public IEnumerable<string> RestrictedDomains
		{
			get
			{
				var folder = SitecoreContext.GetItem<IRestricted_Email_Domain_Folder>(new Guid("{38753F80-CFF8-4DFD-9998-0EE30D69D190}"));

				return EmailDomainsFactory.Create(folder);
			}
		}
	}
}
