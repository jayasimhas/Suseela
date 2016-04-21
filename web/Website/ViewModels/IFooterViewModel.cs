using Glass.Mapper.Sc.Fields;
using Informa.Library.Navigation;
using Informa.Web.Models;
using System.Collections.Generic;

namespace Informa.Web.ViewModels
{
	public interface IFooterViewModel
	{
		string FooterLogoUrl { get; }

		string CopyrightText { get; }

		Link SubscribeLink { get; }

		IEnumerable<IPageLink> LocalLinks { get; }

		string FollowText { get; }

		Link LinkedInLink { get; }

		Link FacebookLink { get; }

		Link TwitterLink { get; }

        string FooterRssLogoUrl { get; }

        Link FooterRssLink { get; }

        string MenuOneHeader { get; }

		IEnumerable<INavigation> MenuOneLinks { get; }

		string MenuTwoHeader { get; }

		IEnumerable<INavigation> MenuTwoLinks { get; }
	}
}
