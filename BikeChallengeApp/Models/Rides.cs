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
        public Rides(string username, string ridetype, decimal rideLength, string ridedestination, string ridesource, string ridedate)
        {
            UserName = (username != null ? username : "");
            RideType = (ridetype != null ? ridetype : "");
            RideLength = (rideLength != null ? rideLength : 0);
            RideName = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");
            RideDes = (username != null ? username + "_Ride" : "");
            RideDate = (ridedate != null ? ridedate : "");
            RideDestination = (ridedestination != null ? ridedestination : "");
            RideSource = (ridesource != null ? ridesource : "");
            
        }

        public Rides()
        {
            UserName = (UserName != null ? UserName : "");
            RideType = (RideType != null ? RideType : "");
            RideLength = (RideLength != null ? RideLength : 0);
            RideName = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");
            RideDes = (RideDes != null ? RideDes : "");
            RideDate = (RideDate != null ? RideDate : "");
            RideDestination = (RideDestination != null ? RideDestination : "");
            RideSource = (RideSource != null ? RideSource : "");
        }
    }
}