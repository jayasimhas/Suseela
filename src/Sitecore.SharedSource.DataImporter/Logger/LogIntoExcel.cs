using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sitecore.SharedSource.DataImporter.Logger
{
    public static class LogIntoExcel
    {
        public static void CMCReport(string articleId, Dictionary<string, string> logValues, string publication)
        {
            string strfilepath = string.Format(@"{0}CMCLogs\Content Migration Checks_{1}.xlsx",
                HttpRuntime.AppDomainAppPath, DateTime.Now.ToString("yyyy.MM.dd.H"));

            FileInfo fileInfo = new FileInfo(strfilepath);

            using (var xlPackage = new ExcelPackage(fileInfo))
            {
                var ws = xlPackage.Workbook.Worksheets.SingleOrDefault(x => x.Name == publication);


                bool isNewfile = false;
                if (ws == null)
                {
                    isNewfile = true;
                    ws = xlPackage.Workbook.Worksheets.Add(publication);
                }

                DataTable dt = new DataTable();
                if (isNewfile)
                    dt = GetDataTable(publication);
                else
                    dt = WorksheetToDataTable(ws);

                DataRow dr = dt.NewRow();

                dr["ArticleId"] = articleId;

                foreach (KeyValuePair<string, string> l in logValues)
                {

                    if (!String.IsNullOrEmpty(l.Value))

                        if (l.Value.EndsWith(","))
                            dr[l.Key] = l.Value.Remove(l.Value.Length - 1);

                        else
                        {
                            dr[l.Key] = l.Value;
                        }
                }

                dt.Rows.Add(dr);

                ws.Cells["A1"].LoadFromDataTable(dt, true);
                xlPackage.Save();
            }


        }

        private static DataTable WorksheetToDataTable(ExcelWorksheet oSheet)
        {
            int totalRows = oSheet.Dimension.End.Row;
            int totalCols = oSheet.Dimension.End.Column;
            DataTable dt = new DataTable(oSheet.Name);
            DataRow dr = null;
            for (int i = 1; i <= totalRows; i++)
            {
                if (i > 1) dr = dt.Rows.Add();
                for (int j = 1; j <= totalCols; j++)
                {
                    if (i == 1)
                        dt.Columns.Add(oSheet.Cells[i, j].Value.ToString());
                    else
                        dr[j - 1] = System.Convert.ToString(oSheet.Cells[i, j].Value);
                }
            }
            return dt;
        }

        //private static void CreateExcel(string publication)
        //{

        //    string fileName = string.Format(@"{0}Content Migration Checks_{1}.xlsx",
        //        HttpRuntime.AppDomainAppPath, DateTime.Now.ToString("yyyy.MM.dd.H.mm") + ".xlsx");

        //    //var outputDir = context.Server.MapPath("/media/generatedfiles/");
        //    //string logPath = string.Format(@"{0}sitecore modules\Shell\Data Import\logs\{1}.{2}.{3}.csv",
        //    //                        HttpRuntime.AppDomainAppPath, importItem.DisplayName.Replace(" ", "-"),
        //    //                        DateTime.Now.ToString("yyyy.MM.dd.H.mm.ss"),
        //    //                        kvp.Key);


        //    //if (!Directory.Exists(fileName))
        //    //{
        //    var file = new FileInfo(fileName);

        //    using (var stream = new MemoryStream())
        //    {

        //        using (var xlPackage = new ExcelPackage(file))
        //        {

        //            var ws = xlPackage.Workbook.Worksheets.Add(publication);

        //            var colHeaders = GetColumnsNames(string.Format(publication, "ColHeaders"));

        //            var colNames = colHeaders.Split(',');

        //            for (int i = 0; i >= colNames.Length; i++)
        //            {
        //                int j = i + 1;
        //                ws.Cells[1, j].Value = colNames[i];

        //            }

        //            xlPackage.Save();
        //        }
        //    }

        //}

        private static DataTable GetDataTable(string publication)
        {

            DataTable dt = new DataTable(publication);

            string colHeaders = GetColumnsNames(publication + "ColHeaders");

            var columns = colHeaders.Split(',');

            foreach (var i in columns)
            {
                dt.Columns.Add(i, typeof(string));
            }

            return dt;
        }

        private static string GetColumnsNames(string colHeaders)
        {
            return System.Convert.ToString(ConfigurationManager.AppSettings[colHeaders]);

        }
    }
}
