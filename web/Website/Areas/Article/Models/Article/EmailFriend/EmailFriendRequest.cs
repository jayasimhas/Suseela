namespace Informa.Web.Areas.Article.Models.Article.EmailFriend
{
	public class EmailFriendRequest
	{
		public string RecipientEmail { get; set; }
		public string SenderName { get; set; }
		public string SenderEmail { get; set; }
		public string PersonalMessage { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleTitle { get; set; }
		public string RecaptchaResponse { get; set; }
	}
}