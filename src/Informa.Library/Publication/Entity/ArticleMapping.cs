using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Informa.Library.Publication.Entity
{
	public class ArticleMapping
	{
		public Guid ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		[Key]
		public Guid PmbiArticleId { get; set; }
		public string PmbiArticleNumber { get; set; }
	}
}
