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

        public DataTable GetAll()
        {
            Organization tmp = new Organization();

            DataTable dt = tmp.readData();


            return dt;
        }

        // POST / Insert organization into the DB 
        //{ "Organizationname":"qwerty", "OrganizationCity": 0, "OrganizationDes":"john q" , "OrganizationEmail":"qwerty", "OrganizationAddress": "12345qwerty", "OrganizationPhone":"john q" , "OrganizationType":"היט"}
        public bool updateDB([FromBody]Organization org)
        {
            //Organization org = new Organization(organizationName, organizationCity, organizationDes, organizationEmail,  organizationAddress, organizationPhone, organizationType);
            
            try
            {
                org.updateDatabase(org);
                
            }
            catch (Exception ex)
            {
                string Response = ("Error updating the Organization database " + ex.Message);
                return false;
            }
            return true;
        }
        

    }
}
