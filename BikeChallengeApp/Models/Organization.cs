using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace BikeChallengeApp.Models
{
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

        string organizationImage;
        public string OrganizationImage
        {
            get { return organizationImage; }
            set { organizationImage = value; }
        }

        string organizationType;
        public string OrganizationType
        {
            get { return organizationType; }
            set { organizationType = value; }
        }

        string organizationCity;
        public string OrganizationCity
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

        public Organization(string organizationName, string organizationCity, string organizationDes, string organizationImage, string organizationType)
        {
            Organizationname = organizationName;
            OrganizationCity = organizationCity;
            OrganizationDes = organizationDes ;
            OrganizationImage = organizationImage;
            OrganizationType = organizationType ;
        }

        public DataTable readData()
        {
            string conString  = "DefaultConnection";
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(conString, "Organizations", "Organization") ;    
            return dbs.dt; 
            
        }
        public void updateDatabase(Organization org)
        {

            DBservices dbs = new DBservices();
            dbs.insertOrganization(org);
        }
    }
}


 
        
        
       

       
        
       
        
       
        
        
