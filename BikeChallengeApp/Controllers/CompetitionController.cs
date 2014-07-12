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
    public class CompetitionController : ApiController
    {
        // GET api/Competition
        public DataTable Get()
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(21);
            return dbs.dt;
        }

        // POST api/Competition
       // {"CompetitionDate":"07-2014","OrgWin":"","GrpWin":"","PlatinumUser":"","GoldUser":"","SilverUser":"","BronzeUser":""}
        public string Post([FromBody]Competition cmpt)
        {
            DBservices dbs = new DBservices();
            int return_val = 0;
            LogFiles lf = new LogFiles();
            List<Object> mlist = new List<Object>();
            mlist.Add(cmpt);
            try
            {
                return_val = dbs.InsertDatabase(mlist);
            }
            catch (Exception ex)
            {
                string Response = ("Error updating the Competition database " + ex.Message);
                lf.Main("Competition", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

    }
}