using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeChallengeApp.Models
{
    public class Rides
    {
        string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        string rideName;
        public string RideName
        {
            get { return rideName; }
            set { rideName = value; }
        }

        string rideDes;
        public string RideDes
        {
            get { return rideDes; }
            set { rideDes = value; }
        }

        string rideType;
        public string RideType
        {
            get { return rideType; }
            set { rideType = value; }
        }

        string rideDate;
        public string RideDate
        {
            get { return rideDate; }
            set { rideDate = value; }
        }
        decimal rideLength;
        public decimal RideLength
        {
            get { return rideLength; }
            set { rideLength = value; }
        }

        string rideSource;
        public string RideSource
        {
            get { return rideSource; }
            set { rideSource = value; }
        }
        string rideDestination;
        public string RideDestination
        {
            get { return rideDestination; }
            set { rideDestination = value; }
        }
        public Rides(string username, string ridetype, decimal rideLength, string ridedestination, string ridesource)
        {
            UserName = username;
            RideType = ridetype;
            RideLength = rideLength;
            RideName = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");
            RideDes = username + "_Ride";
            RideDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            RideDestination = ridedestination;
            RideSource = ridesource;
            
        }

        public Rides()
        {
            // TODO: Complete member initialization
        }
    }
}