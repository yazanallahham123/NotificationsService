using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NotificationsService.Classes
{
    public class FCMTokenClass
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FCMToken { get; set; }

        public FCMTokenClass PopulateClass(string[] fieldNames, SqlDataReader reader)
        {
            var fcmToken = new FCMTokenClass();

            if (fieldNames.Contains("Id"))
                if (!Convert.IsDBNull(reader["Id"]))
                    fcmToken.Id = (int)reader["Id"];

            if (fieldNames.Contains("UserId"))
                if (!Convert.IsDBNull(reader["UserId"]))
                    fcmToken.UserId = (int)reader["UserId"];

            if (fieldNames.Contains("FCMToken"))
                if (!Convert.IsDBNull(reader["FCMToken"]))
                    fcmToken.FCMToken = reader["FCMToken"].ToString();

            return fcmToken;
        }

    }
}