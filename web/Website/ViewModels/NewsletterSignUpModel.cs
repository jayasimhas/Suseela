
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Newsletter;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Glass.Autofac.Mvc.Models;


namespace Informa.Web.ViewModels
{
	public class NewsletterSignUpModel : GlassViewModel<INewsletter_Sign_Up>
	{
	    private readonly IAuthenticatedUserContext UserContext;
        public readonly IQueryNewsletterUserOptIn NewsletterOptIn;
	    public readonly ITextTranslator TextTranslator;

        public NewsletterSignUpModel(
	        IAuthenticatedUserContext userContext,
            IQueryNewsletterUserOptIn newsletterOptIn,
            ITextTranslator textTranslator)
	    {

            UserContext = userContext;
            NewsletterOptIn = newsletterOptIn;
            TextTranslator = textTranslator;
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

	    public string GeneralError => TextTranslator.Translate("Newsletter.GeneralError");

	}
}