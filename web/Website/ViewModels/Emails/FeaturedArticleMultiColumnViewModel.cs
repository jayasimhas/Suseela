namespace Informa.Web.ViewModels.Emails
{
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails.Components;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
    using Jabberwocky.Autofac.Attributes;
    using Jabberwocky.Glass.Autofac.Mvc.Models;
    using Jabberwocky.Glass.Autofac.Mvc.Services;
    using Library.Utilities.StringUtils;
    using Library.Wrappers;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
    using System.Collections;
    using System.Collections.Generic;
    using Glass.Mapper.Sc;
    using Library.Site;
    using Library.Globalization;
    using Library.Authors;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
    using System.Text;

    public class FeaturedArticleMultiColumnViewModel : GlassViewModel<IFeatured_Article_MultiColumn>
    {
        private readonly IDependencies _dependencies;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IAuthorService AuthorClient;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISitecoreUrlWrapper SitecoreUrlWrapper { get; }
            IBylineMaker BylineMaker { get; }
        }

        public FeaturedArticleMultiColumnViewModel(IDependencies dependencies, IRenderingContextService renderingParametersService, ISiteRootContext siteRootContext, ITextTranslator textTranslator, IAuthorService authorClient)
        {
            _dependencies = dependencies;
            RenderingParameters = renderingParametersService.GetCurrentRenderingParameters<IFeatured_Article_Display_Options>();
            if (RenderingParameters == null) return;
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            AuthorClient = authorClient;
        }

        public IFeatured_Article_Display_Options RenderingParameters { get; set; }

        private string _byline;
        public string Byline
        {
            get
            {
                if (string.IsNullOrEmpty(_byline))
                {
                    _byline = _dependencies.BylineMaker.MakeByline(GlassModel.ArticleOne?.Authors);
                }
                return _byline;
            }
        }

        public string GetAuthorByLine(int index)
        {
           if (index == 0)
              return MakeAuthorByline(GlassModel.ArticleOne?.Authors);
            if(index == 1)
                return MakeAuthorByline(GlassModel.ArticleTwo?.Authors);
            if(index == 2)
                return MakeAuthorByline(GlassModel.ArticleThree?.Authors);
            return string.Empty;
        }


        private string _mediaTypeIconSrc;
        public string MediaTypeIconSrc
            => _mediaTypeIconSrc ?? (_mediaTypeIconSrc = GlassModel.ArticleOne?.Media_Type?.Media_Type_Icon?.Src);

        private string _targetArticleUrl;
        public string TargetArticleUrl
            => _targetArticleUrl ?? (_targetArticleUrl = _dependencies.SitecoreUrlWrapper.GetItemUrl(GlassModel.ArticleOne));

        public IList<IArticle> Articles => new List<IArticle> { GlassModel?.ArticleOne, GlassModel?.ArticleTwo, GlassModel?.ArticleThree };

        public IList<string> ShortDescrpitons => new List<string>
                                                    {
                                                        GlassModel.DescriptionArticleOne,
                                                        GlassModel.DescriptionArticleTwo,
                                                        GlassModel.DescriptionArticleThree
                                                    };

        public string publicationcode => SiteRootContext.Item.Publication_Code;

        private string MakeAuthorByline(IEnumerable<I___Person> authors)
        {
            if (authors == null) { return null; }

            var by = $"{TextTranslator.Translate("Article.By")} ";

            StringBuilder names = new StringBuilder();
            names.Append(by+" ");
            foreach (var eachAuthor in authors)
            {
                if (eachAuthor.Inactive)
                {
                    names.Append($"{eachAuthor.First_Name} {eachAuthor.Last_Name}").Append(",");
                }
                else
                {
                    names.Append($"<a  style='color:#{GetBrandingFontColor()}' href='{AuthorClient.ConvertUrlNameToLink(AuthorClient.GetUrlName(eachAuthor._Id))}'>{eachAuthor.First_Name} {eachAuthor.Last_Name}</a>".Trim()).Append(",");
                }
            }

            var result = names.ToString().Trim();
            if (result.Contains(","))
                result = result.Remove(result.LastIndexOf(','));

            return result == by ? string.Empty : result;
        }

        public string GetBrandingFontColor()
        {

            switch (publicationcode)
            {
                case "AGRO": return "D34729";
                case "ANIM": return "632C20";
                case "IEGV": return "2D9E22";
                case "AGRI": return "c44c0a";
                case "INSD": return "DE4361";
                case "LOLS": return "1191D1";
                case "INV": return "ca4d2c";
                case "MTI": return "007cc3";
                case "PINK": return "d82b6a";
                case "ROSE": return "c8206e";
                case "SCRIP": return "be1e2d";
                default: return "3919A5";
            }

        }
    }
}