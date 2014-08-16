using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BikeChallengeApp.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace BikeChallengeApp.Controllers
{
    public class ExcelController : ApiController
    {
       /* public ActionResult HttpResponseMessage(string type, [FromBody]DataTable mlist)
        {
            GridView gv = new GridView();
            gv.DataSource = mlist.Rows[0].ItemArray[0];
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Marklist.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("StudentDetails");
        }

         public HttpResponseMessage Post(string type, [FromBody]DataTable mlist)
        {
            try
            {
                int ColNum = mlist.Columns.Count;
                string location = "~\\Reports\\" + type + "\\";
                string filename = type + DateTime.Now.ToString("dd_MM_yy_hh_mm_ss") + ".pdf";
                var root = HttpContext.Current.Server.MapPath(location);

                Directory.CreateDirectory(root);
                Document document = new Document();
                
                PdfWriter.GetInstance(document, new FileStream(root + filename, FileMode.Create));
                PdfPTable table = new PdfPTable(ColNum);
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(root + "Logo.jpg");

                table.WidthPercentage = 90;
                //fix the absolute width of the table
                table.LockedWidth = true;

                //relative col widths in proportions - 1/3 and 2/3
                float[] widths = new float[] { 10, 10, 10, 10, 20, 10, 10, 10, 10, };
                table.SetWidths(widths);
                table.HorizontalAlignment = 1;
                //leave a gap before and after the table
                table.SpacingBefore = 10f;
                table.SpacingAfter = 10f;

                PdfPCell cell = new PdfPCell(new Phrase(type));
                cell.Colspan = ColNum;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);


                foreach (DataColumn c in mlist.Columns)
                {
                    table.AddCell(c.ColumnName.ToString());
                }

                foreach (DataRow r in mlist.Rows)
                {
                    int i = 0;
                    foreach (DataColumn c in mlist.Columns)
                    {
                        table.AddCell(r.ItemArray[i].ToString());
                        i++;
                    }
                }

                document.Open();
                document.Add(logo);
                document.Add(table);



                document.Close();
                return Request.CreateResponse(HttpStatusCode.OK, root + filename);

            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError, "Catch"); }
        }*/
    }
}