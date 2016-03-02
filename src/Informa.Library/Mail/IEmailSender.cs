namespace Informa.Library.Mail
{
	public interface IEmailSender
	{
		bool Send(IEmail email);
	}
}
