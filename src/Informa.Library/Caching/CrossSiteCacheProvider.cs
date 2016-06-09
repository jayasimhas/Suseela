using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Caching;
using Jabberwocky.Glass.Services;

namespace Informa.Library.Caching
{
	public class CrossSiteCacheProvider : SitecoreCacheDecorator, ICrossSiteCacheProvider
	{
		public CrossSiteCacheProvider(Jabberwocky.Core.Caching.ICacheProvider provider, ISitecoreService database, ISiteContextService siteService) : base(provider, database, siteService)
		{
		}

		protected override string GenerateCacheKey(string key)
		{
			var newKey = $"SitecoreCacheDecorator:{DatabaseName}-{LanguageName}-{key}";
			return newKey;
		}
	}

	public interface ICrossSiteCacheProvider : ICacheProvider { }
}
