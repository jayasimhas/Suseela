using Informa.Models.DCD;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Model.DCD
{
    public class DCDManager
    {
        public Drug GetDrugByRecordNumber(string recordNumber)
        {
            using (var context = new DCDContext())
            {
                return context.Drugs.FirstOrDefault(drug => drug.RecordNumber == recordNumber);
            }
        }

        public Deal GetDealByRecordNumber(string recordNumber)
        {
            Deal dbDeal = null;
            using (DCDContext dContext = new DCDContext())
            {
                dbDeal = dContext.Deals.Where(deal => deal.RecordNumber.Trim() == recordNumber.Trim()).FirstOrDefault();
            }

            return dbDeal;
        }

        public Company GetCompanyByRecordNumber(string recordNumber)
        {
            Company company = null;
            using (DCDContext dContext = new DCDContext())
            {
                company = dContext.Companies.Where(deal => deal.RecordNumber.Trim() == recordNumber.Trim()).FirstOrDefault();
            }

            return company;
        }


        public List<Company> GetAllCompanies()
        {
            List<Company> lstComp = null;
            using (DCDContext dContext = new DCDContext())
            {
                lstComp = dContext.Companies.ToList();
            }

            return lstComp;
        }

        public List<RelatedCompany> GetAllRelatedCompanies()
        {
            List<RelatedCompany> lstRelatedComp = null;
            using (DCDContext dContext = new DCDContext())
            {
                lstRelatedComp = dContext.RelatedCompanies.ToList();
            }

            return lstRelatedComp;
        }
    }
}