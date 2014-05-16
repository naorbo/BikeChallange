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
    LogFiles lf = new LogFiles(); 

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
    // *************************************************************************************
    public DBservices ReadFromDataBase(int select, string data1, string data2)
    {
        DBservices dbS = new DBservices(); // create a helper class
        SqlConnection con = null;
        try
        {
            con = dbS.connect("DefaultConnection"); // open the connection to the database/
            String selectStr = "";
            switch(select)
            {
			case 1:
                    selectStr = @" SELECT anu.UserName, U.UserEmail, U.UserDes, U.UserFname, U.UserLname, U.ImagePath, U.Gender, U.Captain, convert(varchar(10), U.BirthDate, 120) As BirthDate, U.BicycleType, U.UserAddress, C.CityName As RiderCity, G.GroupName, O.OrganizationName, O.OrganiztionImage
                                FROM UsersGroups UG, Users U, AspNetUsers anu, Groups G, Organizations O, Cities C
                                Where U.[User] <> 0
                                AND U.Id = anu.Id  
                                AND G.[Organization] = O.[Organization]
                                AND UG.[User] = U.[User]
                                AND UG.[Group] = G.[Group]
                                AND U.City = C.City
                                AND G.GroupName = '" + data1 + @"'
                                AND O.OrganizationName = '" + data2 + "';"; //GET RIDER PER GRUOP
                                
			break;
			case 2:
            selectStr = @" SELECT anu.UserName, U.UserEmail, U.UserDes, U.UserFname, U.UserLname, U.ImagePath, U.Gender, U.Captain, convert(varchar(10), U.BirthDate, 120) As BirthDate, U.BicycleType, U.UserAddress, C.CityName As RiderCity, G.GroupName, O.OrganizationName, O.OrganiztionImage, CO.CityName As OrgCity
                                FROM UsersGroups UG, Users U, AspNetUsers anu, Groups G, Organizations O, Cities C, Cities CO
                                Where U.[User] <> 0
                                AND U.Id = anu.Id
                                AND G.Organization = O.Organization                            
                                AND UG.[User] = U.[User]
                                AND UG.[Group] = G.[Group]
                                AND U.City = C.City
                                AND O.City = CO.City
                                AND anu.UserName = '" + data1 + "';"; //GET RIDER from USERNAME
			break;
			case 3:
            selectStr = @"SELECT [GroupName],[GroupDes], anu.UserName AS Captain_UserName
                                FROM [Groups] G, Organizations O, AspNetUsers anu, Users U
                                WHERE [GROUP] <> 0 
                                AND G.[Organization] = O.[Organization]
                                AND anu.Id = U.Id
                                AND O.OrganizationName = '" + data1 + @"' 
                                AND U.Captain = 1
                                AND U.[User] in ( SELECT UG.[User]
												FROM UsersGroups UG
												WHERE G.[Group] = UG.[Group]);"; // Group From ORG
			break;
			case 4:

            selectStr = @" SELECT G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, C.CityName As ORG_City, anu.UserName AS Captain_UserName
                                FROM Groups G, Organizations O, AspNetUsers anu, Users U, Cities C
                                Where G.[Group] <> 0
                                AND G.GroupName = '" + data1 + @"'
                                AND G.Organization = O.Organization
                                AND O.OrganizationName = '" + data2 + @"'
                                AND O.City = C.City 
                                AND anu.Id = U.Id
                                AND U.Captain = 1
                                AND U.[User] in ( SELECT UG.[User]
												FROM UsersGroups UG
												WHERE G.[Group] = UG.[Group]);";//ReadFromDataBaseforGroup
			break;
			case 5:
			selectStr = @" SELECT 'Exists'
                                FROM AspNetUsers
                                WHERE [UserName] = '" + data1 + "' ;";
             // ReadFromDataBaseUserName
			break;
			case 6:
            selectStr = @" SELECT O.OrganizationName, O.OrganizationDes, O.OrganizationType, O.OrganiztionImage, C.CityName
                                FROM Organizations O, Cities C
                                Where O.Organization <> 0
                                AND O.OrganizationName = '" + data1 + @"'
                                AND O.City = C.City ; " ; //ReadFromDataBaseforRiderorgname
			break;
			case 7:
			selectStr = "SELECT * FROM " + data1 + " Where " + data2 + " <> 0"; //ReadFromDataBase 
			break;
        }
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
            lf.Main("SelectError", ex.Message);
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
    // **************************************************************************************
   
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
            lf.Main("Organizations", ex.Message);
            return 0;
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
            lf.Main("Organizations", ex.Message);
            return 0;
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
        String prefix = "INSERT INTO Organizations(  OrganizationName, [City], OrganizationDes, OrganizationType, OrganiztionImage ) ";
        sb.AppendFormat("Values('{0}', (select city from Cities where CityName = '{1}' ) ,'{2}', '{3}' )", org.Organizationname, org.OrganizationCity, org.OrganizationDes, org.OrganizationType, org.OrganizationImage);
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
            lf.Main("Users", ex.Message);
            return 0;
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
            lf.Main("Users", ex.Message);
            return 0;
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
            lf.Main("Users", ex.Message);
            return 0;
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
            lf.Main("Users", ex.Message);
            return 0;
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
            lf.Main("Users", ex.Message);
            return 0; 
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
            if ( numEffected_2 == 0 )
                lf.Main("UsersGroups","No record was inserted to the table check if the group " + rdr.Group +" or the username " + rdr.Username + " exists ");
            return numEffected + numEffected_2;
        }
        catch (Exception ex)
        {
            // write to log
            lf.Main("Users", ex.Message);
            return 0;
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
        String prefix = "INSERT INTO Users(  UserEmail, [Route], City, UserDes, UserFname, UserLname, Gender,  UserAddress, UserPhone, BicycleType, ImagePath, BirthDate, [CurDate], [Id], Captain, [Organization] ) ";
        sb.AppendFormat("Values('{0}', {1}, (select [city] from Cities where CityName = '{2}' ), '{3}', '{4}' ,'{5}', '{6}', '{7}', '{8}','{9}', '{10}','{11}', '{12}', (select id from AspNetUsers where UserName = '{13}'), {14}, (select Organization from Organizations where OrganizationName = '{15}'))", rdr.RiderEmail, 0, rdr.City, rdr.RiderDes, rdr.RiderFname, rdr.RiderLname, rdr.Gender, rdr.RiderAddress, rdr.RiderPhone, rdr.BicycleType, rdr.ImagePath, rdr.BirthDate, DateTime.Now.Date.ToString("yyyy-MM-dd"), rdr.Username, rdr.Captain, rdr.Organization);
        command = prefix + sb.ToString();
        return command;
    }
    private String BuildInsertRidersGroup(Rider rdr)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix1 = @"Declare @Group_val int;
                          Declare @User_val int;
                          Set @Group_val = 0;
                          Set @User_val = 0;
                          Set @Group_val = ( Select [Group] From Groups Where GroupName = '"+ rdr.Group + @"');
                          Set @User_val = ( Select [User] From Users Where Id = ( select id from AspNetUsers where UserName = '" + rdr.Username + @"' ) ) ;";
        String prefix = @"  if ( @Group_val <> 0 AND @User_val <> 0 )
                            begin
                            INSERT INTO [UsersGroups]([Group],[User]) Values( @Group_val, @User_val )
                            end";
        command = prefix1 + prefix;
        return command;
    }
    private String BuildDelteRiderCommand(string username)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        String prefix = @"Declare @val int;
                        Set @val = 0;
                        SET @val = ( SELECT u.[User]
				                        FROM [Users] u , [AspNetUsers] asp
				                        Where asp.UserName = '" + username + @"'
				                        AND   asp.ID = u.id
                                        AND   u.[User] <> 0 );
                        if @val <> 0
                        begin
			                   DELETE FROM [UsersGroups] Where [USER] = @val;
                        DELETE FROM [Users] Where [USER] = @val;
                        end"; 
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
                              ,[Route] = {1}
                              ,[City] = (select city from Cities where CityName = '{2}' )
                              ,[UserDes] = '{3}'
                              ,[UserFname] = '{4}'
                              ,[UserLname] = '{5}'
                              ,[Gender] = '{6}'
                              ,[UserAddress] ='{7}'
                              ,[UserPhone] = '{8}'
                              ,[BicycleType] = '{9}'
                              ,[ImagePath] = '{10}'
                              ,[BirthDate] ='{11}'
                              ,[CurDate] = '{12}'
                              ,[Captain] = {13}
                              ,[Organization] = (select [Organization] from Organizations where OrganizationName = '{14}')
                         WHERE [Id] = (select id from AspNetUsers where UserName = '" + username + "');", rdr.RiderEmail, 0, rdr.City, rdr.RiderDes, rdr.RiderFname, rdr.RiderLname, rdr.Gender, rdr.RiderAddress, rdr.RiderPhone, rdr.BicycleType, rdr.ImagePath, rdr.BirthDate, DateTime.Now.Date.ToString("yyyy-MM-dd"), rdr.Captain, rdr.Organization);
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
            lf.Main("Groups", ex.Message);
            return 0; 
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
            lf.Main("Groups", ex.Message);
            return 0;
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
        sb.AppendFormat("Values('{0}', (select Organization from Organizations where OrganizationName = '{1}' ) ,'{2}')",grp.GroupName, grp.OrganizationName, grp.GroupDes);
        command = prefix + sb.ToString();
        return command;
    }
}
