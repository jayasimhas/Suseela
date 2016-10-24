namespace Informa.Web.ViewModels.MyView
{
    using Models;
    using System.Collections.Generic;

    public interface IMyViewPageViewModel
    {
        string MyViewSettingsPageUrl { get; }
        int InitialLaodSectionsCount { get; }
        int ItemsPerSection { get; }
        IList<Section> Sections { get; }
        string JSONData { get; }
    }
}