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
    #region Init
    public SqlDataAdapter da;
    public DataTable dt;
    LogFiles lf = new LogFiles();

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Connections Methods

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
    // **************************************************************************************
#endregion
    
    #region Read From Data Base
    public DBservices ReadFromDataBase(int select, string data1, string data2 = "", string data3 = "", string data4="")
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
                    selectStr = @" SELECT anu.UserName, U.UserEmail, U.UserDes, U.UserFname, U.UserLname, U.ImagePath, U.Gender, U.Captain, U.UserAdrees, U.UserPhone, convert(varchar(10), U.BirthDate, 120) As BirthDate, U.BicycleType, U.UserAddress, C.CityName As RiderCity, G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage
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
            selectStr = @" SELECT anu.UserName, U.UserEmail, U.UserDes, U.UserFname, U.UserLname,U.UserAdrees, U.UserPhone, U.ImagePath, U.Gender, U.Captain, convert(varchar(10), U.BirthDate, 120) As BirthDate, U.BicycleType, U.UserAddress, C.CityName As RiderCity, G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, CO.CityName As OrgCity
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
            selectStr = @"SELECT [GroupName],[GroupDes]
                                FROM [Groups] G, Organizations O
                                WHERE [GROUP] <> 0 
                                AND G.[Organization] = O.[Organization]
                                AND O.OrganizationName = '" + data1 + @"';"; // Group From ORG
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
												WHERE G.[Group] = UG.[Group]) ;";//ReadFromDataBaseforGroup
			break;
			case 5:
			selectStr = @" SELECT 'Exists'
                                FROM AspNetUsers
                                WHERE [UserName] = '" + data1 + "' ;"; // ReadFromDataBaseUserName
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
            case 8:
            selectStr = @"SELECT  R.[RideName], R.[RideType], convert(varchar(10), R.[RideDate], 120) As RideDate, R.[RideLength], R.[RideSource], R.[RideDestination]
                          FROM [Rides] R, Users U, AspNetUsers anu
                          WHERE R.[User] = U.[User]
                          AND   U.Id = anu.Id
                          AND   anu.UserName = '" + data1 + "' ;"; //ReadFromDataBase ,

			break;
            case 9:
            selectStr = @"SELECT R.Route, R.[RouteName], R.[RouteType], R.[RouteDestination], R.[RouteLength], R.[Comments], R.[RouteSource]
                          FROM [Routes] R, Users U, AspNetUsers anu
                          WHERE R.[User] = U.[User]
                          AND   U.Id = anu.Id
                          AND   anu.UserName = '" + data1 + "' ;"; //ReadFromDataBase 
            break;
            case 10:
            selectStr = @"SELECT  DATEPART(mm, R.RideDate) AS [Month], DATEPART(yyyy, R.RideDate) AS [Year], Sum(R.[RideLength]) As User_KM, COUNT(R.RideDate) As Num_of_Rides, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS User_Points, Sum(R.[RideLength])*0.16 As User_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As User_Calories
                        FROM [Rides] R, Users U, AspNetUsers anu
                        Where R.[USER] = U.[User]
                        AND U.Id = anu.Id
                        AND anu.UserName = '" + data1 + @"' 
                        group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate);"; //ReadFromDataBase 
            break; 
            case 11:
            selectStr = @"SELECT DATEPART(mm, R.RideDate) AS [Month], DATEPART(yyyy, R.RideDate) AS [Year], Sum(R.[RideLength]) As Group_KM, COUNT(R.RideDate) As Num_of_Rides, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS Group_Points, Sum(R.[RideLength])*0.16 As Group_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As Group_Calories
                        FROM Groups G, Organizations O, Users U, Rides R
                        Where G.[Group] <> 0
                        AND G.GroupName = '" + data1 + @"'
                        AND G.Organization = O.Organization
                        AND O.OrganizationName = '" + data2 + @"'
                        AND U.[User] in ( SELECT UG.[User]
			                        FROM UsersGroups UG
			                        WHERE G.[Group] = UG.[Group])
                        AND R.[User] = U.[User] 
                        AND U.Gender like '" + data3 + @"%'
                        group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate);"; //ReadFromDataBase 
            break;
            case 12:
            selectStr = @"SELECT DATEPART(mm, R.RideDate) AS [Month], DATEPART(yyyy, R.RideDate) AS [Year], Sum(R.[RideLength]) As Organization_KM, COUNT(R.RideDate) As Num_of_Rides, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS Organization_Points, Sum(R.[RideLength])*0.16 As Organization_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As Organization_Calories
                        FROM Groups G, Organizations O, Users U, Rides R
                        Where G.[Group] <> 0
                        AND G.Organization = O.Organization
                        AND O.OrganizationName = '" + data1 + @"'
                        AND U.[User] in ( SELECT UG.[User]
			                        FROM UsersGroups UG
			                        WHERE G.[Group] = UG.[Group])
                        AND R.[User] = U.[User] 
                        AND U.Gender like '" + data2 + @"%'
                        group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate);"; //ReadFromDataBase 
            break;  
                   case 13:
            selectStr = @" SELECT DATEPART(mm, R.RideDate) AS [Month], DATEPART(yyyy, R.RideDate) AS [Year], Sum(R.[RideLength]) As Users_KM, COUNT(R.RideDate) As Num_of_Rides, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS Users_Points, Sum(R.[RideLength])*0.16 As Users_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As Users_Calories
                            FROM Users U, Rides R
                            Where R.[Ride] <> 0
                            AND R.[User] = U.[User]
                            AND U.Gender like '" + data1 + @"%'
                            group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate);"; //ReadFromDataBase 
            break;
            case 14:
            selectStr = @" SELECT 'Exists'
                                FROM [Users]
                                WHERE [UserEmail] = '" + data1 + "' ;"; // ReadFromDataBaseUserName
            break;
            case 15:
                            data4 = (data4 == "Days" ? "Num_Of_Days_Riden" : (data4 == "Kilometers" ? "User_KM" : "User_Points"));
            selectStr = @" SELECT DATEPART(yyyy, R.RideDate) AS [Year], DATEPART(mm, R.RideDate) AS [Month], anu.UserName, Sum(R.[RideLength]) As User_KM, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS User_Points, Sum(R.[RideLength])*0.16 As User_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As User_Calories
                            FROM Groups G, Organizations O,[Rides] R, Users U, AspNetUsers anu
                            Where U.[User]<> 0
                            AND R.[Ride] <> 0
                            AND R.[USER] = U.[User]
                            AND U.Id = anu.Id
                            AND U.Gender like '" + data3 + @"%'
                            AND G.[Group] <> 0
                            AND G.GroupName like '" + data1 + @"%'
                            AND G.Organization = O.Organization
                            AND O.OrganizationName like '" + data2 + @"%'
                            AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group])
                            Group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), anu.UserName
                            Order By DATEPART(yyyy, R.RideDate)DESC, DATEPART(mm, R.RideDate)DESC, " + data4 + @" DESC ;"; // ReadFromDataBase User Ranking
            break;
            case 16:
            data3 = (data3 == "Days" ? "Num_Of_Days_Riden" : (data3 == "Kilometers" ? "User_KM" : "User_Points"));
            selectStr = @"SELECT DATEPART(yyyy, R.RideDate) AS [Year], DATEPART(mm, R.RideDate) AS [Month], G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, Sum(R.[RideLength]) As Group_KM, COUNT(R.RideDate) As Num_of_Rides, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS Group_Points, Sum(R.[RideLength])*0.16 As Group_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As Group_Calories
                            FROM Groups G, Organizations O, Users U, Rides R
                            Where G.[Group] <> 0
                            AND G.Organization = O.Organization
                            AND O.OrganizationName like '" + data1 + @"%'
                            AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group])
                            AND R.[User] = U.[User] 
                            AND U.Gender like '" + data2 + @"%'
                            group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes
                            order by DATEPART(yyyy, R.RideDate)DESC, DATEPART(mm, R.RideDate)DESC,  " + data3 + @" DESC ;"; // ReadFromDataBase Group Ranking
            break;
            case 17:
            data2 = (data2 == "Days" ? "Num_Of_Days_Riden" : (data2 == "Kilometers" ? "User_KM" : "User_Points"));
            selectStr = @" SELECT DATEPART(yyyy, R.RideDate) AS [Year], DATEPART(mm, R.RideDate) AS [Month], O.OrganizationName, O.OrganizationDes, Sum(R.[RideLength]) As Organization_KM, COUNT(R.RideDate) As Num_of_Rides, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS Organization_Points, Sum(R.[RideLength])*0.16 As Organization_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As Organization_Calories
                            FROM Groups G, Organizations O, Users U, Rides R
                            Where G.[Group] <> 0
                            AND G.Organization = O.Organization
                            AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group])
                            AND R.[User] = U.[User] 
                            AND U.Gender like '" + data1 + @"%'
                            group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes
                            order by DATEPART(yyyy, R.RideDate)DESC, DATEPART(mm, R.RideDate)DESC,  " + data2 + @" DESC ;"; // Read From Data Base Organization Ranking
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
#endregion

    #region Insert New Record
    public int InsertDatabase(List<Object> s, string data1="", string data2="", string data3="", string data4="")
    {
        string _class ="";
        string typeOfObject = s[0].GetType().ToString();
        
        if(typeOfObject.Contains("Routes"))  _class = "Routes";
        else if(typeOfObject.Contains("Organization"))  _class = "Organization";
        else if(typeOfObject.Contains("Group"))  _class = "Group";
        else if (typeOfObject.Contains("Rides")) _class = "Rides";
        
        SqlConnection con;
        SqlCommand cmd;
        String cStr = "";
        try
        {
            con = connect("DefaultConnection"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            lf.Main(_class, ex.Message);
            return 0;
        }
        switch (_class)
        {
            case "Routes":
                foreach (Routes obj in s)
                cStr = BuildInsertRoutesCommand(obj);      // helper method to build the insert string
            break;
            
            case "Group":
                foreach (Group obj in s)
                cStr = BuildInsertGroupCommand(obj);      // helper method to build the insert string
            break;

            case "Organization":
            foreach (Organization obj in s)
                cStr = BuildInsertOrganizationCommand(obj);      // helper method to build the insert string
            break;
            
            case "Rides":
            if (data1 == "")
            {
                foreach (Rides obj in s)
                    cStr = BuildInsertRidesCommand(obj); // helper method to build the insert string
            }
            else
            { 
            string ridename = "";
            ridename = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");
            cStr = BuildInsertRidesFromroutesCommand1(data1, data2, data3, ridename, data4);
            }
            break;
        }       
        cmd = CreateCommand(cStr, con);             // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            lf.Main(_class, ex.Message);
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
    #endregion

    #region Delete Record
    public int DeleteDatabase(string _class, string data1, string data2 = "")
    {
        SqlConnection con;
        SqlCommand cmd;
        String cStr = "";
        try
        {
            con = connect("DefaultConnection"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            lf.Main(_class, ex.Message);
            return 0;
        }
        switch (_class)
        {
            case "Rider":
               cStr = BuildDelteRiderCommand(data1);      // helper method to build the insert string
                break;

            case "Rides":
                cStr = BuildDelteRideCommand(data1, data2);      // helper method to build the insert string
                break;

            case "Routes":
                cStr = BuildDelteRouteCommand(data1, data2);      // helper method to build the insert string
                break;
            
        }
        cmd = CreateCommand(cStr, con);             // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            lf.Main(_class, ex.Message);
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
    #endregion

    #region Organizations
    
    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertOrganizationCommand(Organization org)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "INSERT INTO Organizations(  OrganizationName, [City], OrganizationDes, OrganizationType, OrganiztionImage ) ";
        sb.AppendFormat("Values('{0}', (select city from Cities where CityName = '{1}' ) ,'{2}', '{3}','{4}' )", org.Organizationname, org.OrganizationCity, org.OrganizationDes, org.OrganizationType, org.OrganizationImage);
        command = prefix + sb.ToString();
        return command;
    }
#endregion
    
    #region Rider
    // Update An Existing Rider
    public int updateRiderInDatabase(Rider rdr, string username)
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
        String cStr = BuildUpdateRiderCommand(rdr, username);      // helper method to build the insert string
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
    // Insert new Rider
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
        String prefix = "INSERT INTO Users(  UserEmail, City, UserDes, UserFname, UserLname, UserAddress, UserPhone, Gender, BicycleType, ImagePath, BirthDate, [CurDate], [Id], Captain, [Organization] ) ";
        sb.AppendFormat("Values('{0}', (select [city] from Cities where CityName = '{1}' ), '{2}', '{3}' ,'{4}', '{5}','{6}','{7}', '{8}','{9}', '{10}', '{11}', (select id from AspNetUsers where UserName = '{12}'), {13}, (select Organization from Organizations where OrganizationName = '{14}'))", rdr.RiderEmail, rdr.City, rdr.RiderDes, rdr.RiderFname, rdr.RiderLname, rdr.RiderAddress, rdr.RiderPhone, rdr.Gender, rdr.BicycleType, rdr.ImagePath, rdr.BirthDate, DateTime.Now.Date.ToString("yyyy-MM-dd"), rdr.Username, rdr.Captain, rdr.Organization);
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
    private String BuildUpdateRiderCommand(Rider rdr, string username)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = "UPDATE [Users] ";
        sb.AppendFormat(@"SET [UserEmail] ='{0}'
                              ,[City] = (select city from Cities where CityName = '{1}' )
                              ,[UserDes] = '{2}'
                              ,[UserFname] = '{3}'
                              ,[UserLname] = '{4}'
                              ,[Gender] = '{5}'
                              ,[BicycleType] = '{6}'
                              ,[ImagePath] = '{7}'
                              ,[BirthDate] ='{8}'
                              ,[CurDate] = '{9}'
                              ,[Captain] = {10}
                              ,[Organization] = (select [Organization] from Organizations where OrganizationName = '{11}')
                         WHERE [Id] = (select id from AspNetUsers where UserName = '" + username + "');", rdr.RiderEmail, rdr.City, rdr.RiderDes, rdr.RiderFname, rdr.RiderLname, rdr.Gender, rdr.BicycleType, rdr.ImagePath, rdr.BirthDate, DateTime.Now.Date.ToString("yyyy-MM-dd"), rdr.Captain, rdr.Organization);
        command = prefix + sb.ToString();
        return command;
    }
#endregion
    
    #region Groups
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
    #endregion
    
    #region Rides
    private String BuildInsertRidesFromroutesCommand1(string username, string routename, string ridedate, string ridename, string roundtrip)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        int isroundtrip = 0;
        if ((roundtrip == "True") || (roundtrip == "true"))
            isroundtrip = 2;
        else
            isroundtrip = 1;

        String prefix = @" declare @user_val int;
                           set @user_val = 0;
            select @user_val=U.[User] from Users U, AspNetUsers A where A.UserName = '" + username + @"' AND A.Id = U.Id 
            INSERT INTO [Rides]
           ([RideName]
           ,[User]
           ,[RideDes]
           ,[RideType]
           ,[RideDate]
           ,[RideLength]
           ,[RideSource]
           ,[RideDestination]) ";
        sb.AppendFormat("Values('{0}', @user_val, '{1}', (Select [RouteType] From [Routes] Where [RouteName]='{2}' AND [User] = @user_val) ,'{3}', (Select [RouteLength] * " + isroundtrip + @" From [Routes] Where [RouteName]='{4}' AND [User] = @user_val), (Select [RouteSource] From [Routes] Where [RouteName]='{5}' AND [User] = @user_val), (Select [RouteDestination] From [Routes] Where [RouteName]='{6}' AND [User] = @user_val) )", ridename, "Route_" + routename, routename, ridedate, routename, routename, routename);
        command = prefix + sb.ToString();
        return command;
    }
        
    private String BuildInsertRidesCommand(Rides rds)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = @"INSERT INTO [Rides]
           ([RideName]
           ,[User]
           ,[RideDes]
           ,[RideType]
           ,[RideDate]
           ,[RideLength]
           ,[RideSource]
           ,[RideDestination]) ";
        sb.AppendFormat("Values('{0}', (select U.[User] from Users U, AspNetUsers A where A.UserName = '{1}' AND A.Id = U.Id ), '{2}', '{3}' ,'{4}', {5},'{6}','{7}')", rds.RideName, rds.UserName, rds.RideDes, rds.RideType, rds.RideDate, rds.RideLength,rds.RideSource,rds.RideDestination);
        command = prefix + sb.ToString();
        return command;
    }
    
    private String BuildDelteRideCommand(string username, string ridename)
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
			                   DELETE FROM [Rides] Where [USER] = @val AND [RideName] = '" + ridename + @"';
                        end";
        command = prefix;
        return command;
    }
    #endregion
    
    #region Routes
    private String BuildInsertRoutesCommand(Routes rut)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = @"INSERT INTO [Routes]
           ([RouteName]
           ,[RouteDestination]
           ,[RouteType]
           ,[RouteLength]
           ,[Comments]
           ,[RouteSource]
           ,[User]) ";
        sb.AppendFormat("Values('{0}', '{1}','{2}',{3},'{4}','{5}', (select U.[User] from Users U, AspNetUsers A where A.UserName = '{6}' AND A.Id = U.Id ))", rut.RouteName, rut.RouteDestination, rut.RouteType, rut.RouteLength, rut.Comments, rut.RouteSource, rut.UserName);
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildDelteRouteCommand(string username, string routename)
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
			                   DELETE FROM [Routes] Where [USER] = @val AND [RouteName] = '" + routename + @"';
                        end";
        command = prefix;
        return command;
    }
    #endregion
}
