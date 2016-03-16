namespace Informa.Web.ViewModels
{
	public interface IHeaderViewModel
	{
		string LogoImageUrl { get; }
		string LogoUrl { get; }

		// Account/Sign In/Registration
		string WelcomeText { get; }
        string PolicyText { get; }
        bool IsAuthenticated { get; }
		string MyAccountLinkText { get; }
        string MyAccountLink { get; }
        string SignOutLinkText { get; }
		string RegisterLinkText { get; }
		string SignInLinkText { get; }
        string AdvertisementText { get; }
        string LeaderboardSlotID { get; }
        string LeaderboardAdZone { get; }
        string LeftColumnText { get; }
        string RightColumnText { get; }
        string Link1 { get; }
        string Link2 { get; }
        string Link3 { get; }
        string Link4 { get; }
    }
}
