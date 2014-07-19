using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeChallengeApp.Models
{
    
    public class Report
    {
        string firstCol;
        public string FirstCol
        {
            get { return firstCol; }
            set { firstCol = value; }
        }
        
        string secondCol;
        public string SecondCol
        {
            get { return secondCol; }
            set { secondCol = value; }
        }

        string thirdCol;
        public string ThirdCol
        {
            get { return thirdCol; }
            set { thirdCol = value; }
        }
        
        public Report()
        {
            
        }
    }
}