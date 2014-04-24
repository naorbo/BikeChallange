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

        // GET api/Group?orgname=[The name of the organization] - Not case sensative
        public DataTable Get(string orgname)
        {
            Group grp = new Group();

            DataTable dt = grp.readDataPerORG(orgname);

            return dt;
        }

        // GET api/<controller>
        public DataTable GetAll()
        {
            Group tmp = new Group();

            DataTable dt = tmp.readData();


            return dt;
        }

        // POST api/<controller>
        // {"GroupName":"groupName", "OrganizationsName":"", "GroupDes":"groupDes"}
        public string updateDB([FromBody]Group grp)
        {
            try
            {
                grp.updateDatabase(grp);

            }
            catch (Exception ex)
            {
                string Response = ("Error updating the Group database " + ex.Message);
                return Response;
                
            }
            return "OK";
        }
    }
}