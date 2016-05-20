﻿using System;
using Glass.Mapper.Sc;
using Informa.Library.Mail.ExactTarget;
using Informa.Library.Utilities.StringUtils;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.Emails
{
    public class FeaturedArticleViewModel : GlassViewModel<IFeatured_Article>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; }
            ISitecoreUrlWrapper SitecoreUrlWrapper { get; }
            IBylineMaker BylineMaker { get; }
            ICampaignQueryBuilder CampaignQueryBuilder { get; }
        }

        public FeaturedArticleViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private string _byline;
        public string Byline
        {
            get
            {
                if (string.IsNullOrEmpty(_byline))
                {
                    _byline = _dependencies.BylineMaker.MakeByline(Article.Authors);
                }
                return _byline;
            }
        }

        private IArticle _article;
        public IArticle Article
        {
            get
            {
                if (_article == null || _article._Id == Guid.Empty)
                {
                    _article = _dependencies.SitecoreService.GetItem<IArticle>(GlassModel.Article);
                }
                return _article;
            }
        }

        private string _mediaTypeIconSrc;
        public string MediaTypeIconSrc => _mediaTypeIconSrc ?? (_mediaTypeIconSrc = Article?.Media_Type?.Media_Type_Icon?.Src);

        private string _targetArticleUrl;
        public string TargetArticleUrl
            => _targetArticleUrl ?? (_targetArticleUrl = _dependencies.SitecoreUrlWrapper.GetItemUrl(Article));

    }
}
