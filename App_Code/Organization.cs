using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


/// <summary>
/// Summary description for Organization
/// </summary>
public class Organization
{
	
    string organizationName;
    public string Organizationname
    {
        get { return organizationName; }
        set { organizationName = value; }
    }

    string organizationDes;
    public string OrganizationDes
    {
        get { return organizationDes; }
        set { organizationDes = value; }
    }

    string organizationEmail;
    public string OrganizationEmail
    {
        get { return organizationEmail; }
        set { organizationEmail = value; }
    }

    string organizationAddress;
    public string OrganizationAddress
    {
        get { return organizationAddress; }
        set { organizationAddress = value; }
    }
    
    string organizationPhone;
    public string OrganizationPhone
    {
        get { return organizationPhone; }
        set { organizationPhone = value; }
    }
    
    string organizationType;
    public string OrganizationType
    {
        get { return organizationType; }
        set { organizationType = value; }
    }
    
    int organizationCity;
    public int OrganizationCity
    {
        get { return organizationCity; }
        set { organizationCity = value; }
    }

    public Organization()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
	    }
  

    
    //--------------------------------------------------------------------------
    // get the total price of the organizations
    //--------------------------------------------------------------------------
    /*public double totalCarPrice()
    {
        DBservices dbs = new DBservices();
        double total = dbs.getTotal("carsConnectionString", "usedCars", "price");
        return total;
    }*/

    //--------------------------------------------------------------------------
    // read organizations dataset into a list of organizations
    //--------------------------------------------------------------------------
    public List<Organization> readOrganizationsDS()
    {
        DBservices dbs = new DBservices();
        dbs = dbs.ReadFromDataBase("bikechallangeDBConnectionString", "Organizations");
        List<Organization> organizations = new List<Organization>();
        foreach (DataRow dr in dbs.dt.Rows)
        {
  
            organizationName = (string)dr["organizationsName"];
            organizationDes = (string)dr["organizationDes"];
            organizationEmail = (string)dr["organizationEmail"];
            organizationAddress = (string)dr["organizationAdress"];
            organizationPhone = (string)dr["organizationPhone"];
            organizationType = (string)dr["organizationType"];
            organizationCity = (int)dr["organizationCity"];
         /*     dt.Columns[0].ColumnName = "ארגון (ID)";
       dt.Columns[1].ColumnName = "קוד הארגון";
        dt.Columns[2].ColumnName = "איש קשר (ID)";
        dt.Columns[3].ColumnName = "עיר (ID)";
        dt.Columns[4].ColumnName = "שם הארגון";
        dt.Columns[5].ColumnName = "מייל הארגון";
        dt.Columns[6].ColumnName = "כתובת הארגון";
        dt.Columns[7].ColumnName = "טלפון הארגון";
        dt.Columns[8].ColumnName = "סוג הארגון";
            */
        }
        return organizations;

    }

    //--------------------------------------------------------------------------------
    // read the organizations database into a dataset
    //--------------------------------------------------------------------------------
    public DataTable readOrganizationsDB()
    {
        DBservices dbs = new DBservices();
        dbs = dbs.ReadFromDataBase("bikechallangeDBConnectionString", "Organizations");
        // save the dataset in a session object
        HttpContext.Current.Session["organizationsDataSet"] = dbs;
        return dbs.dt;
    }


    //---------------------------------------------------------------------------------
    // update the database
    //---------------------------------------------------------------------------------
    public void updateDatabase()
    {

        if (HttpContext.Current.Session["organizationsDataSet"] == null) return;

        DBservices dbs = (DBservices)HttpContext.Current.Session["organizationsDataSet"];

        dbs.Update();

    }

    //------------------------------------------------------------------------
    // update the dataset with a new car record
    //------------------------------------------------------------------------
    public void updateTable()
    {

        if (HttpContext.Current.Session["organizationsDataSet"] == null) return;

        DBservices dbs = (DBservices)HttpContext.Current.Session["organizationsDataSet"];

        DataRow dr = dbs.dt.NewRow();
        dr[1] = organizationName;
        dr[3] = organizationCity ;
       dr[4] = organizationDes;
       dr[5] = organizationEmail;
       dr[6] = organizationAddress;
       dr[7] = organizationPhone;
       dr[8] = organizationType;

      /*
        dt.Columns[0].ColumnName = "ארגון (ID)";
       dt.Columns[1].ColumnName = "קוד הארגון";
       dt.Columns[2].ColumnName = "איש קשר (ID)";
       dt.Columns[3].ColumnName = "עיר (ID)";
       dt.Columns[4].ColumnName = "שם הארגון";
       dt.Columns[5].ColumnName = "מייל הארגון";
       dt.Columns[6].ColumnName = "כתובת הארגון";
       dt.Columns[7].ColumnName = "טלפון הארגון";
       dt.Columns[8].ColumnName = "סוג הארגון";
        */
        dbs.dt.Rows.Add(dr);

    }
}