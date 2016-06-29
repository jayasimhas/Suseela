using System.Configuration;
using System.Data.Entity;
using Informa.Library.Publication.Entity;
using Sitecore.Diagnostics;
using Debug = System.Diagnostics.Debug;

namespace Informa.Library.Publication
{
	public class ArticleMappingContext : DbContext
	{
		private static string ConnectionString => ConfigurationManager.ConnectionStrings["pmbiMapping"].ConnectionString;

		public ArticleMappingContext() : base(ConnectionString)
		{
			Debug.Write(Database.Connection.ConnectionString);
		}
		public DbSet<ArticleMapping> Mappings { get; set; }
	}
}
