using Informa.Library.Navigation;
using Informa.Library.Wrappers;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Registration
{
	[AutowireService]
	public class RegisterReturnUrlContext : IRegisterReturnUrlContext
	{
		protected readonly IHttpContextProvider HttpContextProvider;
		protected readonly IReturnUrlContext ReturnUrlContext;

		public RegisterReturnUrlContext(
			IReturnUrlContext returnUrlContext,
			IHttpContextProvider httpContextProvider)
		{
			ReturnUrlContext = returnUrlContext;
			HttpContextProvider = httpContextProvider;
		}

		public string Url
		{
			get
			{
				var returnUrl = ReturnUrlContext.Url;
				var previousUrl = HttpContextProvider.Current.Request.UrlReferrer?.AbsolutePath ?? string.Empty;
				var url = string.IsNullOrEmpty(returnUrl) ? (previousUrl.StartsWith("/") ? previousUrl : string.Empty) : returnUrl;

				return url;
			}
		}
	}
}
