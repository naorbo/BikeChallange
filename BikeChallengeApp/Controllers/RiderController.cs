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

namespace BikeChallengeApp.Controllers
{
    public class RiderController : ApiController
    {
        // GET RIDER PER GRUOP
        // api/Rider?groupname=[The name of the group] - Not case sensative
        public DataTable Get(string groupname)
        {
            Rider rdr = new Rider();

            DataTable dt = rdr.readDataPerGroup(groupname);

            return dt;
        }

        // GET ALL RIDERS
        // api/Rider
        public DataTable GetAll()
        {
            Rider rdr = new Rider();

            DataTable dt = rdr.readData();

            return dt;
        }

        // POST - Insert new Rider into the DB 
        //{"RiderEmail":"Moshe@Moshe.COM", "RiderFname":"Moshe" , "RiderLname":"Moshe", "Gender": "זכר", "RiderAddress":"יצMoshe 29" ,  "City":"חיפה", "RiderPhone":"0508878900",  "BicycleType": "חשמליות" , "ImagePath":"pic location" , "BirthDate":"01-01-1985", "UserName":"tester4", "Captain":1, "Organization":"orgname", "Group":"groupname"}
        public string updateDB([FromBody]Rider rdr)
        {
            try
            {
                rdr.updateDatabase(rdr);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to INSERT the new Rider(user) to the database " + ex.Message);
                return "Error";
            }
            return "Success";
        }

        // DELETE 
        // api/Rider?username=[UserName]
        public string Delete(string username)
        {
            DBservices dbs = new DBservices();
            string Response= "";
            try
            {
                dbs.delteRider(username);
            }
            catch (Exception ex)
            {
                 Response = ("Error in the Delete process the Rider(user) database " + ex.Message);
                return "Error";
            }
             Response = "The Rider " + username +" was Deleted from the Database";
             return "Success";
        }
        // PUT api/Rider?username=[UserName you want to update]
        //{"RiderEmail":"Rider Email", "RiderFname":"Updated val" , "RiderLname":"Updated val", "Gender": "Updated val", "RiderAddress":"Updated val" ,  "City":"Updated val", "RiderPhone":"Updated val",  "BicycleType": "Updated val" , "ImagePath":"Updated val" , "BirthDate":"Updated val", "UserName":"username of the updated rider", "Captain":1, "Organization":"Updated val", "Group":"Updated val"}
        public string Put(string username, [FromBody]Rider rdr)
        {
            LogFiles lf = new LogFiles();
            try
            {
                DBservices dbs = new DBservices();
                dbs.updateRiderInDatabase(username, rdr);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to Update the Rider(user) " + username + "to the database " + ex.Message);
                lf.Main("Groups", Response);
                return "Error";
            }
            return "Success";
        }
    }
}