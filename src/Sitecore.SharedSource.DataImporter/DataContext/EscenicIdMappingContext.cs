using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using Sitecore.SharedSource.DataImporter.DataContext.Entities;

namespace Sitecore.SharedSource.DataImporter.DataContext
{
	public class EscenicIdMappingContext : DbContext
	{
		private static string ConnectionString => ConfigurationManager.ConnectionStrings["custom"].ConnectionString;

		public EscenicIdMappingContext() : base(ConnectionString)
		{
			Debug.Write(Database.Connection.ConnectionString);
		}
		public DbSet<EscenicIdMapping> EscenicIdMappings { get; set; }
	}
}
