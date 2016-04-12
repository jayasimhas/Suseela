using Informa.Model.DCD;
using Informa.Models.DCD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
//using Elsevier.Library.DCD;
using Informa.Models.DCD;
using System.Collections.Specialized;
using System.Web;
using System.Collections;
//using Velir.Utilities;

namespace Informa.Views.Util.DCD
{
    public partial class DcdImportReport : System.Web.UI.Page
    {
        private const string sortParam = "sort";
        private const string sortDirectionParam = "direction";
        private const string startDateParam = "start";
        private const string endDateParam = "end";
        private const string reportParam = "report";
        private const string backParam = "back";
        private string startDate;
        private string endDate;
        private string sortType;
        private string sortDirection;
        private string report;
        private string backUrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            startDate = Request[startDateParam];
            endDate = Request[endDateParam];
            sortType = Request[sortParam];
            sortDirection = Request[sortDirectionParam];
            report = Request[reportParam];
            backUrl = Request[backParam];
            DCDContext dc = new DCDContext();
            if (!IsPostBack)
            {
                DCDManager dcdMgr = new DCDManager();
                //if there is a report parameter, show just that import
                if (!string.IsNullOrEmpty(report))
                {
                    ImportLog import = dcdMgr.GetImportLogById(int.Parse(report));
                    if (import != null)
                    {
                        litSingleImportTitle.Text = "Import: " + import.Id + ", " + import.FileName + "(" + import.ImportStart.ToShortDateString() + " " + import.ImportStart.ToShortTimeString() + ")";
                        phImports.Visible = false;
                        phSingleImport.Visible = true;
                        if (!string.IsNullOrEmpty(backUrl))
                        {
                            hlBack.NavigateUrl = backUrl;
                        }
                        else
                        {
                            hlBack.Visible = false;
                        }
                        DataTable results = new DataTable();
                        results.Columns.Add("Type");
                        results.Columns.Add("RecordId");
                        results.Columns.Add("RecordNumber");
                        results.Columns["RecordId"].DataType = typeof(int);
                        results.Columns.Add("Time");
                        results.Columns.Add("Operation");
                        results.Columns.Add("Result");
                        results.Columns.Add("Notes");
                        results.Columns["Time"].DataType = typeof(DateTime);
                        List<IDCDRecordImportLog> records = dcdMgr.GetRecordsForImport(int.Parse(report));
                        foreach (IDCDRecordImportLog log in records)
                        {
                            DataRow row = results.NewRow();
                            if (log is DealRecordImportLog)
                            {
                                row["Type"] = "Deal";

                                Deal record = dcdMgr.GetDealByRecordId(log.RecordId.Value);
                                if (record != null)
                                {
                                    row["RecordNumber"] = record.RecordNumber;
                                }
                            }
                            else if (log is CompanyRecordImportLog)
                            {
                                row["Type"] = "Company";
                                Company record = dcdMgr.GetCompanyByRecordId(log.RecordId.Value);
                                if (record != null)
                                {
                                    row["RecordNumber"] = record.RecordNumber;
                                }
                            }
                            else if (log is DrugRecordImportLog)
                            {
                                row["Type"] = "Drug";
                                Drug record = dcdMgr.GetDrugByRecordId(log.RecordId.Value);
                                if (record != null)
                                {
                                    row["RecordNumber"] = record.RecordNumber;
                                }
                            }
                            row["RecordId"] = log.RecordId;
                            row["Time"] = log.TimeStamp.ToShortDateString() + " " + log.TimeStamp.ToShortTimeString();
                            row["Operation"] = log.Operation;
                            row["Result"] = log.Result;
                            row["Notes"] = log.Notes;
                            results.Rows.Add(row);
                        }

                        //set default sort
                        if (string.IsNullOrEmpty(sortType))
                        {
                            sortType = "Time";
                        }
                        if (string.IsNullOrEmpty(sortDirection))
                        {
                            sortDirection = "desc";
                        }

                        DataView view = new DataView(results);

                        view.Sort = sortType + " " + sortDirection;
                        dgSingleImport.ShowHeader = true;
                        dgSingleImport.DataSource = view;
                        dgSingleImport.DataBind();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                    {
                        txtDateEnd.Text = DateTime.Today.ToShortDateString();
                        txtDateStart.Text = DateTime.Today.Subtract(new TimeSpan(7, 0, 0, 0)).ToShortDateString();
                        return;
                    }
                    else
                    {
                        txtDateStart.Text = startDate;
                        txtDateEnd.Text = endDate;
                    }
                    DateTime start;
                    DateTime end;
                    if (!DateTime.TryParse(startDate, out start))
                    {
                        dgResults.Visible = false;
                        return;
                    }
                    if (!DateTime.TryParse(endDate, out end))
                    {
                        dgResults.Visible = false;
                        return;
                    }
                    List<ImportLog> logs = dcdMgr.GetImports(start, end);

                    DataTable results = new DataTable();
                    results.Columns.Add("Id");
                    results.Columns["Id"].DataType = typeof(int);
                    results.Columns.Add("Start Date");
                    results.Columns.Add("End Date");
                    results.Columns.Add("File Name");
                    results.Columns.Add("Result");
                    results.Columns.Add("Total");
                    results.Columns.Add("Succeeded");
                    results.Columns.Add("Skipped");
                    results.Columns.Add("Failed");
                    results.Columns["Start Date"].DataType = typeof(DateTime);
                    results.Columns["End Date"].DataType = typeof(DateTime);
                    results.Columns["Total"].DataType = typeof(int);
                    results.Columns["Succeeded"].DataType = typeof(int);
                    results.Columns["Skipped"].DataType = typeof(int);
                    results.Columns["Failed"].DataType = typeof(int);
                    foreach (ImportLog log in logs)
                    {
                        DataRow row = results.NewRow();
                        row["Id"] = log.Id;
                        row["Start Date"] = log.ImportStart.ToShortDateString() + " " + log.ImportStart.ToShortTimeString();
                        row["End Date"] = log.ImportEnd.ToShortDateString() + " " + log.ImportEnd.ToShortTimeString();
                        row["File Name"] = log.FileName;
                        row["Result"] = log.Result;
                        row["Total"] = dcdMgr.GetTotalRecords(log);
                        row["Succeeded"] = dcdMgr.GetTotalSuccess(log);
                        row["Skipped"] = dcdMgr.GetTotalSkipped(log);
                        row["Failed"] = dcdMgr.GetTotalFailed(log);
                        results.Rows.Add(row);
                    }

                    //set default sort
                    if (string.IsNullOrEmpty(sortType))
                    {
                        sortType = "Start Date";
                    }
                    if (string.IsNullOrEmpty(sortDirection))
                    {
                        sortDirection = "desc";
                    }

                    DataView view = new DataView(results);

                    view.Sort = sortType + " " + sortDirection;
                    dgResults.ShowHeader = true;
                    dgResults.DataSource = view;
                    dgResults.DataBind();
                }
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string url = addParameterToUrl(Request, startDateParam, txtDateStart.Text);
            url = addParameterToUrl(url, endDateParam, txtDateEnd.Text);
            Response.Redirect(url);
        }

        protected void dgResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (sortType == e.SortExpression)
            {
                toggleDirection();
            }
            else
            {
                sortDirection = "asc";
            }
            sortType = e.SortExpression;
            string url = addParameterToUrl(Request, sortParam, sortType);
            url = addParameterToUrl(url, sortDirectionParam, sortDirection);
            Response.Redirect(url);
        }

        private void toggleDirection()
        {
            sortDirection = (sortDirection == "asc") ? "desc" : "asc";
        }

        protected void dgResults_rowDatabound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    DataControlFieldHeaderCell obj = (DataControlFieldHeaderCell)e.Row.Cells[i];
                    if (!String.IsNullOrEmpty(sortType) && obj.ContainingField.SortExpression == sortType)
                    {
                        if (sortDirection == "asc")
                        {
                            obj.Attributes.Add("class", "sortup");
                        }
                        else
                        {
                            obj.Attributes.Add("class", "sortdown");
                        }
                        break;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                if (dr != null)
                {
                    string reportUrl = ClearUrlParameters(Request);
                    reportUrl = addParameterToUrl(reportUrl, reportParam, dr["Id"].ToString());
                    reportUrl = addParameterToUrl(reportUrl, backParam, Request.RawUrl);
                    e.Row.Attributes["onclick"] = "window.location=\"" + reportUrl + "\"";
                }
            }
        }

        protected void dgSingleImport_rowDatabound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    DataControlFieldHeaderCell obj = (DataControlFieldHeaderCell)e.Row.Cells[i];
                    if (!String.IsNullOrEmpty(sortType) && obj.ContainingField.SortExpression == sortType)
                    {
                        if (sortDirection == "asc")
                        {
                            obj.Attributes.Add("class", "sortup");
                        }
                        else
                        {
                            obj.Attributes.Add("class", "sortdown");
                        }
                        break;
                    }
                }
            }
        }

        private string addParameterToUrl(HttpRequest request, string parameterName, string parameterValue)
        {
            return addParameterToUrl(request.Url.Scheme + "://" + request.Url.Host + request.RawUrl, parameterName, parameterValue);
        }

        private string addParameterToUrl(string fullUrl, string parameterName, string parameterValue)
        {
            try
            {
                Uri uri = new Uri(fullUrl);
                NameValueCollection parameters = HttpUtility.ParseQueryString(uri.Query);
                if (!string.IsNullOrEmpty(parameters[parameterName]))
                    parameters[parameterName] = parameterValue;
                else
                    parameters.Add(parameterName, parameterValue);
                return new UriBuilder(uri.Scheme, uri.Host)
                {
                    Path = uri.AbsolutePath,
                    Query = constructQueryString(parameters, false)
                }.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private string constructQueryString(NameValueCollection parameters, bool splitOnCommas)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in parameters.AllKeys.SelectMany(parameters.GetValues, (k, v) => new { key = k, value = v }))
            {
                query[item.key] = item.value;
            }

            return query.ToString();

            //if (parameters == null)
            //    return string.Empty;
            //return EnumerableExtensions.ToString((IEnumerable)parameters, "&", (Func<object, string>)(o =>
            //{
            //    string key = (string)o;
            //    string str = parameters[key];
            //    string[] strArray;
            //    if (!splitOnCommas)
            //        strArray = new string[1]
            //        {
            //str
            //        };
            //    else
            //        strArray = str.Split(new char[1]
            //        {
            //','
            //        }, StringSplitOptions.RemoveEmptyEntries);
            //    return EnumerableExtensions.ToString((IEnumerable)strArray, "&", (Func<object, string>)(s =>
            //    {
            //        if (s != null)
            //            return key + "=" + HttpUtility.UrlEncode((string)s).Replace("+", "%20");
            //        return key + "=";
            //    }));
            //}));
        }

        private string ClearUrlParameters(HttpRequest request)
        {
            return clearUrlParameters(request.Url.Scheme + "://" + request.Url.Host + request.RawUrl);
        }

        private string clearUrlParameters(string fullUrl)
        {
            try
            {
                Uri uri = new Uri(fullUrl);
                return new UriBuilder(uri.Scheme, uri.Host)
                {
                    Path = uri.AbsolutePath,
                    Query = string.Empty
                }.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}