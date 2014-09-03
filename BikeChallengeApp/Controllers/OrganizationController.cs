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




namespace BikeChallengeApp.Controller
{

  
    public class OrganizationController : ApiController
    {
        LogFiles lf = new LogFiles();
        int return_val = 0;
        DBservices dbs = new DBservices();
        // GET organization 
        // api/Organization?orgname=[The name of the organization] - Not case sensative
        [Authorize]
        public DataTable GetUser(string orgname)
        {
            orgname = orgname.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(6, orgname);
            return dbs.dt;
        }

        [Authorize]
        public DataTable GetAll()
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(7, "Organizations", "Organization");
            return dbs.dt;
        }

        // POST / Insert organization into the DB 
        //{ "Organizationname":"MedaTech", "OrganizationCity": "טירת הכרמל", "OrganizationImage":"[Image Location]" , "OrganizationType":"הייטק"}
        [Authorize]
        public string updateDB([FromBody]Organization org)
        {
            List<Object> mlist = new List<Object>();
            mlist.Add(org);
            try
            {
                return_val = dbs.InsertDatabase(mlist);     
            }
            catch (Exception ex)
            {
                string Response = ("Error updating the Organization database " + ex.Message);
                lf.Main("Organizations", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        // DELETE Organization 
        // api/Organization?orgname=organizationanme
        [Authorize(Users = "bcadministrator")]
        public string DeleteOrganization(string orgname)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            string Response = "";
            orgname = orgname.Replace("'", "''");
            try
            {
                return_val = dbs.DeleteDatabase("Organizations", orgname);
            }
            catch (Exception ex)
            {
                Response = ("Error in the Delete process the Organization from the database " + ex.Message);
                lf.Main("Organizations", Response);
                return "Error";
            }
            Response = "The Organization " + orgname + " was Deleted from the Event";
            lf.Main("Organizations", Response);
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        

    }
}
