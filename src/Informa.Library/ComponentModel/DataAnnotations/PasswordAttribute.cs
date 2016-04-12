using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Informa.Library.ComponentModel.DataAnnotations
{
	public class PasswordAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var password = Convert.ToString(value);

			var valid = password.Length >= 7 &&
						password.Length <= 40 &&
						password.Any(c => !char.IsLetterOrDigit(c)) &&
						password.Any(c => char.IsNumber(c));

			return valid;
		}
	}
}
