namespace Informa.Web.ViewModels.SiteDebugging
{
	public interface IUsernameViewModel
	{
		string ClearInputName { get; }
		string InputValue { get; }
		string InputName { get; }
		string LabelText { get; }
		string SubmitText { get; }
		bool IsDebugging { get; }
	}
}