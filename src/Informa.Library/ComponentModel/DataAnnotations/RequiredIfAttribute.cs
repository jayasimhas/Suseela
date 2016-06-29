using System.ComponentModel.DataAnnotations;

namespace Informa.Library.ComponentModel.DataAnnotations
{
	public class RequiredIfAttribute : RequiredAttribute
	{
		private string PropertyName { get; set; }
		private object RequiredValue { get; set; }

		public RequiredIfAttribute(string propertyName, object requiredValue)
		{
			PropertyName = propertyName;
			RequiredValue = requiredValue;
		}

		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var instance = context.ObjectInstance;
			var type = instance.GetType();
			var propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);

			if (!propertyValue.Equals(RequiredValue) && propertyValue.ToString() != RequiredValue.ToString())
			{
				return ValidationResult.Success;
			}

			return base.IsValid(value, context);
		}
	}
}
