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
        public bool Get(string username)
        {
            string return_val = "";
            DBservices dbs = new DBservices();
            return_val = dbs.ReadFromDataBaseUserName("DefaultConnection", username);
            if (return_val == "Exists")
                return true;
            else
            return false;
        }
    }
}