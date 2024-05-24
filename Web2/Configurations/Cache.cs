using System;

using System.IO;

namespace Web2.Configurations
{
    public class Cache
    {
        public static int GetDefaultCacheTimeInSeconds()
        {
            return int.Parse(System.Configuration.ConfigurationManager.AppSettings["defaultCacheTimeInSeconds"]);
        }
    }
}