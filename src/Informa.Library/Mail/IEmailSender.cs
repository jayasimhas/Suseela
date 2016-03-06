namespace Informa.Library.Mail
{
	public interface IEmailSender
	{
		bool Send(IEmail email);
		bool SendWorkflowNotification(IEmail email,string replyEmail);		
	}
}
