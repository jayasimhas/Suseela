using System;
using System.IO;
using AutoMapper;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport.Models;
using Informa.Library.Services.NlmExport.Serialization;
using Informa.Library.Services.NlmExport.Validation;
using Informa.Library.Utilities;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.Settings;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using log4net;
using Sitecore.Data.Items;

namespace Informa.Library.Services.NlmExport
{
	public class NlmExportService : INlmExportService
	{
		private readonly IDependencies _;
		private readonly ILog _logger;

		public NlmExportService(IDependencies dependencies, ILog logger)
		{
			if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));
			if (logger == null) throw new ArgumentNullException(nameof(logger));
			_ = dependencies;
			_logger = logger;
		}

		public Stream GenerateNlm(ArticleItem article, PublicationType? type = null)
		{
			var model = _.Mapper.Map<ArticleItem, NlmArticleModel>(article);
			if (type != null)
			{
				model.Front.ArticleMeta.PubDate.DateType = type.Value.ToString().ToLowerInvariant();
			}

			var memStream = new MemoryStream();

			_.Serializer.Serialize(model, memStream);
			memStream.Seek(0, SeekOrigin.Begin);

			return memStream;
		}

		public ExportResult ExportNlm(ArticleItem article, ExportType exportType, PublicationType? type = null)
		{
			try
			{
				// Generate NLM XML
				var stream = GenerateNlm(article, type);

				// Validate NLM
				var validationResult = _.ValidationService.ValidateXml(stream);
				if (validationResult.ValidationSuccessful)
				{
					// Write to disk
					var exportFolder = Path.GetFullPath(_.Settings.NlmExportPath);
					Directory.CreateDirectory(exportFolder);
					var fileName = GetFilenamePrefix(article) + ".xml";

					using (var file = File.Open(Path.Combine(exportFolder, fileName), FileMode.Create))
					{
						stream.Seek(0, SeekOrigin.Begin);
						stream.CopyTo(file);
					}
				}

				return Result(validationResult);
			}
			catch (Exception ex)
			{
				return Result(ex: ex);
			}
		}

		public bool DeleteNlm(ArticleItem article)
		{
			try
			{
				var exportFolder = Path.GetFullPath(_.Settings.NlmExportPath);
				var fileName = GetFilenamePrefix(article) + "_del.xml";
				Directory.CreateDirectory(exportFolder);
				File.Open(Path.Combine(exportFolder, fileName), FileMode.Create).Dispose();
			}
			catch (Exception ex)
			{
				_logger.Error($"Unable to create 'delete' NLM XML for article '{article?.Article_Number}'", ex);
				return false;
			}

			return true;
		}


		private string GetFilenamePrefix(ArticleItem article)
		{
			if (article == null || article.Article_Number == null)
				return string.Empty;

			using (var db = new SitecoreService(Constants.MasterDb))
			{
				var articleItem = db.GetItem<Item>(article._Id);
				var publicationItem = ArticleExtension.GetAncestorItemBasedOnTemplateID(articleItem);
				Guid publicationGuid = publicationItem.ID.Guid;

				string publicationName;
				publicationName = Constants.PublicationPrefixDictionary.TryGetValue(publicationGuid, out publicationName)
					? publicationName
					: Constants.ScripPublicationName;

				return $"{publicationName}_{article.Article_Number}";
			}
		}

		private static ExportResult Result(ValidationResult validationResult = null, Exception ex = null)
		{
			return new ExportResult
			{
				ValidationResult = validationResult,
				ExportSuccessful = (validationResult?.ValidationSuccessful ?? false) && ex == null,
				Exception = ex
			};
		}


		[AutowireService(true)]
		public interface IDependencies
		{
			INlmSerializer Serializer { get; }
			INlmValidationService ValidationService { get; }
			ISiteSettings Settings { get; }
			IMapper Mapper { get; }
		}
	}
}
