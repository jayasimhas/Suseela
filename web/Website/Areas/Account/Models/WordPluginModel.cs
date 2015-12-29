using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Informa.Web.Areas.Account.Models
{
	public class WordPluginModel
	{
		public class DirectoryStruct
		{
			/// <remarks/>
			public string Name { get; set; }

			/// <remarks/>
			public string[] Children { get; set; }
			public List<string> ChildrenList { get; set; }
		}

		public class TaxonomyStruct : ITaxonomy
		{
			public string Name { get; set; }
			/// <remarks/>
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

			private string nameField;

			private HDirectoryStruct[] childrenField;
			public List<HDirectoryStruct> ChildrenList { get; set; }

			private System.Guid idField;

			/// <remarks/>
			public string Name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
			public HDirectoryStruct[] Children
			{
				get
				{
					return this.childrenField;
				}
				set
				{
					this.childrenField = value;
				}
			}

			/// <remarks/>
			public System.Guid ID
			{
				get
				{
					return this.idField;
				}
				set
				{
					this.idField = value;
				}
			}
		}

		public class MediaItemStruct
		{

			private string extensionField;

			private byte[] dataField;

			private string nameField;

			private string urlField;

			private System.DateTime uploadDateField;

			private string uploaderField;

			private string pathField;

			private string widthField;

			private string heightField;

			/// <remarks/>
			public string Extension
			{
				get
				{
					return this.extensionField;
				}
				set
				{
					this.extensionField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
			public byte[] Data
			{
				get
				{
					return this.dataField;
				}
				set
				{
					this.dataField = value;
				}
			}

			/// <remarks/>
			public string Name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}

			/// <remarks/>
			public string Url
			{
				get
				{
					return this.urlField;
				}
				set
				{
					this.urlField = value;
				}
			}

			/// <remarks/>
			public System.DateTime UploadDate
			{
				get
				{
					return this.uploadDateField;
				}
				set
				{
					this.uploadDateField = value;
				}
			}

			/// <remarks/>
			public string Uploader
			{
				get
				{
					return this.uploaderField;
				}
				set
				{
					this.uploaderField = value;
				}
			}

			/// <remarks/>
			public string Path
			{
				get
				{
					return this.pathField;
				}
				set
				{
					this.pathField = value;
				}
			}

			/// <remarks/>
			public string Width
			{
				get
				{
					return this.widthField;
				}
				set
				{
					this.widthField = value;
				}
			}

			/// <remarks/>
			public string Height
			{
				get
				{
					return this.heightField;
				}
				set
				{
					this.heightField = value;
				}
			}
		}
	}
}