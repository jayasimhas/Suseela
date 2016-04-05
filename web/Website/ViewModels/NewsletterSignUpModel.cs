
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Newsletter;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Glass.Autofac.Mvc.Models;


namespace Informa.Web.ViewModels
{
	public class NewsletterSignUpModel : GlassViewModel<INewsletter_Sign_Up>
	{
	    private readonly IAuthenticatedUserContext UserContext;
        public readonly IQueryNewsletterUserOptIn NewsletterOptIn;
	    public readonly ITextTranslator TextTranslator;
	    private readonly ISiteRootContext SiteRootContext;
	    private readonly ISitecoreService SitecoreService;

        public NewsletterSignUpModel(
	        IAuthenticatedUserContext userContext,
            IQueryNewsletterUserOptIn newsletterOptIn,
            ITextTranslator textTranslator,
            ISiteRootContext siteRootContext,
            ISitecoreService sitecoreService)
	    {

            UserContext = userContext;
            NewsletterOptIn = newsletterOptIn;
            TextTranslator = textTranslator;
            SiteRootContext = siteRootContext;
            SitecoreService = sitecoreService;

	    }

        public bool IsAuthenticated => UserContext.IsAuthenticated;

	    public bool HasSubscribed
	    {
	        get
	        {
	            if (!UserContext.IsAuthenticated)
	                return false;

                var userNewsOptInStatus = NewsletterOptIn.Query(UserContext.User);
                if (userNewsOptInStatus.Success)
                {
                    var result = userNewsOptInStatus.NewsletterOptIns.Where(a => a.Name.ToLower().Equals(NewsletterType.Scrip.ToDescriptionString().ToLower()));
                    return (result.Any())
                        ? result.First().ReceivesNewsletterAlert
                        : false;
                }
	            return false;
	        }
	    }
        public string PreferencesURL => SitecoreService.GetItem<I___BasePage>(SiteRootContext.Item.Email_Preferences_Page)?._Url ?? "#";
        public string GeneralError => TextTranslator.Translate("Newsletter.GeneralError");
	}
}