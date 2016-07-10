using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Informa.Library.CustomSitecore.Pipelines.ExpandNewTokensOnAllItems {
    public class InformaMasterVariablesReplacer : MasterVariablesReplacer
    {
        public override string Replace(string text, Item targetItem)
        {

            //still need to assert these here
            Assert.ArgumentNotNull(text, "text");
            Assert.ArgumentNotNull(targetItem, "targetItem");
            string modText = base.Replace(text, targetItem);

            return ReplacePublicationNameToken(modText, targetItem);
        }

        public string ReplacePublicationNameToken(string text, Item targetItem)
        {
            string pubToken = "$publicationname";
            if (!text.Contains(pubToken))
                return text;

            var rootItem = targetItem.Axes
                .GetAncestors()
                .FirstOrDefault(a => a.Template.ID.Guid.Equals(ISite_RootConstants.TemplateId.Guid));

            if (rootItem == null)
                return text;

            var factory = DependencyResolver.Current.GetService<Func<string, ISitecoreService>>();
            if (factory == null)
                return text;

            var service = factory(targetItem.Database.Name);
            if (service == null)
                return text;

            var glassRoot = service.GetItem<ISite_Root>(rootItem.ID.Guid);
            if (glassRoot == null)
                return text;

            return text.Replace(pubToken, glassRoot.Publication_Name);
        }
    }
}
