using Informa.Library.User;
using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels.Articles
{
	public class ArticleEntitledViewModel : EntitledViewModel<IArticle>
	{
		public ArticleEntitledViewModel(
            IIsEntitledProducItemContext entitledProductContext, 
            IAuthenticatedUserContext authenticatedUserContext, 
						ISitecoreUserContext sitecoreUserContext) : base(entitledProductContext, authenticatedUserContext, sitecoreUserContext)
		{
		}

		public override bool IsFree => GlassModel.Free;
	}
}