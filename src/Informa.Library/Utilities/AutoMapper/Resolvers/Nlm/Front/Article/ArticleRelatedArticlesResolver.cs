﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticleRelatedArticlesResolver : BaseValueResolver<ArticleItem, List<NlmRelatedArticleModel>>
    {
        protected override List<NlmRelatedArticleModel> Resolve(ArticleItem source, ResolutionContext context)
        {
            if (source == null) return null;

            var articles = GetRelatedArticles(source).ToList();

            return articles.Any()
                ? articles
                : null;
        }

        private IEnumerable<NlmRelatedArticleModel> GetRelatedArticles(ArticleItem source)
        {
            Func<IArticle, string, NlmRelatedArticleModel> toModel = (item, category) => new NlmRelatedArticleModel
            {
                ArticleType = category,
                Href = item.Article_Number
            };

            // Sidebar Articles
            var sidebarArticles = source.Related_Articles?.Where(article => article.Is_Sidebar_Article) ?? Enumerable.Empty<IArticle>();
            foreach (var article in sidebarArticles)
            {
                yield return toModel(article, "sidebar");
            }

            // Companion Articles
            var companionArticles = source.Related_Articles?.Concat(source.Referenced_Articles ?? Enumerable.Empty<IArticle>())
                .Where(article => !article.Is_Sidebar_Article)
                    ?? Enumerable.Empty<IArticle>();
            foreach (var article in companionArticles)
            {
                yield return toModel(article, "companion");
            }

            // Reference Deal
            if (!string.IsNullOrWhiteSpace(source.Referenced_Deals))
            {
                yield return new NlmRelatedArticleModel
                {
                    ArticleType = "deal",
                    Href = source.Referenced_Deals
                };
            }

            // Reference Companies
            if (!string.IsNullOrWhiteSpace(source.Referenced_Companies))
            {
                yield return new NlmRelatedArticleModel
                {
                    ArticleType = "company",
                    Href = source.Referenced_Companies
                };
            }
        } 
    }
}
