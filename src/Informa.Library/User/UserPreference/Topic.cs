namespace Informa.Library.User.UserPreference
{
    public class Topic : ITopic
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public int TopicOrder { get; set; }
    }
}
