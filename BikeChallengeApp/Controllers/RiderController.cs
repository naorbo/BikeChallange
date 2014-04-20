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
    public class RiderController : ApiController
    {
        bool insert_flag = false;

        // GET api/Rider?groupname=[The name of the group] - Not case sensative
        public DataTable Get(string groupname)
        {
            Rider rdr = new Rider();

            DataTable dt = rdr.readDataPerGroup(groupname);

            return dt;
        }

        // GET api/Rider
        public DataTable GetAll()
        {
            Rider rdr = new Rider();

            DataTable dt = rdr.readData();

            return dt;
        }

        // POST / Insert Rider into the DB 
        //{"RiderEmail":"Moshe@Moshe.COM", "RiderFname":"Moshe" , "RiderLname":"Moshe", "Gender": "זכר", "RiderAddress":"יצMoshe 29" ,  "City":"חיפה", "RiderPhone":"0508878900",  "BicycleType": "חשמליות" , "ImagePath":"pic location" , "BirthDate":"01-01-1985", "UserName":"tester4", "Captain":1, "Organization":"orgname", "Group":"groupname"}
        public bool updateDB([FromBody]Rider rdr)
        {

            try
            {
                rdr.updateDatabase(rdr);

            }
            catch (Exception ex)
            {
                string Response = ("Error updating the Organization database " + ex.Message);
                return false;
            }

            return true;
        }
    }
}