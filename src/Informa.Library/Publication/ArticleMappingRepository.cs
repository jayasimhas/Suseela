using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Publication.Entity;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Publication
{
	[AutowireService()]
	public class ArticleMappingRepository : IArticleMappingRepository
	{
		private readonly ArticleMappingContext _context;

		public ArticleMappingRepository(ArticleMappingContext context)
		{
			_context = context;
		}

		public void Insert(Guid articleId, string articleNumber, Guid pmbiArticleId, string pmbiArticleNumber)
		{
			if (articleId != Guid.Empty && !string.IsNullOrWhiteSpace(articleNumber) &&
			    pmbiArticleId != Guid.Empty)
			{
				var articleMapping = new ArticleMapping {ArticleId = articleId, ArticleNumber = articleNumber, PmbiArticleId = pmbiArticleId, PmbiArticleNumber = pmbiArticleNumber};
				_context.Mappings.Add(articleMapping);
				_context.SaveChanges(); 
			}
		}

		public bool Exist(Guid id)
		{
			return _context.Mappings.Find(id) != null;
		}

		public void Update(Guid articleId, string articleNumber, Guid pmbiArticleId, string pmbiArticleNumber)
		{
			if (articleId != Guid.Empty && !string.IsNullOrWhiteSpace(articleNumber) &&
				pmbiArticleId != Guid.Empty)
			{
				var obj = _context.Mappings.Find(pmbiArticleId);
				if (obj != null)
				{
					obj.ArticleNumber = articleNumber;
					obj.ArticleId = articleId;
					obj.PmbiArticleNumber = pmbiArticleNumber;

					_context.SaveChanges();
				}
			}
		}

		public ArticleMapping GetMappingByPmbiArticleId(Guid pmbiArticleId)
		{
			return _context.Mappings.Find(pmbiArticleId);
		}

		public IEnumerable<ArticleMapping> GetMappingsByIds(IEnumerable<Guid> ids)
		{
			//return ids.Select(id => _context.Mappings.Find(id));		
			var values = string.Empty;
			foreach (var id in ids)
			{
				values = string.IsNullOrWhiteSpace(values) ? $"'{id}'" : $"{values},'{id}'";
			}

			var query = $"SELECT * FROM [ArticleMappings] WHERE [PmbiArticleId] IN ({values})";
			return _context.Mappings.SqlQuery(query);
		}
	}
}
