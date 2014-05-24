﻿using System;
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
        DBservices dbs = new DBservices();

        // GET api/Group?orgname=[The name of the organization] - Not case sensative
        public DataTable Get(string orgname)
        {
            dbs = dbs.ReadFromDataBase(3, orgname);
            return dbs.dt;
        }

        // GET api/Group?grpname=[The name of the group]&orgname=[The name of the organization] - Not case sensative
        public DataTable GetGroup(string grpname, string orgname)
        {
            dbs = dbs.ReadFromDataBase(4, grpname, orgname);
            return dbs.dt;
        }

        // GET api/Group
        public DataTable GetAll()
        {
            dbs = dbs.ReadFromDataBase(7, "Groups", "[Group]");
            return dbs.dt;
        }

        // POST api/Group
        // {"GroupName":"groupName", "OrganizationName":"", "GroupDes":"groupDes"}
        public string updateDB([FromBody]Group grp)
        {
            List<Object> mlist = new List<Object>();
            mlist.Add(grp);
            try
            {
                return_val = dbs.InsertDatabase(mlist);
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