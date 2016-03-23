namespace Informa.Web.ViewModels.SiteDebugging
{
	public interface IEntitlementsCheckEnabledViewModel
	{
		string InputName { get; }
		string EnabledInputValue { get; }
		string DisabledInputValue { get; }
		bool IsDebugging { get; }
		string LabelText { get; }
		string SubmitText { get; }
	}
}