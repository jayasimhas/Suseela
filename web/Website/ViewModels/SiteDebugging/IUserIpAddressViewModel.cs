namespace Informa.Web.ViewModels.SiteDebugging
{
	public interface IUserIpAddressViewModel
	{
		string IpAddressLabelText { get; }
		string IpAddressInputName { get; }
		string ClearIpAddressInputName { get; }
		string IpAddressSubmitText { get; }
		bool IsDebugging { get; }
		string IpAddress { get; }
	}
}