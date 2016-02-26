using System;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;
using Informa.Web.Areas.Account.Models;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SignInViewModel : ISignInViewModel
	{
		protected readonly ITextTranslator TextTranslator;

        public SignInViewModel() { }

		public SignInViewModel(
			ITextTranslator textTranslator)
		{
			TextTranslator = textTranslator;
		}

		public string SignInButtonText => TextTranslator.Translate("Header.SignInLink");
		public string SignInInvalidText => TextTranslator.Translate("Header.SignInInvalid");
		public string PasswordPlaceholderText => TextTranslator.Translate("Header.PasswordPlaceholder");
		public string RememberMeText => TextTranslator.Translate("Header.RememberMe");
		public string ForgotPasswordText => TextTranslator.Translate("Header.ForgotPassword");
		public string ForgotPasswordLinkText => TextTranslator.Translate("Header.ForgotPasswordLink");
		public string ForgotPasswordHelpText => TextTranslator.Translate("Header.ForgotPasswordHelp");
		public string ForgotPasswordButtonText => TextTranslator.Translate("Header.ForgotPasswordButton");
		public string ForgotPasswordConfirmationText => TextTranslator.Translate("Header.ForgotPasswordConfirmation");
		public IHtmlString ForgotPasswordContactText => new HtmlString("Need help? Contact us at <b>(800) 332-2181</b>, <b>+1 (908) 748-1221</b>, or <a href=\"#\">custcare@informa.com</a>");
		public string EmailPlaceholderText => TextTranslator.Translate("Header.EmailPlaceholder");
		public string UsernamePlaceholderText => TextTranslator.Translate("Header.UsernamePlaceholder");

        [Required]
        [EmailAddress]               
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
                                               
        public bool RememberMe { get; set; }
                                                                    
	}

    
}