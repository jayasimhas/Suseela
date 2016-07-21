using Informa.Model.DCD;
using Informa.Models.DCD;
using PluginModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DealInfo = PluginModels.DealInfo;

namespace Informa.Web.Controllers
{
    [Route]
    public class GetDealInfoController : ApiController
    {
        [HttpGet]
        public DealInfo GetDealInfo(string recordNumber)
        {
            DealInfo dealInfo = new DealInfo();
            try
            {
                Deal deal = new DCDManager().GetDealByRecordNumber(recordNumber);

				dealInfo = new DealInfo
				{
					DealDate = deal.Created,
					ID = deal.RecordId.ToString(),
					LastUpdated = deal.LastModified,
					Name = deal.Title,
					RecordNumber = deal.RecordNumber,
                    Url = string.Format(Sitecore.Configuration.Settings.GetSetting("DCD.OldDealsURL"), recordNumber)
                };
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("GetDealInfo API error:", ex, "LogFileAppender");
            }

            return dealInfo;
        }
    }

    [Route]
    public class GetAllCompaniesController : ApiController
    {
        [HttpGet]
        public List<CompanyWrapper> GetAllCompanies()
        {
            List<CompanyWrapper> lstDbCompanies = null;
            try
            {
                List<Company> tempLstComp = new DCDManager().GetAllCompanies();

                lstDbCompanies = tempLstComp.Select(comp => new CompanyWrapper { Parent = null, RecordID = comp.RecordId, RecordNumber = comp.RecordNumber, RelatedCompanies = new List<CompanyWrapper>(), Title = comp.Title, }).ToList();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("GetAllCompanies API error:", ex, "LogFileAppender");
            }

            return lstDbCompanies;
        }
    }

    [Route]
    public class GetAllCompaniesWithRelatedController : ApiController
    {
        private static string logAccum = string.Empty;
        [HttpGet]
        public List<CompanyWrapper> GetAllCompaniesWithRelated()
        {
            List<CompanyWrapper> lstDbCompanies = null;
            try
            {
                lstDbCompanies = GetWithRelated().ToList();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("GetAllCompaniesWithRelated API error:", ex, "LogFileAppender");
            }

            return lstDbCompanies;
        }

        private IEnumerable<CompanyWrapper> GetWithRelated()
        {
            DCDManager dcdMgr = new DCDManager();

            // build a dictionary of all of the companies, key is the company id
            Dictionary<int, Company> allCompanies = dcdMgr.GetAllCompanies().ToDictionary(x => x.RecordId, x => x);

            // pull down a list of all related companies
            RelatedCompany[] allRelatedCompanies = dcdMgr.GetAllRelatedCompanies().ToArray();

            // our results dictionary, key is the company id
            Dictionary<int, CompanyWrapper> results = new Dictionary<int, CompanyWrapper>();

            // keep track of where a company is when it's inserted into "RelatedCompanies" so we don't have to dig through every companies
            // RelatedCompanies to try to find a company.
            Dictionary<int, List<CompanyWrapper>> pointers = new Dictionary<int, List<CompanyWrapper>>();

            // gather all of the "Root Companies" because there are a ton of duplicates.
            RelatedCompany[] rootCompanies =
                (from i in allRelatedCompanies
                 where i.CompanyRecordId.ToString() == i.RelatedCompanyRecordNumber
                 orderby i.RelatedCompanyPath ascending
                 select i).ToArray();

            // build out the wrappers we're going to return.
            foreach (var relatedCompany in rootCompanies)
            {
                Company currentCompany = allCompanies[relatedCompany.CompanyRecordId];
                CompanyWrapper wrapper = new CompanyWrapper { RecordID = currentCompany.RecordId, RecordNumber = currentCompany.RecordNumber, Title = currentCompany.Title, Parent = null };
                results.Add(relatedCompany.CompanyRecordId, wrapper);
            }

            // iterate through all of our root companies
            foreach (var relatedCompany in rootCompanies)
            {
                // split the path by '/' to get all of the seperate results.
                string[] pieces = relatedCompany.RelatedCompanyPath.Split('/');

                // ignore paths with no slash, because they don't have parents.
                if (pieces.Length == 1)
                {
                    continue;
                }

                // join all of the pieces except for the last piece to give us the parent of the current item
                string parent = string.Join("/", pieces.Take(pieces.Length - 1).ToArray());

                // get the parent from our root companies
                RelatedCompany parentCompany = rootCompanies.FirstOrDefault(x => x.RelatedCompanyPath == parent);

                // check to make sure it's legit and isn't itself for some reason.
                if (parentCompany == null || parentCompany.CompanyRecordId == relatedCompany.CompanyRecordId)
                {
                    continue;
                }

                // get the parent from the "results" dictionary
                CompanyWrapper parentWrapper = Get(results, pointers, parentCompany.CompanyRecordId, false);

                // check to make sure it exists.
                if (parentWrapper == null)
                {
                    continue;
                }

                // get the current item from the results collection and delete it from wherever it is.
                CompanyWrapper current = Get(results, pointers, relatedCompany.CompanyRecordId, true);

                if (current == null)
                {
                    continue;
                }

                // keep track of where we're putting it.
                pointers[current.RecordID] = parentWrapper.RelatedCompanies;

                if (parentWrapper.RelatedCompanies == null)
                    parentWrapper.RelatedCompanies = new List<CompanyWrapper>();

                // add it to it's parent.
                parentWrapper.RelatedCompanies.Add(current);
            }

            // just select the values (the CompanyWrappers)
            List<CompanyWrapper> returnWrappers = results.Select(x => x.Value).ToList();

            // recursively sort all of the results
            Sort(returnWrappers);

            return returnWrappers;
        }

        /// <summary>
        /// Basic sorting routine which will recursively sort a list of wrappers and any related companies
        /// </summary>
        /// <param name="wrappers"></param>
        private void Sort(List<CompanyWrapper> wrappers)
        {
            if (wrappers == null)
            {
                return;
            }

            wrappers.Sort((x, y) => String.CompareOrdinal(x.Title.ToLower(), y.Title.ToLower()));

            foreach (var companyWrapper in wrappers)
            {
                Sort(companyWrapper.RelatedCompanies);
            }
        }

        private CompanyWrapper Get(Dictionary<int, CompanyWrapper> results, Dictionary<int, List<CompanyWrapper>> pointers, int recordId, bool delete)
        {
            CompanyWrapper wrapper;
            if (results.TryGetValue(recordId, out wrapper) && delete)
            {
                results.Remove(recordId);
            }

            if (wrapper == null)
            {
                List<CompanyWrapper> parent;
                if (pointers.TryGetValue(recordId, out parent) && parent != null)
                {
                    wrapper = Get(parent, recordId, delete);
                }
            }

            return wrapper;
        }

        private CompanyWrapper Get(List<CompanyWrapper> wrappers, int recordId, bool delete)
        {
            CompanyWrapper wrapper = wrappers.FirstOrDefault(x => x.RecordID == recordId);

            if (wrapper != null && delete)
            {
                wrappers.Remove(wrapper);
            }

            return wrapper;
        }
    }
}
