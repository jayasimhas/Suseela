using Glass.Mapper.Sc.Fields;
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

		string MenuOneHeader { get; }

		IEnumerable<IPageLink> MenuOneLinks { get; }

		string MenuTwoHeader { get; }

		IEnumerable<IPageLink> MenuTwoLinks { get; }
	}
}
