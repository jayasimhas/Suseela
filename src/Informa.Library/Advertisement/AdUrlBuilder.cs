﻿using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Advertisement
{
    public interface IAdUrlBuilder
    {
        string GetLink(IAdvertisement adItem, bool isEmailLayout);
        string GetImageSrc(IAdvertisement adItem, bool isEmailLayout);
    }

    [AutowireService]
    public class AdUrlBuilder : IAdUrlBuilder
    {
        private readonly IDependencies _dependencies;
        private const string AdUrlFormat = 
            "http://oasc-eu1.247realmedia.com/RealMedia/ads/{0}/{1}/{2}/11001%40{3}";
        private const string LinkDesignation = "click_nx.ads";
        private const string ImageDesignation = "adstream_nx.ads";

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISiteRootContext SiteRootContext { get; }
        }

        public AdUrlBuilder(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string GetLink(IAdvertisement adItem, bool isEmailLayout = false) => MakeUrl(adItem, true, isEmailLayout);
        public string GetImageSrc(IAdvertisement adItem, bool isEmailLayout = false) => MakeUrl(adItem, false, isEmailLayout);

        private string MakeUrl(IAdvertisement adItem, bool isLink, bool isEmailLayout)
        {
            if (string.IsNullOrEmpty(adItem?.Slot_ID) || string.IsNullOrEmpty(adItem.Zone))
            { return string.Empty; }

			var adDomain = isEmailLayout ? _dependencies.SiteRootContext.Item?.Email_Ad_Domain : _dependencies.SiteRootContext.Item?.Ad_Domain;
            if (string.IsNullOrEmpty(adDomain))
            { return string.Empty; }

            return Format(isLink, adDomain, adItem.Zone, adItem.Slot_ID);
        }

        private string Format(bool isLink, string adDomain, string zone, string slotId)
        {
            var designation = isLink ? LinkDesignation : ImageDesignation;

            return string.Format(AdUrlFormat, designation, adDomain, zone, slotId);
        }
    }
}