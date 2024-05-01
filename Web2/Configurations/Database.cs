using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Configurations
{
    public class Database
    {
        public static string getConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["consultorio2"].ConnectionString;
        }
    }
}