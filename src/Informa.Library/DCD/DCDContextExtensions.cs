using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.DCD
{
    public class DCDContextExtensions : DCDContext
    {
        //public string GetCompanyName(string companyId)
        //{
        //    Company company = Companies.FirstOrDefault(c => c.RecordNumber == companyId);
        //    if (company != null)
        //    {
        //        return company.Title;
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Gets the first company record number found by its name.
        ///// </summary>
        ///// <param name="companyName"></param>
        ///// <returns>Null if no company matches</returns>
        //public string GetCompanyId(string companyName)
        //{
        //    Company company = Companies.FirstOrDefault(c => c.Title == companyName);
        //    if (company != null)
        //    {
        //        return company.RecordNumber;
        //    }
        //    return null;
        //}

        //public string GetDealName(string dealId)
        //{
        //    Deal deal = Deals.FirstOrDefault(c => c.RecordNumber == dealId);
        //    if (deal != null)
        //    {
        //        return deal.Title;
        //    }
        //    return null;
        //}

        //public string GetDrugName(string drugId)
        //{
        //    Drug drug = Drugs.FirstOrDefault(c => c.RecordNumber == drugId);
        //    if (drug != null)
        //    {
        //        return drug.Title;
        //    }
        //    return null;
        //}

        //public string GetAllCompanyNames(ArticleItem article)
        //{
        //    List<string> companyIds = new List<string>();
        //    companyIds.AddRange(article.RelatedCompanies.Text.Split(',').Select(s => s.Trim()));
        //    companyIds.AddRange(article.ReferencedCompanies.Text.Split(',').Where(s => !companyIds.Contains(s)).Select(s => s.Trim()));
        //    IEnumerable<string> companyNames = companyIds.Select(GetCompanyName);
        //    string result = string.Join(";", companyNames.Where(i => !string.IsNullOrEmpty(i)).ToArray());
        //    if (!string.IsNullOrEmpty(result))
        //    {
        //        return result;
        //    }
        //    return string.Empty;
        //}

        public ImportLog CreateImportLogEntry(DateTime start, string fileName)
        {
            try
            {
                ImportLog log = new ImportLog();
                log.ImportStart = start;
                log.ImportEnd = start;
                log.Result = DCDImporter.ImportResult.InProgress.ToString();
                log.FileName = fileName;
                ImportLogs.InsertOnSubmit(log);
                SubmitChanges();
                return log;
            }
            catch (Exception ex)
            {
                DCDImportLogger.Log.Error("Error writing log message to database", ex);
            }
            return null;
        }

        public void UpdateImportLogEntry(ImportLog log)
        {
            try
            {
                if (!ImportLogs.Any(l => l.Id == log.Id))
                {
                    ImportLogs.InsertOnSubmit(log);
                }
                SubmitChanges();
            }
            catch (Exception ex)
            {
                DCDImportLogger.Log.Error("Error writing log message to database", ex);
            }
        }

        public bool ProcessRecord<RecordType, RecordLogType>(Record record, ImportLog importLog)
            where RecordType : IDCD, new()
            where RecordLogType : IDCDRecordImportLog, new()
        {
            //First see if the record already exists
            IDCD entry = GetRecord<RecordType>(record.Identification.RecordId);

            //Process delete
            if (record.Command == CommandType.Delete)
            {
                if (entry == null)
                {
                    CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, DCDImporter.RecordImportResult.Skipped, DCDImporter.RecordImportOperation.Delete, "Cannot delete. Record doesn't exist.");
                    return true;
                }
                else
                {
                    try
                    {
                        entry.DeleteRecordOnSubmit(this);
                        SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, DCDImporter.RecordImportResult.Failure, DCDImporter.RecordImportOperation.Delete, "Error deleting record.", e);
                        return false;
                    }

                    CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, DCDImporter.RecordImportResult.Success, DCDImporter.RecordImportOperation.Delete, string.Empty);
                    return true;
                }
            }
            //Upsert
            else
            {
                DCDImporter.RecordImportOperation op = DCDImporter.RecordImportOperation.Insert;
                IDCD processingRecord;
                try
                {
                    //If the record already exists
                    if (entry != null)
                    {
                        op = DCDImporter.RecordImportOperation.Update;
                        //Only records that are newer than the records in the feed will be skipped
                        if (entry.LastModified > record.Identification.LastModified)
                        {
                            CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, DCDImporter.RecordImportResult.Skipped, op, "Existing record has a newer Last Modified Date.");
                            return true;
                        }
                        else if (record.Content == null || string.IsNullOrEmpty(record.Content.InnerXml))
                        {
                            CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, DCDImporter.RecordImportResult.Skipped, op, "Record does not have any content.");
                            return true;
                        }
                        else
                        {
                            entry.RecordNumber = record.Identification.RecordNumber;
                            entry.Title = DCDContentUtil.RemoveBrackets(record.Identification.Title);
                            entry.LastModified = record.Identification.LastModified;
                            entry.Created = record.Identification.Created;
                            entry.Published = record.Identification.Created;
                            if (!record.Identification.Published.ToString().Equals(DateTime.MinValue.ToString()))
                            {
                                entry.Published = record.Identification.Published;
                            }
                            entry.Content = record.Content.InnerXml;

                        }
                    }
                    else
                    {
                        op = DCDImporter.RecordImportOperation.Insert;
                        processingRecord = new RecordType();
                        processingRecord.RecordId = record.Identification.RecordId;
                        processingRecord.RecordNumber = record.Identification.RecordNumber;
                        processingRecord.Title = DCDContentUtil.RemoveBrackets(record.Identification.Title);
                        processingRecord.LastModified = record.Identification.LastModified;
                        processingRecord.Created = record.Identification.Created;
                        processingRecord.Published = record.Identification.Created;
                        if (!record.Identification.Published.ToString().Equals(DateTime.MinValue.ToString()))
                        {
                            processingRecord.Published = record.Identification.Published;
                        }
                        processingRecord.Content = record.Content.InnerXml;
                        processingRecord.InsertRecordOnSubmit(this);
                    }
                    SubmitChanges();
                }
                catch (Exception e)
                {
                    CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, DCDImporter.RecordImportResult.Failure, op, "Error inserting/updating record.", e);
                    return false;
                }

                CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, DCDImporter.RecordImportResult.Success, op, string.Empty);
                return true;
            }
        }

        public void CreateRecordLogEntry<RecordLogType>(int recordId, ImportLog importLog, DCDImporter.RecordImportResult result, DCDImporter.RecordImportOperation operation, string notes) where RecordLogType : IDCDRecordImportLog, new()
        {
            CreateRecordLogEntry<RecordLogType>(recordId, importLog, result, operation, notes, null);
        }

        public void CreateRecordLogEntry<RecordLogType>(int recordId, ImportLog importLog, DCDImporter.RecordImportResult result, DCDImporter.RecordImportOperation operation, string notes, Exception e) where RecordLogType : IDCDRecordImportLog, new()
        {
            try
            {
                RecordLogType log = new RecordLogType();
                log.RecordId = recordId;
                log.ImportLog = importLog;
                log.Result = result.ToString();
                log.Operation = operation.ToString();
                log.Notes = notes;
                log.TimeStamp = DateTime.Now;
                log.InsertRecordOnSubmit(this);
                SubmitChanges();
            }
            catch (Exception ex)
            {
                DCDImportLogger.Log.Error("Error writing log message to database", ex);
            }

            if (result == DCDImporter.RecordImportResult.Failure)
            {
                if (e != null)
                {
                    DCDImportLogger.Log.Error("Error processing record " + recordId + ".  " + notes, e);
                }
                else
                {
                    DCDImportLogger.Log.Error("Error processing record " + recordId + ".  " + notes);
                }
            }
            else
            {
                DCDImportLogger.Log.Info("Successfully processed record " + recordId + ".");
            }
        }

        public Deal GetDeal(int recordId)
        {
            Deal deal = Deals.FirstOrDefault(c => c.RecordId == recordId);
            if (deal != null)
            {
                return deal;
            }
            return null;
        }

        public Deal GetDeal(string recordNumber)
        {
            Deal deal = Deals.FirstOrDefault(c => c.RecordNumber == recordNumber);
            if (deal != null)
            {
                return deal;
            }
            return null;
        }

        public Company GetCompany(int recordId)
        {
            Company company = Companies.FirstOrDefault(c => c.RecordId == recordId);
            if (company != null)
            {
                return company;
            }
            return null;
        }

        public Company GetCompany(string recordNumber)
        {
            Company company = Companies.FirstOrDefault(c => c.RecordNumber == recordNumber);
            if (company != null)
            {
                return company;
            }
            return null;
        }

        public Drug GetDrug(int recordId)
        {
            Drug drug = Drugs.FirstOrDefault(c => c.RecordId == recordId);
            if (drug != null)
            {
                return drug;
            }
            return null;
        }

        public Drug GetDrug(string recordNumber)
        {
            Drug drug = Drugs.FirstOrDefault(c => c.RecordNumber == recordNumber);
            if (drug != null)
            {
                return drug;
            }
            return null;
        }

        public IDCD GetRecord<T>(int recordId) where T : IDCD
        {
            if (typeof(T) == typeof(Deal))
            {
                return Deals.FirstOrDefault(r => r.RecordId == recordId);
            }
            else if (typeof(T) == typeof(Company))
            {
                return Companies.FirstOrDefault(r => r.RecordId == recordId);
            }
            else if (typeof(T) == typeof(Drug))
            {
                return Drugs.FirstOrDefault(r => r.RecordId == recordId);
            }
            return null;
        }

        public List<ImportLog> GetImports(DateTime startDate, DateTime endDate)
        {
            //change the span to be: start 12am to end 11:59pm
            DateTime start = startDate.Date;
            DateTime end = GeneralUtil.GetEndOfDay(endDate);

            return ImportLogs.Where(i => i.ImportStart <= end && i.ImportEnd >= start).OrderBy(i => i.ImportStart).ToList();
        }

        public int GetTotalRecords(ImportLog log)
        {
            int total = 0;
            total += DealRecordImportLogs.Where(d => d.ImportLog.Id == log.Id).Count();
            total += CompanyRecordImportLogs.Where(c => c.ImportLog.Id == log.Id).Count();
            total += DrugRecordImportLogs.Where(d => d.ImportLog.Id == log.Id).Count();
            return total;
        }

        public int GetTotalSuccess(ImportLog log)
        {
            int total = 0;
            total += DealRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == DCDImporter.RecordImportResult.Success.ToString()).Count();
            total += CompanyRecordImportLogs.Where(c => c.ImportLog.Id == log.Id && c.Result == DCDImporter.RecordImportResult.Success.ToString()).Count();
            total += DrugRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == DCDImporter.RecordImportResult.Success.ToString()).Count();
            return total;
        }

        public int GetTotalSkipped(ImportLog log)
        {
            int total = 0;
            total += DealRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == DCDImporter.RecordImportResult.Skipped.ToString()).Count();
            total += CompanyRecordImportLogs.Where(c => c.ImportLog.Id == log.Id && c.Result == DCDImporter.RecordImportResult.Skipped.ToString()).Count();
            total += DrugRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == DCDImporter.RecordImportResult.Skipped.ToString()).Count();
            return total;
        }

        public int GetTotalFailed(ImportLog log)
        {
            int total = 0;
            total += DealRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == DCDImporter.RecordImportResult.Failure.ToString()).Count();
            total += CompanyRecordImportLogs.Where(c => c.ImportLog.Id == log.Id && c.Result == DCDImporter.RecordImportResult.Failure.ToString()).Count();
            total += DrugRecordImportLogs.Where(d => d.ImportLog.Id == log.Id && d.Result == DCDImporter.RecordImportResult.Failure.ToString()).Count();
            return total;
        }

        public List<IDCDRecordImportLog> GetRecordsForImport(int importId)
        {
            List<IDCDRecordImportLog> records = new List<IDCDRecordImportLog>();
            records.AddRange(DealRecordImportLogs.Where(d => d.ImportId == importId).Select(d => (IDCDRecordImportLog)d));
            records.AddRange(CompanyRecordImportLogs.Where(c => c.ImportId == importId).Select(c => (IDCDRecordImportLog)c));
            records.AddRange(DrugRecordImportLogs.Where(d => d.ImportId == importId).Select(d => (IDCDRecordImportLog)d));
            return records;
        }

        public bool AddRelatedCompanies(int companyId, List<CompanyPath> paths)
        {
            try
            {
                foreach (CompanyPath path in paths)
                {
                    RelatedCompany relatedCompany =
                        RelatedCompanies.FirstOrDefault(
                            c =>
                            c.Company.RecordId == companyId && c.RelatedCompanyRecordNumber == path.id);
                    if (relatedCompany == null)
                    {
                        relatedCompany = new RelatedCompany();
                        relatedCompany.CompanyRecordId = companyId;
                        relatedCompany.RelatedCompanyRecordNumber = path.id;
                        relatedCompany.RelatedCompanyPath = path.Value;
                        RelatedCompanies.InsertOnSubmit(relatedCompany);
                    }
                    else
                    {
                        relatedCompany.RelatedCompanyPath = path.Value;
                    }
                }
                SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
