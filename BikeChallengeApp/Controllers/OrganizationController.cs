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




namespace BikeChallengeApp.Controller
{

    //[Authorize]
    //[RoutePrefix("api/Organization")]
    public class OrganizationController : ApiController
    {
        LogFiles lf = new LogFiles();
        int return_val = 0;
        // GET organization 
        // api/Organization?orgname=[The name of the organization] - Not case sensative
        public DataTable GetUser(string orgname)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(4, orgname, "");
            return dbs.dt;
        }

        public DataTable GetAll()
        {
            Organization tmp = new Organization();
            DataTable dt = tmp.readData();
            return dt;
        }

        // POST / Insert organization into the DB 
        //{ "Organizationname":"MedaTech", "OrganizationCity": "טירת הכרמל", "OrganizationDes":"מידעטק טכנולוגיות" , "OrganizationImage":"[Image Location]" , "OrganizationType":"הייטק"}
        public string updateDB([FromBody]Organization org)
        {
            //Organization org = new Organization(organizationName, organizationCity, organizationDes, organizationEmail,  organizationAddress, organizationPhone, organizationType);
            DBservices dbs = new DBservices();
            try
            {
                return_val = dbs.insertOrganization(org);     
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
        

    }
}
