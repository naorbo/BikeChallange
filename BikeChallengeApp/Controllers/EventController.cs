using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BikeChallengeApp.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web;

namespace BikeChallengeApp.Controllers
{
    public class EventController : ApiController
    {   

        // GET api/event?username=[The name of the organization]
        public DataTable Get(string username)
        {
            DBservices dbs = new DBservices();
            username = username.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(20, username);
            return dbs.dt;
        }

        // GET api/event/GetEvents
        // get all of the events
        public DataTable GetEvents()
        {
            DBservices dbs = new DBservices();
            dbs = dbs.ReadFromDataBase(21);
            return dbs.dt;
        }

        // GET api/event/GetUsers?eventname
        // get all of users from a specific event
        public DataTable GetUsers(string eventname)
        {
            DBservices dbs = new DBservices();
            eventname = eventname.Replace("'", "''");
            dbs = dbs.ReadFromDataBase(22, eventname);
            return dbs.dt;
        }

        // POST api/Event
        // insert new event
        // {"EventName":"", "City":"","EventType":"", "EventDate":"", "EventStatus":""}
        public string updateDB([FromBody]Event evt)
        {
            DBservices dbs = new DBservices();
            int return_val = 0;
            LogFiles lf = new LogFiles();
            List<Object> mlist = new List<Object>();
            mlist.Add(evt);
            try
            {
                return_val = dbs.InsertDatabase(mlist);
            }
            catch (Exception ex)
            {
                string Response = ("Error updating the Group database " + ex.Message);
                lf.Main("Events", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        // POST insert rider into an event 
        // api/Event?eventname=&username=
        public string RiderInEvent(string eventname, string username)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            eventname = eventname.Replace("'", "''");
            username = username.Replace("'", "''");
            try
            {
                return_val = dbs.updateRiderIneventDatabase(eventname, username);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to Update the Rider(user) " + username + " to the event " + eventname + ", "+ ex.Message);
                lf.Main("UsersEvents", Response);
                return "Error";
            }
            if ( return_val == 0 ){return "Error";}
            return "Success";
        }

        // DELETE User From Event 
        // api/Event?usernme=
        public string Delete(string username)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            string Response = "";
            username = username.Replace("'", "''");
            try
            {
                return_val = dbs.DeleteDatabase("UserEvent", username);
            }
            catch (Exception ex)
            {
                Response = ("Error in the Delete process the Rider(user) from an event database " + ex.Message);
                lf.Main("UserEvent", Response);
                return "Error";
            }
            Response = "The Rider " + username + " was Deleted from the Event";
            lf.Main("UserEvent", Response);
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        // DELETE Event 
        // api/Event?eventname=
        public string DeleteEvent(string eventname)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            string Response = "";
            eventname = eventname.Replace("'", "''");
            try
            {
                return_val = dbs.DeleteDatabase("Event", eventname);
            }
            catch (Exception ex)
            {
                Response = ("Error in the Delete process the Event from an event database " + ex.Message);
                lf.Main("UserEvent", Response);
                return "Error";
            }
            Response = "The Event " + eventname + " was Deleted from the Event";
            lf.Main("UserEvent", Response);
            if (return_val == 0) { return "Error"; }
            return "Success";
        }

        // PUT api/Event?eventname=
        // {"EventName":"", "City":"","EventType":"", "EventDate":"","EventStatus":""}
        public string Put(string eventname, [FromBody]Event evt)
        {
            int return_val = 0;
            LogFiles lf = new LogFiles();
            DBservices dbs = new DBservices();
            eventname = eventname.Replace("'", "''");
            try
            {
                return_val = dbs.updateEventInDatabase(evt, eventname);
            }
            catch (Exception ex)
            {
                string Response = ("Error while trying to Update the Event " + eventname + " to the database " + ex.Message);
                lf.Main("Events", Response);
                return "Error";
            }
            if (return_val == 0) { return "Error"; }
            return "Success";
        }
    }
}