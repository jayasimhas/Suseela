namespace Informa.Library.User.Newsletter
{
    public interface INewsletterUserOptIn
    {
        bool OptIn { get; set; }
        string NewsletterType { get; }
        string SalesforceId { get; }
    }
}
