using SitecoreTreeWalker.SitecoreServer;

namespace SitecoreTreeWalker.UI.Controllers
{
    /// <summary>
    /// Interface for all tab controllers to implement
    /// </summary>
    interface ITabController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if tab has changed. Otherwise, false.</returns>
        bool HasChanged();
    }
}
