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
    public class CitiesController : ApiController
    {
        // GET api/Cities/
        [AllowAnonymous]
        public DataTable GetAll()
        {
            Cities ct = new Cities();
            DataTable dt = ct.readData();
            return dt;
        }
    }
}