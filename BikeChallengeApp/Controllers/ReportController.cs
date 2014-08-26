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
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BikeChallengeApp.Controllers
{
    public class ReportController : ApiController
    {
        // The Data From:
        // api/Rider -> Get ALL RIDERS

        // api/Report?type=Users
        public HttpResponseMessage Post(string type, [FromBody]DataTable mlist)
        {
        
            LogFiles lf = new LogFiles();
            try
            {        
                string[] hstr = { "Email", "שם משתמש", "שם פרטי", "שם משפחה", "ת.לידה", "ת.הצטרפות", "עיר", "קבוצה", "ארגון" };
                int ColNum = hstr.Length;
                string location = "~\\Reports\\" + type + "\\";
                string slocation = "~\\sources\\reports\\" ;
                string filename = type + DateTime.Now.ToString("dd_MM_yy_hh_mm_ss") + ".pdf";
                var root = HttpContext.Current.Server.MapPath(location);
                var sroot = HttpContext.Current.Server.MapPath(slocation);

                Directory.CreateDirectory(root);
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(root + filename, FileMode.Create));

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(sroot + "Logo.png");

                document.Open();
                document.NewPage();
                BaseFont bfArialUniCode = BaseFont.CreateFont(sroot + "ariali.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(bfArialUniCode, 10);
                Font fontB = new Font(bfArialUniCode, 24);
                // add logo
                document.Add(logo);

                PdfPTable table = new PdfPTable(ColNum);
                PdfPTable htable = new PdfPTable(ColNum);

                table.DefaultCell.NoWrap = false;
                table.HorizontalAlignment = 1;
                htable.HorizontalAlignment = 1;
                table.LockedWidth = false;
                table.WidthPercentage = 110;
                
                //Create a regex expression to detect hebrew or arabic code points 
                const string regex_match_arabic_hebrew = @"[\u0600-\u06FF,\u0590-\u05FF]+";
                if (Regex.IsMatch("מה קורה", regex_match_arabic_hebrew, RegexOptions.IgnoreCase))
                {
                    table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    htable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
                //Title
                PdfPCell head = new PdfPCell(new Phrase(" רוכבים לעבודה - דו''ח רוכבים ", fontB));
                head.Border = Rectangle.NO_BORDER;
                head.Colspan = ColNum;
                htable.AddCell(head);
                htable.SpacingAfter = 20f;
                document.Add(htable);
                
                // Headers
                for (int x = 0; x < ColNum; x++)
                {
                    //Create a cell and add text to it 
                    PdfPCell text = new PdfPCell(new Phrase(hstr[x], font));
                    //Ensure that wrapping is on, otherwise Right to Left text will not display 
                    text.NoWrap = false;

                    //Add the cell to the table 
                    table.AddCell(text);
                }

                foreach (DataRow r in mlist.Rows)
                {
                    int i = 0;
                    //table.AddCell(j.ToString());
                    foreach (DataColumn c in mlist.Columns)
                    {
                        if ((c.ColumnName == "UserPhone") || (c.ColumnName == "UserAddress") || (c.ColumnName == "BicycleType") || (c.ColumnName == "ImagePath") || (c.ColumnName == "Gender") || (c.ColumnName == "Captain") || (c.ColumnName == "GroupName") || (c.ColumnName == "OrganiztionImage") || (c.ColumnName == "OrganizationName") || (c.ColumnName == "OrgCity"))
                        { i++; }
                        else
                        {

                            PdfPCell text = new PdfPCell(new Phrase(r.ItemArray[i].ToString(), font));
                            //Ensure that wrapping is on, otherwise Right to Left text will not display 
                            text.NoWrap = false;
                            if (i == 1)
                                text.Normalize();
                            
                            //Add the cell to the table 
                            table.AddCell(text);
                           // table1.AddCell(r.ItemArray[i].ToString());
                            i++;
                        }
                    }
                }
                //Add the table to the document 
                document.Add(table);
                /* table.WidthPercentage = 100;
                 //fix the absolute width of the table
                 table.LockedWidth = false;

                 //relative col widths in proportions - 1/3 and 2/3
                 float[] widths = new float[] { 9f, 9f, 10f, 9f, 9f, 8f, 9f, 9f, 9f, 9f, 9f };
                 table.SetWidths(widths);
                 table.HorizontalAlignment = 1;
                 //leave a gap before and after the table
                 table.SpacingBefore = 0f;
                 table.SpacingAfter = 0f;

                 PdfPCell cell = new PdfPCell(new Phrase(type));
                 cell.Colspan = ColNum;
                 cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                 table.AddCell(cell);



                 

                 
                
                
                 document.Add(table);
                
                 */
                

                document.Close();

                Process AcrobatReader = new Process();
                AcrobatReader.StartInfo.FileName = root + filename;
                AcrobatReader.Start();
                return Request.CreateResponse(HttpStatusCode.OK, root + filename);

            }
            catch(Exception ex)
            {
                lf.Main("Report", ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,"Catch");
            }
        }
    }
}