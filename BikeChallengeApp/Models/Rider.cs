using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace BikeChallengeApp.Models
{
    public class Rider
    {
        private string group;

        public string Group
        {
            get { return group; }
            set { group = value; }
        }
        private string organization;

        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }
        private string city;

        public string City
        {
            get { return city; }
            set { city = value; }
        }
        private string birthDate;

        public string BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }
        private string riderdes;

        public string RiderDes
        {
            get { return riderdes; }
            set {  }
        }

        private string riderfname;

        public string RiderFname
        {
            get { return riderfname; }
            set { riderfname = value; }
        }

        private string riderlname;

        public string RiderLname
        {
            get { return riderlname; }
            set { riderlname = value; }
        }

        private string gender;

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        private string rideremail;

        public string RiderEmail
        {
            get { return rideremail; }
            set { rideremail = value; }
        }

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }
        private string bicycletype;

        public string BicycleType
        {
            get { return bicycletype; }
            set { bicycletype = value; }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value;
                riderdes = value;
                }
        }

        private int captain;

        public int Captain
        {
            get { return captain; }
            set { captain = value; }
        }
        private string riderPhone;

        public string RiderPhone
        {
            get { return riderPhone; }
            set { riderPhone = value; }
        }
        private string riderAddress;

        public string RiderAddress
        {
            get { return riderAddress; }
            set { riderAddress = value; }
        }

        public Rider()
        {

        }
        public Rider(string rideremail, string riderfname, string riderlname, string riderphone, string rideraddress, string gender, string ridercity, string bicycletype, string imagePath, string birthDate, string username, int captain, string organization, string group)
        {
            RiderEmail = (rideremail != null ? rideremail : "");
            RiderDes = ((riderfname != null && riderfname != null) ? riderfname + " " + riderlname : ""); 
            RiderFname = (riderfname != null ? riderfname : ""); 
            RiderLname = (riderlname != null ? riderlname : "");
            RiderAddress = (rideraddress != null ? rideraddress : "");
            RiderPhone = (riderphone != null ? riderphone : ""); 
            Gender = (gender != null ? gender : ""); 
            BicycleType = (bicycletype != null ? bicycletype : ""); 
            City = (ridercity != null ? ridercity : ""); 
            ImagePath = (imagePath != null ? imagePath : ""); 
            BirthDate = (birthDate != null ? birthDate : ""); 
            Username = (username != null ? username : "");
            Captain = captain; 
            Organization = (organization != null ? organization : ""); 
            Group = (group != null ? group : ""); 
        }
     }
}