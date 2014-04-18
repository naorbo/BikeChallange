using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace BikeChallengeApp.Models
{
    public class Cities
    {
        string cityName;
        public string Cityname
        {
            get { return cityName; }
            set { cityName = value; }
        }

        string cityDes;
        public string CityDes
        {
            get { return cityDes; }
            set { cityDes = value; }
        }

        string cityCountry;
        public string CityCountry
        {
            get { return cityCountry; }
            set { cityCountry = value; }
        }
    

        public List<Cities> readCitiesDB()
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase("DefaultConnection", "Cities");
            List<Cities> cities = new List<Cities>();
            foreach (DataRow dr in dbs.dt.Rows)
            {
  
                cityName = (string)dr["cityName"];
                cityDes = (string)dr["cityDes"];
                cityCountry = (string)dr["cityCountry"];

            }
            return cities;

        }
        public DataTable readData()
        {
            string conString = "DefaultConnection";
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(conString, "Cities");
            return dbs.dt;

        }
    }
}