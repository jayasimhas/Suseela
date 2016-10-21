namespace Informa.Library.User.UserPreference
{
    public interface ITopic
    {
        string TopicId { get; set; }
        string TopicCode { get; set; }
        string TopicName { get; set; }
        int TopicOrder { get; set; }
        bool IsFollowing { get; set; }
        string Taxonomy { get; set; }
    }
}
