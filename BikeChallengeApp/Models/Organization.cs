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
            Organizationname = (Organizationname != null ? Organizationname : "");
            OrganizationCity = (OrganizationCity != null ? OrganizationCity : "");
            OrganizationDes = (OrganizationDes != null ? "" : "");
            OrganizationImage = (OrganizationImage != null ? OrganizationImage : "");
            OrganizationType = (OrganizationType != null ? OrganizationType : "");
        }
        public Organization(string organizationName, string organizationCity, string organizationDes, string organizationImage, string organizationType)
        {
            Organizationname = (organizationName != null ? organizationName : "" );
            OrganizationCity = (organizationCity != null ? organizationCity : "" );
            OrganizationDes = (organizationDes != null ? "" : "");
            OrganizationImage = (organizationName != null ? organizationName : "");
            OrganizationType = (organizationType != null ? organizationType : "");
        }
    }
}


 
        
        
       

       
        
       
        
       
        
        
