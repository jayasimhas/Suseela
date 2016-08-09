using Informa.Library.User;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
	public class ArticleEntitledViewModel : EntitledViewModel<IArticle>
	{
		public ArticleEntitledViewModel(IIsEntitledProducItemContext entitledProductContext, ISitecoreUserContext sitecoreUserContext) : base(entitledProductContext, sitecoreUserContext)
		{
		}

		public override bool IsFree => GlassModel.Free;
	}
}