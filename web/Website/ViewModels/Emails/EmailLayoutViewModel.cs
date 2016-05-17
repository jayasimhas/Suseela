using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Utilities.References;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.Emails
{
    public class EmailLayoutViewModel : GlassViewModel<IExactTarget_Email>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ITextTranslator TextTranslator { get; }
            ISitecoreUrlWrapper SitecoreUrlWrapper { get; }
            ISiteRootContext SiteRootContext { get; }
        }

        public EmailLayoutViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private string _viewOnlineVersionPreLink;
        public string ViewOnlineVersionPreLink => _viewOnlineVersionPreLink ?? 
                                                (_viewOnlineVersionPreLink = _dependencies.TextTranslator?.Translate(DictionaryKeys.ViewOurOnlineVersionBeforeLink));

        private string _viewOnlineVersionLinkText;
        public string ViewOnlineVersionLinkText => _viewOnlineVersionLinkText ??
                                                (_viewOnlineVersionLinkText = _dependencies.TextTranslator?.Translate(DictionaryKeys.ViewOurOnlineVersionLinkText));

        private string _onlineVersionUrl;
        public string OnlineVersionUrl => _onlineVersionUrl ??
                                          (_onlineVersionUrl = _dependencies.SitecoreUrlWrapper.GetItemUrl(GlassModel));

        private ISite_Root _siteRoot;
        public ISite_Root SiteRoot => _siteRoot ??
                                      (_siteRoot = _dependencies.SiteRootContext.Item);
    }
}
