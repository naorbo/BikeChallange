using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace BikeChallengeApp.Models
{
    public class Group
    {
      
        string groupName;
        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
                groupName = groupName.Replace("'", "''");
                groupDes = groupName;
                groupName = groupName.Replace("&", "'+CHAR(37)+'26");
                groupName = groupName.Replace(" ", "'+CHAR(37)+'20");
            }
        }

        string organizationName;
        public string OrganizationName
        {
            get { return organizationName; }
            set { organizationName = value; organizationName = organizationName.Replace("'", "''"); }
        }

        string groupDes;
        public string GroupDes
        {
            get { return groupDes; }
            set {  }
        }

        public Group()
        {

        }

        public Group(string groupName, string organizationName, string groupDes)
        {
            GroupName = (groupName != null ? groupName : "");
            OrganizationName = (organizationName != null ? organizationName : ""); 
            GroupDes = (groupDes != null ? groupDes : ""); 
        }
    }
}














