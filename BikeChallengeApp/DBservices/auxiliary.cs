using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
/*
/// <summary>
/// Summary description for auxiliary
/// </summary>
public class auxiliary
{
    public auxiliary()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //---------------------------------------------------------------------------------------
    // Create a ASP:Table from a data table
    //---------------------------------------------------------------------------------------
    public Table createTable(DataTable dt)
    {

        Table tbl = new Table();

        // build the header row
        TableRow trH = new TableRow();

        foreach (DataColumn dc in dt.Columns)
        {
            AddCells(trH, dc.ColumnName);
            trH.BackColor = System.Drawing.Color.Yellow;
            tbl.Rows.Add(trH);
        }

        foreach (DataRow row in dt.Rows)
        {
            TableRow tr = new TableRow();
            foreach (DataColumn dc in dt.Columns)
            {
                AddCells(tr, row[dc.ColumnName].ToString());
            }
            tbl.Rows.Add(tr);
        }
        return tbl;
    }

    //---------------------------------------------------------------------------------------
    // Add Cells to a  DataRow
    //---------------------------------------------------------------------------------------
    void AddCells(TableRow tr, string text)
    {

        TableCell tc = new TableCell();
        tc.Text = text;
        tr.Cells.Add(tc);

    }

    //---------------------------------------------------------------------------------
    // return the dataset from the session
    //---------------------------------------------------------------------------------
    public DataTable getTableFromSession(string sessionName)
    {

        if (HttpContext.Current.Session[sessionName] != null)
        {
            DBservices dbs = (DBservices)(HttpContext.Current.Session[sessionName]);
            return dbs.dt;
        }
        else
        {
            return null;
        }

    }

}*/