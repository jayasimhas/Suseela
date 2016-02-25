using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Fields;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace Informa.Web.Areas.Account.Models
{
	public class WordPluginModel
	{
		public struct ArticleSize
		{
			public string Name { get; set; }
			public int MinimumWordCount { get; set; }
			public int MaximumWordCount { get; set; }
			public Guid ID { get; set; }
		}

		public class DirectoryStruct
		{
			public string Name { get; set; }			
			public string[] Children { get; set; }
			public List<string> ChildrenList { get; set; }
		}

		public class TaxonomyStruct : ITaxonomy
		{
			public string Name { get; set; }
			public System.Guid ID { get; set; }
			public string Section { get; set; }
		}

		public interface ITaxonomy
		{
			string Name { get; set; }
			Guid ID { get; set; }
			string Section { get; set; }
		}

		public class HDirectoryStruct
		{
			public List<HDirectoryStruct> ChildrenList { get; set; }

			/// <remarks/>
			public string Name { get; set; }

			/// <remarks/>
			[System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
			public HDirectoryStruct[] Children { get; set; }

			/// <remarks/>
			public System.Guid ID { get; set; }
		}

		public class MediaItemStruct
		{
			/// <remarks/>
			public string Extension { get; set; }

			/// <remarks/>
			[System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
			public byte[] Data { get; set; }

			/// <remarks/>
			public string Name { get; set; }

			/// <remarks/>
			public string Url { get; set; }

			/// <remarks/>
			public System.DateTime UploadDate { get; set; }

			/// <remarks/>
			public string Uploader { get; set; }

			/// <remarks/>
			public string Path { get; set; }

			/// <remarks/>
			public string Width { get; set; }

			/// <remarks/>
			public string Height { get; set; }
		}

		public struct ItemStruct
		{
			public string Name { get; set; }
			public Guid ID { get; set; }
		}
		public class ArticleStruct
		{
			private Guid _publication;
			public Guid ArticleGuid { get; set; }
			public string Title { get; set; }
			public DateTime PrintPublicationDate { get; set; }
			public DateTime WebPublicationDate { get; set; }
			public string ArticleNumber { get; set; }
			public Guid ArticleCategory { get; set; }
			public Guid WebCategory { get; set; }
			public string NotesToEditorial { get; set; }			
			public string Subtitle { get; set; }
			public string Summary { get; set; }
			public int WordCount { get; set; }
			public WorkflowState WorkflowState { get; set; }
			public Guid CommandID { get; set; }
			public List<TaxonomyStruct> Taxonomoy { get; set; }
			public List<Guid> RelatedInlineArticles { get; set; }
			public List<Guid> RelatedArticles { get; set; }
			public List<Guid> ChildArticles { get; set; }
			public List<string> SupportingDocumentPaths { get; set; }
			public List<ArticlePreviewInfo> ReferencedArticlesInfo { get; set; }
			public List<ArticlePreviewInfo> RelatedArticlesInfo { get; set; }

			[Obsolete]
			public List<string> PotentialCompanyNames { get; set; }
			public List<string> ReferencedDeals { get; set; }

			public List<StaffStruct> Authors { get; set; }
			public List<StaffStruct> ArticleSpecificNotifications { get; set; }
			public List<StaffStruct> GlobalNotifications { get; set; }

			public int WordDocVersionNumber { get; set; }
			public string WordDocLastUpdateDate { get; set; }
			public string WordDocLastUpdatedBy { get; set; }

			public Guid Publication
			{
				get
				{
					return _publication == Guid.Empty ? new Guid("{3818C47E-4B75-4305-8F01-AB994150A1B0}") : _publication;
				}
				set { _publication = value; }
			}

			public Boolean Embargoed { get; set; }
			public bool IsPublished { get; set; }
			public Guid Label { get; set; }
			public Guid MediaType { get; set; }
			public Guid FeaturedImage { get; set; }
			public string FeaturedImageCaption { get; set; }
			public string FeaturedImageSource { get; set; }
		}

		public class ArticlePreviewInfo
		{
			public string Title { get; set; }
			public List<string> Authors { get; set; }
			public string ArticleNumber { get; set; }
			public string Publication { get; set; }
			public DateTime Date { get; set; }
			public string PreviewUrl { get; set; }
			public Guid Guid { get; set; }
		}

		public class StaffStruct
		{
			public string Name { get; set; }
			public Guid[] Publications { get; set; }
			public Guid ID { get; set; }
		}
		public struct DealInfo
		{
			public string Name { get; set; }
			public string ID { get; set; }
			public DateTime DealDate { get; set; }
			public DateTime LastUpdated { get; set; }
			public List<string> Companies { get; set; }
			public string Url { get; set; }
		}

		public class WordStyleStruct
		{
			public string WordStyle { get; set; }
			public string CssElement { get; set; }
			public string CssClass { get; set; }
		}
		
		public struct CheckoutStatus
		{
			public string User;
			public bool Locked;

		}

		public class WorkflowCommand
		{
			public string DisplayName { get; set; }
			public string StringID { get; set; }
			public List<StaffStruct> GlobalNotifyList { get; set; }
			public bool SendsToFinal { get; set; }
		}

		public class WorkflowState
		{
			public string DisplayName { get; set; }
			public List<WorkflowCommand> Commands { get; set; }
			public bool IsFinal { get; set; }
		}

		public struct UserStatusStruct
		{
			public string UserName { get; set; }
			public int LoginAttemptsRemaining { get; set; }
			public bool LockedOut { get; set; }
			public bool LoginSuccessful { get; set; }
		}
		public class CreateArticleRequest
		{
			public string Name;
			public Guid PublicationID;
			public string PublicationDate;
		}

		public class SaveArticleDetails
		{
			public string ArticleNumber;
			public ArticleStruct ArticleData;
		}

		public class SaveArticleDetailsByGuid
		{
			public Guid ArticleGuid;
			public ArticleStruct ArticleData;
		}

		public class SaveArticleTextByGuid
		{
			public Guid ArticleGuid;
			public string WordText;
			public ArticleStruct ArticleData;
		}

		public class SaveArticleText
		{
			public string ArticleNumber;
			public string WordText;
			public ArticleStruct ArticleData;
		}

		public class SendDocumentToSitecoreByGuid
		{
			public Guid ArticlGuid;
			public byte[] Data;
			public string Extension;
		}
		public class SendDocumentToSitecore
		{
			public string ArticleNumber;
			public byte[] Data;
			public string Extension;
		}

		public class LoginModel
		{
			public string Username { get; set; }
			public string Password { get; set; }
		}

		public class CompanyWrapper
		{
			/// <remarks/>
			public int RecordID { get; set; }
			public string RecordNumber { get; set; }
			
			public string Title { get; set; }
			
			public CompanyWrapper[] RelatedCompanies { get; set; }
			
			[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
			public System.Nullable<int> Parent { get; set; }
		}

	}
}