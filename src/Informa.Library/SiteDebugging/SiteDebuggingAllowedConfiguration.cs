using System.Collections.Generic;
using System.Net;
using System.Linq;
using Informa.Library.Net;
using System;
using Informa.Library.Site;

namespace Informa.Library.SiteDebugging
{
	public class SiteDebuggingAllowedConfiguration : ISiteDebuggingAllowedConfiguration
	{
		protected readonly IIpAddressFactory IpAddressFactory;
		protected readonly ISiteRootContext SiteRootContext;

		public SiteDebuggingAllowedConfiguration(
			IIpAddressFactory ipAddressFactory,
			ISiteRootContext siteRootContext)
		{
			IpAddressFactory = ipAddressFactory;
			SiteRootContext = siteRootContext;
		}

		public IEnumerable<IPAddress> IpAddresses => GetValues(SiteRootContext.Item?.Debug_IP_Access).Select(ip => IpAddressFactory.Create(ip)).Where(ip => ip != null).ToList();
		public IEnumerable<string> EmailAddresses => GetValues(SiteRootContext.Item?.Debug_Email_Access);
		
		public IEnumerable<string> GetValues(string value)
		{
			return string.IsNullOrWhiteSpace(value) ? Enumerable.Empty<string>() : value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		}


	}
}
