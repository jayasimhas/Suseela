using System.Collections.Generic;
using Informa.Models.FactoryInterface;

namespace Informa.Web.ViewModels.DataTools
{
    public interface IDataToolTagsViewModel
    {
        IEnumerable<ILinkable> Tags { get; }
        string TagsLableText { get; }
    }
}