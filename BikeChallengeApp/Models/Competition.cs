using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeChallengeApp.Models
{
    public class Competition
    {
        string competitionDate;
        public string CompetitionDate
        {
            get { return competitionDate; }
            set { competitionDate = value;}
        }

        string orgWin;
        public string OrgWin
        {
            get { return orgWin; }
            set { orgWin = value; }
        }

        string grpWin;
        public string GrpWin
        {
            get { return grpWin; }
            set { grpWin = value; }
        }

        string bronzeUser;
        public string BronzeUser
        {
            get { return bronzeUser; }
            set { bronzeUser = value; }
        }

        string silverUser;
        public string SilverUser
        {
            get { return silverUser; }
            set { silverUser = value; }
        }

        string goldUser;
        public string GoldUser
        {
            get { return goldUser; }
            set {  goldUser = value;}
        }

        string platinumUser;
        public string PlatinumUser
        {
            get { return platinumUser; }
            set {  platinumUser = value;}
        }

        string grpOrgWin;
            public string GrpOrgWin
        {
            get { return grpOrgWin; }
            set { grpOrgWin = value; }
        }
         public Competition()
        {
            
        }
         
    }
}