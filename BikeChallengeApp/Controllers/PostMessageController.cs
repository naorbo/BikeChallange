using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using BikeChallengeApp.Models;
using BikeChallengeApp.Providers;
using BikeChallengeApp.Results;
using System.Net.Mail;


namespace BikeChallengeApp.Controllers
{
    public class PostMessageController : ApiController
    {

        /* POST @ api/PostMessage - Used for sending a msg to all users  
         
         
        {
            
            "Subject": Subject ,
            "Body": Content,

        }
         
         Output - "Success" / "Failure"  
        
         (!!) Important - do not forget to add our account password to web.config 
         
         */



        
        [HttpPost]
        [Authorize(Users = "bcadministrator")]
        public string PublishMessage([FromBody]MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {


                // Fetch dist list by email
                DBservices dbs = new DBservices();
                dbs = dbs.ReadFromDataBase(28);
                for (int i = 0; i < dbs.dt.Rows.Count; i++)
                {
                    string userName = dbs.dt.Rows[i].ItemArray[1].ToString();
                    string emailAddress = dbs.dt.Rows[i].ItemArray[0].ToString();

                    
                    // DO NOT UNCOMMENT - IT WILL SEND SPAM MESSAGES TO ALL EMAIL ADDRESSES IN DB
                    
                    //MailMessage mail = new MailMessage();
                    //StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Models/mailTemplates/adminMessage.html"));
                    //string readFile = reader.ReadToEnd();
                    //string StrContent = readFile;
                    //StrContent = StrContent.Replace("[Recepient]", userName);
                    //StrContent = StrContent.Replace("[MailContent]", _objModelMail.Body);
                    //mail.IsBodyHtml = true;
                    ////mail.To.Add(emailAddress);
                    //mail.Subject = _objModelMail.Subject;
                    //mail.Body = StrContent.ToString();
                    //mail.IsBodyHtml = true;
                    //SmtpClient smtp = new SmtpClient();
                    //smtp.Send(mail);
                }
                
               
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }
    }

    }







