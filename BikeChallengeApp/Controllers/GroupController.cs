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
    public class GroupController : ApiController
    {
        int return_val = 0;
        LogFiles lf = new LogFiles();

        // GET api/Group?orgname=[The name of the organization] - Not case sensative
        public DataTable Get(string orgname)
        {
            Group grp = new Group();
            DataTable dt = grp.readDataPerORG(orgname);
            return dt;
        }

        // GET api/Group?grpname=[The name of the group]&orgname=[The name of the organization] - Not case sensative
        public DataTable GetGroup(string grpname, string orgname)
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBaseforGroup("DefaultConnection", grpname, orgname);
            return dbs.dt;
        }

        // GET api/Group
        public DataTable GetAll()
        {
            Group tmp = new Group();
            DataTable dt = tmp.readData();
            return dt;
        }

        // POST api/Group
        // {"GroupName":"groupName", "OrganizationName":"", "GroupDes":"groupDes"}
        public string updateDB([FromBody]Group grp)
        {
            DBservices dbs = new DBservices();
            try
            {
                return_val = dbs.insertGroup(grp);
            }
            catch (Exception ex)
            {
                string Response = ("Error updating the Group database " + ex.Message);
                lf.Main("Groups", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }
    }
}