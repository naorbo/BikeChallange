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
        private int route;

        public int Route
        {
            get { return route; }
            set { route = value; }
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
            set { riderdes = value; }
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
        private string rideraddres;

        public string RiderAddress
        {
            get { return rideraddres; }
            set { rideraddres = value; }
        }

        private string riderphone;

        public string RiderPhone
        {
            get { return riderphone; }
            set { riderphone = value; }
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
            set { username = value; }
        }

        private int captain;

        public int Captain
        {
            get { return captain; }
            set { captain = value; }
        }

        public Rider()
        {
            RiderEmail = (RiderEmail != null ? RiderEmail : "");
            RiderDes = (RiderDes != null ? RiderDes : "");
            RiderFname = (RiderFname != null ? RiderFname : "");
            RiderLname = (RiderLname != null ? RiderLname : "");
            Gender = (Gender != null ? Gender : "");
            RiderAddress = (RiderAddress != null ? RiderAddress : "");
            RiderPhone = (RiderPhone != null ? RiderPhone : "");
            BicycleType = (BicycleType != null ? BicycleType : "");
            City = (City != null ? City : "");
            ImagePath = (ImagePath != null ? ImagePath : "");
            BirthDate = (BirthDate != null ? BirthDate : "");
            Username = (Username != null ? Username : "");
            Captain = (Captain != null ? Captain : 0);
            Organization = (Organization != null ? Organization : "");
            Group = (Group != null ? Group : ""); 
        }
        public Rider(string rideremail, string riderfname, string riderlname, string gender, string rideraddres, string ridercity, string riderphone, string bicycletype, string imagePath, string birthDate, string username, int captain, string organization, string group)
        {
            RiderEmail = (rideremail != null ? rideremail : "");
            RiderDes = ((riderfname != null && riderfname != null) ? riderfname + " " + riderlname : ""); 
            RiderFname = (riderfname != null ? riderfname : ""); 
            RiderLname = (riderlname != null ? riderlname : ""); 
            Gender = (gender != null ? gender : ""); 
            RiderAddress = (rideraddres != null ? rideraddres : ""); 
            RiderPhone = (riderphone != null ? riderphone : ""); 
            BicycleType = (bicycletype != null ? bicycletype : ""); 
            City = (ridercity != null ? ridercity : ""); 
            ImagePath = (imagePath != null ? imagePath : ""); 
            BirthDate = (birthDate != null ? birthDate : ""); 
            Username = (username != null ? username : ""); 
            Captain = (captain != null ? captain : 0 ); 
            Organization = (organization != null ? organization : ""); 
            Group = (group != null ? group : ""); 
        }
     }
}