namespace Informa.Web.ViewModels.Emails
{
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails.Components;
    using Jabberwocky.Glass.Autofac.Mvc.Models;
    public class NewsletterGreetingViewModel : GlassViewModel<INewsletter_Greeting_Content>
    {
        public string GreetingTitle => GlassModel.Title;

        public string GreetingSummary => GlassModel.Summary;

        public string DownloadLinkUrl => GlassModel.Download_Link.Url;

        public string ImagePath => GlassModel.Image.Src;
    }
}