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

        public Organization(string organizationName, string organizationCity, string organizationDes, string organizationEmail, string organizationAddress, string organizationPhone, string organizationType)
        {
            Organizationname = organizationName;
            OrganizationCity = organizationCity;
            OrganizationDes = organizationDes ;
            OrganizationEmail = organizationEmail ;
            OrganizationAddress = organizationAddress ;
            OrganizationPhone = organizationPhone;
            OrganizationType = organizationType ;
        }

        public DataTable readData()
        {
            string conString  = "DefaultConnection";
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(conString, "Organizations") ;    
            return dbs.dt; 
            
        }
        public void updateDatabase(Organization org)
        {

            DBservices dbs = new DBservices();
            dbs.insertOrganization(org);

        }
    }
}


 
        
        
       

       
        
       
        
       
        
        
