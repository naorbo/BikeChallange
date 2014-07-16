using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using BikeChallengeApp.Models;



namespace BikeChallengeApp
{
    
  public class MailController
  {

      public void SendMail(string ToAddress, string Subject, string Body)
      {

          MailMessage mail = new MailMessage();
          mail.From = new MailAddress("shlomi.avihou@gmail.com");
          mail.To.Add("shlomiavihou@gmail.com");
          mail.Subject = "This is a test";
          mail.Body = "Test content";

          SmtpClient smtp = new SmtpClient();
          smtp.Send(mail);


         

      }

  }



}