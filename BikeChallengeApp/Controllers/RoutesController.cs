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
        int return_val = 0;
        LogFiles lf = new LogFiles();

        // POST - Insert new Route into the DB 
        // api/Routes
        //{"UserName":"tester1", "RouteName":"מסלול 1", "RouteType":"קבוע", "RouteLength":10, "Comments":"מסלול מהעבודה לבית של האישה", "RouteSource":"בית של הפילגש", "RouteDestination":"עבודה בנתניה"}
        public string updateDB([FromBody]Routes rut)
        {
            DBservices dbs = new DBservices();
            try
            {
                return_val = dbs.insertRoutes(rut);
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
            dbs = dbs.ReadFromDataBase(9, username, "");
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
    }
}