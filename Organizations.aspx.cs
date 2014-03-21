using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class Organizations : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    //--------------------------------------------------------------------
    // Show the DataSet
    //--------------------------------------------------------------------
    protected void showDSBTN_Click(object sender, EventArgs e)
    {

        Organization organization = new Organization();
        DataTable dt = new auxiliary().getTableFromSession("organizationsDataSet");

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
       /* dt.Columns[0].ColumnName = "ארגון (ID)";
        dt.Columns[1].ColumnName = "קוד הארגון";
        dt.Columns[2].ColumnName = "איש קשר (ID)";
        dt.Columns[3].ColumnName = "עיר (ID)";
        dt.Columns[4].ColumnName = "שם הארגון";
        dt.Columns[5].ColumnName = "מייל הארגון";
        dt.Columns[6].ColumnName = "כתובת הארגון";
        dt.Columns[7].ColumnName = "טלפון הארגון";
        dt.Columns[8].ColumnName = "סוג הארגון";*/
        Table tbl = aux.createTable(dt);
        tablePH.Controls.Add(tbl);
    }


    //-------------------------------------------------------------------------
    // Load from the Database
    //-------------------------------------------------------------------------
    protected void loadFromDBBTN_Click(object sender, EventArgs e)
    {

        Organization organization = new Organization();
        DataTable dt = organization.readOrganizationsDB(); // read from the DataBase

        ShowTable(dt);
    }

    //-------------------------------------------------------------------------
    // Update the DataSet
    //-------------------------------------------------------------------------
    protected void updateDSBTN_Click(object sender, EventArgs e)
    {

        Organization organization = new Organization();

        organization.Organizationname = OrganizationNameTB.Text;
        organization.OrganizationDes = OrganizationDesTB.Text;
        organization.OrganizationEmail = OrganizationEmailTB.Text ;
        organization.OrganizationAddress = OrganizationAddressTB.Text;
        organization.OrganizationPhone = OrganizationPhoneTB.Text;
        organization.OrganizationType = OrganizationTypeTB.Text;
        organization.OrganizationCity = Convert.ToInt16( CityDDL.SelectedValue ) ;


        organization.updateTable();
        ShowTable(new auxiliary().getTableFromSession("organizationsDataSet"));

    }

    //-------------------------------------------------------------------------
    // update the database
    //-------------------------------------------------------------------------
    protected void updateDBBTN_Click(object sender, EventArgs e)
    {
        Organization organization = new Organization();
        try
        {
            organization.updateDatabase();
        }
        catch (Exception ex)
        {
            Response.Write("Error updating the Organization database " + ex.Message);
        }
    }

}
