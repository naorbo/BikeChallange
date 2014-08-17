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
    
    
    public class MailManController : ApiController
    {

        /* POST @ api/MailMan - Is used to send contact message from users in the webapp.  
         
         
        {
            "Name": Contact Name ,
            "Email": Conatct Mail Address,
            "Subject": Subject ,
            "Body": Content,

        }
         
         Output - "Success" / "Failure"  
        
         (!!) Important - do not forget to add our account password to web.config 
         
         */



        
        public string SendMailer([FromBody]MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Models/mailTemplates/contactTemplate.html"));
                string readFile = reader.ReadToEnd();
                string StrContent = readFile;
                StrContent = StrContent.Replace("[ContactName]", _objModelMail.Name);
                StrContent = StrContent.Replace("[ContactMail]", _objModelMail.Email);
                StrContent = StrContent.Replace("[ContactContent]", _objModelMail.Body);
                mail.IsBodyHtml = true;
                mail.To.Add("bchallengeisrael@gmail.com");
                _objModelMail.Subject = "רוכבים לעבודה, פנייה ממשתמש - " + _objModelMail.Subject;
                mail.Subject = _objModelMail.Subject;
                //string Body = _objModelMail.Body;
                mail.Body = StrContent.ToString();
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }

        
        
    }
}




