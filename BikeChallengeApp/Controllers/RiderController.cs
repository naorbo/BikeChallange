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
    public class RiderController : ApiController
    {
        // GET RIDER PER GRUOP
        // api/Rider?grpname=[The name of the group]&orgname=[The name of the organization] - Not case sensative
        public DataTable Get(string grpname, string orgname)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(1, grpname, orgname);
            return dbs.dt;
        }

        // GET RIDER from USERNAME
        // api/Rider?username=[The username of the rider] - Not case sensative
        public DataTable GetUser(string username)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(2, username);
            return dbs.dt;
        }

        // GET ALL RIDERS
        // api/Rider
        public DataTable GetAll()
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(7, "Users", "[User]");
            return dbs.dt;
        }

        // POST - Insert new Rider into the DB 
        //{"RiderEmail":"Moshe@Moshe.COM", "RiderFname":"Moshe" , "RiderLname":"Moshe", "Gender": "M", "RiderAddress":"יצMoshe 29" ,  "City":"חיפה", "RiderPhone":"0508878900",  "BicycleType": "חשמליות" , "ImagePath":"pic location" , "BirthDate":"01-01-1985", "UserName":"tester4", "Captain":1, "Organization":"orgname", "Group":"groupname"}
        public string updateDB([FromBody]Rider rdr)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            try
            {
                return_val = dbs.insertRider(rdr);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to INSERT the new Rider(user) to the database " + ex.Message);
                lf.Main("Users", Response);
                return "Error";
            }
            if (return_val != 2) { return "Error"; }
            return "Success";
        }

        // DELETE 
        // api/Rider?username=[UserName]
        public string Delete(string username)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            string Response= "";
            try
            {
                return_val = dbs.DeleteDatabase("Rider",username);
            }
            catch (Exception ex)
            {
                 Response = ("Error in the Delete process the Rider(user) database " + ex.Message);
                 lf.Main("Users", Response);
                return "Error";
            }
             Response = "The Rider " + username +" was Deleted from the Database";
             lf.Main("Users", Response);
             if (return_val == 0) { return "Error"; } 
            return "Success";
        }

        // PUT api/Rider?username=[UserName you want to update]
        //{"RiderEmail":"Rider@updated.Email", "RiderFname":"עודכן" , "RiderLname":"עודכן", "RiderAddress":"Updated val" ,  "City":"רעננה", "RiderPhone":"888888",  "BicycleType": "הרים" , "ImagePath":"Updated val" , "BirthDate":"04-04-2004", "Organization":"ebay", "Group":"secondGroup"}
        public string Put(string username, [FromBody]Rider rdr)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            try
            {
                return_val = dbs.updateRiderInDatabase(rdr,username);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to Update the Rider(user) " + username + " to the database " + ex.Message);
                lf.Main("Users", Response);
                return "Error";
            }
            if ( return_val == 0 ){return "Error";}
            return "Success";
        }
    }
}