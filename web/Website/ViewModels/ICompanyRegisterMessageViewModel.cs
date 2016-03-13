namespace Informa.Web.ViewModels
{
	public interface ICompanyRegisterMessageViewModel
	{
		bool Display { get; }
		string Message { get; }
		string RegisterLinkText { get; }
		string RegisterLinkUrl { get; }
	}
}
