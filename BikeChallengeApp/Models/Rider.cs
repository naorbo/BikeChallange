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
        private int group;

        public int Group
        {
            get { return group; }
            set { group = value; }
        }
        private int organization;

        public int Organization
        {
            get { return organization; }
            set { organization = value; }
        }
        private int city;

        public int City
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

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
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
        public Rider(string rideremail, string riderfname, string riderlname, string gender, string rideraddres, int ridercity, string riderphone, string bicycletype, string imagePath, string birthDate, string id, int captain)
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
            Id = id;
            Captain = captain;
        }
        
        public DataTable readData()
        {
            string conString = "DefaultConnection";
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(conString, "Users");
            return dbs.dt;

        }

        public void updateDatabase(Rider rdr)
        {

            DBservices dbs = new DBservices();
            dbs.insertRider(rdr);

        }
    }
}