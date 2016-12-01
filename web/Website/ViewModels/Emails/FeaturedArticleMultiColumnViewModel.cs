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
    public class FeaturedArticleMultiColumnViewModel : GlassViewModel<IFeatured_Article_Agri>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISitecoreUrlWrapper SitecoreUrlWrapper { get; }
            IBylineMaker BylineMaker { get; }
        }

        public FeaturedArticleMultiColumnViewModel(IDependencies dependencies, IRenderingContextService renderingParametersService)
        {
            _dependencies = dependencies;
            RenderingParameters = renderingParametersService.GetCurrentRenderingParameters<IFeatured_Article_Display_Options>();
            if (RenderingParameters == null) return;
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

        private string _mediaTypeIconSrc;
        public string MediaTypeIconSrc
            => _mediaTypeIconSrc ?? (_mediaTypeIconSrc = GlassModel.ArticleOne?.Media_Type?.Media_Type_Icon?.Src);

        private string _targetArticleUrl;
        public string TargetArticleUrl
            => _targetArticleUrl ?? (_targetArticleUrl = _dependencies.SitecoreUrlWrapper.GetItemUrl(GlassModel.ArticleOne));

        public IList<IArticle> Articles => new List<IArticle> { GlassModel?.ArticleOne, GlassModel?.ArticleTwo, GlassModel?.ArticleThree };

        public IList<string> MediaTypeIconSrcs => new List<string>
                                                    {
                                                        GlassModel.ArticleOne?.Media_Type?.Media_Type_Icon?.Src,
                                                        GlassModel.ArticleTwo?.Media_Type?.Media_Type_Icon?.Src,
                                                        GlassModel.ArticleThree?.Media_Type?.Media_Type_Icon?.Src
                                                    };


    }
}