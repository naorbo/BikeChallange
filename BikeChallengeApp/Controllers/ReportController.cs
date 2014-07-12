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
    public class ReportController : ApiController
    {
        // api/Report/
        // {"GroupName":"1", "OrganizationName":"1"},{"GroupName":"2", "OrganizationName":"2"}

        public string myAction([FromBody]List<Group> grp)
        {
            var resolveRequest = Request.Content.ReadAsStringAsync();
            
            string s = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            List<Group> model = new List<Group>();

           // resolveRequest.Seek(0, SeekOrigin.Begin);
            //string jsonString = new StreamReader(resolveRequest).ReadToEnd();
            if ("" != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                model = (List<Group>)Newtonsoft.Json.JsonConvert.DeserializeObject(Request.Content.ToString(), typeof(List<Group>));
            }
            //Your operations..
            List<Object> mlist = new List<Object>();
            mlist.Add(grp);
            
            string str = "LOGO";
            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream(@"C:\Temp\test.pdf", FileMode.Create));
            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 400f;
            //fix the absolute width of the table
            table.LockedWidth = true;

            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 2f, 4f, 6f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 1;
            //leave a gap before and after the table
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            PdfPCell cell = new PdfPCell(new Phrase("Groups"));
            cell.Colspan = 3;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            //table.AddCell(grp.GroupDes);
            //table.AddCell(grp.OrganizationName);
            //table.AddCell(grp.GroupName);
            table.AddCell("Col 1 Row 2");
            table.AddCell("Col 2 Row 2");
            table.AddCell("Col 3 Row 2");

            document.Open();
            document.Add(table);

            Paragraph P = new Paragraph(str, FontFactory.GetFont("Arial", 10));

            //Paragraph P2 = new Paragraph(val.ToString(), FontFactory.GetFont("Arial", 10));
            document.Add(P);
            //document.Add(P2);
            document.Close();
            
            return "";
        }
    }
}