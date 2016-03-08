using System;
using System.ComponentModel.DataAnnotations;

namespace Informa.Library.ComponentModel.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class MustBeTrueAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			return value != null && value is bool && (bool)value;
		}
	}
}
