using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Data;



/// <summary>
/// Summary description for User
/// </summary>
public class User
{

    //user.Id, user.Name, user.Price, user.ImageUrl, user.Amount, user.Discount
    private int group;

    public int Group
    {
        get { return group; }
        set { group = value; }
    }
    private int organization;

    public int Organization
    {
        get { return organization; }
        set { organization = value; }
    }
    private string city;

    public string City
    {
        get { return city; }
        set { city = value; }
    }
    private int route;

    public int Route
    {
        get { return route; }
        set { route = value; }
    }
    private string username;

    public string UserName
    {
        get { return username; }
        set { username = value; }
    }
    private string userdes;

    public string UserDes
    {
        get { return userdes; }
        set { userdes = value; }
    }

    private string userfname;

    public string UserFname
    {
        get { return userfname; }
        set { userfname = value; }
    }

    private string userlname;

    public string UserLname
    {
        get { return userlname; }
        set { userlname = value; }
    }

    private string gender;

    public string Gender
    {
        get { return gender; }
        set { gender = value; }
    }

    private string useremail;

    public string UserEmail
    {
        get { return useremail; }
        set { useremail = value; }
    }
    private string useraddres;

    public string UserAddres
    {
        get { return useraddres; }
        set { useraddres = value; }
    }

    private string userphone;

    public string UserPhone
    {
        get { return userphone; }
        set { userphone = value; }
    }

    private string imagePath;

    public string ImagePath
    {
        get { return imagePath; }
        set { imagePath = value; }
    }
    private string bicycletype;

    public string BicycleType
    {
        get { return bicycletype; }
        set { bicycletype = value; }
    }


    public User()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    //concstructor gets 10 arrguments: Username, First name, Last name, Gender, Email, Address, City, Phone, Bicycle Type, ImagePath
    public User(string username, string userfname, string userlname, string gender, string useremail, string useraddres, string usercity, string userphone, string bicycletype, string imagePath)
    {
        UserName = username;
        UserDes = userfname + userlname;
        UserFname = userfname;
        UserLname = userlname;
        Gender = gender;
        UserEmail = useremail;
        UserAddres = useraddres;
        UserPhone = userphone;
        BicycleType = bicycletype;
        City = usercity;
        WebClient wb = new WebClient();
        string fileFullPath = HttpContext.Current.Server.MapPath("~/images/") + username + Path.GetExtension(imagePath);
        wb.DownloadFile(imagePath, fileFullPath);
        ImagePath = "images/" + username + Path.GetExtension(imagePath);
    }
    public int insert()
    {
        // Ensure Username is unique
        /*   User user = new User();
         
          DataTable dt = user.readUserDB();
          foreach (DataRow item in dt.Rows)
          {
              if (item.ItemArray[1].Equals(username))
              {

                  throw new System.ArgumentException("Username already exist");
              }
          } */
        DBservices dbs = new DBservices();
        int numAffected = dbs.insert(this);
        return numAffected;
    }
    public DataTable readUserDB()
    {
        if (HttpContext.Current.Session["showdDiscount"] != null)
        {
            if ((int)HttpContext.Current.Session["showdDiscount"] == 1)
            {
                HttpContext.Current.Session["showdDiscount"] = 0;
                HttpContext.Current.Session["discountDbs"] = HttpContext.Current.Session["productsDataSet"];
                return ((DBservices)HttpContext.Current.Session["productsDataSet"]).dt;
            }
        }
        DBservices dbs = new DBservices();
        dbs = dbs.ReadFromDataBase("bikechallangeDBConnectionString", "Products");
        // save the dataset in a session object
        HttpContext.Current.Session["productsDataSet"] = dbs;
        return dbs.dt;

    }



    public void showDiscount(double minInventory, double discount)
    {
        DBservices dbs = new DBservices();
        dbs = (DBservices)HttpContext.Current.Session["productsDataSet"];
        foreach (DataRow item in dbs.dt.Rows)
        {
            if (Convert.ToDouble(item[4]) > minInventory)
            {

                item[2] = Convert.ToDouble(item[2]) * (1 - discount / 100);
                item[5] = 1;

            }
        }
        HttpContext.Current.Session["productsDataSet"] = dbs;
        HttpContext.Current.Session["showdDiscount"] = 1;

    }

    public void updateDatabase()
    {
        if (HttpContext.Current.Session["discountDbs"] == null) return;

        DBservices dbs = (DBservices)HttpContext.Current.Session["discountDbs"];

        dbs.Update();
    }

    public DataTable readDBForShooping()
    {
        DBservices dbs = new DBservices();
        dbs = dbs.ReadFromDataBase("bikechallangeDBConnectionString", "Products");
        // save the dataset in a session object
        HttpContext.Current.Session["productsDataSet"] = dbs;
        if (HttpContext.Current.Session["tbarr"] == null)
        {
            string[] tbArr = new string[System.Int16.MaxValue];
            HttpContext.Current.Session["tbarr"] = tbArr;
        }
        return dbs.dt;

    }
    public DataTable readDBForCashier()
    {
        DBservices dbs = new DBservices();
        if (HttpContext.Current.Session["productsDataSet"] != null)
        {
            dbs = HttpContext.Current.Session["productsDataSet"] as DBservices;
            return dbs.dt;
        }
        else
        {
            return dbs.dt;
        }

    }

    public void ApprovePurchase()
    {
        DBservices dbs = HttpContext.Current.Session["productsDataSet"] as DBservices;
        string[] tbArr = HttpContext.Current.Session["tbarr"] as string[];

        foreach (DataRow item in dbs.dt.Rows)
        {
            item[4] = Convert.ToInt16(item[4].ToString()) - Convert.ToInt16(tbArr[Convert.ToInt16(item[0].ToString()) - 1]);
        }


        dbs.Update();

    }
}