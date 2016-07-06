﻿using System;
using System.Text.RegularExpressions;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.Attributes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Authors
{
    public interface IAuthorService
    {
        string ConvertAuthorNameToUrlName(string authorName);
        string GetUrlName(Guid authorId);
        string GetUrlName([NotNull] IStaff_Item authorItem);
    }

    [AutowireService]
    public class AuthorService : IAuthorService
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
        }

        public AuthorService(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string GetUrlName(Guid authorId)
            => GetUrlName(_dependencies.SitecoreService.GetItem<IStaff_Item>(authorId));
        public string GetUrlName([NotNull] IStaff_Item authorItem)
        {
            return ConvertAuthorNameToUrlName(authorItem._Name);
        }
        

        public string ConvertAuthorNameToUrlName(string authorName)
        {
            var nonCharsRegex = new Regex(@"[^a-z]+");
            var name = authorName.ToLower();
            name = nonCharsRegex.Replace(name, "-");
            name = name.Trim('-');

            return name;
        }
    }
}