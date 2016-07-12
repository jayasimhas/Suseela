using System.Collections.Generic;
using System.Xml.Serialization;

namespace Informa.Models.DCD
{
    [XmlRoot("Content")]
    public class CompanyContent
    {
        public string Name { get; set; }
        public CompanyContactInfo ContactInfo { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
        public ParentsAndDivisions ParentsAndDivisions { get; set; }

        [XmlElement("CodingSet")]
        public CodingSet[] CodingSets { get; set; }

        [XmlElement("Financials")]
        public FinancialData FinancialData { get; set; }

        [XmlArray("Employees")]
        [XmlArrayItem("Employee")]
        public string[] Employees { get; set; }
    }

    public class CompanyInfo
    {
        public string EntityType { get; set; }
        public string DateFounded { get; set; }
        public string StockExch { get; set; }
        public string StockTickerSymbol { get; set; }
        public string Sales { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Ownership { get; set; }
        public string LocationPath { get; set; }
    }

    public class CompanyContactInfo
    {
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Province { get; set; }
        public string Zip { get; set; }
        [XmlElement(ElementName = "POBox")]
        public string PoBox { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
    }

    public class ParentsAndDivisions
    {
        [XmlElement(ElementName = "CompanyPath")]
        public CompanyPath[] CompanyPaths { get; set; }
    }

    public class CompanyPath
    {
        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }
        [XmlText]
        public string Path { get; set; }
    }

    public class FinancialData
    {
        public FiscalYearEnd FiscalYearEnd { get; set; }
        public string Source { get; set; }

        [XmlElement(ElementName = "Financial")]
        public Financial[] Financials { get; set; }
        
    }

    public class FiscalYearEnd
    {
        [XmlAttribute("month")]
        public int Month { get; set; }

        [XmlAttribute("day")]
        public int Day { get; set; }
    }

    public class Financial
    {
        public int FinancialYear { get; set; }
        public long FinancialSales { get; set; }
        public long FinancialEarnings { get; set; }
    }

    public class CodingSet
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("Coding")]
        public Coding[] Codings { get; set; }
    }

    public class Coding
    {
        [XmlAttribute("primary")]
        public bool IsPrimary { get; set; }

        [XmlText]
        public string Name { get; set; }
    }
}
