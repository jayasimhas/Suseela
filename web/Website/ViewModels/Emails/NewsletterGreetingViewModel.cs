namespace Informa.Web.ViewModels.Emails
{
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails.Components;
    using Jabberwocky.Glass.Autofac.Mvc.Models;
    using Library.Globalization;

    public class NewsletterGreetingViewModel : GlassViewModel<INewsletter_Greeting_Content>
    {
        public readonly ITextTranslator TextTranslator;
        public NewsletterGreetingViewModel(ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }
    }
}