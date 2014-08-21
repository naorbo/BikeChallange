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
        // i.e : api/Ranking?grpname=&orgname=&gender=&order=&date=06-01-2014
        [Authorize]
        public DataTable GetUserRank(string grpname, string orgname, string gender, string order, string date)
        {
            DBservices dbs = new DBservices();
            grpname = (grpname != null ? grpname : "");
            orgname = (orgname != null ? orgname : "");
            gender = (gender != null ? gender : "");
            order = (order != null ? order : "Points");
            date = (date != null ? date : "");
            grpname = grpname.Replace("'", "''");
            orgname = orgname.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(15, grpname, orgname, gender,order, date);
            return dbs.dt;
        }
        // GET Ranking per Group
        //orgname=[If you want to get the ranking within the org if not = "all"]/ gender=[if you want to get the ranking according to gender if not = ""]
        // api/Ranking?orgname=[]&gender=[M/F not mendatory]&order=[Points/Days/Kilometers]
        // i.e : api/Ranking?orgname=&gender=&order=&date=
        [Authorize]
        public DataTable GetGroupRank(string orgname, string gender, string order, string date)
        {
            DBservices dbs = new DBservices();
            orgname = (orgname != "all" ? orgname : "");
            gender = (gender != null ? gender : "");
            order = (order != null ? order : "Points");
            date = (date != null ? date : "");
            orgname = orgname.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(16, orgname, gender, order, date);
            return dbs.dt;
        }
        // GET Ranking per Organization

        // api/Ranking?gender=[M/נקבה not mendatory if not = "")]&order=[Points/Days/Kilometers]]&date=07-07-2014
        [Authorize]
        public DataTable GetOrganizationRank(string gender, string order, string date)
        {
            DBservices dbs = new DBservices();
            gender = (gender != null ? gender : "");
            order = (order != null ? order : "Points");
            date = (date != null ? date : "");
            dbs = dbs.ReadFromDataBase(17, gender, order, date);
            return dbs.dt;
        }
        // GET Self Ranking
        // api/Ranking?username=messi10&date=06-01-2014
        [Authorize]
        public DataTable GetRank(string username, string date) 
        {
            DBservices dbs = new DBservices();
            username = (username != null ? username : "");
            date = (date != null ? date : "");
            dbs = dbs.ReadFromDataBase(19, username, date);
            return  dbs.dt1 ;

        }
    }
}