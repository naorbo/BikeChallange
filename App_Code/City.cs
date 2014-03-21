using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Cities
/// </summary>
public class City
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

  

    public City()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------
    // get the total price of the cities
    //--------------------------------------------------------------------------
    /*public double totalCarPrice()
    {
        DBservices dbs = new DBservices();
        double total = dbs.getTotal("carsConnectionString", "usedCars", "price");
        return total;
    }*/

    //--------------------------------------------------------------------------
    // read cities dataset into a list of cities
    //--------------------------------------------------------------------------
    public List<City> readCitiesDS()
    {
        DBservices dbs = new DBservices();
        dbs = dbs.ReadFromDataBase("bikechallangeDBConnectionString", "Cities");
        List<City> cities = new List<City>();
        foreach (DataRow dr in dbs.dt.Rows)
        {
  
            cityName = (string)dr["cityName"];
            cityDes = (string)dr["cityDes"];
            cityCountry = (string)dr["cityCountry"];

        }
        return cities;

    }

    //--------------------------------------------------------------------------------
    // read the cities database into a dataset
    //--------------------------------------------------------------------------------
    public DataTable readCitiesDB()
    {
        DBservices dbs = new DBservices();
        dbs = dbs.ReadFromDataBase("bikechallangeDBConnectionString", "Cities");
        // save the dataset in a session object
        HttpContext.Current.Session["citiesDataSet"] = dbs;
        return dbs.dt;
    }


    //---------------------------------------------------------------------------------
    // update the database
    //---------------------------------------------------------------------------------
    public void updateDatabase()
    {

        if (HttpContext.Current.Session["citiesDataSet"] == null) return;

        DBservices dbs = (DBservices)HttpContext.Current.Session["citiesDataSet"];

        dbs.Update();

    }

    //------------------------------------------------------------------------
    // update the dataset with a new car record
    //------------------------------------------------------------------------
    public void updateTable()
    {

        if (HttpContext.Current.Session["citiesDataSet"] == null) return;

        DBservices dbs = (DBservices)HttpContext.Current.Session["citiesDataSet"];

        DataRow dr = dbs.dt.NewRow();
        dr["cityName"] = cityName;
        dr["cityDes"] = cityDes;
        dr["cityCountry"] = cityCountry;


        dbs.dt.Rows.Add(dr);

    }
}