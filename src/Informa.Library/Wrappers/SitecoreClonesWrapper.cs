using System;
using Informa.Library.Utilities.References;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Wrappers
{
    public interface ISitecoreClonesWrapper
    {
        void CreateClone(Guid sourceId, Guid desitnationParentId, bool isDeep = false);
    }

    [AutowireService]
    public class SitecoreClonesWrapper : BaseSitecoreWrapper, ISitecoreClonesWrapper
    {
        public void CreateClone(Guid sourceId, Guid desitnationParentId, bool isDeep=false)
        {
            var sourceItem = GetItem(sourceId, Constants.MasterDb);
            var destinationItem = GetItem(desitnationParentId, Constants.MasterDb);

            sourceItem.CloneTo(destinationItem, isDeep);
        }
    }
}