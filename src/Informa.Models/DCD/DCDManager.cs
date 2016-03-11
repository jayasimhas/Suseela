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

        public Deal GetDealByRecordId(int recordId)
        {
            Deal dbDeal = null;
            using (DCDContext dContext = new DCDContext())
            {
                dbDeal = dContext.Deals.Where(deal => deal.RecordId == recordId).FirstOrDefault();
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

        public Company GetCompanyByRecordId(int recordId)
        {
            Company company = null;
            using (DCDContext dContext = new DCDContext())
            {
                company = dContext.Companies.Where(deal => deal.RecordId == recordId).FirstOrDefault();
            }

            return company;
        }

        public Drug GetDrugByRecordId(int recordId)
        {
            Drug drug = null;
            using (DCDContext dContext = new DCDContext())
            {
                drug = dContext.Drugs.Where(drg => drg.RecordId == recordId).FirstOrDefault();
            }

            return drug;
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
        public ImportLog GetImportLogById(int id)
        {
            ImportLog log = null;
            using (DCDContext dc = new DCDContext())
            {
                log = dc.ImportLogs.FirstOrDefault(i => i.Id == id);
            }

            return log;
        }

        public List<IDCDRecordImportLog> GetRecordsForImport(int importId)
        {
            List<IDCDRecordImportLog> records = new List<IDCDRecordImportLog>();
            using (DCDContext dc = new DCDContext())
            {
                records.AddRange(dc.DealRecordImportLogs.Where(d => d.ImportId == importId).Select(d => (IDCDRecordImportLog)d));
                records.AddRange(dc.CompanyRecordImportLogs.Where(c => c.ImportId == importId).Select(c => (IDCDRecordImportLog)c));
                records.AddRange(dc.DrugRecordImportLogs.Where(d => d.ImportId == importId).Select(d => (IDCDRecordImportLog)d));
            }

            return records;
        }

        public List<ImportLog> GetImports(DateTime startDate, DateTime endDate)
        {
            //change the span to be: start 12am to end 11:59pm
            DateTime start = startDate.Date;
            DateTime end = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            List<ImportLog> logs = new List<ImportLog>();
            using (DCDContext dc = new DCDContext())
            {
                logs = dc.ImportLogs.Where(i => i.ImportStart <= end && i.ImportEnd >= start).OrderBy(i => i.ImportStart).ToList();
            }

            return logs;
        }

        public int GetTotalRecords(ImportLog log)
        {
            int total = 0;
            using (DCDContext dc = new DCDContext())
            {
                total += dc.DealRecordImportLogs.Where(d => d.ImportLog.Id == log.Id).Count();
                total += dc.CompanyRecordImportLogs.Where(c => c.ImportLog.Id == log.Id).Count();
                total += dc.DrugRecordImportLogs.Where(d => d.ImportLog.Id == log.Id).Count();
            }
            return total;
        }

        public int GetTotalSuccess(ImportLog log)
        {
            int total = 0;
            using (DCDContext dc = new DCDContext())
            {
                total += dc.DealRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == RecordImportResult.Success.ToString()).Count();
                total += dc.CompanyRecordImportLogs.Where(c => c.ImportLog.Id == log.Id && c.Result == RecordImportResult.Success.ToString()).Count();
                total += dc.DrugRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == RecordImportResult.Success.ToString()).Count();
            }
            return total;
        }

        public int GetTotalSkipped(ImportLog log)
        {
            int total = 0;
            using (DCDContext dc = new DCDContext())
            {
                total += dc.DealRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == RecordImportResult.Skipped.ToString()).Count();
                total += dc.CompanyRecordImportLogs.Where(c => c.ImportLog.Id == log.Id && c.Result == RecordImportResult.Skipped.ToString()).Count();
                total += dc.DrugRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == RecordImportResult.Skipped.ToString()).Count();
            }
            return total;
        }

        public int GetTotalFailed(ImportLog log)
        {
            int total = 0;
            using (DCDContext dc = new DCDContext())
            {
                total += dc.DealRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == RecordImportResult.Failure.ToString()).Count();
                total += dc.CompanyRecordImportLogs.Where(c => c.ImportLog.Id == log.Id && c.Result == RecordImportResult.Failure.ToString()).Count();
                total += dc.DrugRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == RecordImportResult.Failure.ToString()).Count();
            }
            return total;
        }
    }
}