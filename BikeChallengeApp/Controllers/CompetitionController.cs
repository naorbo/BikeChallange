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
        // GET api/Competition?date=2014-07 [leave empty to retrieve all records]
        [AllowAnonymous]
        public DataTable GetSealedComptetion(string date)
        {
            DBservices dbs = new DBservices();
            date = (date != null ? date : "");
            dbs = dbs.ReadFromDataBase(26, date);
            return dbs.dt;
        }
        //api/Competition
        [Authorize(Users = "bcadministrator")]
        public DataTable GetOpenComptetion()
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(29);
            return dbs.dt;
        }
        // POST api/Competition // Debug Use
       // {"CompetitionDate":"2014-07","OrgWin":"","GrpWin":"","PlatinumUser":"","GoldUser":"","SilverUser":"","BronzeUser":""}
        [Authorize(Users = "bcadministrator")]
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

        // PUT api/Competition?CompetitionDate=07-2014
        // {"OrgWin":"","GrpWin":"","GrpOrgWin":"", "PlatinumUser":"","GoldUser":"","SilverUser":"","BronzeUser":""}
        [Authorize(Users = "bcadministrator")]
        public string Put(string CompetitionDate, [FromBody]Competition cmpt)
        {
            string userName = string.Empty;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;              
            }
            DBservices dbs = new DBservices();
            int return_val = 0;
            LogFiles lf = new LogFiles();
            List<Object> mlist = new List<Object>();
            mlist.Add(cmpt);
            try
            {
                return_val = dbs.updateCompetitionInDatabase(cmpt, CompetitionDate);
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