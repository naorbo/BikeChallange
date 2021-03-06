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
    public class RidesController : ApiController
    {
        // POST - Insert new Ride into the DB 
        // api/Rides
        //{"UserName":"tester1", "RideType":"" , "RideLength":10, "RideSource":"A" ,"RideDate":"01-01-2014" "RideDestination":"B" }
        [Authorize]
        public string updateDB([FromBody]Rides rds)
        {
            LogFiles lf = new LogFiles();
            if (DateTime.Now.Day.CompareTo(Convert.ToDateTime(rds.RideDate).Day) < 0 || DateTime.Now.Month.CompareTo(Convert.ToDateTime(rds.RideDate).Month) != 0) { lf.Main("Rides", "The Ride Date:" + rds.RideDate + " Can't be in the future or in a diffetent month "); return "Error"; }
            int return_val = 0;
            DBservices dbs = new DBservices();
            List<Object> mlist = new List<Object>();
            mlist.Add(rds);
            try
            {
                return_val = dbs.InsertDatabase(mlist);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to INSERT the new Ride to the database " + ex.Message);
                lf.Main("Rides", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        // POST - Insert new Ride From an exiting Route 
        // api/Rides?username=tester1&routename=[Existing Route Name]&ridedate=01-01-1985&roundtrip=True/False
        [Authorize]
        public string updateDB(string username, string routename, string ridedate, string roundtrip)
        {
            LogFiles lf = new LogFiles();
            if (DateTime.Now.Day.CompareTo(Convert.ToDateTime(ridedate).Day) < 0 || DateTime.Now.Month.CompareTo(Convert.ToDateTime(ridedate).Month) != 0) { lf.Main("Rides", "The Ride Date:" + ridedate + " Can't be in the future or in a diffetent month "); return "Error"; }
            List<Object> mlist = new List<Object>();
            int return_val = 0;
            DBservices dbs = new DBservices();
            Rides rds = new Rides();
            username = username.Replace("'", "''");
            mlist.Add(rds);
            try
            {
                return_val = dbs.InsertDatabase(mlist,username, routename, ridedate, roundtrip);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to INSERT the new Ride to the database " + ex.Message);
                lf.Main("Rides", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        // GET RIDE per USERNAME
        // api/Rides?username=[The username of the rider] - Not case sensative
        [Authorize]
        public DataTable GetUser(string username)
        {
            DBservices dbs = new DBservices();
            username = ( username == null ? "" : username);
            username = username.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(8, username);
            return dbs.dt;
        }
        // DELETE 
        // api/Rides?username=[UserName]&ridename=[RideName]
        [Authorize]
        public string Delete(string username, string ridename)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            string Response = "";
            username = username.Replace("'", "''");
            try
            {
                return_val = dbs.DeleteDatabase("Rides",username, ridename);
            }
            catch (Exception ex)
            {
                Response = ("Error in the Delete process of the Ride " + ridename + " of " + username + ", " + ex.Message);
                lf.Main("Rides", Response);
                return "Error";
            }
            Response = "The Ride " + ridename + " was Deleted from the Database";
            lf.Main("Rides", Response);
            if (return_val == 0) { return "Error"; }
            return "Success";
        }
    }
}