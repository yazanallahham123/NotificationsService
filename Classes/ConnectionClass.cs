using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace NotificationsService.Classes
{
    public class ConnectionClass
    {
        public static SqlConnection Con_instance = null;
        // Lock synchronization object
        private static object syncLock = new object();
        public static SqlConnection DataConnection()
        {
            if (Con_instance == null)
            {
                lock (syncLock)
                {
                    if (Con_instance == null)
                        return new SqlConnection(WebConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
                }
            }

            return Con_instance;
        }
    }
}