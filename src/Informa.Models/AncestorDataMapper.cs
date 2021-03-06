





#pragma warning disable 1591
#pragma warning disable 0108
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Team Development for Sitecore.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Glass.Mapper;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;

namespace Informa.Models
{
    public class AncestorDataMapper : Glass.Mapper.AbstractDataMapper
    {
        public AncestorDataMapper()
        {
            this.ReadOnly = true;
        }
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scContext = mappingContext as SitecoreDataMappingContext;
            if (scContext == null || scContext.Item == null)
            {
                return new Guid[0];
            }

            var guidStrs = scContext.Item.Paths.LongID.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            List<Guid> guids = new List<Guid>();
            foreach (var guidStr in guidStrs)
            {
                Guid guid;
                if (Guid.TryParse(guidStr, out guid))
                {
                    guids.Add(guid);
                }
            }

            return guids;
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            var scConfig = configuration as SitecoreInfoConfiguration;

            if (scConfig == null)
                return false;

            Type type = scConfig.PropertyInfo.PropertyType;

            return typeof(IEnumerable) == type;

        }
    }
}