namespace Informa.Web.Areas.Article.Models.Article.AskAnalyst
{
    public class EmailAnalyst
    {        
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string PersonalQuestion { get; set; }
        public string ArticleNumber { get; set; }
        public string ArticleTitle { get; set; }
        public string RecaptchaResponse { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string PublicationName { get; set; }
        public string AskTheAnalystEmail { get; set; }
    }
}