using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.Articles
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticlePrologueEmailViewModel : IArticlePrologueEmailViewModel
	{
	}
}