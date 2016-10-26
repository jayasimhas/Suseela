using System.Xml.Serialization;

namespace Informa.Models.DCD
{
    [XmlRoot("Content")]
    public class DealContent
    {
        public string Headline { get; set; }
        public DealInfo DealInfo { get; set; }
        public string DealSummary { get; set; }
        public string DealDetail { get; set; }

        [XmlElement(ElementName = "DealUpdates")]
        public DealUpdate[] DealUpdates { get; set; }

        [XmlElement("CodingSet")]
        public CodingSet[] CodingSets { get; set; }

        public DealFinancials DealFinancials { get; set; }

        public DealCompany[] DealCompanies { get; set; }
    }

    public class DealInfo
    {
        public string TransactionDate { get; set; }
        public string PublishedDate { get; set; }
        public string DealStatus { get; set; }
        public string DealType { get; set; }

        [XmlElement(ElementName = "Characteristic")]
        public string[] Characteristics { get; set; }
    }

    public class DealUpdate
    {
        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    public class DealFinancials
    {
        public string PotentialDealValue { get; set; }
        
        [XmlElement(ElementName = "Transactions")]
        public Transaction[] Transactions { get; set; }

    }

    public class Transaction
    {
        [XmlAttribute(AttributeName = "number")]
        public string Number { get; set; }

        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; }

        public TotalValue TotalValue { get; set; }
        public AssuranceInd AssuranceInd { get; set; }
    }

    public class TotalValue
    {
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }

        [XmlAttribute(AttributeName = "display")]
        public string Display { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class AssuranceInd
    {
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }

        [XmlAttribute(AttributeName = "display")]
        public string Display { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class DealCompany
    {
        [XmlElement(ElementName = "Company")]
        public InnerCompany Company { get; set; }
    }

    public class InnerCompany
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlText]
        public string Name { get; set; }
    }

}
