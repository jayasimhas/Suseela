using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Informa.Library.DCD.XML
{
    [XmlRoot(ElementName = "IBI-Content")]
    public class IBIContent
    {
        private Feed feedField;

        private List<Record> recordSetField;

        private IBIContentType typeField;

        public IBIContent()
        {
            this.recordSetField = new List<Record>();
            this.feedField = new Feed();
        }

        public Feed Feed
        {
            get
            {
                return this.feedField;
            }
            set
            {
                this.feedField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayItemAttribute("Record", IsNullable = false)]
        public List<Record> RecordSet
        {
            get
            {
                return this.recordSetField;
            }
            set
            {
                this.recordSetField = value;
            }
        }

        [XmlAttribute("type")]
        public IBIContentType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    public class Feed
    {

        private Generated generatedField;

        private string numberOfRecordsField;

        private FeedType typeField;

        public Feed()
        {
            this.generatedField = new Generated();
        }

        public Generated Generated
        {
            get
            {
                return this.generatedField;
            }
            set
            {
                this.generatedField = value;
            }
        }

        public string NumberOfRecords
        {
            get
            {
                return this.numberOfRecordsField;
            }
            set
            {
                this.numberOfRecordsField = value;
            }
        }

        [XmlAttribute("type")]
        public FeedType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    public class Generated
    {

        private string dateField;

        public string date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }
    }

    public enum FeedType
    {

        /// <remarks/>
        ALLRECORDS,

        /// <remarks/>
        UPDATEDRECORDS,
    }

    public enum CommandType
    {

        /// <remarks/>
        Upsert,

        /// <remarks/>
        Delete,
    }

    public class Record
    {

        private CommandType commandField;

        private Identification identificationField;

        private Content contentField;

        private int numberField;

        public Record()
        {
            this.contentField = new Content();
            this.identificationField = new Identification();
        }

        public CommandType Command
        {
            get
            {
                return this.commandField;
            }
            set
            {
                this.commandField = value;
            }
        }

        public Identification Identification
        {
            get
            {
                return this.identificationField;
            }
            set
            {
                this.identificationField = value;
            }
        }

        public Content Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }

        [XmlAttribute("number")]
        public int number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }
    }

    public class Identification
    {

        private int recordIdField;

        private string recordNumberField;

        private string titleField;

        private DateTime createdField;

        private DateTime publishedField;

        private DateTime lastModifiedField;

        public int RecordId
        {
            get
            {
                return this.recordIdField;
            }
            set
            {
                this.recordIdField = value;
            }
        }

        public string RecordNumber
        {
            get
            {
                return this.recordNumberField;
            }
            set
            {
                this.recordNumberField = value;
            }
        }

        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        public DateTime Created
        {
            get
            {
                return this.createdField;
            }
            set
            {
                this.createdField = value;
            }
        }

        public DateTime Published
        {
            get
            {
                return this.publishedField;
            }
            set
            {
                this.publishedField = value;
            }
        }

        public DateTime LastModified
        {
            get
            {
                return this.lastModifiedField;
            }
            set
            {
                this.lastModifiedField = value;
            }
        }
    }

    public class Content : IXmlSerializable
    {
        public string InnerXml { get; set; }

        public Content()
        {
            InnerXml = string.Empty;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XDocument subDoc = XDocument.Load(reader.ReadSubtree());
            InnerXml = subDoc.ToString();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public enum IBIContentType
    {

        /// <remarks/>
        deal,

        /// <remarks/>
        company,

        /// <remarks/>
        drug,
    }

    public class RecordSet
    {

        private List<Record> recordField;

        public RecordSet()
        {
            this.recordField = new List<Record>();
        }

        public List<Record> Record
        {
            get
            {
                return this.recordField;
            }
            set
            {
                this.recordField = value;
            }
        }
    }

    public class CodingSet
    {

        private List<Coding> codingField;

        private CodingSetType typeField;

        public CodingSet()
        {
            this.codingField = new List<Coding>();
        }

        [XmlElement("Coding")]
        public List<Coding> Coding
        {
            get
            {
                return this.codingField;
            }
            set
            {
                this.codingField = value;
            }
        }

        [XmlAttribute]
        public CodingSetType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    public class Coding
    {

        private bool primaryField;

        private string valueField;

        public Coding()
        {
            this.primaryField = false;
        }

        [System.Xml.Serialization.XmlAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool primary
        {
            get
            {
                return this.primaryField;
            }
            set
            {
                this.primaryField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                string value = HttpUtility.HtmlDecode(this.valueField);
                if (string.IsNullOrEmpty(value))
                {
                    return value;
                }
                List<string> splitValues = value.Split('>', '/').Select(v => v.Trim()).ToList();

                //TODO: Remove this when this is replaced in the feed
                if (splitValues.Count > 0 && splitValues[0] == "Supplies, Equipment and Devices")
                {
                    splitValues[0] = "Medical Devices";
                }

                return string.Join("/", splitValues.ToArray());
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    public enum CodingSetType
    {

        /// <remarks/>
        indstry,

        /// <remarks/>
        theracat,

        /// <remarks/>
        mktsegmt,
    }
}
