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
    public class RankingController : ApiController
    {

        // GET Ranking per User
        // grpname=[If you want to get the ranking within the group if not = ""] /orgname=[If you want to get the ranking within the group/org if not = ""]/ gender=[if you want to get the ranking according to gender if not = ""]
        // api/Ranking?grpname=[]&orgname=[]&gender=[F/M not mendatory]&order=[Points/Days/Kilometers]
        public DataTable GetUserRank(string grpname, string orgname, string gender, string order)
        {
            DBservices dbs = new DBservices();
            grpname = (grpname != null ? grpname : "");
            orgname = (orgname != null ? orgname : "");
            gender = (gender != null ? gender : "");
            order = (order != null ? order : "Points");
            dbs = dbs.ReadFromDataBase(15, grpname, orgname, gender,order);
            return dbs.dt;
        }
        // GET Ranking per Group
        //orgname=[If you want to get the ranking within the org if not = "all"]/ gender=[if you want to get the ranking according to gender if not = ""]
        // api/Ranking?orgname=[]&gender=[M/F not mendatory]&order=[Points/Days/Kilometers]
        public DataTable GetGroupRank(string orgname, string gender, string order)
        {
            DBservices dbs = new DBservices();
            orgname = (orgname != "all" ? orgname : "");
            gender = (gender != null ? gender : "");
            order = (order != null ? order : "Points");
            dbs = dbs.ReadFromDataBase(16, orgname, gender, order);
            return dbs.dt;
        }
        // GET Ranking per Organization
        //orgname=[If you want to get the ranking within the org]/ gender=[if you want to get the ranking according to gender]
        // api/Ranking?gender=[M/נקבה not mendatory if not = "")]&order=[Points/Days/Kilometers]
        public DataTable GetOrganizationRank(string gender, string order)
        {
            DBservices dbs = new DBservices();
            gender = (gender != null ? gender : "");
            order = (order != null ? order : "Points");
            dbs = dbs.ReadFromDataBase(17, gender, order);
            return dbs.dt;
        }
    }
}