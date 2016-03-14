using Sitecore.Common;

namespace Informa.Library.Publishing.Switcher
{
    /// <summary>
    /// Used to indicate that the current publish operation is executing 
    /// within the context of a scheduled publish
    /// </summary>
    public class ScheduledPublishContext : Switcher<ScheduledState>
    {
        public ScheduledPublishContext()
            : base(ScheduledState.IsScheduledPublish)
        {
        }
    }
}
