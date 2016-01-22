using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticlePrologueEmailViewModel : IArticlePrologueEmailViewModel
	{
	}
}