using System;
using Informa.Library.Utilities.References;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Wrappers
{
    public interface ISitecoreClonesWrapper
    {
        void CreateClone(Guid sourceId, Guid desitnationParentId, bool IsDeep = false);
    }

    [AutowireService]
    public class SitecoreClonesWrapper : BaseSitecoreWrapper, ISitecoreClonesWrapper
    {
        public void CreateClone(Guid sourceId, Guid desitnationParentId, bool IsDeep=false)
        {
            var sourceItem = GetItem(sourceId, Constants.MasterDb);
            var destinationItem = GetItem(desitnationParentId, Constants.MasterDb);

            sourceItem.CloneTo(destinationItem, IsDeep);
        }
    }
}