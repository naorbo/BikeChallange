using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class Cities : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    //--------------------------------------------------------------------
    // Show the DataSet
    //--------------------------------------------------------------------
    protected void showDSBTN_Click(object sender, EventArgs e)
    {

        City city = new City();
        DataTable dt = new auxiliary().getTableFromSession("citiesDataSet");

        if (dt != null)
        {
            ShowTable(dt);
        }
        else
        {
            Response.Write("The DataTable is no longer in the session");
        }

    }


    //---------------------------------------------------------------------------------------
    // Show the Data Table using an asp table
    //---------------------------------------------------------------------------------------
    private void ShowTable(DataTable dt)
    {

        auxiliary aux = new auxiliary();
        /*dt.Columns["City"].ColumnName = "עיר (ID)";
        dt.Columns["CityName"].ColumnName = "קוד העיר";
        dt.Columns["CityDes"].ColumnName = "שם העיר";
        dt.Columns["CityCountry"].ColumnName = "ארץ";*/
        Table tbl = aux.createTable(dt);
        tablePH.Controls.Add(tbl);
    }


    //-------------------------------------------------------------------------
    // Load from the Database
    //-------------------------------------------------------------------------
    protected void loadFromDBBTN_Click(object sender, EventArgs e)
    {

        City city = new City();
        DataTable dt = city.readCitiesDB(); // read from the DataBase

        ShowTable(dt);
    }

    //-------------------------------------------------------------------------
    // Update the DataSet
    //-------------------------------------------------------------------------
    protected void updateDSBTN_Click(object sender, EventArgs e)
    {

        City city = new City();
        city.Cityname = CityNameTB.Text;
        city.CityDes = CityDesTB.Text;
        city.CityCountry = "ישראל";
        
        city.updateTable();
        ShowTable(new auxiliary().getTableFromSession("citiesDataSet"));

    }

    //-------------------------------------------------------------------------
    // update the database
    //-------------------------------------------------------------------------
    protected void updateDBBTN_Click(object sender, EventArgs e)
    {
        City city = new City();
        try
        {
            city.updateDatabase();
        }
        catch (Exception ex)
        {
            Response.Write("Error updating the City database " + ex.Message);
        }
    }

}
