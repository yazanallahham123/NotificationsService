using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationsService.Classes
{
    public static class Config
    {
        public static string StaticURL = "http://66.70.177.142:6745";
        public static bool fcmStatus = false;

        public static List<NotificationClass> IndexNotificationList = new List<NotificationClass>();

        public static List<NotificationClass> CurrentNotificationList = new List<NotificationClass>();
    }
}