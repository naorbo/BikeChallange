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

namespace BikeChallengeApp.Controllers
{
    public class StatController : ApiController
    {
        
        LogFiles lf = new LogFiles();
        /*
         int return_val = 0;
        // POST - Insert new Ride into the DB 
        // api/Rides
        //{"UserName":"tester1", "RideType":"" , "RideLength":10, "RideSource":"A" , "RideDestination":"B" }
        public string updateDB([FromBody]Rides rds)
        {
            DBservices dbs = new DBservices();
            try
            {
                return_val = dbs.insertRide(rds);
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
        public string updateDB(string username, string routename, string ridedate, string roundtrip)
        {
            DBservices dbs = new DBservices();
            try
            {
                return_val = dbs.insertRideFromRoute(username, routename, ridedate, roundtrip);
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
        */
        // GET Stats per USERNAME
        // api/Stats?date1=06-09-1985&date2=01-01-2014&username=[The username of the rider]
        public DataTable GetUser( string date1, string date2, string username)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(10, date1, date2, username);
            return dbs.dt;
        }
        // GET Stats per Group
        // api/Stats?grpname=[The name of the group]&orgname=[The name of the organizations]&date1=06-09-1985&date2=01-01-2014
        public DataTable GetUser( string grpname, string orgname, string date1, string date2)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(11, grpname, orgname, date1, date2);
            return dbs.dt;
        }
    }
}