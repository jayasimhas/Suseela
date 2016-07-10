using Informa.Library.User.Authentication;
using Informa.Library.User.Authentication.Web.SiteDebugging;
using Informa.Library.Utilities.WebUtils;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Web.ViewModels.SiteDebugging
{
	[AutowireService(LifetimeScope.Default)]
	public class UsernameViewModel : IUsernameViewModel
	{
		protected readonly IRefreshPage RefreshPage;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly ISiteDebuggingWebLoginUser DebugWebLoginUser;

		public UsernameViewModel(
			IRefreshPage refreshPage,
			IAuthenticatedUserContext authenticatedUserContext,
			ISiteDebuggingWebLoginUser debugWebLoginUser)
		{
			RefreshPage = refreshPage;
			AuthenticatedUserContext = authenticatedUserContext;
			DebugWebLoginUser = debugWebLoginUser;
		}

		public string LabelText => "Spoof Email";
		public string InputName => "debugUsername";
		public string ClearInputName => "debugClearUsername";
		public string SubmitText => IsDebugging ? "Clear" : "OK";
		public bool IsDebugging
		{
			get
			{
				if (IsCleared)
				{
					DebugWebLoginUser.StopDebugging();
					RefreshPage.Refresh();

					return false;
				}

				if (DebugWebLoginUser.IsDebugging)
				{
					return true;
				}

				var username = HttpContext.Current.Request[InputName];

				if (string.IsNullOrWhiteSpace(username))
				{
					return false;
				}

				DebugWebLoginUser.StartDebugging(username);
				RefreshPage.Refresh();

				return true;
			}
		}
		public bool IsCleared => !string.IsNullOrWhiteSpace(HttpContext.Current.Request[ClearInputName]);
		public string InputValue => IsDebugging ? AuthenticatedUserContext.User?.Username ?? string.Empty : string.Empty;
	}
}