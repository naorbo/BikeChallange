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
        //[{"UserEmail":"Rider@updated. Email","UserDes":"betaTester","UserFname":"עודד","UserLname":"מנשה","UserAddress":"העצמאות 1","UserPhone":"888888","ImagePath":"ProfileImages\\Users\\betaTester\\ProfileImage07_6_2014.jpg","Gender":"M","Captain":false,"BirthDate":"2004-04-04","JoinDate":"2014-05-09","BicycleType":"הרים","RiderCity":"רעננה","GroupName":"פיתוח","GroupDes":"פיתוח","OrganizationName":"Microsoft","OrganizationDes":"Microsoft","OrganiztionImage":"ProfileImages\\Organizations\\Microsoft\\ProfileImage02_6_2014.png","OrgCity":"אבטליון"},
        //{"UserEmail":"carmela@harim.com","UserDes":"betaTester2","UserFname":"כרמלה","UserLname":"מנשה","UserAddress":"ההסתדרות 10","UserPhone":"054444666","ImagePath":"ProfileImages\\Users\\undefined\\ProfileImage09_5_2014.jpg","Gender":"M","Captain":false,"BirthDate":"1970-11-05","JoinDate":"2014-05-09","BicycleType":"הרים","RiderCity":"אבני חפץ","GroupName":"SecondGroup","GroupDes":"SecondGroup","OrganizationName":"EBAY","OrganizationDes":"EBAY","OrganiztionImage":"ProfileImages\\Organizations\\EBAY\\ProfileImage02_6_2014.png","OrgCity":"אבטליון"}]
        // [{"FirstCol":"A1","SecondCol":"A2","ThirdCol":"A3"},{"FirstCol":"B1","SecondCol":"B2","ThirdCol":"B3"}]
        //public HttpResponseMessage Post(int ColNum, string type, List<Report> mlist )
        public HttpResponseMessage Post(string type, [FromBody]DataTable mlist)
        {
            try
            {
                int ColNum = 12;
                string location = "~\\Reports\\" + type + "\\";
                string filename = type + DateTime.Now.ToString("dd_MM_yy_hh_mm_ss") + ".pdf";
                var root = HttpContext.Current.Server.MapPath(location);

                Directory.CreateDirectory(root);
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(root + filename, FileMode.Create));
                PdfPTable table = new PdfPTable(ColNum);
                BaseFont bf = BaseFont.CreateFont(root + "/arial.ttf", BaseFont.IDENTITY_H, true);
                Font font = new Font(bf, 14);
                //BaseFont unicode = BaseFont.CreateFont(root + "mriam.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
               // Font bold = new Font(unicode, 14);
               // iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(root + "Logo.jpg");

                table.WidthPercentage = 100;
                //fix the absolute width of the table
                table.LockedWidth = false;

                //relative col widths in proportions - 1/3 and 2/3
                float[] widths = new float[] { 6f, 6f, 9f, 9f, 9f, 9f, 7f, 9f, 9f, 9f, 9f, 9f};
                table.SetWidths(widths);
                table.HorizontalAlignment = 1;
                //leave a gap before and after the table
                table.SpacingBefore = 0f;
                table.SpacingAfter = 0f;

                PdfPCell cell = new PdfPCell(new Phrase(type));
                cell.Colspan = ColNum;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                
                
                // Fix the path to the font if needed
                
                /*ColumnText column = new ColumnText();
                column.setSimpleColumn(36, 770, 569, 36);
                column.setRunDirection(PdfWriter.RUN_DIRECTION_RTL);
                String text = "הטקסט שלך בעברית"; // Your text in hebrew
                column.addElement(new Paragraph(text, font));
                column.go();
                document.close();*/

                foreach (DataColumn c in mlist.Columns)
                {
                    if ((c.ColumnName == "BicycleType") || (c.ColumnName == "ImagePath") || (c.ColumnName == "Captain") || (c.ColumnName == "GroupName") || (c.ColumnName == "OrganiztionImage") || (c.ColumnName == "OrganizationName") || (c.ColumnName == "OrgCity"))
                    { continue; }
                    else
                        table.AddCell(c.ColumnName);
                }

                int j = 1;
                foreach (DataRow r in mlist.Rows)
                {
                    int i = 0;
                    //table.AddCell(j.ToString());
                    foreach (DataColumn c in mlist.Columns)
                    {
                        if ((c.ColumnName == "BicycleType") || (c.ColumnName == "ImagePath") || (c.ColumnName == "Captain") || (c.ColumnName == "GroupName") || (c.ColumnName == "OrganiztionImage") || (c.ColumnName == "OrganizationName") || (c.ColumnName == "OrgCity"))
                        { i++; }
                        else
                        {
                            table.AddCell(r.ItemArray[i].ToString());
                            i++;
                        }
                    }
                    j++;
                }

                document.Open();
                //document.Add(logo);
                document.Add(table);



                document.Close();
                return Request.CreateResponse(HttpStatusCode.OK, root + filename);

            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError,"Catch"); }
        }
    }
}