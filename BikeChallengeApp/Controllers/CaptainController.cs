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

    }
}