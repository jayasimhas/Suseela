using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
	public class ArticleEntitledViewModel : EntitledViewModel<IArticle>
	{
		public ArticleEntitledViewModel(
            IIsEntitledProducItemContext entitledProductContext, 
            IAuthenticatedUserContext authenticatedUserContext) : base(entitledProductContext, authenticatedUserContext)
		{
		}

		public override bool IsFree => GlassModel.Free;
	}
}