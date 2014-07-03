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
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace BikeChallengeApp.Controllers
{
    public class GroupController : ApiController
    {
       
       

        // GET api/Group?orgname=[The name of the organization] - Not case sensative
        public DataTable Get(string orgname)
        {
            DBservices dbs = new DBservices();
            orgname = orgname.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(3, orgname);
            return dbs.dt;
        }

        // GET api/Group?grpname=[The name of the group]&orgname=[The name of the organization] - Not case sensative
        public DataTable GetGroup(string grpname, string orgname)
        {
            DBservices dbs = new DBservices();
            orgname = orgname.Replace("'", "''");
            grpname = grpname.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(4, grpname, orgname);
            return dbs.dt;
        }

        // GET api/Group
        public DataTable GetAll()
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(7, "Groups", "[Group]");
            return dbs.dt;
        }

        // POST api/Group
        // {"GroupName":"groupName", "OrganizationName":""}
        public string updateDB([FromBody]Group grp)
        {
            DBservices dbs = new DBservices();
            int return_val = 0;
            LogFiles lf = new LogFiles();
            List<Object> mlist = new List<Object>();
            mlist.Add(grp);
            try
            {
                return_val = dbs.InsertDatabase(mlist);
            }
            catch (Exception ex)
            {
                string Response = ("Error updating the Group database " + ex.Message);
                lf.Main("Groups", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }
        // DELETE Group 
        // api/Group?grpname=groupname&orgname=organizationanme
        public string DeleteGroup(string grpname, string orgname)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            string Response = "";
            grpname = grpname.Replace("'", "''");
            orgname = orgname.Replace("'", "''");
            try
            {
                return_val = dbs.DeleteDatabase("Groups", grpname, orgname);
            }
            catch (Exception ex)
            {
                Response = ("Error in the Delete process the group from an event database " + ex.Message);
                lf.Main("Groups", Response);
                return "Error";
            }
            Response = "The Group " + grpname + " was Deleted from the Event";
            lf.Main("Groups", Response);
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        public void PDF(int f)
        {
            string str = HttpContext.Current.ToString();
            
            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream(@"C:\Temp\test.pdf", FileMode.Create));
            document.Open();
            Paragraph P = new Paragraph(str, FontFactory.GetFont("Arial", 10));
            //Paragraph P2 = new Paragraph(val.ToString(), FontFactory.GetFont("Arial", 10));
            document.Add(P);
            //document.Add(P2);
            document.Close();
        }

     /*   private object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData
                    .GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            return null;
        }*/
    }
}