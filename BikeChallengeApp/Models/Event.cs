using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace BikeChallengeApp.Models
{
    public class Event
    {

        string eventName;
        public string EventName
        {
            get { return eventName; }
            set
            {
                eventName = value;
                eventName = eventName.Replace("'", "''");
                eventDes = eventName;
                eventName = eventName.Replace("&", "'+CHAR(37)+'26");
                eventName = eventName.Replace(" ", "'+CHAR(37)+'20");
            }
        }

        string city;
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        string eventDes;
        public string EventDes
        {
            get { return eventDes; }
            set {  }
        }

        string eventType;
        public string EventType
        {
            get { return eventType; }
            set { eventType = value;
            eventType = eventType.Replace("'", "''");
            }
        }

        string eventStatus;
        public string EventStatus
        {
            get { return eventStatus; }
            set { eventStatus = value; eventStatus = eventStatus.Replace("'", "''"); }
        }

        string eventDate;
        public string EventDate
        {
            get { return eventDate; }
            set { eventDate = value; }
        }


         public Event()
        {
            
        }
         public Event(string eventName, string eventDate, string eventDes, string city, string eventType, string eventStatus)
        {
            EventName = (eventName != null ? eventName : "");
            EventDate = (eventDate != null ? eventDate : "");
            EventDes = (eventDes != null ? eventDes : "");
            EventType = (eventType != null ? eventType : "");
            EventStatus = (eventStatus != null ? eventStatus : "");
            City = (city != null ? city : "");

        }
    }
}