using Informa.Library.Wrappers;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Navigation
{
	[AutowireService]
	public class ReturnUrlContext : IReturnUrlContext
	{
		private const string returnUrlKey = "returnUrl";

		protected readonly IHttpContextProvider HttpContextProvider;

		public ReturnUrlContext(
			IHttpContextProvider httpContextProvider)
		{
			HttpContextProvider = httpContextProvider;
		}

		public string Url
		{
			get
			{
				var url = HttpContextProvider.Current.Request[Key] ?? string.Empty;

				return url.StartsWith("/") ? url : string.Empty;
			}
		}
		public string Key => returnUrlKey;
	}
}
