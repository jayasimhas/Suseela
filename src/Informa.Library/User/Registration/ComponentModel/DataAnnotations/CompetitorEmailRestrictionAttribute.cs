using Autofac;
using Jabberwocky.Glass.Autofac.Util;
using System.Collections.Generic;

namespace Informa.Library.User.Registration.ComponentModel.DataAnnotations
{
	public class CompetitorEmailRestrictionAttribute : EmailRestrictionAttribute
	{
		protected ICompetitorRestrictedEmailDomains RestrictedEmailDomains { get { return AutofacConfig.ServiceLocator.Resolve<ICompetitorRestrictedEmailDomains>(); } }

		public override IEnumerable<string> RestrictedDomains => RestrictedEmailDomains.RestrictedDomains;
	}
}
