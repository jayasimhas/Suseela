namespace Informa.Web.Areas.Account.Models.User.ResetPassword
{
	public static class ResetPasswordValidationReason
	{
		public const string MissingToken = "MissingToken";
		public const string PasswordMismatch = "PasswordMismatch";
		public const string PasswordRequirements = "PasswordRequirements";
	}
}