namespace Informa.Library.Mail
{
	public interface IEmail
	{
		string From { get; set; }
		string To { get; set; }
		string Subject { get; set; }
		string Body { get; set; }
		bool IsBodyHtml { get; set; }
	}
}
