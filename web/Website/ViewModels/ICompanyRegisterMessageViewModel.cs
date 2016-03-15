namespace Informa.Web.ViewModels
{
	public interface ICompanyRegisterMessageViewModel
	{
		bool Display { get; }
		string Message { get; }
		string DismissText { get; }
		string RegisterLinkText { get; }
		string RegisterLinkUrl { get; }
	}
}
