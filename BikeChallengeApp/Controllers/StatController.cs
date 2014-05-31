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
    public class StatController : ApiController
    {
        
        // GET Stat per USERNAME
        // api/Stat?username=[The username of the rider]
        public DataTable GetUser( string username)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(10, username);
            return dbs.dt;
        }
        // GET Stat per Group
        // api/Stat?grpname=[The name of the group]&orgname=[The name of the organizations]&gender=[זכר/נקבה not mendatory]
        public DataTable GetGroup(string grpname, string orgname, string gender="")
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(11, grpname, orgname, gender);
            return dbs.dt;
        }
        // GET Stat per Organization
        // api/Stat?orgname=[The name of the group]&gender=[M/נקבה not mendatory]
        public DataTable GetOrganization(string orgname, string gender="")
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(12, orgname, gender);
            return dbs.dt;
        }
        // GET Stat for all users
        // api/Stat?gender=[M/נקבה not mendatory]
        public DataTable GetUsers(string gender = "")
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(13, gender);
            return dbs.dt;
        }
    }
}