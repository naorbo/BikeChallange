using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BikeChallengeApp.Controllers
{
    public class OrganizationExistsController : ApiController
    {
        // GET api/OrganizationExists?orgname=
        public string GetEmail(string orgname)
        {
            DBservices dbs = new DBservices();
            orgname = orgname.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(23, orgname);
            if (dbs.dt.Rows.Count > 0)
                return dbs.dt.Rows[0].ItemArray[0].ToString();
            else
                return "NOT EXISTS";
        }
    }
}