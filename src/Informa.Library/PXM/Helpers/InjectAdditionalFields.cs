using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.PXM.Helpers
{
    public interface IInjectAdditionalFields
    {
        string InjectIntoHtml(string html, IArticle article);
    }

    [AutowireService]
    public class InjectAdditionalFields : IInjectAdditionalFields
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {

        }

        public InjectAdditionalFields(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string InjectIntoHtml(string html, IArticle article)
        {
            var result = InjectAuthors(html, article);
            result = InjectTitles(result, article);
            return result;
        }

        public string InjectAuthors(string html, IArticle article)
        {
            return html;
        }

        public string InjectTitles(string html, IArticle article)
        {
            return html;
        }
    }
}