﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeChallengeApp.Models
{
    public class Routes
    {
        string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        string routeName;
        public string RouteName
        {
            get { return routeName; }
            set { routeName = value; }
        }

        string routeDestination;
        public string RouteDestination
        {
            get { return routeDestination; }
            set { routeDestination = value; }
        }

        string routeSource;
        public string RouteSource
        {
            get { return routeSource; }
            set { routeSource = value; }
        }
        string routeType;
        public string RouteType
        {
            get { return routeType; }
            set { routeType = value; }
        }

        string comments;
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        decimal routeLength;
        public decimal RouteLength
        {
            get { return routeLength; }
            set { routeLength = value; }
        }

        public Routes(string username, string routeType, decimal routelength, string comments, string routesource, string routedestination)
        {
            UserName = username;
            RouteType = routeType;
            Comments = comments;
            RouteName = username + DateTime.Now.ToString("dd-MM-hh-mm");
            RouteDestination = routedestination;
            RouteSource = routesource;
            RouteLength = routelength;
            
        }
    }
}