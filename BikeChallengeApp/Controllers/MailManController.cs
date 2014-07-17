using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
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
        
        /* POST @ api/MailMan 
         
         
         Input -  JSON Body - {
                    "To": Multiple records devided by comma (') ,
                    "Subject": Subject Of the mail,
                    "Body" : Mail's content
           }
         
         Output - "Success" / "Failure"  
        
         (!!) Important - do not forget to add our account password to web.config 
         
         */

        

        [HttpPost]
        public string SendMailer([FromBody]MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();

                string[] MultipleAddresses = _objModelMail.To.Split(',');
                foreach (string EmailAddress in MultipleAddresses)
                {
                    mail.To.Add(EmailAddress);
                }
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
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



