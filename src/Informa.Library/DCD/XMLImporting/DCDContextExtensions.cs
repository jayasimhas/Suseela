using Informa.Models.DCD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.DCD.XMLImporting
{
    public static class DCDContextExtensions
    {
        public static bool ProcessRecord<RecordType, RecordLogType>(this DCDContext dc, Record record, ImportLog importLog)
            where RecordType : IDCD, new()
             where RecordLogType : IDCDRecordImportLog, new()
        {
            //First see if the record already exists
            IDCD entry = dc.GetRecord<RecordType>(record.Identification.RecordId);

            //Process delete
            if (record.Command == CommandType.Delete)
            {
                if (entry == null)
                {
                    dc.CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, RecordImportResult.Skipped, RecordImportOperation.Delete, "Cannot delete. Record doesn't exist.");
                    return true;
                }
                else
                {
                    try
                    {
                        entry.DeleteRecordOnSubmit(dc);
                        dc.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        dc.CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, RecordImportResult.Failure, RecordImportOperation.Delete, "Error deleting record.", e);
                        return false;
                    }

                    dc.CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, RecordImportResult.Success, RecordImportOperation.Delete, string.Empty);
                    return true;
                }
            }
            //Upsert
            else
            {
                RecordImportOperation op = RecordImportOperation.Insert;
                IDCD processingRecord;
                try
                {
                    //If the record already exists
                    if (entry != null)
                    {
                        op = RecordImportOperation.Update;
                        //Only records that are newer than the records in the feed will be skipped
                        if (entry.LastModified > record.Identification.LastModified)
                        {
                            dc.CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, RecordImportResult.Skipped, op, "Existing record has a newer Last Modified Date.");
                            return true;
                        }
                        else if (record.Content == null || string.IsNullOrEmpty(record.Content.InnerXml))
                        {
                            dc.CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, RecordImportResult.Skipped, op, "Record does not have any content.");
                            return true;
                        }
                        else
                        {
                            entry.RecordNumber = record.Identification.RecordNumber;
                            entry.Title = XMLFileUtilities.RemoveBrackets(record.Identification.Title);
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
                        op = RecordImportOperation.Insert;
                        processingRecord = new RecordType();
                        processingRecord.RecordId = record.Identification.RecordId;
                        processingRecord.RecordNumber = record.Identification.RecordNumber;
                        processingRecord.Title = XMLFileUtilities.RemoveBrackets(record.Identification.Title);
                        processingRecord.LastModified = record.Identification.LastModified;
                        processingRecord.Created = record.Identification.Created;
                        processingRecord.Published = record.Identification.Created;
                        if (!record.Identification.Published.ToString().Equals(DateTime.MinValue.ToString()))
                        {
                            processingRecord.Published = record.Identification.Published;
                        }
                        processingRecord.Content = record.Content.InnerXml;
                        processingRecord.InsertRecordOnSubmit(dc);
                    }
                    dc.SubmitChanges();
                }
                catch (Exception e)
                {
                    dc.CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, RecordImportResult.Failure, op, "Error inserting/updating record.", e);
                    return false;
                }

                dc.CreateRecordLogEntry<RecordLogType>(record.Identification.RecordId, importLog, RecordImportResult.Success, op, string.Empty);
                return true;
            }
        }

        public static IDCD GetRecord<T>(this DCDContext dc, int recordId) where T : IDCD
        {
            if (typeof(T) == typeof(Deal))
            {
                return dc.Deals.FirstOrDefault(r => r.RecordId == recordId);
            }
            else if (typeof(T) == typeof(Informa.Models.DCD.Company))
            {
                return dc.Companies.FirstOrDefault(r => r.RecordId == recordId);
            }
            else if (typeof(T) == typeof(Drug))
            {
                return dc.Drugs.FirstOrDefault(r => r.RecordId == recordId);
            }
            return null;
        }

        public static void CreateImportLogEntry(this DCDContext dc, DateTime start, string fileName, out ImportLog log)
        {
            try
            {
                log = new ImportLog();
                log.ImportStart = start;
                log.ImportEnd = start;
                log.Result = ImportResult.InProgress.ToString();
                log.FileName = fileName;

                dc.ImportLogs.InsertOnSubmit(log);
                dc.SubmitChanges();
                //return log;
            }
            catch (Exception ex)
            {
                DCDImportLogger.Error("Error writing log message to database", ex);
                log = null;
            }
            //return null;
        }

        public static void UpdateImportLogEntry(this DCDContext dc, ImportLog log)
        {
            try
            {
                var existingLog = dc.ImportLogs.FirstOrDefault(l => l.Id == log.Id);
                if (existingLog == null)
                    dc.ImportLogs.InsertOnSubmit(log);
                else
                {
                    existingLog.FileName = log.FileName;
                    existingLog.ImportEnd = log.ImportEnd;
                    existingLog.ImportStart = log.ImportStart;
                    existingLog.Notes = log.Notes;
                    existingLog.Result = log.Result;
                }

                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                DCDImportLogger.Error("Error writing log message to database", ex);
            }
        }

        public static void CreateRecordLogEntry<RecordLogType>(this DCDContext dc, int recordId, ImportLog importLog, RecordImportResult result, RecordImportOperation operation, string notes, Exception e) where RecordLogType : IDCDRecordImportLog, new()
        {
            try
            {
                RecordLogType log = new RecordLogType();
                log.RecordId = recordId;
                log.ImportLog = importLog;
                log.Result = result.ToString();
                log.Operation = operation.ToString();
                log.Notes = notes + (e != null ? $"[{e.ToString()}]" : string.Empty);
                log.TimeStamp = DateTime.Now;
                log.InsertRecordOnSubmit(dc);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                DCDImportLogger.Error("Error writing log message to database", ex);
            }

            if (result == RecordImportResult.Failure)
            {
                if (e != null)
                {
                    DCDImportLogger.Error("Error processing record " + recordId + ".  " + notes, e);
                }
                else
                {
                    DCDImportLogger.Error("Error processing record " + recordId + ".  " + notes);
                }
            }
            else
            {
                DCDImportLogger.Info("Successfully processed record " + recordId + ".");
            }
        }

        public static void CreateRecordLogEntry<RecordLogType>(this DCDContext dc, int recordId, ImportLog importLog, RecordImportResult result, RecordImportOperation operation, string notes) where RecordLogType : IDCDRecordImportLog, new()
        {
            dc.CreateRecordLogEntry<RecordLogType>(recordId, importLog, result, operation, notes, null);
        }

        public static bool AddRelatedCompanies(this DCDContext dc, int companyId, List<CompanyPath> paths)
        {
            try
            {
                foreach (CompanyPath path in paths)
                {
                    RelatedCompany relatedCompany =
                        dc.RelatedCompanies.FirstOrDefault(
                            c =>
                            c.Company.RecordId == companyId && c.RelatedCompanyRecordNumber == path.id);
                    if (relatedCompany == null)
                    {
                        relatedCompany = new RelatedCompany();
                        relatedCompany.CompanyRecordId = companyId;
                        relatedCompany.RelatedCompanyRecordNumber = path.id;
                        relatedCompany.RelatedCompanyPath = path.Value;
                        dc.RelatedCompanies.InsertOnSubmit(relatedCompany);
                    }
                    else
                    {
                        relatedCompany.RelatedCompanyPath = path.Value;
                    }
                }
                dc.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
