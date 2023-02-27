using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NotificationsService.Classes
{
    public class DeliveryUserFCMTokenClass
    {
        public int DeliveryUserId { get; set; }

        public string DeliveryUserFCMToken { get; set; }

        public string Platform { get; set; }

        public DeliveryUserFCMTokenClass PopulateDeliveryUserFCMToken(string[] fieldNames, SqlDataReader reader)
        {
            var userFCMToken = new DeliveryUserFCMTokenClass();

            if (fieldNames.Contains("DeliveryUserId"))
                if (!Convert.IsDBNull(reader["DeliveryUserId"]))
                    userFCMToken.DeliveryUserId = (int)reader["DeliveryUserId"];

            if (fieldNames.Contains("RegistrationId"))
                if (!Convert.IsDBNull(reader["RegistrationId"]))
                    userFCMToken.DeliveryUserFCMToken = reader["RegistrationId"].ToString();

            if (fieldNames.Contains("Platform"))
                if (!Convert.IsDBNull(reader["Platform"]))
                    userFCMToken.Platform = reader["Platform"].ToString();

            return userFCMToken;
        }
    }
}