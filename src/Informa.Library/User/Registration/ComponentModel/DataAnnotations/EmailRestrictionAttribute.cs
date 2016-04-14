using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Informa.Library.User.Registration.ComponentModel.DataAnnotations
{
	public abstract class EmailRestrictionAttribute : ValidationAttribute
	{
		public abstract IEnumerable<string> RestrictedDomains { get; }

		public override bool IsValid(object value)
		{
			var email = Convert.ToString(value);

			return !RestrictedDomains.Any(rd => email.ToLower().Contains(rd.ToLower()));
		}
	}
}
