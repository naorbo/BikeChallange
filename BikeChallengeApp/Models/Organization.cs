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
            set 
            {
            organizationName = value;
            organizationDes = value;
            organizationName = organizationName.Replace("&", "'+CHAR(37)+'26");
            organizationName = organizationName.Replace(" ", "'+CHAR(37)+'20");
            }

        }
        //(value.Contains(" ") ? value.Replace(" ", "'+CHAR(37)+'20") : (value.Contains("&") ? value.Replace("&", "'+CHAR(37)+'26") : organizationName))
        string organizationDes;
        public string OrganizationDes
        {
            get { return organizationDes; }
            set { organizationDes = organizationDes; }
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

        }
        public Organization(string organizationName, string organizationCity, string organizationDes, string organizationImage, string organizationType)
        {
            Organizationname = (organizationName.Contains(" ") ? organizationName.Replace("&", "%20") : (organizationName.Contains("&") ? organizationName.Replace("&", "%26") : organizationName));
            OrganizationCity = (organizationCity != null ? organizationCity : "" );
            OrganizationDes = (organizationDes != null ? "" : "");
            OrganizationImage = (organizationName != null ? organizationName : "");
            OrganizationType = (organizationType != null ? organizationType : "");
        }
    }
}


 
        
        
       

       
        
       
        
       
        
        
