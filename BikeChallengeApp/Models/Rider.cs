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
            //
            // TODO: Add constructor logic here
            //
        }
        public Rider(string rideremail, string riderfname, string riderlname, string gender, string rideraddres, string ridercity, string riderphone, string bicycletype, string imagePath, string birthDate, string username, int captain, string organization, string group)
        {
            RiderEmail = rideremail;
            RiderDes = riderfname + " " + riderlname;
            RiderFname = riderfname;
            RiderLname = riderlname;
            Gender = gender;
            RiderAddress = rideraddres;
            RiderPhone = riderphone;
            BicycleType = bicycletype;
            City = ridercity;
            ImagePath = imagePath;
            BirthDate = birthDate;
            Username = username;
            Captain = captain;
            Organization = organization;
            Group = group;
        }
     }
}