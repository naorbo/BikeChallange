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
    public SqlDataAdapter da, db, dc, dd;
    public DataTable dt, dt1;
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
    public DBservices ReadFromDataBase(int select, string data1 = "", string data2 = "", string data3 = "", string data4 = "", string data5 = "")
    {
        DBservices dbS = new DBservices(); // create a helper class
        SqlConnection con = null;
        try
        {
            con = dbS.connect("DefaultConnection"); // open the connection to the database/
            String selectStr, selectStr1, selectStr2, selectStr3, selectStr4, selectStr5, selectStr6;
            selectStr = selectStr1 = selectStr2 = selectStr3 = selectStr4 = selectStr5 = selectStr6 = "";
            switch (select)
            {
                case 1:
                    selectStr = @" SELECT anu.UserName, U.UserEmail, U.UserDes, U.UserFname, U.UserLname, U.ImagePath, U.Gender, U.Captain, U.UserAddress, U.UserPhone, convert(varchar(10), U.BirthDate, 120) As BirthDate, U.BicycleType, C.CityName As RiderCity, G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, U.UserFname +' '+ U.UserLname As UserDisplayName, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS User_Points
                                    FROM UsersGroups UG, Users U, AspNetUsers anu, Groups G, Organizations O, Cities C,[Rides] R
                                    Where U.[User] <> 0
                                    AND U.Id = anu.Id  
                                    AND G.[Organization] = O.[Organization]
                                    AND UG.[User] = U.[User]
                                    AND UG.[Group] = G.[Group]
                                    AND U.City = C.City
                                    AND G.GroupDes = '" + data1 + @"'
                                    AND O.OrganizationDes = '" + data2 + @"'
                                    AND R.[USER] = U.[User]
                                    AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, GETDATE()) AND DATEPART(mm, R.RideDate) like DATEPART(mm, GETDATE())
                                    group by anu.UserName, U.UserEmail, U.UserDes, U.UserFname, U.UserLname, U.ImagePath, U.Gender, U.Captain, U.UserAddress, U.UserPhone,U.BirthDate, U.BicycleType, C.CityName, G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, U.UserFname +' '+ U.UserLname"; //GET RIDER PER GRUOP

                    break;
                case 2:
                    if (data1 != "bcadministrator")
                    {
                        selectStr =
                        @" SELECT U.UserDes AS UserName, U.UserEmail, U.UserDes, U.UserFname, U.UserLname,U.UserAddress, U.UserPhone, U.ImagePath, U.Gender, U.Captain, convert(varchar(10), U.BirthDate, 120) As BirthDate, U.BicycleType, C.CityName As RiderCity, G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, CO.CityName As OrgCity, U.Designer
                                FROM UsersGroups UG, Users U, Groups G, Organizations O, Cities C, Cities CO
                                Where U.[User] <> 0
                                AND G.Organization = O.Organization                            
                                AND UG.[User] = U.[User]
                                AND UG.[Group] = G.[Group]
                                AND U.City = C.City
                                AND O.City = CO.City
                                AND U.UserDes = '" + data1 + "';";
                        //GET RIDER from USERNAME
                    }
                    else
                    {
                        selectStr =
                        @" SELECT U.UserDes AS UserName,U.UserEmail, U.UserDes, U.ImagePath, U.Gender, U.Captain, convert(varchar(10), U.BirthDate, 120) As BirthDate, U.Designer
                        FROM Users U
                        Where U.UserDes = 'bcadministrator';" ;
                    }
                    break;
                case 3:
                    selectStr = @"SELECT [GroupName],[GroupDes]
                                FROM [Groups] G, Organizations O
                                WHERE [GROUP] <> 0 
                                AND G.[Organization] = O.[Organization]
                                AND O.OrganizationDes = '" + data1 + @"';"; // Group From ORG
                    break;
                case 4:

                    selectStr = @" SELECT G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, C.CityName As ORG_City, anu.UserName AS Captain_UserName, U.UserFname + ' ' + U.UserLname As DisplayName
                                FROM Groups G, Organizations O, AspNetUsers anu, Users U, Cities C
                                Where G.[Group] <> 0
                                AND G.GroupDes = '" + data1 + @"'
                                AND G.Organization = O.Organization
                                AND O.OrganizationDes = '" + data2 + @"'
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
                                AND O.OrganizationDes = '" + data1 + @"'
                                AND O.City = C.City ; "; //ReadFromDataBaseforRiderorgname
                    break;
                case 7:
                    if (data1 == "Organizations")
                        selectStr = @"SELECT O.OrganizationDes, o.OrganizationType, o.OrganiztionImage, C.CityName, COUNT(G.[group]) OrgGroupCount, ( SELECT Count(UG.[User])
                                        FROM UsersGroups UG, Groups G1
                                        WHERE G1.[Group] = UG.[Group]
                                        AND G1.Organization = O.Organization
                                         ) As OrgHeadCount
                                        FROM Organizations O, Cities C, Groups G
                                        Where O.Organization <> 0 
                                        and o.City = C.city 
                                        and o.Organization = G.Organization
                                        Group by O.OrganizationDes, o.OrganizationType, o.OrganiztionImage, C.CityName, O.Organization";
                    else if(data1 == "Groups")
                        {
                            selectStr = @"SELECT G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, C.CityName As ORG_City, anu.UserName AS Captain_UserName, U.UserFname + ' ' + U.UserLname As DisplayName, COUNT(U2.[user]) GroupHeadCount
                                        FROM Groups G, Organizations O, AspNetUsers anu, Users U,Users U2, Cities C
                                        Where G.[Group] <> 0
                                        AND G.GroupDes like '%'
                                        AND G.Organization = O.Organization
                                        AND O.OrganizationDes like '%'
                                        AND O.City = C.City 
                                        AND anu.Id = U.Id
                                        AND U.Captain = 1
                                        AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group] )
                                        AND U2.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group] )
                                        group by G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, C.CityName , anu.UserName , U.UserFname ,U.UserLname";
                        }
                    else 
                        selectStr = "SELECT * FROM " + data1 + " Where " + data2 + " <> 0"; //ReadFromDataBase 
                    break;
                /**/
                case 8:
                    if (data1 != "")
                    {
                        selectStr = @"SELECT  R.[RideName], R.[RideType], convert(varchar(10), R.[RideDate], 120) As RideDate, R.[RideLength], R.[RideSource], R.[RideDestination]
                          FROM [Rides] R, Users U, AspNetUsers anu
                          WHERE R.[User] = U.[User]
                          AND   U.Id = anu.Id
                          AND   anu.UserName like '" + data1 + "%' ;"; //ReadFromDataBase
                    }
                    else
                    {
                        selectStr = @"SELECT  R.[RideName], R.[RideType], convert(varchar(10), R.[RideDate], 120) As RideDate, R.[RideLength], R.[RideSource], R.[RideDestination],anu.UserName, C.CityName As RiderCity, G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, U.UserFname +' '+ U.UserLname As UserDisplayName
                                        FROM [Rides] R, UsersGroups UG, Users U, AspNetUsers anu, Groups G, Organizations O, Cities C
                                        WHERE U.[User] <> 0
                                        AND   R.[User] = U.[User]
                                        AND   U.Id = anu.Id
                                        AND   anu.UserName like '%' 
                                        AND   G.[Organization] = O.[Organization]
                                        AND   UG.[User] = U.[User]
                                        AND   UG.[Group] = G.[Group]
                                        AND   u.City = c.City;";
                    }

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
                        AND G.GroupDes = '" + data1 + @"'
                        AND G.Organization = O.Organization
                        AND O.OrganizationDes = '" + data2 + @"'
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
                        AND O.OrganizationDes = '" + data1 + @"'
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
                    selectStr = @" SELECT " + (data5 != "" ? "TOP 10" : "") + @" DATEPART(yyyy, R.RideDate) AS [Year], DATEPART(mm, R.RideDate) AS [Month], anu.UserName,U.UserFname +' '+ U.UserLname As UserDisplayName, Sum(R.[RideLength]) As User_KM, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS User_Points, Sum(R.[RideLength])*0.16 As User_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As User_Calories, G.GroupDes, O.OrganizationDes, C.Cityname As UserCity, C1.CityName As OrganizationCity
                            FROM Groups G, Organizations O,[Rides] R, Users U, AspNetUsers anu, Cities C, Cities C1
                            Where U.[User]<> 0
                            AND R.[Ride] <> 0
                            AND R.[USER] = U.[User]
                            AND U.Id = anu.Id
                            AND U.Gender like '" + data3 + @"%'
                            AND G.[Group] <> 0
                            AND G.GroupDes like '" + data1 + @"%'
                            AND G.Organization = O.Organization
                            AND O.OrganizationDes like '" + data2 + @"%'
                            AND U.City = C.City
                            AND O.City = C1.City
                            AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group])
                            " + (data5 != "" ? " AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data5 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data5 + @"')" : "") + @"
                            Group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), anu.UserName, U.UserFname +' '+ U.UserLname, G.GroupDes, O.OrganizationDes, C.Cityname, C1.CityName 
                            Order By DATEPART(yyyy, R.RideDate)DESC, DATEPART(mm, R.RideDate)DESC, " + data4 + @" DESC ;"; // ReadFromDataBase User Ranking
                    break;
                case 16:
                    data3 = (data3 == "Days" ? "Num_Of_Days_Riden" : (data3 == "Kilometers" ? "Group_KM" : "Group_Points"));
                    selectStr = @"SELECT " + (data4 != "" ? "TOP 10" : "") + @" DATEPART(yyyy, R.RideDate) AS [Year], DATEPART(mm, R.RideDate) AS [Month], G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, Sum(R.[RideLength]) As Group_KM, COUNT(R.RideDate) As Num_of_Rides, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS Group_Points, Sum(R.[RideLength])*0.16 As Group_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As Group_Calories, C.CityName
                            FROM Groups G, Organizations O, Users U, Rides R, Cities C
                            Where G.[Group] <> 0
                            AND G.Organization = O.Organization
                            AND O.OrganizationDes like '" + data1 + @"%'
                            AND O.City = C.City
                            AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group])
                            " + (data4 != "" ? " AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data4 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data4 + @"')" : "") + @"
                            AND R.[User] = U.[User] 
                            AND U.Gender like '" + data2 + @"%'
                            group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, C.CityName
                            order by DATEPART(yyyy, R.RideDate)DESC, DATEPART(mm, R.RideDate)DESC,  " + data3 + @" DESC ;"; // ReadFromDataBase Group Ranking
                    break;
                case 17:
                    data2 = (data2 == "Days" ? "Num_Of_Days_Riden" : (data2 == "Kilometers" ? "Organization_KM" : "Organization_Points"));
                    selectStr = @" SELECT " + (data3 != "" ? "TOP 10" : "") + @" DATEPART(yyyy, R.RideDate) AS [Year], DATEPART(mm, R.RideDate) AS [Month], O.OrganizationName, O.OrganizationDes, Sum(R.[RideLength]) As Organization_KM, COUNT(R.RideDate) As Num_of_Rides, COUNT(distinct R.RideDate) As Num_Of_Days_Riden, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS Organization_Points, Sum(R.[RideLength])*0.16 As Organization_CO2_Kilograms_Saved, Sum(R.[RideLength])*25 As Organization_Calories, C.CityName
                            FROM Groups G, Organizations O, Users U, Rides R, Cities C
                            Where G.[Group] <> 0
                            AND G.Organization = O.Organization
                            AND O.City = C.City
                            AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group])
                            AND R.[User] = U.[User] 
                            AND U.Gender like '" + data1 + @"%'
                            " + (data3 != "" ? " AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data3 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data3 + @"')" : "") + @"
                            group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), O.OrganizationName, O.OrganizationDes, C.CityName
                            order by DATEPART(yyyy, R.RideDate)DESC, DATEPART(mm, R.RideDate)DESC,  " + data2 + @" DESC ;"; // Read From Data Base Organization Ranking
                    break;
                case 18:

                    selectStr = "Select COUNT( [User]) As NumOfUsers from users where [user]<>0 ;";
                    selectStr1 = "Select COUNT( [Organization]) As NumOfOrganizations from Organizations where Organization<>0;";
                    selectStr2 = "Select COUNT([Group]) As NumOfGroups from Groups where [Group]<>0;";
                    selectStr3 = @"Select COUNT( [Ride]) As NumOfRides,Sum( [RideLength]) As TotalKM,
                           Sum( [RideLength])*0.16 As TotalCO2, Sum( [RideLength])*25 As TotalCalories from Rides where Ride<>0;"; // Read From Data Base Organization Ranking 
                    break;

                case 19:
                    // Works - count the num of rows above the user // ,  1 AS Organization_Points,  1 AS Group_Points,
                    selectStr = @"SELECT G.GroupDes, O.OrganizationDes
                                FROM UsersGroups UG, Users U, Groups G, Organizations O
                                Where U.[User] <> 0
                                AND G.Organization = O.Organization                            
                                AND UG.[User] = U.[User]
                                AND UG.[Group] = G.[Group]
                                AND U.UserDes = '"+data1+@"'
                                group by G.GroupDes, O.OrganizationDes;";// Read From Data Base Organization Ranking
                    break;

                case 20:

                    selectStr = @"  Select e.EventName, e.EventDes, convert(varchar(10), e.EventDate, 120) As EventDate, e.EventType, C.CityName,COUNT(ue2.[User]) AS NumOfRidersInEvent, e.[EventTime], e.[EventAddress], e.[EventDetails]
                            From UsersEvents ue, Users U, [Events] e, Cities C, UsersEvents UE2
                            Where ue.[User] = u.[User]
                            AND u.UserDes = '" + data1 + @"'
                            AND ue.[Event] = e.[Event]
                            AND e.City = c.City
                            AND ue.[Event] = UE2.[Event]
                            group by  e.EventName,e.EventDes, convert(varchar(10), e.EventDate, 120), e.EventType, C.CityName, e.[EventTime], e.[EventAddress], e.[EventDetails] "; // Read From Data Base Organization Ranking
                    break;
                case 21:

                    selectStr = @"  Select e.EventName, e.EventDes, convert(varchar(10), e.EventDate, 120) As EventDate, e.EventType, e.EventStatus, C.CityName, COUNT(ue.[User])-1 NumOfRidersInEvent, e.[EventTime], e.[EventAddress], e.[EventDetails]
                            From [Events] e, Cities C, usersevents ue
                            Where e.[Event] <> 0
                            AND e.City = c.City
                            AND e.[event] = ue.[event]
                            group by e.EventName, e.EventDes, convert(varchar(10), e.EventDate, 120) , e.EventType, e.EventStatus, C.CityName, e.[EventTime], e.[EventAddress], e.[EventDetails]"; // Read From Data Base Organization Ranking
                    break;
                case 22:

                    selectStr = @"  Select e.EventName,e.EventDes, convert(varchar(10), e.EventDate, 120) As EventDate, e.EventType, e.[EventTime], e.[EventAddress], e.[EventDetails], C.CityName AS EventCity, U.UserEmail, U.UserDes, U.UserFname, U.UserLname, U.ImagePath, U.Gender, U.Captain, U.UserAddress, U.UserPhone, convert(varchar(10), U.BirthDate, 120) As BirthDate, U.BicycleType, CU.CityName As RiderCity, G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, U.UserFname +' '+ U.UserLname As UserDisplayName
                            From UsersEvents ue, [Events] e,  Users U, Cities C, Cities CU, Groups g, Organizations o, usersgroups ug
                            Where e.[Event] <> 0
                            AND e.EventName = '" + data1 + @"'
							AND e.City = c.City
                            AND e.[Event] = ue.[Event]
                            AND ue.[User] = U.[User]
                            AND U.City = cu.city
                            AND U.[User] = ug.[user]
                            AND ug.[Group] = g.[group]
                            AND g.[Organization] = o.[Organization] ;"; // Read From Data Base Organization Ranking
                    break;
                case 23:
                    selectStr = @" SELECT 'Exists'
                                FROM [Organizations]
                                WHERE [OrganizationDes] = '" + data1 + "' ;"; // ReadFromDataBaseUserName
                    break;
                case 24:
                    selectStr = @"  IF EXISTS (SELECT '" + data3 + @"' AS Category, [UserEmail],[UserDes],[UserFname],[UserLname],[UserAddress],[UserPhone],convert(varchar(10), [BirthDate], 120) As BirthDate,[BicycleType],[ImagePath],[Gender], g.GroupDes,o.OrganizationDes,O.OrganiztionImage, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS RiderPoints
                            FROM [Rides] R, Users U, UsersGroups ug, Groups g, Organizations o
                            Where U.[User]<> 0
                            AND R.[USER] = U.[User]
                            AND  U.[User]=ug.[User]
                            AND ug.[Group]=g.[Group]
                            AND g.Organization=o.Organization
                            AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy,'" + data2 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm,'" + data2 + @"')
                            Group by [UserEmail],[UserDes],[UserFname],[UserLname],[UserAddress],[UserPhone],[BirthDate],[BicycleType],[ImagePath],[Gender], g.GroupDes,o.OrganizationDes,O.OrganiztionImage
                            having Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) > " + data1 + @") 
                            BEGIN
                                SELECT '" + data3 + @"' AS Category, [UserEmail],[UserDes],[UserFname],[UserLname],[UserAddress],[UserPhone],[BirthDate],[BicycleType],[ImagePath],[Gender], g.GroupDes,o.OrganizationDes,O.OrganiztionImage, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS RiderPoints
                                FROM [Rides] R, Users U, UsersGroups ug, Groups g, Organizations o
                                Where U.[User]<> 0
                                AND R.[USER] = U.[User]
                                AND  U.[User]=ug.[User]
                                AND ug.[Group]=g.[Group]
                                AND g.Organization=o.Organization
                                AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data2 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm,'" + data2 + @"')
                                Group by [UserEmail],[UserDes],[UserFname],[UserLname],[UserAddress],[UserPhone],[BirthDate],[BicycleType],[ImagePath],[Gender], g.GroupDes,o.OrganizationDes,O.OrganiztionImage
                                having Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) >  " + data1 + @"
                            END
                            ELSE
                            BEGIN
                                SELECT '" + data3 + @"' AS Category, 'No Rider At This Category' AS UserEmail,'No Rider At This Category' AS UserDes,'No Rider At This Category' AS UserFname,'No Rider At This Category' AS UserLname,'No Rider At This Category' AS UserAddress,'No Rider At This Category' AS UserPhone,'" + data2 + @"' AS BirthDate,'No Rider At This Category' AS BicycleType,'No Rider At This Category' AS ImagePath,'No Rider At This Category' AS Gender, 'No Rider At This Category' AS GroupDes,'No Rider At This Category' AS OrganizationDes,'No Rider At This Category' AS OrganiztionImage, 0 AS RiderPoints
                                From Users Where [User]=0;
                            END "; // ReadFromDataBaseUserName
                    break;
                case 25:
                    selectStr = @"  SELECT U.UserEmail, U.UserDes, U.UserFname, U.UserLname,U.UserAddress, U.UserPhone, U.ImagePath, U.Gender, U.Captain, convert(varchar(10), U.BirthDate, 120) As BirthDate,convert(varchar(10), U.[CurDate], 120) As JoinDate, U.BicycleType, C.CityName As RiderCity, G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, CO.CityName As OrgCity
                            FROM UsersGroups UG, Users U, Groups G, Organizations O, Cities C, Cities CO
                            Where U.[User] <> 0
                            AND G.Organization = O.Organization                            
                            AND UG.[User] = U.[User]
                            AND UG.[Group] = G.[Group]
                            AND U.City = C.City
                            AND O.City = CO.City "; // ReadFromDataBaseUserName
                    break;
                
                case 26:
                    selectStr = @"  SELECT [CompetitionDate]
                                          ,[OrgWin], O.OrganizationType OrgWin_Type , O.OrganiztionImage OrgWin_Image, OC.CityName OrgWin_City
                                          ,[GrpWin], Og.OrganizationDes GrpWin_Org, Cg.cityname GrpWin_Org_City, og.OrganiztionImage GrpWin_Org_Img
                                          ,[PlatinumUser], Pusers.ImagePath PlatinumImg, convert(varchar(10), Pusers.BirthDate, 120) As PlatinumBirthDate, Pusers.BicycleType PlatinumBicycleType, Pusers.CityName PlatinumCity, Pusers.Groupdes PlatinumGroup, Pusers.OrganizationDes PlatinumOrganization --, C.CityName As RiderCity, G.GroupDes, O.OrganizationDes, O.OrganiztionImage, Pusers.UserFname +' '+ Pusers.UserLname As UserDisplayName
                                          ,[GoldUser], Gusers.ImagePath GoldImg, convert(varchar(10), Gusers.BirthDate, 120) As GoldBirthDate, Gusers.BicycleType GoldBicycleType, Gusers.CityName GoldCity, Gusers.Groupdes GoldGroup, Gusers.OrganizationDes GoldOrganization
                                          ,[SilverUser], Susers.ImagePath SilverImg, convert(varchar(10), Susers.BirthDate, 120) As SilverBirthDate, Susers.BicycleType SilverBicycleType, Susers.CityName SilverCity, Susers.Groupdes SilverGroup, Susers.OrganizationDes SilverOrganization
                                          ,[BronzeUser], Busers.ImagePath BronzeImg, convert(varchar(10), Busers.BirthDate, 120) As BronzeBirthDate, Busers.BicycleType BronzeBicycleType, Busers.CityName BronzeCity, Busers.Groupdes BronzeGroup, Busers.OrganizationDes BronzeOrganization
                                        FROM [Competition], Users Pusers, Users Gusers, Users Susers, Users Busers, Groups g, Organizations o, Organizations og, Cities OC, Cities CG
                                        Where [CompetitionDate] like '%" +data1+@"' 
                                        AND o.OrganizationDes = OrgWin
                                        And O.City = oc.city
                                        AND g.GroupDes = GrpWin
                                        And g.Organization = og.Organization
                                        And og.City = Cg.City
                                        And Pusers.UserDes = PlatinumUser
                                        And Gusers.UserDes = [GoldUser]
                                        And Susers.UserDes = [SilverUser]
                                        And Busers.UserDes = [BronzeUser] ";
                               // ReadFromDataBaseUserName
                    break;

                // Added for fetch user's ASP FW username by its registered mail address
                case 27:
                    selectStr = @"  SELECT Users.Id, dbo.Users.UserDes, dbo.Users.UserFname 
                                    FROM [Users], [AspNetUsers]
                                    WHERE (dbo.AspNetUsers.Id = dbo.Users.Id ) and (dbo.Users.UserEmail = '" + data1 + "')";
                    break;
                
                //  Retrieves all registered email addresses in the BC DB (mail dist. system)
                case 28:
                    selectStr = @"  SELECT Users.UserEmail, dbo.Users.UserFname
                                    FROM Users
                                    WHERE UserEmail != ''";
                    break;
                
                /**/

            }
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter  
            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);
            DataTable dt = ds.Tables[0]; // Fill the datatable (in the dataset), using the Select command

            /******************************* Select Querry of all of the data in the DB ********************/
            if (select == 1)
            {
                
                selectStr3 = BuildGroupPointsCommand(data1, data2);

                SqlDataAdapter dj = new SqlDataAdapter(selectStr3, con); // create the data adapter  
                DataSet dx = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
                dj.Fill(dx);
                DataTable dt1 = dx.Tables[0];
                dt.Merge(dt1);
            }
            else if (select == 18)
            {
                SqlDataAdapter db = new SqlDataAdapter(selectStr1, con);
                SqlDataAdapter dc = new SqlDataAdapter(selectStr2, con);
                SqlDataAdapter dd = new SqlDataAdapter(selectStr3, con);

                db.Fill(ds.Tables[0]);
                dc.Fill(ds.Tables[0]);
                dd.Fill(ds.Tables[0]);
                dt = ds.Tables[0];

                DataRow newRow = dt.NewRow();
                newRow["NumOfUsers"] = dt.Rows[0].ItemArray[0];
                newRow["NumOfGroups"] = dt.Rows[1].ItemArray[1];
                newRow["NumOforganizations"] = dt.Rows[2].ItemArray[2];
                newRow["NumOfRides"] = dt.Rows[3].ItemArray[3];
                newRow["TotalKM"] = dt.Rows[3].ItemArray[4];
                newRow["TotalCO2"] = dt.Rows[3].ItemArray[5];
                newRow["TotalCalories"] = dt.Rows[3].ItemArray[6];

                foreach (DataRow dr in dt.Rows)
                {
                    dr.Delete();
                }
                dt.Rows.Add(newRow);
                dt.AcceptChanges();
                dbS.db = db;
                dbS.dc = dc;
                dbS.dd = dd;
            }
            /**************END OF******* Select Querry of all of the data in the DB  *******END OF**************/

            /******************************* Handle the Rank of the User / Group / Organization ********************/

            else if (select == 19)
            {
                data4 = data2;
               
                data2 = dt.Rows[0].ItemArray[0].ToString();
                data3 = dt.Rows[0].ItemArray[1].ToString();
                
                selectStr1 = BuildUserrankCommand(data1, data4);
                selectStr2 = BuildGrouprankCommand(data2, data3, data4);
                selectStr3 = BuildOrganizationrankCommand(data3, data4);
               

                SqlDataAdapter dj = new SqlDataAdapter(selectStr1, con); // create the data adapter 
                SqlDataAdapter db = new SqlDataAdapter(selectStr2, con);
                SqlDataAdapter dc = new SqlDataAdapter(selectStr3, con);

                DataSet dx = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
                DataSet dsb = new DataSet();
                DataSet dsc = new DataSet();

                dj.Fill(dx);
                db.Fill(dsb);
                dc.Fill(dsc);
               
                DataTable dt1 = dx.Tables[0];

                selectStr4 = CountNumUsersCommand(data4);
                selectStr5 = CountNumGroupCommand(data4);
                selectStr6 = CountNumOrgCommand(data4);

                SqlDataAdapter dba = new SqlDataAdapter(selectStr4, con);
                SqlDataAdapter dxa = new SqlDataAdapter(selectStr5, con);
                SqlDataAdapter dca = new SqlDataAdapter(selectStr6, con);

                DataSet dsba = new DataSet();
                DataSet dsca = new DataSet();
                DataSet dsfa = new DataSet();

                dba.Fill(dsba);
                dxa.Fill(dsca);
                dca.Fill(dsfa);

                string userpoint = (dt1.Rows.Count != 0 ? dt1.Rows[0].ItemArray[0].ToString() : "0");
                int UserRanking = (Convert.ToDouble(userpoint) > 0 ? dt1.Rows.Count : dsba.Tables[0].Rows.Count + 1);

                string groupoints = (dsb.Tables[0].Rows.Count != 0 ? dsb.Tables[0].Rows[0].ItemArray[0].ToString() : "0");
                int GroupRanking = (Convert.ToDouble(groupoints) > 0 ? dsb.Tables[0].Rows.Count : dsca.Tables[0].Rows.Count + 1);

                string orgpoints = (dsc.Tables[0].Rows.Count != 0 ? dsc.Tables[0].Rows[0].ItemArray[0].ToString() : "0");
                int OrgRanking = (Convert.ToDouble(orgpoints) > 0 ? dsc.Tables[0].Rows.Count : dsfa.Tables[0].Rows.Count + 1);
                


                DataRow newRow = dt1.NewRow();
                newRow["UserRanking"] = UserRanking;
                newRow["UserPoints"] = userpoint;
                newRow["GroupRanking"] = GroupRanking;
                newRow["GroupPoints"] = groupoints;
                newRow["OrganizationRanking"] = OrgRanking;
                newRow["OrganizationPoints"] = orgpoints;
                foreach (DataRow dr in dt1.Rows)
                {
                    dr.Delete();
                }
                dt1.AcceptChanges();
                dt1.Rows.Add(newRow);
                dbS.db = db;
                dbS.dc = dc;
                dbS.dt1 = dt1;
                
            }
            //*****END OF******* Handle the Rank of the User / Group / Organization ****END OF********/

            //********************************** Shuffle the winner **********************************/
            else if (select == 24)
            {
                int Upper = dt.Rows.Count;
                int Lower = 0;
                Random random = new Random();

                int randomNumber = random.Next(Lower, Upper);

                foreach (DataRow dr in dt.Rows)
                {
                    if (dt.Rows.IndexOf(dr) != randomNumber)//|| (string)dr.ItemArray[0] != "No one")
                        dr.Delete();
                }
                dt.AcceptChanges();
                // 0 - 500
                selectStr = Shuffle("150", data2, "SilverWinner");
                da = new SqlDataAdapter(selectStr, con); // create the data adapter  
                DataSet db = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
                da.Fill(db);
                Upper = db.Tables[0].Rows.Count;
                randomNumber = random.Next(Lower, Upper);

                foreach (DataRow dr in db.Tables[0].Rows)
                {
                    if (db.Tables[0].Rows.IndexOf(dr) != randomNumber)// || (string)dr.ItemArray[0] != "No one")
                        dr.Delete();
                }
                db.Tables[0].AcceptChanges();
                dt.ImportRow(db.Tables[0].Rows[0]);
                // 0 - 150
                selectStr = Shuffle("500", data2, "GoldWinner");
                da = new SqlDataAdapter(selectStr, con); // create the data adapter  
                DataSet dc = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
                da.Fill(dc);
                Upper = dc.Tables[0].Rows.Count;
                randomNumber = random.Next(Lower, Upper);

                foreach (DataRow dr in dc.Tables[0].Rows)
                {
                    if (dc.Tables[0].Rows.IndexOf(dr) != randomNumber)// || (string)dr.ItemArray[0] != "No one")
                        dr.Delete();
                }
                dc.Tables[0].AcceptChanges();
                dt.ImportRow(dc.Tables[0].Rows[0]);
                // 0 - 50
                selectStr = Shuffle("1500", data2, "PlatinaWinner");
                da = new SqlDataAdapter(selectStr, con); // create the data adapter  
                DataSet dd = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
                da.Fill(dd);
                Upper = dd.Tables[0].Rows.Count;
                randomNumber = random.Next(Lower, Upper);

                foreach (DataRow dr in dd.Tables[0].Rows)
                {
                    if (dd.Tables[0].Rows.IndexOf(dr) != randomNumber)// || (string)dr.ItemArray[0] != "No one")
                        dr.Delete();
                }
                dd.Tables[0].AcceptChanges();
                dt.ImportRow(dd.Tables[0].Rows[0]);
                // Organization And Group Shuffle
                selectStr = ShuffleOrg(data2);
                da = new SqlDataAdapter(selectStr, con); // create the data adapter  
                DataSet dor = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
                da.Fill(dor);
                dt.ImportRow(dor.Tables[0].Rows[0]);
                selectStr = ShuffleGrp(data2);
                da = new SqlDataAdapter(selectStr, con); // create the data adapter  
                DataSet dgr = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
                da.Fill(dgr);
                dt.ImportRow(dgr.Tables[0].Rows[0]);

            }
            /**************END OF******* Handle the Rank of the User / Group / Organization *******END OF**************/
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
    public int InsertDatabase(List<Object> s, string data1 = "", string data2 = "", string data3 = "", string data4 = "")
    {
        string _class = "";
        string typeOfObject = s[0].GetType().ToString();

        if (typeOfObject.Contains("Routes")) _class = "Routes";
        else if (typeOfObject.Contains("Organization")) _class = "Organization";
        else if (typeOfObject.Contains("Group")) _class = "Group";
        else if (typeOfObject.Contains("Rides")) _class = "Rides";
        else if (typeOfObject.Contains("Event")) _class = "Event";
        else if (typeOfObject.Contains("Competition")) _class = "Competition";

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
            case "Event":
                foreach (Event obj in s)
                    cStr = BuildInsertEventsCommand(obj);      // helper method to build the insert string
                break;
            case "Competition":
                foreach (Competition obj in s)
                    cStr = BuildInsertCompetitionCommand(obj);      // helper method to build the insert string
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
            case "UserEvent":
                cStr = BuildDelteUserEventCommand(data1, data2);      // helper method to build the insert string
                break;
            case "Event":
                cStr = BuildDelteEventCommand(data1);      // helper method to build the insert string
                break;
            case "Groups":
                cStr = BuildDelteGroupCommand(data1,data2);      // helper method to build the insert string
                break;
            case "Organizations":
                cStr = BuildDelteOrganizationCommand(data1);      // helper method to build the insert string
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

    #region Group/ Organization Rank AND Shuffle Method
     private String CountNumUsersCommand(string data4)
     {
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String command = @"SELECT  Count(*)
                            FROM [Rides] R, Users U, Groups G, Organizations O
                            Where U.[User]<> 0
                            AND U.Organization = O.Organization
                            AND O.Organization = G.Organization
                            AND R.[USER] = U.[User]
                            AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group])
                            AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy,'" + data4 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data4 + @"')
                            Group by  U.[User],DATEPART(yyyy, R.RideDate) , DATEPART(mm, R.RideDate)";
        /**/
        return command;
     }
     
         private String CountNumGroupCommand(string data4)
     {
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String command = @"SELECT  Count(*)
                        FROM Groups G, Organizations O, Users U, Rides R
                        Where G.[Group] <> 0
                        AND G.Organization = O.Organization
                        AND R.[User] = U.[User] 
                        AND U.[User] in ( SELECT UG.[User]
                                    FROM UsersGroups UG
                                    WHERE G.[Group] = UG.[Group])
                        AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data4 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data4 + @"')
                        group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), G.GroupName";
        /**/
        return command;
     }
      
          private String CountNumOrgCommand(string data4)
     {
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String command = @"SELECT Count(*)
                            FROM Groups G, Organizations O, Users U, Rides R
                            Where G.[Group] <> 0
                            AND G.Organization = O.Organization
                            AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group])
                            AND R.[User] = U.[User] 
                            AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data4 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data4 + @"')
                            group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), O.OrganizationName";
        /**/
        return command;
     }
    private String BuildGroupPointsCommand(string data1, string data2)
    {

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String command = @"SELECT anu.UserName, U.UserEmail, U.UserDes, U.UserFname, U.UserLname, U.ImagePath, U.Gender, U.Captain, U.UserAddress, U.UserPhone, convert(varchar(10), U.BirthDate, 120) As BirthDate, U.BicycleType, C.CityName As RiderCity, G.GroupName, G.GroupDes, O.OrganizationName, O.OrganizationDes, O.OrganiztionImage, U.UserFname +' '+ U.UserLname As UserDisplayName, 0.0 AS User_Points
                        FROM UsersGroups UG, Users U, AspNetUsers anu, Groups G, Organizations O, Cities C
                        Where U.[User] <> 0
                        AND U.Id = anu.Id  
                        AND G.[Organization] = O.[Organization]
                        AND UG.[User] = U.[User]
                        AND UG.[Group] = G.[Group]
                        AND U.City = C.City
                        AND G.GroupDes = '" + data1 + @"'
                        AND O.OrganizationDes = '" + data2 + @"'
                        AND U.[User] not in (
					                        SELECT U.[user]
					                        FROM UsersGroups UG, Users U, AspNetUsers anu, Groups G, Organizations O, [Rides] R
					                        Where U.[User] <> 0
					                        AND U.Id = anu.Id  
					                        AND G.[Organization] = O.[Organization]
					                        AND UG.[User] = U.[User]
					                        AND UG.[Group] = G.[Group]
					                        AND G.GroupDes = '" + data1 + @"'
					                        AND O.OrganizationDes = '" + data2 + @"'
					                        AND R.[USER] = U.[User]
					                        AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, GETDATE()) AND DATEPART(mm, R.RideDate) like DATEPART(mm, GETDATE())
					                        group by U.[user]
					                        );";
        /**/
        return command;
    }
    private String BuildUserrankCommand(string data1, string data4)
    {

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String command = @"Select  Sum( R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS UserRanking,'x' AS UserPoints, 'x' AS GroupRanking, 'x' AS GroupPoints,  'x' AS OrganizationRanking, 'x' AS OrganizationPoints
                        From Rides R
                        Where  DATEPART(yyyy, R.RideDate) like DATEPART(yyyy,  '" + data4 + @"')
                        AND DATEPART(mm, R.RideDate) like DATEPART(mm,  '" + data4 + @"')
                        group by DATEPART(yyyy, R.RideDate),DATEPART(mm, R.RideDate),[User]
                        having Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) >=
                        (  
                        SELECT Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate)
                        FROM Users U, Rides R
                        Where U.[User] in ( SELECT UG.[User]
                                    FROM UsersGroups UG, Users U2
                                    WHERE U2.[User] = UG.[User]
                                    AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy,  '" + data4 + @"')
                                    AND DATEPART(mm, R.RideDate) like DATEPART(mm,  '" + data4 + @"')
                                    AND U2.UserDes = '" + data1 + @"'
                                    )
                        AND R.[User] = U.[User] 
                        )
                        order by Sum( R.[RideLength]) + 20 * COUNT(distinct R.RideDate) ASC";
        /**/
        return command;
    }
    /**/
    private String BuildGrouprankCommand(string data2, string data3, string data4)
    {

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String command = @"SELECT Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS GroupRanking, G.GroupDes
                                FROM Groups G, Users U, Rides R
                                Where G.[Group] <> 0
                                AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data4 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data4 + @"')
                                AND R.[User] = U.[User] 
                                AND U.[User] in ( SELECT UG.[User]
                                            FROM UsersGroups UG
                                            WHERE G.[Group] = UG.[Group])
                                group by G.GroupDes
                                having Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) >=
			                                (
			                                SELECT Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate)
			                                FROM Groups G, Organizations O, Users U, Rides R
			                                Where G.[Group] <> 0
			                                AND G.Organization = O.Organization
			                                AND O.OrganizationDes like '" + data3 + @"%'
			                                AND G.GroupDes like '" + data2 + @"%'
			                                AND U.[User] in ( SELECT UG.[User]
						                                FROM UsersGroups UG
						                                WHERE G.[Group] = UG.[Group])
			                                AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data4 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data4 + @"')
			                                AND R.[User] = U.[User] 
			                                group by G.GroupName )
		  
                                order by Sum( R.[RideLength]) + 20 * COUNT(distinct R.RideDate) ASC;";
        return command;
    }

    private String BuildOrganizationrankCommand(string data3, string data4)
    {

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String command = @"SELECT Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS OrganizationRanking, O.OrganizationDes,  'x' AS Organization_Points
                                FROM Groups G, Organizations O, Users U, Rides R
                                Where G.[Group] <> 0
                                AND G.Organization = O.Organization
                                AND U.[User] in ( SELECT UG.[User]
                                            FROM UsersGroups UG
                                            WHERE G.[Group] = UG.[Group])
                                AND R.[User] = U.[User] 
                                AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data4 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data4 + @"')
                                group by O.OrganizationDes
                                having Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) >=
			                                (
			                                SELECT Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate)
			                                FROM Groups G, Organizations O, Users U, Rides R
			                                Where G.[Group] <> 0
			                                AND G.Organization = O.Organization
			                                AND O.OrganizationDes like '" + data3 + @"%'
			                                AND U.[User] in ( SELECT UG.[User]
						                                FROM UsersGroups UG
						                                WHERE G.[Group] = UG.[Group])
			                                AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data4 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data4 + @"')
			                                AND R.[User] = U.[User] 
			                                group by O.OrganizationName )
                                order by Organization_Points ASC ;";

        return command;
    }
    private String Shuffle(string data1, string data2, string data3)
    {
        // Fill the datatable (in the dataset), using the Select command 

        String selectstr = @"IF EXISTS (SELECT '" + data3 + @"' AS Category, [UserEmail],[UserDes],[UserFname],[UserLname],[UserAddress],[UserPhone],convert(varchar(10), [BirthDate], 120) As BirthDate,[BicycleType],[ImagePath],[Gender], g.GroupDes,o.OrganizationDes,O.OrganiztionImage, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS RiderPoints
                            FROM [Rides] R, Users U, UsersGroups ug, Groups g, Organizations o
                            Where U.[User]<> 0
                            AND R.[USER] = U.[User]
                            AND  U.[User]=ug.[User]
                            AND ug.[Group]=g.[Group]
                            AND g.Organization=o.Organization
                            AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy,'" + data2 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm,'" + data2 + @"')
                            Group by [UserEmail],[UserDes],[UserFname],[UserLname],[UserAddress],[UserPhone],[BirthDate],[BicycleType],[ImagePath],[Gender], g.GroupDes,o.OrganizationDes,O.OrganiztionImage
                            having Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) > " + data1 + @") 
                            BEGIN
                                SELECT '" + data3 + @"' AS Category, [UserEmail],[UserDes],[UserFname],[UserLname],[UserAddress],[UserPhone],[BirthDate],[BicycleType],[ImagePath],[Gender], g.GroupDes,o.OrganizationDes,O.OrganiztionImage, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS RiderPoints
                                FROM [Rides] R, Users U, UsersGroups ug, Groups g, Organizations o
                                Where U.[User]<> 0
                                AND R.[USER] = U.[User]
                                AND  U.[User]=ug.[User]
                                AND ug.[Group]=g.[Group]
                                AND g.Organization=o.Organization
                                AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data2 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm,'" + data2 + @"')
                                Group by [UserEmail],[UserDes],[UserFname],[UserLname],[UserAddress],[UserPhone],[BirthDate],[BicycleType],[ImagePath],[Gender], g.GroupDes,o.OrganizationDes,O.OrganiztionImage
                                having Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) >  " + data1 + @"
                            END
                            ELSE
                            BEGIN
                                SELECT '" + data3 + @"' AS Category, 'No Rider At This Category' AS UserEmail,'No Rider At This Category' AS UserDes,'No Rider At This Category' AS UserFname,'No Rider At This Category' AS UserLname,'No Rider At This Category' AS UserAddress,'No Rider At This Category' AS UserPhone,'" + data2 + @"' AS BirthDate,'No Rider At This Category' AS BicycleType,'No Rider At This Category' AS ImagePath,'No Rider At This Category' AS Gender, 'No Rider At This Category' AS GroupDes,'No Rider At This Category' AS OrganizationDes,'No Rider At This Category' AS OrganiztionImage, 0 AS RiderPoints
                                From Users Where [User]=0;
                            END ";

        return selectstr;
    }
    private String ShuffleOrg(string data1)
    {
        // Fill the datatable (in the dataset), using the Select command 

        String selectstr = @"SELECT TOP 1 'Winning Organization' AS Category, 'Winning Organization' AS UserEmail, 'Winning Organization' AS UserDes,'Winning Organization' AS UserFname,'Winning Organization' AS UserLname,'Winning Organization' AS UserAddress,'Winning Organization' AS UserPhone,'" + data1 + @"' AS BirthDate,'Winning Organization' AS BicycleType,'Winning Organization' AS ImagePath,'Winning Organization' AS Gender, 'Winning Organization' AS GroupDes,O.OrganizationDes AS OrganizationDes, O.OrganizationName AS OrganiztionImage, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS RiderPoints
                            FROM Groups G, Organizations O, Users U, Rides R
                            Where G.[Group] <> 0
                            AND G.Organization = O.Organization
                            AND U.[User] in ( SELECT UG.[User]
                                        FROM UsersGroups UG
                                        WHERE G.[Group] = UG.[Group])
                            AND R.[User] = U.[User] 
                            AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data1 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data1 + @"')
                            group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), O.OrganizationName, O.OrganizationDes
                            order by DATEPART(yyyy, R.RideDate)DESC, DATEPART(mm, R.RideDate)DESC,  Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) DESC ;";

        return selectstr;
    }
    private String ShuffleGrp(string data1)
    {
        // Fill the datatable (in the dataset), using the Select command 

        String selectstr = @"SELECT TOP 1 'Winning Group' AS Category, 'Winning Group' AS UserEmail, 'Winning Group' AS UserDes,'Winning Group' AS UserFname,'Winning Group' AS UserLname,'Winning Group' AS UserAddress,'Winning Group' AS UserPhone,'" + data1 + @"' AS BirthDate,'Winning Group' AS BicycleType,'Winning Group' AS ImagePath,'Winning Group' AS Gender, G.GroupName AS GroupDes,G.GroupDes AS OrganizationDes, O.OrganizationName AS OrganiztionImage, Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) AS RiderPoints
            FROM Groups G, Organizations O, Users U, Rides R
            Where G.[Group] <> 0
            AND G.Organization = O.Organization
            AND U.[User] in ( SELECT UG.[User]
                        FROM UsersGroups UG
                        WHERE G.[Group] = UG.[Group])
            AND DATEPART(yyyy, R.RideDate) like DATEPART(yyyy, '" + data1 + @"') AND DATEPART(mm, R.RideDate) like DATEPART(mm, '" + data1 + @"')
            AND R.[User] = U.[User] 
            group by DATEPART(yyyy, R.RideDate), DATEPART(mm, R.RideDate), G.GroupName, G.GroupDes, O.OrganizationName
            order by DATEPART(yyyy, R.RideDate)DESC, DATEPART(mm, R.RideDate)DESC,  Sum(R.[RideLength]) + 20 * COUNT(distinct R.RideDate) DESC ;";

        return selectstr;
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
    public int updateRiderInDatabase(Rider rdr, string username, string new_cap_user = "")
    {
        SqlConnection con;
        SqlCommand cmd, cmd1;

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
        String cStr = (new_cap_user == "" ? BuildUpdateRiderCommand(rdr, username) : BuildUpdateCaptainCommand(username, new_cap_user));      // helper method to build the insert string
        cmd = CreateCommand(cStr, con);             // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            if (numEffected == 2 && new_cap_user == "")
            {
                String cStr1 = @"Update [Users] Set [Captain] = 1 
                                Where UserDes = '" + username + @"'
                                AND not exists (SELECT 'x'
                                                FROM Groups G, Organizations O, Users U
                                                Where G.[Group] <> 0
                                                AND G.Groupname = '" + rdr.Group + @"'
                                                AND G.Organization = O.Organization
                                                AND O.OrganizationName = '" + rdr.Organization + @"'
                                                AND U.Captain = 1
                                                AND U.[User] in ( SELECT UG.[User]
								                                FROM UsersGroups UG
								                                WHERE G.[Group] = UG.[Group]) )";
                cmd1 = CreateCommand(cStr1, con);
                int numEffected1 = cmd1.ExecuteNonQuery();
            }

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
            if (numEffected_2 == 0)
                lf.Main("UsersGroups", "No record was inserted to the table check if the group " + rdr.Group + " or the username " + rdr.Username + " exists ");
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
        sb.AppendFormat("Values('{0}', (select [city] from Cities where CityName = '{1}' ), '{2}', '{3}' ,'{4}', '{5}','{6}','{7}', '{8}','{9}', '{10}', '{11}', (select id from AspNetUsers where UserName = '{12}'), {13}, (select Organization from Organizations where OrganizationDes = '{14}'))", rdr.RiderEmail, rdr.City, rdr.RiderDes, rdr.RiderFname, rdr.RiderLname, rdr.RiderAddress, rdr.RiderPhone, rdr.Gender, rdr.BicycleType, rdr.ImagePath, rdr.BirthDate, DateTime.Now.Date.ToString("yyyy-MM-dd"), rdr.Username, rdr.Captain, rdr.Organization);
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
                          Set @Group_val = ( Select [Group] From Groups Where GroupDes = '" + rdr.Group + @"');
                          Set @User_val = ( Select [User] From Users Where UserDes = '" + rdr.Username + @"' ) ;";
        String prefix = @"  if ( @Group_val <> 0 AND @User_val <> 0 )
                            begin
                            INSERT INTO [UsersGroups]([Group],[User]) Values( @Group_val, @User_val )
                            UPDATE [Users]
                            SET 
                                [GroupDes] = (Select g.GroupDes From Groups g, UsersGroups ug Where ug.[User] = Users.[User] And ug.[Group] = g.[group] )
                                ,[OrganizationDes] = (Select OrganizationDes From Organizations Where Organization = Users.Organization)
                                ,[CityName] = (Select CityName From Cities Where City = Users.City)
                            Where [Users].[User] = @User_val 
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
        String sb;
        String sb2;

        String prefix = "UPDATE [Users] ";
        sb = @"SET [UserEmail] = " + (rdr.RiderEmail != null ? "'" + rdr.RiderEmail + "'" : "[UserEmail]") + @"
                              ,[City] = " + (rdr.City != null ? " (select city from Cities where CityName = '" + rdr.City + "' )" : "[City]") + @"
                              ,[UserFname] = " + (rdr.RiderFname != null ? "'" + rdr.RiderFname + "'" : "[UserFname]") + @"
                              ,[UserLname] = " + (rdr.RiderLname != null ? "'" + rdr.RiderLname + "'" : "[UserLname]") + @"
                              ,[UserAddress] = " + (rdr.RiderAddress != null ? "'" + rdr.RiderAddress + "'" : "[UserAddress]") + @"
                              ,[UserPhone] = " + (rdr.RiderPhone != null ? "'" + rdr.RiderPhone + "'" : "[UserPhone]") + @"
                              ,[BicycleType] = " + (rdr.BicycleType != null ? "'" + rdr.BicycleType + "'" : "[BicycleType]") + @"
                              ,[ImagePath] = " + (rdr.ImagePath != null ? "'" + rdr.ImagePath + "'" : "[ImagePath]") + @"
                              ,[BirthDate] = " + (rdr.BirthDate != null ? "'" + rdr.BirthDate + "'" : "[BirthDate]") + @"
                              ,[Gender] = " + (rdr.Gender != null ? "'" + rdr.Gender + "'" : "[Gender]") + @"
                              ,[Captain] = 0 
                              ,[Organization] = " + (rdr.Organization != null && rdr.Group != null ? "(select [Organization] from Organizations where OrganizationDES = '" + rdr.Organization + "' AND Exists ( Select G.Organization From Groups G Where G.[GroupName] = '" + rdr.Group + @"' AND G.Organization = Organization  ))" : "[Organization]") + @"
                             WHERE UserDes = '" + username + @"';";
        sb2 = @" Update [UsersGroups] Set [Group] = " + (rdr.Group != null ? "( Select [Group] From Groups Where GroupName = '" + rdr.Group + "' )" : "[Group]") + @"  
                        Where [User] = ( Select [User] From [Users] Where UserDes = '" + username + @"');
                        UPDATE [Users]
                            SET 
                                [GroupDes] = (Select g.GroupDes From Groups g, UsersGroups ug Where ug.[User] = Users.[User] And ug.[Group] = g.[group] )
                                ,[OrganizationDes] = (Select OrganizationDes From Organizations Where Organization = Users.Organization)
                                ,[CityName] = (Select CityName From Cities Where City = Users.City)
                            Where UserDes = '" + username + @"' ";
        command = prefix + sb + sb2;
        return command;
    }

    private String BuildUpdateCaptainCommand(string username, string new_cap_usr)
    {
        String command;
        String sb;


        String prefix = @"UPDATE [Users]
            SET [Captain] = 1
            WHERE UserDes = '" + new_cap_usr + @"'
            AND exists 
             ( Select 'x' From Users U, Users U2, UsersGroups UG, UsersGroups UG2 
	        where U.UserDes = '" + username + @"' And U.[User]=UG.[User] 
	        AND U2.[UserDes] = '" + new_cap_usr + @"' AND U2.[User] = UG2.[User] And UG2.[Group]=UG.[Group] AND U.Captain = 1 )";
        sb = @"UPDATE [Users]
            SET [Captain] = 0
            WHERE UserDes = '" + username + @"'
            AND exists 
             ( Select 'x' From Users U, Users U2, UsersGroups UG, UsersGroups UG2 
	        where U.UserDes = '" + new_cap_usr + @"' And U.[User]=UG.[User] 
	        AND U2.[UserDes] = '" + username + @"' AND U2.[User] = UG2.[User] And UG2.[Group]=UG.[Group] AND U.Captain = 1 ) ";
        command = prefix + sb;
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
        sb.AppendFormat("Values('{0}', (select Organization from Organizations where OrganizationDes = '{1}' ) ,'{2}') ", grp.GroupName, grp.OrganizationName, grp.GroupDes);
        String command2 = @" INSERT INTO dbo.UsersGroups([Group]
                            ,[User])
                               VALUES
                        ( (Select [Group] From [Groups] Where GroupName = '" + grp.GroupName + @"' )   
                       ,0) ";
        command = prefix + sb.ToString();
        return command + command2;
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
        sb.AppendFormat("Values('{0}', (select U.[User] from Users U, AspNetUsers A where A.UserName = '{1}' AND A.Id = U.Id ), '{2}', '{3}' ,'{4}', {5},'{6}','{7}')", rds.RideName, rds.UserName, rds.RideDes, rds.RideType, rds.RideDate, rds.RideLength, rds.RideSource, rds.RideDestination);
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

    #region Events

    // Update An Existing Rider
    public int updateEventInDatabase(Event evt, string eventname)
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
            lf.Main("Events", ex.Message);
            return 0;
        }
        String cStr = BuildUpdateEventCommand(evt, eventname);      // helper method to build the insert string
        cmd = CreateCommand(cStr, con);             // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            lf.Main("Events", ex.Message);
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

    private String BuildUpdateEventCommand(Event evt, string eventname)
    {

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String command = @"UPDATE [Events]
                           SET [City] = " + (evt.City != "" ? " (select city from Cities where CityName = '" + evt.City + "' )" : "[City]") + @"
                              ,[EventDes] = " + (evt.EventDes != "" ? "'" + evt.EventDes + "'" : "[EventDes]") + @"
                              ,[EventType] = " + (evt.EventType != "" ? "'" + evt.EventType + "'" : "[EventType]") + @"
                              ,[EventStatus] = " + (evt.EventStatus != "" ? "'" + evt.EventStatus + "'" : "[EventStatus]") + @"
                              ,[EventDate] = " + (evt.EventDate != "" ? "'" + evt.EventDate + "'" : "[EventDate]") + @"
                              ,[EventTime] = " + (evt.EventTime != "" ? "'" + evt.EventTime + "'" : "[EventTime]") + @"
                              ,[EventAddress] = " + (evt.EventAddress != "" ? "'" + evt.EventAddress + "'" : "[EventAddress]") + @"
                              ,[EventDetails] = " + (evt.EventDetails != "" ? "'" + evt.EventDetails + "'" : "[EventDetails]") + @"
                         WHERE [EventName] = '" + eventname + "' ";
        return command;
    }


    
    private String BuildInsertEventsCommand(Event evt)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = @"INSERT INTO [Events]([EventName]
           ,[City]
           ,[EventDes]
           ,[EventType]
           ,[EventStatus]
           ,[EventDate]
           ,[EventTime]
           ,[EventAddress]
           ,[EventDetails])";
        sb.AppendFormat("Values('{0}', (select City from Cities Where CityName = '{1}'),'{2}','{3}','{4}','{5}','{6}','{7}','{8}')", evt.EventName, evt.City, evt.EventDes, evt.EventType, evt.EventStatus, evt.EventDate, evt.EventTime, evt.EventAddress, evt.EventDetails);
        String command2 = @"INSERT INTO USERSEVENTS([Event]
                            ,[User])
                               VALUES
                        ( (Select Event From [Events] Where EventName = '" + evt.EventName + @"' )   
                       ,0) ";
        command = prefix + sb.ToString() + command2;
        return command;
    }


    public int updateRiderIneventDatabase(string eventname, string username)
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
            lf.Main("UsersEvents", ex.Message);
            return 0;
        }
        String cStr = BuildInsertRiderEventCommand(eventname, username);      // helper method to build the insert string
        cmd = CreateCommand(cStr, con);             // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            lf.Main("UsersEvents", ex.Message);
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
    private String BuildInsertRiderEventCommand(string eventname, string username)
    {
        String command = @"INSERT INTO USERSEVENTS([Event]
                            ,[User])
                               VALUES
                        ( (Select Event From [Events] Where EventName = '" + eventname + @"' )   
                       ,(Select [User] From [Users] Where UserDes = '" + username + @"') )";

        return command;
    }
    private String BuildDelteUserEventCommand(string username, string eventname)
    {
        StringBuilder sb = new StringBuilder();
        String command = @"  DELETE FROM [UsersEvents] Where [USER] = ( Select [User] From Users Where UserDes = '" + username + @"' )
                                                       AND [Event] = (Select [Event] From [Events] Where EventName = '"+eventname+"' );";
        return command;
    }

    private String BuildDelteEventCommand(string eventname)
    {
        String command = @"  DELETE FROM [UsersEvents] Where [Event] = (Select [Event] from [Events] Where [EventName]  = '" + eventname + @"') ;";
        StringBuilder sb = new StringBuilder();
        String prefix = @"  DELETE FROM [Events] Where [EventName]  = '" + eventname + @"' ;";

        return command + prefix;
    }
    private String BuildDelteGroupCommand(string grpname, string orgname)
    {
        //String command = @"  DELETE FROM [UsersGroups] Where [Group] = ( Select [group] From Groups where GroupDes =  '" + grpname + @"') ;";

        String prefix = @"  DELETE FROM [Groups] Where [GroupDes] = '" + grpname + @"' AND Organization = ( Select Organization From Organizations where OrganizationDes =  '" + orgname + @"') ;";

        return prefix;
    }

    private String BuildDelteOrganizationCommand(string orgname)
    {

        String command = @" DELETE FROM Organizations where OrganizationDes = '" + orgname + @"' ;";

        return command;
    }
    #endregion

    #region Competition
    private String BuildInsertCompetitionCommand(Competition cmpt)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String prefix = @"INSERT INTO [Competition]
           ([CompetitionDate]
           ,[OrgWin]
           ,[GrpWin]
           ,[PlatinumUser]
           ,[GoldUser]
           ,[SilverUser]
           ,[BronzeUser])";
        sb.AppendFormat("Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", cmpt.CompetitionDate, cmpt.OrgWin, cmpt.GrpWin, cmpt.PlatinumUser, cmpt.GoldUser, cmpt.SilverUser, cmpt.BronzeUser);

        command = prefix + sb.ToString();
        return command;
    }
    public int updateCompetitionInDatabase(Competition cmpt, string CompetitionDate)
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
            lf.Main("Competition", ex.Message);
            return 0;
        }
        String cStr = BulidCompetitionInDatabase(cmpt, CompetitionDate);
        cmd = CreateCommand(cStr, con);             // create the command
       try
       {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command 

            return numEffected;
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
    private String BulidCompetitionInDatabase(Competition cmpt, string CompetitionDate)
    {

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        String command = @"UPDATE [igroup1_prod].[dbo].[Competition]
                           SET [OrgWin] = " + (cmpt.OrgWin != "" ? "'" + cmpt.OrgWin + "'" : "[OrgWin]") + @"
                              ,[GrpWin] = " + (cmpt.GrpWin != "" ? "'" + cmpt.GrpWin + "'" : "[GrpWin]") + @"
                              ,[PlatinumUser] = " + (cmpt.PlatinumUser != "" ? "'" + cmpt.PlatinumUser + "'" : "[PlatinumUser]") + @"
                              ,[GoldUser] = " + (cmpt.GoldUser != "" ? "'" + cmpt.GoldUser + "'" : "[GoldUser]") + @"
                              ,[SilverUser] = " + (cmpt.SilverUser != "" ? "'" + cmpt.SilverUser + "'" : "[SilverUser]") + @"
                              ,[BronzeUser] = " + (cmpt.BronzeUser != "" ? "'" + cmpt.BronzeUser + "'" : "[BronzeUser]") + @"
                         WHERE [CompetitionDate] = '" + CompetitionDate + "' ";

        return command;
    }

       #endregion
}
