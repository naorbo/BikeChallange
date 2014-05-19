using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BikeChallengeApp.Controllers
{
    public class UserNameExistsController : ApiController
    {
        // GET api/UserNameExists?username=<tester9>
        public string Get(string username)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(5, username);
            if (dbs.dt.Rows.Count > 0)
                return dbs.dt.Rows[0].ItemArray[0].ToString();
            else
                return "NOT EXISTS";
            //if (return_val == "Exists")
           //     return true;
           // else
           // return false;
        }
    }
}