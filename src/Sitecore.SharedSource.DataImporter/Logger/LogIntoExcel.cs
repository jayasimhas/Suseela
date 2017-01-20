using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Sitecore.SharedSource.DataImporter.Logger
{
    public static class LogIntoExcel
    {
        public static void CMCReport(string articleId, Dictionary<string, string> logValues,string publication)
        {
            
            string strfilepath = WebConfigurationManager.AppSettings["Logging_path"];

            using (var xlPackage = new ExcelPackage())
            {
                using (var stream = new FileStream(strfilepath, FileMode.Open))
                {
                    xlPackage.Load(stream);

                    var ws = xlPackage.Workbook.Worksheets.SingleOrDefault(x => x.Name == publication);

                    

                    DataTable dt = WorksheetToDataTable(ws);

                    //var rowsToUpdate = dt.AsEnumerable().Where(r => r.Field<string>("ArticleId") == articleId);


                    //if (rowsToUpdate.ToList().Count() > 0)
                    //{
                    //    foreach (DataRow dr in dt.Rows)
                    //    {

                    //        if (System.Convert.ToString(dr["ArticleId"]) == articleId)
                    //        {

                    //            //for (int i = 0; i < colNames.Length; i++)
                    //            //{

                    //            //    dr[colNames[i]] = colValues[i];
                    //            //}

                    //            foreach (KeyValuePair<string, string> l in logValues)
                    //            {
                    //                if (!String.IsNullOrEmpty(l.Value))

                    //                    if (l.Value.EndsWith(","))
                    //                        dr[l.Key] = l.Value.Remove(l.Value.Length - 1);

                    //                    else
                    //                    {
                    //                        dr[l.Key] = l.Value;
                    //                    }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                        DataRow dr = dt.NewRow();

                        dr["ArticleId"] = articleId;

                        //for (int i = 0; i < colNames.Length; i++)
                        //{

                        //    dr[colNames[i]] = colValues[i];
                        //}

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

                   // }

                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                }

                //xlPackage.Save();

                Byte[] bin = xlPackage.GetAsByteArray();
                File.WriteAllBytes(strfilepath, bin);
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
    }
}
