﻿using System;
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
        // api/Report/?ColNum=3&type=Groups
        // [{"FirstCol":"A1","SecondCol":"A2","ThirdCol":"A3"},{"FirstCol":"B1","SecondCol":"B2","ThirdCol":"B3"}]
        public HttpResponseMessage Post(int ColNum, string type, List<Report> mlist )
        {
            
            string location ="~\\Reports\\"+type+"\\";
            string filename = type + DateTime.Now.ToString("dd_MM_yy_hh_mm")+".pdf";
            var root = HttpContext.Current.Server.MapPath(location);
            Directory.CreateDirectory(root);
            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream(root + filename, FileMode.Create));
            PdfPTable table = new PdfPTable(ColNum);
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(root + "Logo.jpg");
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

            PdfPCell cell = new PdfPCell(new Phrase(type));
            cell.Colspan = ColNum;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            foreach (Report r in mlist)
            {

                table.AddCell(r.FirstCol);
                table.AddCell(r.SecondCol);
                table.AddCell(r.ThirdCol);
            }
            
            document.Open();
            document.Add(logo);
            document.Add(table);

            
            
            document.Close();
            return Request.CreateResponse(HttpStatusCode.OK, root + filename);
            
        }
    }
}