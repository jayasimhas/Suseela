namespace Informa.Web.Areas.Account.Models.User.Registration
{
	public static class RegisterValidationReason
	{
		public const string Required = "Required";
		public const string UsernameRequirements = "UsernameRequirements";
		public const string UsernameExists = "UsernameExists";
		public const string UsernameCompetitorRestriction = "UsernameCompetitorRestriction";
		public const string UsernamePublicRestriction = "UsernamePublicRestriction";
		public const string PasswordMismatch = "PasswordMismatch";
		public const string PasswordRequirements = "PasswordRequirements";
		public const string TermsNotAccepted = "TermsNotAccepted";
		public const string MasterIdInvalid = "MasterIdInvalid";
		public const string MasterIdExpired = "MasterIdExpired";
	}
}