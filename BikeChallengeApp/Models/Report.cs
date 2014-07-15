using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeChallengeApp.Models
{
    
    public class Report
    {
        string firstRow;
        public string FirstRow
        {
            get { return firstRow; }
            set { firstRow = value; }
        }

        string secondRow;
        public string SecondRow
        {
            get { return secondRow; }
            set { secondRow = value; }
        }

        string thirdRow;
        public string ThirdRow
        {
            get { return thirdRow; }
            set { thirdRow = value; }
        }

        public Report()
        {
            
        }
    }
}