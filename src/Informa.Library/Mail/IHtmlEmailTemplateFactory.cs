namespace Informa.Library.Mail
{
	public interface IHtmlEmailTemplateFactory
	{
		IHtmlEmailTemplate Create(string template);
	}
}