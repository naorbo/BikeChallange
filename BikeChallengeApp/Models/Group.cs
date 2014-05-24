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
            set { groupName = value; }
        }

        string organizationName;
        public string OrganizationName
        {
            get { return organizationName; }
            set { organizationName = value; }
        }

        string groupDes;
        public string GroupDes
        {
            get { return groupDes; }
            set { groupDes = value; }
        }

        public Group()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Group(string groupName, string organizationName, string groupDes)
        {
            GroupName = groupName;
            OrganizationName = organizationName;
            GroupDes = groupDes;
        }
    }
}














