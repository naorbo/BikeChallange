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
    public class ShuffleController : ApiController
    {
        
        // GET api/Shuffle?date=07-07-2014
        [Authorize(Users = "bcadministrator")]
        public DataTable Get(string date)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(24,"50",date, "BronzeWinner");
            
            return dbs.dt;

        }
    }
}