using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class EmailArticlePopOutViewModel : IEmailArticlePopOutViewModel
	{
	}
}