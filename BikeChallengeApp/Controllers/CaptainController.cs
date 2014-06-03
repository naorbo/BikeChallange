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
    public class CaptainController : ApiController
    {

        // Put api/captain?cap_usr=""&new_cap_usr=""
        // cap_usr-"The user name of old captain", new_cap_usr-"The user name of new captain"
        public string Put(string cap_usr, string new_cap_usr)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            Rider rdr = new Rider();
            try
            {
                return_val = dbs.updateRiderInDatabase(rdr, cap_usr, new_cap_usr);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to switch the Captain(user) " + cap_usr + " in the database: " + ex.Message);
                lf.Main("Users", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        // POST - Insert new Ride into the DB 
        // api/Captain
        //{"UserName":"name of the user you want to insert a ride", "RideType":"" , "RideLength":10, "RideSource":"A" ,"RideDate":"01-01-2014" "RideDestination":"B" }
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
                lf.Main("Captain", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

    }
}