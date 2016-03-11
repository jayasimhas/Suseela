using Informa.Library.Company;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class CompanyRegisterMessageViewModel : ICompanyRegisterMessageViewModel
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ICompanyContext CompanyContext;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;

		public CompanyRegisterMessageViewModel(
			ISiteRootContext siteRootContext,
			ICompanyContext companyContext,
			IAuthenticatedUserContext authenticatedUserContext)
		{
			SiteRootContext = siteRootContext;
			CompanyContext = companyContext;
			AuthenticatedUserContext = authenticatedUserContext;
		}

		public string CompanyName => CompanyContext.Company?.Name ?? string.Empty;
		public string Message => (SiteRootContext.Item?.Recognized_IP_Announcment_Text ?? string.Empty).ReplacePatternCaseInsensitive("#Company_Name#", CompanyName);
		public bool Display => !AuthenticatedUserContext.IsAuthenticated && CompanyContext.Company != null;
		public string RegisterLinkText => SiteRootContext.Item?.Register_Link?.Text;
		public string RegisterLinkUrl => SiteRootContext.Item?.Register_Link?.Url;
	}
}