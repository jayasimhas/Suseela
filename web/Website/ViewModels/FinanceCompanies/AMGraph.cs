using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class AMGraph
    {
        public string GraphID { get; set; }
        public string GraphTitle { get; set; }
        public string GraphType { get; set; }
        public string GraphColor { get; set; }
        public string FinanceResult { get; set; }
    }

    public class PeerCompanyGraph
    {
        public string GraphID { get; set; }
        public string GraphName { get; set; }
        public List<PeerCompany> CompanyGraphs { get; set; }
    }

    public class PeerCompany
    {
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string FinanceResult { get; set; }
    }

    public class MergersAquisitionsResult
    {
        public string Month { get; set; }
        public string Acquirer { get; set; }
        public string Target { get; set; }
        public string TargetSector { get; set; }
        public string TargetLocation { get; set; }
        public string Detail { get; set; }
        public string Price { get; set; }
    }
    public class FinanceCompany
    {
        public string CompanyID { get; set; }
        public string Path { get; set; }
    }
}