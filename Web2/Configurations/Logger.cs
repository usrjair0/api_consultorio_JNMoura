using System;
using System.IO;

namespace Web2.Configurations
{
    public class Logger
    {
        public static string getPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["caminhoLog"];
        }
        public static string getFileName() 
        {
            return $"{DateTime.Now:yyyy-MM-dd}.txt";
        }
        public static string getFullPath() 
        { 
            return Path.Combine(getPath(),getFileName());
        }
    }
}