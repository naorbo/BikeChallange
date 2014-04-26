using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Newtonsoft.Json;

class LogFiles
{
    public void Main(string tableName, string ex_msg)
    {
        var LocationLogFile = "~\\LogFiles\\"; // you could put this to web.config
        var root = HttpContext.Current.Server.MapPath(LocationLogFile);
        Directory.CreateDirectory(root);
        string path = root + tableName + "Log.txt";
        // This text is added only once to the file. 
        if (!File.Exists(path))
        {
            
            // Create a file to write to. 
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("The " + tableName + " LOG file");
            }
        }

        // This text is always added, making the file longer over time 
        // if it is not deleted. 
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.Write(DateTime.Now.ToLongTimeString());
            sw.WriteLine(" : " + ex_msg);

        }

        // Open the file to read from. 
       // using (StreamReader sr = File.OpenText(path))
        //{
         //   string s = "";
          //  while ((s = sr.ReadLine()) != null)
          //  {
             //   Console.WriteLine(s);
          //  }
       // }
    }
}