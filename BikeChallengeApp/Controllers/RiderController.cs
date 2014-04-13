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
        // GET api/Rider
        public DataTable GetAll()
        {
            Rider rdr = new Rider();

            DataTable dt = rdr.readData();

            return dt;
        }

        // POST / Insert Rider into the DB 
        //{ "RiderEmail":"NAOR@NAOR.COM", "RiderFname":"john" , "RiderLname":"qwerty", "Gender": "זכר", "RiderAddress":"joh" ,  "City ":0, "RiderPhone":"היט",  "BicycleType": "12erty" , "ImagePath":"kl;kl;" , "BirthDate":"01-01-1985"}
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