using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NotificationsService.Classes
{
    public class UserOneSignalTokenClass
    {
        public int UserId { get; set; }

        public string UserOneSignalToken { get; set; }

        public int UserTypeId { get; set; }

        public string Platform { get; set; }

        public UserOneSignalTokenClass PopulateUserOneSignalToken(string[] fieldNames, SqlDataReader reader)
        {
            var userOneSignalToken = new UserOneSignalTokenClass();

            if (fieldNames.Contains("UserId"))
                if (!Convert.IsDBNull(reader["UserId"]))
                    userOneSignalToken.UserId = (int)reader["UserId"];

            if (fieldNames.Contains("RegistrationId"))
                if (!Convert.IsDBNull(reader["RegistrationId"]))
                    userOneSignalToken.UserOneSignalToken = reader["RegistrationId"].ToString();

            if (fieldNames.Contains("UserType"))
                if (!Convert.IsDBNull(reader["UserType"]))
                    userOneSignalToken.UserTypeId = (int)reader["UserType"];

            if (fieldNames.Contains("Platform"))
                if (!Convert.IsDBNull(reader["Platform"]))
                    userOneSignalToken.Platform = reader["Platform"].ToString();

            return userOneSignalToken;
        }
    }
}