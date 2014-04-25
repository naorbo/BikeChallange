using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using BikeChallengeApp.Models;

/// <summary>
/// Class designed to provide access to the data layer
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    
    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }

    public DBservices ReadFromDataBase(string conString, string tableName)
    {

        DBservices dbS = new DBservices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database/

            String selectStr = "SELECT * FROM " + tableName; // create the select that will be used by the adapter to select data from the DB

            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);                        // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the dataa adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }
    
    public DBservices ReadFromDataBaseRider(string conString, string groupname)
    {

        DBservices dbS = new DBservices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database/

            String selectStr = @" SELECT anu.UserName, U.UserEmail, U.UserDes, U.UserFname, U.UserLname, U.ImagePath, U.Gender, U.[User]
                                FROM UsersGroups UG, Users U, AspNetUsers anu
                                Where U.[User] <> 0
                                AND U.Id = anu.Id                            
                                AND UG.[User] = U.[User]
                                AND UG.[Group] = ( Select [GROUP] From Groups Where GroupName = '" + groupname + "' )"; // create the select that will be used by the adapter to select data from the DB

            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);                        // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the dataa adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    public DBservices ReadFromDataBaseGroup(string conString, string orgname)
    {

        DBservices dbS = new DBservices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database/

            String selectStr = @" SELECT [GroupName],[GroupDes]
                                FROM [Groups]
                                WHERE [GROUP] <> 0 
                                AND [Organization] = (SELECT Organization From Organizations Where OrganizationsName = '"+ orgname +"' ) " ; // create the select that will be used by the adapter to select data from the DB
                                      

            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);                        // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the dataa adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }
    

    internal void Update()
    {
        SqlCommandBuilder builder = new SqlCommandBuilder(da);
        da.Update(dt);
    }
//  **********************ORGANIZATION***********************************************
    public int insertOrganization(Organization org)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DefaultConnection"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertOrganizationCommand(org);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {

            // write to log
            throw (ex);
            //return 0;
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }




    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertOrganizationCommand(Organization org)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "INSERT INTO Organizations(  OrganizationsName, [City], OrganizationDes, OrganizationEmail, OrganizationAddress, OrganizationPhone, OrganizationType ) ";
        sb.AppendFormat("Values('{0}', (select city from Cities where CityName = '{1}' ) ,'{2}', '{3}', '{4}', '{5}', '{6}' )", org.Organizationname, org.OrganizationCity, org.OrganizationDes, org.OrganizationEmail, org.OrganizationAddress, org.OrganizationPhone, org.OrganizationType);
        command = prefix + sb.ToString();

        return command;
    }

    //  ********************** RIDERS  ***********************************************
    
    public int updateRiderInDatabase(string username, Rider rdr)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DefaultConnection"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateRiderCommand(username, rdr);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
            //return 0;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public int delteRider(string username)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DefaultConnection"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildDelteRiderCommand(username);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
            //return 0;
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    public int insertRider(Rider rdr)
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlCommand crmd;

        try
        {
            con = connect("DefaultConnection"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertRidersCommand(rdr);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            int numEffected_2 = 0;
            if (numEffected > 0)
            {
                String ins = BuildInsertRidersGroup(rdr);
                crmd = CreateCommand(ins, con);
                numEffected_2 = crmd.ExecuteNonQuery();
            }
            return numEffected + numEffected_2;
        }
        catch (Exception ex)
        {

            // write to log
            throw (ex);
            //return 0;
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }




    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertRidersCommand(Rider rdr)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "INSERT INTO Users(  UserEmail, [Group], [Route], City, UserDes, UserFname, UserLname, Gender,  UserAddress, UserPhone, BicycleType, ImagePath, BirthDate, [CurDate], [Id], Captain, [Organization] ) ";
        sb.AppendFormat("Values('{0}', {1}, {2}, (select city from Cities where CityName = '{3}' ), '{4}', '{5}' ,'{6}', '{7}', '{8}', '{9}','{10}', '{11}','{12}', '{13}', (select id from AspNetUsers where UserName = '{14}'), {15}, (select Organization from Organizations where OrganizationsName = '{16}'))", rdr.RiderEmail, 0, 0, rdr.City, rdr.RiderDes, rdr.RiderFname, rdr.RiderLname, rdr.Gender, rdr.RiderAddress, rdr.RiderPhone, rdr.BicycleType, rdr.ImagePath, rdr.BirthDate, DateTime.Now.Date.ToString("yyyy-MM-dd"), rdr.Username, rdr.Captain, rdr.Organization);
        command = prefix + sb.ToString();

        return command;
    }
    private String BuildInsertRidersGroup(Rider rdr)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "INSERT INTO [UsersGroups]([Group],[User]) ";
        sb.AppendFormat(" Values( ( Select [Group] From Groups Where GroupName = '{0}'), ( Select [User] From Users Where Id = ( select id from AspNetUsers where UserName = '{1}' ) ) ) ", rdr.Group, rdr.Username);
        command = prefix + sb.ToString();

        return command;
    }

    private String BuildDelteRiderCommand(string username)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        String prefix = @"Declare @val int;
                        SET @val = ( SELECT u.[User]
				                        FROM [Users] u , [AspNetUsers] asp
				                        Where asp.UserName = '" + username + @"'
				                        AND   asp.ID = u.id );
                        DELETE FROM [UsersGroups] Where [USER] = @val;
                        DELETE FROM [Users] Where [USER] = @val;"; 
        command = prefix;

        return command;
    }

    private String BuildUpdateRiderCommand(string username, Rider rdr)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "UPDATE [Users] ";
        sb.AppendFormat(@"SET [UserEmail] ='{0}'
                              ,[Group] = {1}
                              ,[Route] = {2}
                              ,[City] = (select city from Cities where CityName = '{3}' )
                              ,[UserDes] = '{4}'
                              ,[UserFname] = '{5}'
                              ,[UserLname] = '{6}'
                              ,[Gender] = '{7}'
                              ,[UserAddress] ='{8}'
                              ,[UserPhone] = '{9}'
                              ,[BicycleType] = '{10}'
                              ,[ImagePath] = '{11}'
                              ,[BirthDate] ='{12}'
                              ,[CurDate] = '{13}'
                              ,[Captain] = {14}
                              ,[Organization] = (select [Organization] from Organizations where OrganizationsName = '{15}')
                         WHERE [Id] = (select id from AspNetUsers where UserName = '" + username + "');", rdr.RiderEmail, 0, 0, rdr.City, rdr.RiderDes, rdr.RiderFname, rdr.RiderLname, rdr.Gender, rdr.RiderAddress, rdr.RiderPhone, rdr.BicycleType, rdr.ImagePath, rdr.BirthDate, DateTime.Now.Date.ToString("yyyy-MM-dd"), rdr.Captain, rdr.Organization);
        command = prefix + sb.ToString();

        return command;
    }

    //  **********************Groups***********************************************
    public int insertGroup(Group grp)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DefaultConnection"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertGroupCommand(grp);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {

            // write to log
            throw (ex);
            //return 0;
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }




    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertGroupCommand(Group grp)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "INSERT INTO Groups( GroupName, Organization, GroupDes ) ";
        sb.AppendFormat("Values('{0}', (select Organization from Organizations where OrganizationsName = '{1}' ) ,'{2}')",grp.GroupName, grp.OrganizationsName, grp.GroupDes);
        command = prefix + sb.ToString();

        return command;
    }
}
