using Informa.Library.Threading;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.PerRequest)]
	public class PublicationsNewsletterUserOptInContext : ThreadSafe<IEnumerable<IPublicationNewsletterUserOptIn>>, IPublicationsNewsletterUserOptInContext
	{
		protected readonly ISitesNewsletterTypes SitesNewsletterTypes;
		protected readonly IFindSiteNewsletterUserOptInsContext FindSiteNewsletterUserOptIns;
		protected readonly ISiteNewsletterUserOptedIn SiteNewsletterUserOptedIn;

		public PublicationsNewsletterUserOptInContext(
			ISitesNewsletterTypes sitesNewsletterTypes,
			IFindSiteNewsletterUserOptInsContext findSiteNewsletterUserOptIns,
			ISiteNewsletterUserOptedIn siteNewsletterUserOptedIn)
		{
			SitesNewsletterTypes = sitesNewsletterTypes;
			FindSiteNewsletterUserOptIns = findSiteNewsletterUserOptIns;
			SiteNewsletterUserOptedIn = siteNewsletterUserOptedIn;
		}

		public IEnumerable<IPublicationNewsletterUserOptIn> OptIns => SafeObject;

		protected override IEnumerable<IPublicationNewsletterUserOptIn> UnsafeObject
		{
			get
			{
				var publicationOptIns = new List<IPublicationNewsletterUserOptIn>();
				var siteTypes = SitesNewsletterTypes.SiteTypes;

				foreach (var siteType in siteTypes)
				{
                    var optIns = FindSiteNewsletterUserOptIns.Find(siteType);
                    var optedIn = SiteNewsletterUserOptedIn.Check(optIns);

                    var dailyOptIns = FindSiteNewsletterUserOptIns.FindDailyOptins(siteType);
                    var dailyOptedIn = SiteNewsletterUserOptedIn.Check(dailyOptIns);

                    var weeklyOptIns = FindSiteNewsletterUserOptIns.FindWeeklyOptins(siteType);                   
                    var weeklyOptedIn = SiteNewsletterUserOptedIn.Check(weeklyOptIns);

                    publicationOptIns.Add(new PublicationNewsletterUserOptIn
					{
                        OptIn = optedIn,                  
                        DailyOptIn = dailyOptedIn,
                        WeeklyOptIn = weeklyOptedIn,
                        Publication = siteType.Publication
					});
				}

				return publicationOptIns;
			}
		}
	}
}
