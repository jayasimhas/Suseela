namespace Informa.Library.User.UserPreference
{
    public class Topic : ITopic
    {
        public string TopicId { get; set; }
        public string TopicCode { get; set; }
        public string TopicName { get; set; }        
        public int TopicOrder { get; set; }
        public bool IsFollowing { get; set; }
    }
}
