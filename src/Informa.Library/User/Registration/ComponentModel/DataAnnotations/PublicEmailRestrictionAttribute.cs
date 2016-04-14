using Autofac;
using Jabberwocky.Glass.Autofac.Util;
using System.Collections.Generic;

namespace Informa.Library.User.Registration.ComponentModel.DataAnnotations
{
	public class PublicEmailRestrictionAttribute : EmailRestrictionAttribute
	{
		protected IPublicRestrictedEmailDomains RestrictedEmailDomains { get { return AutofacConfig.ServiceLocator.Resolve<IPublicRestrictedEmailDomains>(); } }

		public override IEnumerable<string> RestrictedDomains => RestrictedEmailDomains.RestrictedDomains;
	}
}
