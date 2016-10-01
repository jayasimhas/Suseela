namespace Informa.Library.User.UserPreference
{
    public interface ITopic
    {
        int TopicId { get; set; }
        string TopicName { get; set; }
        int TopicOrder { get; set; }
    }
}
