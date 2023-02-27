using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace NotificationsService.Classes
{
   public class FCMClass
   {

        //Machlah
        //public static string FCMServerKey = "AAAAbpZXL2o:APA91bGg7DHjZrGqNktB-aO3EWBppdBBFw3jMjTWzQkb45VrVimgmfQJQfTGOIxthanKRfumNVj8gu_hFW9hsH9WiJUNAA4Fp6_wjyJVhHozgb5VxQI3xhnrxIzlp3f53ONI_vur_YFN";
        //public static string SenderId = "474968698730";

        //Matjar
        public static string FCMServerKey = "AAAAbpZXL2o:APA91bGg7DHjZrGqNktB-aO3EWBppdBBFw3jMjTWzQkb45VrVimgmfQJQfTGOIxthanKRfumNVj8gu_hFW9hsH9WiJUNAA4Fp6_wjyJVhHozgb5VxQI3xhnrxIzlp3f53ONI_vur_YFN";
        public static string SenderId = "474968698730";

        //Those OneSignal Config For Mobile Platform and Web End User
        //Calligra
        //public static string OneSignal_app_id = "b3cc9c10-8c6d-4b80-995e-46c42cc0cb80";
        //public static string Onesignal_apiKey = "Basic NzM2ZTNiMmItMzRjMi00NmVlLWE3ZDEtOTM3NzgwZWIzMWNk";

        public static string OneSignal_app_id = "8878168d-57b5-4fce-80bf-8a40361a7041";
        public static string Onesignal_apiKey = "Basic NjBlNmIzNGYtMWNlMC00NWEwLWE3MjYtZTc3MDIyMGExMTRj";

        public static void CallDb_OnChange()
      {
         //Dp_OnChange(null, null);
      }

      public static void initSQLDep()
      {
         try
         {
            using (SqlConnection conn = ConnectionClass.DataConnection())
            {
               conn.Open();
               SqlCommand cmd = new SqlCommand();
               cmd.Connection = conn;
               cmd.CommandText = "SELECT Id FROM dbo.[NotificationsTbl]";
               cmd.Notification = null;
               //SqlDependency dp = new SqlDependency(cmd);
               //dp.OnChange += Dp_OnChange;
               cmd.ExecuteReader();

               //Errors.LogError(1, "Log: initSQLDep", "initSQLDep", "1.0.3", "API", "Log: initSQLDep", "initSQLDep", "");
            }
         }
         catch (Exception e)
         {
            Errors.LogError(1, e.Message, e.StackTrace, "1.0.3", "API", "initSQLDep", e.Source, "");
         }
      }

      /*private static void Dp_OnChange(object sender, SqlNotificationEventArgs e)
      {

         try
         {
            using (SqlConnection conn = ConnectionClass.DataConnection())
            {
               conn.Open();
               SqlCommand cmd = new SqlCommand();
               cmd.Connection = conn;
               cmd.CommandType = System.Data.CommandType.StoredProcedure;
               cmd.CommandText = "Admin_GetNewNotifications_1";

               SqlDataReader reader = cmd.ExecuteReader();

               if (reader.HasRows)
               {
                  var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                  Errors.LogError(1, "Log: found new notifications", "Admin_GetNewNotifications_1", "1.0.3", "API", "Log: Admin_GetNewNotifications", "Admin_GetNewNotifications", "");
                  List<NotificationClass> notificationsList = new List<NotificationClass>();
                  NotificationClass notification;
                  while (reader.Read())
                  {
                     notification = new NotificationClass().PopulateNotification(fieldNames, reader);

                     notificationsList.Add(notification);

                     Errors.LogError(1, "Log: Add Notification to list :" + notification.NotificationContent, "Admin_GetNewNotifications", "1.0.3", "API", "Dp_OnChange", "Dp_OnChange", "");

                  }

                  Errors.LogError(1, "Log: Start filling stacks :", "Dp_OnChange", "1.0.3", "API", "Log: Dp_OnChange", "Dp_OnChange", "");
                  foreach (NotificationClass Notification in notificationsList)
                  {
                     if (Notification.Platform == null)
                        Notification.Platform = "";
                     Errors.LogError(1, "Log: Notification :" + Notification.NotificationContent, "Dp_OnChange", "1.0.3", "API", "Log: Dp_OnChange", "Dp_OnChange", "");
                     Errors.LogError(1, "Log: Notification Platform :" + Notification.Platform, "Dp_OnChange", "1.0.3", "API", "Log: Dp_OnChange", "Dp_OnChange", "");
                     //New Added For dublicated Notification
                     //Add To static List
                     if (!Config.IndexNotificationList.Exists(x => x.Id.Equals(Notification.Id)))
                     {
                        Config.IndexNotificationList.Add(Notification);
                        Config.CurrentNotificationList.Add(Notification);
                        Errors.LogError(1, "Log: Notification Not in stack :" + Notification.Id, "Dp_OnChange", "1.0.3", "API", "Log: Dp_OnChange", "Dp_OnChange", "");
                        //Task.Delay(300).ContinueWith(x => FCMClass.SendNotification(Notification));
                        //FCMClass.SendNotification(Notification);
                     }
                  }
                  if (Config.fcmStatus == false)
                  {
                     Errors.LogError(1, "Log: Stack is not busy", "Dp_OnChange", "1.0.3", "API", "Log: Dp_OnChange", "Dp_OnChange", "");
                     //yazan disabled
                     //ProcessFCM();
                  }
               }
            }
         }
         catch (Exception err)
         {

            Config.fcmStatus = false;
            Errors.LogError(1, err.Message, err.StackTrace, "1.0.3", "API", "Dp_OnChange", err.Source, "");
         }

         initSQLDep();
      }*/

      static string ProcessNotification(NotificationClass notification)
      {
         string descriptiveResult;
         string res = FCMClass.SendNotification(notification);
         //if (notification.Platform.ToLower() != "ios")  //Android- or web (FCM)  ++ Add IOS to FCM
         {
            //Errors.LogError(1, "Log: Android OR web ", "ProcessNotification", "1.0.3", "API", "Log: ProcessNotification", "ProcessNotification", "");
            //Errors.LogError(2, "Log: res " + res, "ProcessNotification", "1.0.3", "API", "Log: ProcessNotification", "ProcessNotification", "");

            //int index = res.IndexOf("success") + "success".Length + 2; //+2 for ("success":)
            //descriptiveResult = res.Substring(index, 1);
         }
         //else //IOS
         //{
         //   Errors.LogError(1, "Log: IOS ", "ProcessNotification", "1.0.3", "API", "Log: ProcessNotification", "ProcessNotification", "");
         //   Errors.LogError(1, "Log: res " + res, "ProcessNotification", "1.0.3", "API", "Log: ProcessNotification", "ProcessNotification", "");

         int index = res.IndexOf("error");
         if (index > 0)  //It has error in response then Failed
            descriptiveResult = "-1";
         else
            descriptiveResult = "1";
         //}

         return descriptiveResult;

      }

      static void ProcessFCM()
      {
         //Errors.LogError(1, "Log: Start Processing Notifications", "FCMProcess", "1.0.3", "API", "Log: FCMProcess", "FCMProcess", "");
         Config.fcmStatus = true;
         bool removed = false;
         while (Config.CurrentNotificationList.Count > 0)
         {
            //Thread.Sleep(150);

            int noOfTries = 0;
            Config.CurrentNotificationList.Sort((p, q) => p.Id.CompareTo(q.Id));
            NotificationClass currentNotification = Config.CurrentNotificationList[0];
            removed = false;
            string descriptiveResult = "-1";
            while (noOfTries <= 0)
            {
               descriptiveResult = "-1";
               descriptiveResult = ProcessNotification(currentNotification);
               if (Convert.ToInt32(descriptiveResult) > 0) //res is succed                                                           
               {
                  Thread.Sleep(1200);
                  var fail = CheckFailStatus(currentNotification.Id);
                  if (fail == 0) //wait for client reply
                  {
                     descriptiveResult = "-1";
                  }
                  else if (fail == 1)
                  {
                     descriptiveResult = "1";
                     //Errors.LogError(1, "Log: Notification sucessfully Sent", "FCMProcess", "1.0.3", "API", "Log: FCMProcess", "FCMProcess", "");
                     Config.CurrentNotificationList.RemoveAt(0);
                     //Errors.LogError(1, "Log: Notification removed from stack", "FCMProcess", "1.0.3", "API", "Log: FCMProcess", "FCMProcess", "");
                     removed = true;
                     break;
                  }
               }

               if (Convert.ToInt32(descriptiveResult) <= 0)
               {
                  removed = false;
                  //Errors.LogError(1, "Log:Failed to send notification " + currentNotification.Id.ToString() + " TRY NO. :" + noOfTries.ToString() + " descriptiveResult: " + descriptiveResult, "FCMProcess", "1.0.3", "API", "Log: FCMProcess", "FCMProcess", "");
                  noOfTries++;
               }
            }
            if (!removed)
            {
               NotificationClass.UpdateNotification(currentNotification.Id, 2);
               Config.CurrentNotificationList.RemoveAt(0);
               //Errors.LogError(1, "Log: Notification removed from stack", "FCMProcess", "1.0.3", "API", "Log: FCMProcess", "FCMProcess", "");
            }
         }
         Config.fcmStatus = false;
      }

      private static int CheckFailStatus(int notificationId)
      {
         //Fail
         // 0 -> Pending
         // 1 -> Success
         // 2 -> Fail
         var result = NotificationClass.GetNotification(notificationId);
         if (result.Result != null)
            return result.Result.Fail;
         else
            return -1;
      }


      public static string SendNotification(NotificationClass notification)
      {
         if (notification.Platform.ToLower() == "web_test") // Those Config For LocalHost (EndUser + Control Panel)
         {
            OneSignal_app_id = "061094e3-88d8-409b-9310-a48d311abb21";
            Onesignal_apiKey = "Basic MDU1ZGMxYzktZDUyYy00MjkwLThlYzAtZGU0NGJmYjU5NmJh";
         }

         if (notification.Platform.ToLower() == "matjar_control_panel") //Those Config For Control Panel Web
         {

                //Calligra
                //OneSignal_app_id = "9bec9096-d6f3-4ebc-9a95-c8f6844c1a47";
                //Onesignal_apiKey = "Basic NWZkMmI5ZjItMjFhNi00OGQ5LWFjNWYtOTRlMmRmYTI0NDFk";

                OneSignal_app_id = "d7e990d6-4206-44bc-83cc-cb6f3636dd49";
                Onesignal_apiKey = "Basic OTA3ZDQ4YTYtNWJiZS00ZmYxLWI2ZTEtOTVkNmJlN2ZmZGM2";
            }

            string deviceId = notification.FCMRegistrationId;
         string userPointsBalance = notification.PointsBalance.ToString();
         string userTotalSent = notification.TotalSent.ToString();
         string userTotalReceived = notification.TotalReceived.ToString();
         string userTotalReceivedWaiting = notification.TotalReceived_Waiting.ToString();

         string fcm_response =

         "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
           "\"priority\":\"" + "high" + "\"" +
           "\"notification\": {\"sound\":\"" + "default" + "\"," + "\"click_action\":\"" + "FCM_PLUGIN_ACTIVITY" + "\"," +
                       "\"body\":\"" + notification.NotificationContent + "\",\"title\":\"" + notification.NotificationTitle + "\"}" +
             "\"data\": {\"body\":\"" + notification.NotificationContent + "\",\"title\":\"" + notification.NotificationTitle + "\"," +
             "\"Type\":\"" + notification.Type + "\"," +
             "\"userPointsBalance\":\"" + userPointsBalance + "\"," +
             "\"userTotalSent\":\"" + userTotalSent + "\"," +
             "\"userTotalReceived\":\"" + userTotalReceived + "\"," +
             "\"content\":\"" + notification.NotificationContent + "\"," +
             "\"sourceId\":\"" + notification.SourceId + "\"," +
             "\"NotificationId\":\"" + notification.Id + "\"," +
             "\"quantityReservationRenewalCount\":\"" + notification.QuantityReservationRenewalCount + "\"," +
             "\"notification\": {\"sound\":\"" + "default" + "\"," +
                       "\"body\":\"" + notification.NotificationContent + "\",\"title\":\"" + notification.NotificationTitle + "\"}" +
             "\"userTotalReceivedWaiting\":\"" + userTotalReceivedWaiting + "\"}}";


         string fcm_response_web =

        "{ \"registration_ids\": [ \"" + deviceId + "\" ], " +
          "\"priority\":\"" + "high" + "\"" +
           "\"data\": {\"body\":\"" + notification.NotificationContent + "\",\"title\":\"" + notification.NotificationTitle + "\"," +
            "\"Type\":\"" + notification.Type + "\"," +
            "\"userPointsBalance\":\"" + userPointsBalance + "\"," +
            "\"userTotalSent\":\"" + userTotalSent + "\"," +
            "\"userTotalReceived\":\"" + userTotalReceived + "\"," +
            "\"content\":\"" + notification.NotificationContent + "\"," +
            "\"sourceId\":\"" + notification.SourceId + "\"," +
            "\"NotificationId\":\"" + notification.Id + "\"," +
            "\"quantityReservationRenewalCount\":\"" + notification.QuantityReservationRenewalCount + "\"," +
            "\"notification\": {\"sound\":\"" + "default" + "\"," +
                      "\"body\":\"" + notification.NotificationContent + "\",\"title\":\"" + notification.NotificationTitle + "\"}" +
            "\"userTotalReceivedWaiting\":\"" + userTotalReceivedWaiting + "\"}}";


         string Content = notification.NotificationContent;
         if (Content.Trim() == "")
            Content = "Matjar App";

         string onesignal_response = "{"
            + "\"app_id\": \"" + OneSignal_app_id + "\","
            + "\"data\": {\"Type\": \"" + notification.Type +
                  "\",\"userPointsBalance\": \"" + userPointsBalance +
                  "\",\"userTotalSent\": \"" + userTotalSent +
                  "\",\"userTotalReceived\": \"" + userTotalReceived +
                  "\",\"content\": \"" + notification.NotificationContent +
                  "\",\"userTotalReceivedWaiting\": \"" + userTotalReceivedWaiting +
                  "\",\"sourceId\": \"" + notification.SourceId +
                  "\",\"NotificationId\": \"" + notification.Id +
                  "\",\"quantityReservationRenewalCount\": \"" + notification.QuantityReservationRenewalCount + "\"},"
            + "\"contents\": {\"en\": \"" + Content + "\"},"
            + "\"include_player_ids\": [\"" + deviceId + "\"]}";



            //YAZAN DISABLED IT 
            return "";
            /*
             if (notification.Platform.ToLower() == "web")
             {
                return SendFCMNotification(Onesignal_apiKey, FCMServerKey, onesignal_response, fcm_response_web, notification.Platform);
             }
             else
                return SendFCMNotification(Onesignal_apiKey, FCMServerKey, onesignal_response, fcm_response, notification.Platform);
                */
            //return response;
        }

      public static string SendFCMNotification(string onesignal_apiKey, string fcm_apiKey, string onesignal_response, string fcm_response, string platform, string postDataContentType = "application/json; charset=utf-8")
      {
         ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);
         HttpWebRequest Request;
         Stream dataStream;
         byte[] byteArray;
         string fcm_responseLine = "";
         string onesignal_responseLine = "";
         // *****************************************Android - FCM ++ Add IOS 
         if ((platform.Trim() == ""))// || (platform.ToLower() == "android") || (platform.ToLower() == "web" || (platform.ToLower() == "ios")))
         {
            //  MESSAGE CONTENT
            byteArray = Encoding.UTF8.GetBytes(fcm_response);


            //Errors.LogError(1, "Log: Request fcm_response: " + fcm_response, "SendFCMNotification", "1.0.3", "API", "Log: FCMProcess", "FCMProcess", "");


            //  CREATE REQUEST
            Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            Request.Method = "POST";
            Request.KeepAlive = true;
            Request.ContentType = postDataContentType;
            Request.Headers.Add(string.Format("Authorization: key={0}", fcm_apiKey));
            Request.Headers.Add(string.Format("Sender: id={0}", SenderId));

            Request.ContentLength = byteArray.Length;

            dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //
            //  SEND MESSAGE
            try
            {
               WebResponse Response = Request.GetResponse();
               HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
               if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
               {
                  return "Unauthorized - need new token";
               }
               else if (!ResponseCode.Equals(HttpStatusCode.OK))
               {
                  return "Response from web service isn't OK";
               }

               StreamReader Reader = new StreamReader(Response.GetResponseStream());
               fcm_responseLine = Reader.ReadToEnd();
               Reader.Close();
            }
            catch (Exception e)
            {
               Errors.LogError(-1, e.Message, e.StackTrace, "1.0.3", "API", "SendFCMNotification", e.Source, "");
               fcm_responseLine = e.Message;
            }
         }
         else

         //***********************************************IOS - one_signal
         //if ((platform.ToLower() == "webenduser"))
         {

            //  MESSAGE CONTENT
            byteArray = Encoding.UTF8.GetBytes(onesignal_response);

            //
            //  CREATE REQUEST
            Request = (HttpWebRequest)WebRequest.Create("https://onesignal.com/api/v1/notifications");
            Request.Method = "POST";
            Request.KeepAlive = true;
            Request.ContentType = postDataContentType;
            Request.Headers.Add(string.Format("Authorization: key={0}", onesignal_apiKey));
            Request.ContentLength = byteArray.Length;

            dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            //
            //  SEND MESSAGE
            try
            {
               WebResponse Response = Request.GetResponse();
               HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
               if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
               {
                  return "Unauthorized - need new token";
               }
               else if (!ResponseCode.Equals(HttpStatusCode.OK))
               {
                  return "Response from web service isn't OK";
               }

               StreamReader Reader = new StreamReader(Response.GetResponseStream());
               onesignal_responseLine = Reader.ReadToEnd();
               Reader.Close();
            }
            catch (Exception e)
            {
               Errors.LogError(-1, e.Message, e.StackTrace, "1.0.3", "API", "SendFCMNotification", e.Source, "");
               onesignal_responseLine = e.Message;
            }
         }

         return onesignal_responseLine + " - " + fcm_responseLine;
      }


      public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
      {
         return true;
      }

   }
}