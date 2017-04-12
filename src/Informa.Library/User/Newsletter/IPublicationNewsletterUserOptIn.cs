using Informa.Library.Publication;

namespace Informa.Library.User.Newsletter
{
    public interface IPublicationNewsletterUserOptIn
    {
        bool OptIn { get; set; }
        bool DailyOptIn { get; set; }
        bool WeeklyOptIn { get; set; }
        ISitePublication Publication { get; }
    }
}