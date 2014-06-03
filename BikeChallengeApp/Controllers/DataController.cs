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
    public class DataController : ApiController
    {
        // GET api/Data/
        // Return the Total Users/Groups/Organizations/Rides/Routes/KM/CO2/Calories/Days Riden/  
        public DataTable GetAll()
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(18);
            return dbs.dt;
        }
    }
}