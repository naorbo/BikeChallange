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
        // GET api/Shuffle?minvalue=&maxvaule=
        public DataTable Get(string minvalue, string maxvalue)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(24,minvalue,maxvalue);
            
            return dbs.dt;

        }
    }
}