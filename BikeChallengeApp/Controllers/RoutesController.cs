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
    public class RoutesController : ApiController
    {
        // POST - Insert new Route into the DB 
        // api/Routes
        //{"UserName":"tester1", "RouteType":"מסלול 1", "RouteLength":10, "Comments":"מסלול מהעבודה לבית של האישה", "RouteSource":"בית של הפילגש", "RouteDestination":"עבודה בנתניה"}
        public string updateDB([FromBody]Routes rut)
        {
            DBservices dbs = new DBservices();
            int return_val = 0;
            LogFiles lf = new LogFiles();
            List<Object> mlist = new List<Object>();
            mlist.Add(rut);
            try
            {
                return_val = dbs.InsertDatabase(mlist);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to INSERT the new Route to the database " + ex.Message);
                lf.Main("Routes", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        // GET Route per USERNAME
        // api/Routes?username=[The username of the rider] - Not case sensative
        public DataTable GetUser(string username)
        {
            DBservices dbs = new DBservices();
            username = username.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(9, username);
            return dbs.dt;
        }

        // GET ALL Routes
        // api/Routes
        public DataTable GetAll()
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(7, "Routes", "[Route]");
            return dbs.dt;
        }

        // DELETE 
        // api/Routes?username=[UserName]&routename=[RouteName]
        public string Delete(string username, string routename)
        {
            DBservices dbs = new DBservices();
            int return_val = 0;
            LogFiles lf = new LogFiles();
            username = username.Replace("'", "''");
            string Response = "";
            try
            {
                return_val = dbs.DeleteDatabase("Routes",username, routename);
            }
            catch (Exception ex)
            {
                Response = ("Error in the Delete process of the Route " + routename + " of " + username + ", " + ex.Message);
                lf.Main("Routes", Response);
                return "Error";
            }
            Response = "The Route " + routename + " was Deleted from the Database";
            lf.Main("Routes", Response);
            if (return_val == 0) { return "Error"; }
            return "Success";
        }
    }
}