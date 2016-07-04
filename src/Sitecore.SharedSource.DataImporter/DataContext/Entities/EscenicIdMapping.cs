using System.ComponentModel.DataAnnotations;

namespace Sitecore.SharedSource.DataImporter.DataContext.Entities
{
	public class EscenicIdMapping
	{
		[Key]
		public string EscenicId { get; set; }
		public string ArticleNumber { get; set; }
	}
}