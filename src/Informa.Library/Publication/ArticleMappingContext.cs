using System.Data.Entity;
using Informa.Library.Publication.Entity;
using Sitecore.Diagnostics;
using Debug = System.Diagnostics.Debug;

namespace Informa.Library.Publication
{
	public class ArticleMappingContext : DbContext
	{
		public ArticleMappingContext() : base(@"user id=sitecore;password=felixx;Data Source=VWSQL2012;Database=Informa_Mapping")
		{
			Debug.Write(Database.Connection.ConnectionString);
		}
		public DbSet<ArticleMapping> Mappings { get; set; }
	}
}
