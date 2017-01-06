using Glass.Mapper.Sc;
using Informa.Library.Utilities.CMSHelpers;
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
				var folder = SitecoreContext.GetItem<IRestricted_Email_Domain_Folder>(new Guid(!string.IsNullOrEmpty(ItemIdResolver.GetItemIdByKey("RestrictedEmailDomainsCompetitor")) ? ItemIdResolver.GetItemIdByKey("RestrictedEmailDomainsCompetitor") : Guid.Empty.ToString()));

				return EmailDomainsFactory.Create(folder);
			}
		}
	}
}
