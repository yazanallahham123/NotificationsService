using NotificationsService.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Configuration;

namespace NotificationsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "NotificationsServiceAPI" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select NotificationsServiceAPI.svc or NotificationsServiceAPI.svc.cs at the Solution Explorer and start debugging.

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class NotificationsServiceAPI : INotificationsServiceAPI
    {

        public string SendNotifications(List<string> receivers, string manifestNumber, string stationName, string manifestGUID,
            string stationGUID, string title, string content, bool isLastStation)
        {
            try
            {
                //Check if there are any users to recieve notifications
                if (receivers.Count > 0)
                {

                    string receiversString = "";

                    for (int i = 0; i < receivers.Count; i++)
                        
                        receiversString = receiversString + "\"" + receivers[i] + "\"" + ',';

                    if (receiversString.EndsWith(","))
                        receiversString = receiversString.Substring(0, receiversString.Length - 1);
                    //DevelopiStore
                    string FCMServerKey = "AAAA0CYRy60:APA91bFOJky-izkt-10AVI13ModOa4_hacCtbQnbYIf6CHpyJbuGHLc8y9ZMlZuNAxt5kOrRT4CSxAfrzIpLLRzrsykmi6V8kDUxUaDnP7-fj6ZHgXJBXs8XQYN2KIhA4RVHcq-Op79V";
                    //Matjar
                    //string FCMServerKey = "AAAAGuRJ66w:APA91bFYwu4UjrtV_k4SYYKRmVutcX5AzhYwcbhcqkqgfU25wFhpCYKiK7QvmJLNGm_zH3l1IDZ9wPkW5pJ-s-zv0DJ92RI5Qe2cKlUl1A7gjJ1gUvU6UMxaxYSOnaBn8mv3zi53h9CC";
                    //Machlah
                    //string FCMServerKey = "AAAAe4p95K0:APA91bGsFqCoIFjym02YeoEuW67DftSaNXSEnZgBYaXDFuUeleuVZkGNwwI2yqhKjtMyPKJXN2hM0tkqRxtbdFt-7mxrJEuFfK9ptOhWZAp5gdUbvoRA125hO544sDQzPmH86CaeqNCe";

                    if (receiversString.Trim() != "")
                    {


                        string fcm_response = "{ \"registration_ids\": [ " + receiversString + " ], " +
                          "\"priority\":\"" + "high" + "\"" +
                          "\"content_available\":" + "true" + "," +
                            "\"data\": {\"body\":\"" + content + "\",\"title\":\"" + title + "\"," +
                            "\"manifestNumber\":\"" + manifestNumber + "\"," +
                            "\"stationName\":\"" + stationName + "\"," +
                            "\"manifestGUID\":\"" + manifestGUID + "\"," +
                            "\"stationGUID\":\"" + stationGUID + "\"," +
                            "\"isLastStation\":\"" + isLastStation.ToString() + "\"," +
                            "\"Reserved\":\"" + "0" + "\"," +
                            "\"notification\": {\"sound\":\"" + "default" + "\"," +
                                      "\"body\":\"" + content + "\",\"title\":\"" + title + "\"}" +
                            "\"Type\":\"" + "0" + "\"}}";

                        string postDataContentType = "application/json; charset=utf-8";
                        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FCMClass.ValidateServerCertificate);
                        HttpWebRequest Request;
                        Stream dataStream;
                        byte[] byteArray;


                        //  MESSAGE CONTENT
                        //byteArray = Encoding.UTF8.GetBytes(onesignal_response);
                        byteArray = Encoding.UTF8.GetBytes(fcm_response);

                        //
                        // CREATE REQUEST
                        //Request = (HttpWebRequest)WebRequest.Create("https://onesignal.com/api/v1/notifications");
                        Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                        Request.Method = "POST";
                        Request.KeepAlive = true;
                        Request.ContentType = postDataContentType;
                        //Request.Headers.Add(string.Format("Authorization: key={0}", Onesignal_apiKey));
                        Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                        Request.ContentLength = byteArray.Length;

                        dataStream = Request.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                        string error;
                        //
                        //  SEND MESSAGE
                        try
                        {
                            WebResponse Response = Request.GetResponse();
                            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                            {
                                error = "Unauthorized - need new token";
                            }
                            else if (!ResponseCode.Equals(HttpStatusCode.OK))
                            {
                                error = "Response from web service isn't OK";
                            }

                            StreamReader Reader = new StreamReader(Response.GetResponseStream());
                            Reader.Close();
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Errors.LogError(1, e.Message, e.StackTrace, "1.0.3", "API", "SendNotifications", e.Source, "");
            }

            return "";
        }

        public string FakeCall()
        {
            return "done";
            /*try
            {
                Errors.LogError(1, "Log: FakeCall", "FakeCall", "1.0.3", "API", "Log: FakeCall", "FakeCall", "");
                SqlDependency.Start(WebConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
                FCMClass.initSQLDep();
                return "done";
            }
            catch (Exception e)
            {
                Errors.LogError(1, e.Message, e.StackTrace, "1.0.3", "API", "FakeCall", e.Source, "");
                return e.Message;
            }*/
        }

        
        public string SendNotification(List<int> Receivers, string NotificationTitle, string NotificationContent, string NotificationType, string Platform, 
            string SourceId, string QuantityReservationRenewalCount, string NotificationId, string ImageURL, string ReferenceType, string ReferenceId, string CheckOutType)
        {
            string onesignal_responseLine = "";
            string ReceiversWebPlayerIds = "";
            string ReceiversMobilePlayerIds = "";
            string AdminReceiversPlayerIds = "";
            string NoId = "";

            if ((NotificationId == "") || (NotificationId == null))
                NoId = "0";
            else
                NoId = NotificationId;

            try
            {
                //Get Receivers Tokens
                using (SqlConnection conn = ConnectionClass.DataConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "Admin_GetUsersOneSginalTokens";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    List<SqlParameter> Params = new List<SqlParameter>()
                    { };

                    if (Receivers.Count > 0)
                    {

                        //receivers
                        DataTable users;
                        using (users = new DataTable())
                        {
                            users.Columns.Add("Item", typeof(string));
                            foreach (int x in Receivers)
                                users.Rows.Add(x);
                        }
                        var pList = new SqlParameter("@UsersIds", System.Data.SqlDbType.Structured);
                        pList.Value = users;
                        Params.Add(pList);
                    }

                    cmd.Parameters.AddRange(Params.ToArray());

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                        List<UserOneSignalTokenClass> ReceiversMobileTokens = new List<UserOneSignalTokenClass>();
                        List<UserOneSignalTokenClass> ReceiversWebTokens = new List<UserOneSignalTokenClass>();

                        List<UserOneSignalTokenClass> AdminReceiversTokens = new List<UserOneSignalTokenClass>();
                        UserOneSignalTokenClass receiverToken;

                        while (reader.Read())
                        {
                            receiverToken = new UserOneSignalTokenClass().PopulateUserOneSignalToken(fieldNames, reader);
                            if (receiverToken != null)
                            {
                                if (receiverToken.UserTypeId != null)
                                {
                                    if ((receiverToken.UserTypeId != 1) && (receiverToken.UserTypeId != 2) && (receiverToken.UserTypeId != 8))     //not admins
                                    {
                                        if (receiverToken.Platform != null)
                                        {
                                            if (receiverToken.Platform.ToLower() == "web")
                                            {
                                                ReceiversWebTokens.Add(receiverToken);
                                                ReceiversWebPlayerIds = ReceiversWebPlayerIds + "\"" + receiverToken.UserOneSignalToken + "\"" + ",";
                                            }
                                            else
                                            {
                                                ReceiversMobileTokens.Add(receiverToken);
                                                ReceiversMobilePlayerIds = ReceiversMobilePlayerIds + "\"" + receiverToken.UserOneSignalToken + "\"" + ",";
                                            }
                                        }
                                        else
                                        {
                                            ReceiversMobileTokens.Add(receiverToken);
                                            ReceiversMobilePlayerIds = ReceiversMobilePlayerIds + "\"" + receiverToken.UserOneSignalToken + "\"" + ",";
                                        }
                                    }
                                    else
                                    {
                                        AdminReceiversPlayerIds = AdminReceiversPlayerIds + "\"" + receiverToken.UserOneSignalToken + "\"" + ",";
                                        AdminReceiversTokens.Add(receiverToken);
                                    }
                                }
                            }
                        }

                        if (ReceiversMobilePlayerIds.EndsWith(","))
                            ReceiversMobilePlayerIds = ReceiversMobilePlayerIds.Substring(0, ReceiversMobilePlayerIds.Length - 1);

                        if (ReceiversWebPlayerIds.EndsWith(","))
                            ReceiversWebPlayerIds = ReceiversWebPlayerIds.Substring(0, ReceiversWebPlayerIds.Length - 1);

                        if (AdminReceiversPlayerIds.EndsWith(","))
                            AdminReceiversPlayerIds = AdminReceiversPlayerIds.Substring(0, AdminReceiversPlayerIds.Length - 1);

                        //Caligra Client
                        /*
                        string OneSignal_app_id = "b3cc9c10-8c6d-4b80-995e-46c42cc0cb80";
                        string Onesignal_apiKey = "Basic NzM2ZTNiMmItMzRjMi00NmVlLWE3ZDEtOTM3NzgwZWIzMWNk";

                        //Calligra Admin
                        string AdminOneSignal_app_id = "9bec9096-d6f3-4ebc-9a95-c8f6844c1a47";
                        string AdminOnesignal_apiKey = "Basic NWZkMmI5ZjItMjFhNi00OGQ5LWFjNWYtOTRlMmRmYTI0NDFk";
                        */

                        /*
                        string OneSignal_app_id = "8878168d-57b5-4fce-80bf-8a40361a7041";
                        string Onesignal_apiKey = "Basic NjBlNmIzNGYtMWNlMC00NWEwLWE3MjYtZTc3MDIyMGExMTRj";

                        string AdminOneSignal_app_id = "d7e990d6-4206-44bc-83cc-cb6f3636dd49";
                        string AdminOnesignal_apiKey = "Basic OTA3ZDQ4YTYtNWJiZS00ZmYxLWI2ZTEtOTVkNmJlN2ZmZGM2";
                        */


                        //Developi Store
                                              
                        string FCMServerKey = "AAAA0CYRy60:APA91bFOJky-izkt-10AVI13ModOa4_hacCtbQnbYIf6CHpyJbuGHLc8y9ZMlZuNAxt5kOrRT4CSxAfrzIpLLRzrsykmi6V8kDUxUaDnP7-fj6ZHgXJBXs8XQYN2KIhA4RVHcq-Op79V";
                        //Matjar Server Key   
                        //string FCMServerKey = "AAAAbpZXL2o:APA91bGg7DHjZrGqNktB-aO3EWBppdBBFw3jMjTWzQkb45VrVimgmfQJQfTGOIxthanKRfumNVj8gu_hFW9hsH9WiJUNAA4Fp6_wjyJVhHozgb5VxQI3xhnrxIzlp3f53ONI_vur_YFN";

                        //Atlas Server Key
                        //string FCMServerKey = "AAAAP2GQm2g:APA91bGZSpJAfOx4mWP204G6lUoCJAEx4r6bgh-TESmit5Z-bpMdFJE_-OPMmZVbngB0PjKazYa6X-BmK06bHEaKWF4jywFiWDZTVYZBpLTudMg8-WCefX7Bhpu4MSbdBDdwRHkDFdoG";
                        //string SenderId = "474968698730";

                        //Machlah ServerKey
                        //string FCMServerKey = "AAAAe4p95K0:APA91bGsFqCoIFjym02YeoEuW67DftSaNXSEnZgBYaXDFuUeleuVZkGNwwI2yqhKjtMyPKJXN2hM0tkqRxtbdFt-7mxrJEuFfK9ptOhWZAp5gdUbvoRA125hO544sDQzPmH86CaeqNCe";

                        //Machlah ServerKey
                        //string FCMServerKey = "AAAAe4p95K0:APA91bGsFqCoIFjym02YeoEuW67DftSaNXSEnZgBYaXDFuUeleuVZkGNwwI2yqhKjtMyPKJXN2hM0tkqRxtbdFt-7mxrJEuFfK9ptOhWZAp5gdUbvoRA125hO544sDQzPmH86CaeqNCe";

                        //Tresbelle
                        //string FCMServerKey = "AAAAlLHd8MI:APA91bE2H6pLOsoAeLl3ToYMAp4qDKNNkC69WfSEfu59wo1Wsl15OhKXtoGIW-caU1DgEfUUJBGZ9NfQO1vSyIIHcAJp4RU36sdl3bW_j0PYcA9SCDj9CBxMtGyn4Epe7FE1vKO_wAp4";

                        /*
                        if (Platform.ToLower() == "web_test") // Those Config For LocalHost (EndUser + Control Panel)
                        {
                            OneSignal_app_id = "061094e3-88d8-409b-9310-a48d311abb21";
                            Onesignal_apiKey = "Basic MDU1ZGMxYzktZDUyYy00MjkwLThlYzAtZGU0NGJmYjU5NmJh";
                            AdminOneSignal_app_id = OneSignal_app_id;
                            AdminOnesignal_apiKey = Onesignal_apiKey;
                        }
                        */


                        if (ReceiversMobilePlayerIds.Trim() != "")
                        {


                            string fcm_response = "{ \"registration_ids\": [ " + ReceiversMobilePlayerIds + " ], " +
                              "\"priority\":\"" + "high" + "\"" +

                              "\"notification\": {\"sound\":\"" + "default" + "\"," + "\"click_action\":\"" + "FLUTTER_NOTIFICATION_CLICK" + "\"," +
                              "\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\"," +
                              "\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"}" +
                              //"\"image\":\"" + ImageURL + "\"}" +
                              "\"content_available\":" + "true" + "," +    //with this ios won't receive notification (this is for data messages)
                                                                           //"\"notification\": {\"sound\":\"" + "default" + "\"," + "\"click_action\":\"" + "MainActivity" + "\"," +
                                                                           //"\"body\":\"" + NotificationContent + "\",\"title\":\"" + NotificationTitle + "\"}" +

                                "\"data\": {\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"," +
                                "\"Type\":\"" + NotificationType + "\"," +
                                "\"userPointsBalance\":\"" + "0" + "\"," +
                                "\"userTotalSent\":\"" + "0" + "\"," +
                                "\"userTotalReceived\":\"" + "0" + "\"," +
                                "\"content\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\"," +
                                "\"sourceId\":\"" + SourceId + "\"," +
                                "\"NotificationId\":\"" + NoId + "\"," +
                                "\"image\":\"" + ImageURL + "\"," +
                                "\"ReferenceType\":\"" + ReferenceType + "\"," +
                                "\"ReferenceId\":\"" + ReferenceId + "\"," +
                                "\"CheckOutType\":\"" + CheckOutType + "\"," +
                                "\"quantityReservationRenewalCount\":\"" + QuantityReservationRenewalCount + "\"," +
                                "\"notification\": {\"sound\":\"" + "default" + "\"," +
                                          "\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"}" +
                                "\"userTotalReceivedWaiting\":\"" + "0" + "\"}}";


                            /*string onesignal_response = "{"
                           + "\"app_id\": \"" + OneSignal_app_id + "\","
                           + "\"data\": {\"Type\": \"" + NotificationType +
                                 "\",\"userPointsBalance\": \"" + "0" +
                                 "\",\"userTotalSent\": \"" + "0" +
                                 "\",\"userTotalReceived\": \"" + "0" +
                                 "\",\"content\": \"" + NotificationContent +
                                 "\",\"userTotalReceivedWaiting\": \"" + "0" +
                                 "\",\"sourceId\": \"" + SourceId +
                                 "\",\"NotificationTitle\": \"" + NotificationTitle +
                                 "\",\"NotificationId\": \"" + "0" +
                                 "\",\"quantityReservationRenewalCount\": \"" + "0" + "\"},"
                           + "\"contents\": {\"en\": \"" + NotificationContent + "\"},"
                           + "\"include_player_ids\": [" + ReceiversPlayerIds + "]}";
                           */

                            {
                                string postDataContentType = "application/json; charset=utf-8";
                                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FCMClass.ValidateServerCertificate);
                                HttpWebRequest Request;
                                Stream dataStream;
                                byte[] byteArray;


                                //  MESSAGE CONTENT
                                //byteArray = Encoding.UTF8.GetBytes(onesignal_response);
                                byteArray = Encoding.UTF8.GetBytes(fcm_response);

                                //
                                // CREATE REQUEST
                                //Request = (HttpWebRequest)WebRequest.Create("https://onesignal.com/api/v1/notifications");
                                Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                                Request.Method = "POST";
                                Request.KeepAlive = true;
                                Request.ContentType = postDataContentType;
                                //Request.Headers.Add(string.Format("Authorization: key={0}", Onesignal_apiKey));
                                Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                                Request.ContentLength = byteArray.Length;

                                dataStream = Request.GetRequestStream();
                                dataStream.Write(byteArray, 0, byteArray.Length);
                                dataStream.Close();

                                string error;
                                //
                                //  SEND MESSAGE
                                try
                                {
                                    WebResponse Response = Request.GetResponse();
                                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                                    if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                                    {
                                        error = "Unauthorized - need new token";
                                    }
                                    else if (!ResponseCode.Equals(HttpStatusCode.OK))
                                    {
                                        error = "Response from web service isn't OK";
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
                        }

                        if (ReceiversWebPlayerIds.Trim() != "")
                        {


                            string fcm_response = "{ \"registration_ids\": [ " + ReceiversWebPlayerIds + " ], " +
                              "\"priority\":\"" + "high" + "\"" +
                                "\"data\": {\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"," +
                                "\"Type\":\"" + NotificationType.Replace(Environment.NewLine, " ") + "\"," +
                                "\"userPointsBalance\":\"" + "0" + "\"," +
                                "\"userTotalSent\":\"" + "0" + "\"," +
                                "\"userTotalReceived\":\"" + "0" + "\"," +
                                "\"content\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\"," +
                                "\"sourceId\":\"" + SourceId + "\"," +
                                "\"NotificationId\":\"" + NoId + "\"," +
                                "\"ImageUrl\":\"" + ImageURL + "\"," +
                                "\"ReferenceType\":\"" + ReferenceType + "\"," +
                                "\"ReferenceId\":\"" + ReferenceId + "\"," +
                                "\"CheckOutType\":\"" + CheckOutType + "\"," +
                                "\"quantityReservationRenewalCount\":\"" + QuantityReservationRenewalCount + "\"," +
                                "\"notification\": {\"sound\":\"" + "default" + "\"," +
                                          "\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"}" +
                                "\"userTotalReceivedWaiting\":\"" + "0" + "\"}}";


                            /*string onesignal_response = "{"
                           + "\"app_id\": \"" + OneSignal_app_id + "\","
                           + "\"data\": {\"Type\": \"" + NotificationType +
                                 "\",\"userPointsBalance\": \"" + "0" +
                                 "\",\"userTotalSent\": \"" + "0" +
                                 "\",\"userTotalReceived\": \"" + "0" +
                                 "\",\"content\": \"" + NotificationContent +
                                 "\",\"userTotalReceivedWaiting\": \"" + "0" +
                                 "\",\"sourceId\": \"" + SourceId +
                                 "\",\"NotificationTitle\": \"" + NotificationTitle +
                                 "\",\"NotificationId\": \"" + "0" +
                                 "\",\"quantityReservationRenewalCount\": \"" + "0" + "\"},"
                           + "\"contents\": {\"en\": \"" + NotificationContent + "\"},"
                           + "\"include_player_ids\": [" + ReceiversPlayerIds + "]}";
                           */

                            {
                                string postDataContentType = "application/json; charset=utf-8";
                                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FCMClass.ValidateServerCertificate);
                                HttpWebRequest Request;
                                Stream dataStream;
                                byte[] byteArray;


                                //  MESSAGE CONTENT
                                //byteArray = Encoding.UTF8.GetBytes(onesignal_response);
                                byteArray = Encoding.UTF8.GetBytes(fcm_response);

                                //
                                // CREATE REQUEST
                                //Request = (HttpWebRequest)WebRequest.Create("https://onesignal.com/api/v1/notifications");
                                Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                                Request.Method = "POST";
                                Request.KeepAlive = true;
                                Request.ContentType = postDataContentType;
                                //Request.Headers.Add(string.Format("Authorization: key={0}", Onesignal_apiKey));
                                Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                                Request.ContentLength = byteArray.Length;

                                dataStream = Request.GetRequestStream();
                                dataStream.Write(byteArray, 0, byteArray.Length);
                                dataStream.Close();

                                string error;
                                //
                                //  SEND MESSAGE
                                try
                                {
                                    WebResponse Response = Request.GetResponse();
                                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                                    if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                                    {
                                        error = "Unauthorized - need new token";
                                    }
                                    else if (!ResponseCode.Equals(HttpStatusCode.OK))
                                    {
                                        error = "Response from web service isn't OK";
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
                        }

                        if (AdminReceiversPlayerIds.Trim() != "")
                        {
                            /*string onesignal_response = "{"
                           + "\"app_id\": \"" + AdminOneSignal_app_id + "\","
                           + "\"data\": {\"Type\": \"" + NotificationType +
                                 "\",\"userPointsBalance\": \"" + "0" +
                                 "\",\"userTotalSent\": \"" + "0" +
                                 "\",\"userTotalReceived\": \"" + "0" +
                                 "\",\"content\": \"" + NotificationContent +
                                 "\",\"userTotalReceivedWaiting\": \"" + "0" +
                                 "\",\"sourceId\": \"" + SourceId +
                                 "\",\"NotificationTitle\": \"" + NotificationTitle +
                                 "\",\"NotificationId\": \"" + "0" +
                                 "\",\"quantityReservationRenewalCount\": \"" + "0" + "\"},"
                           + "\"contents\": {\"en\": \"" + NotificationContent + "\"},"
                           + "\"include_player_ids\": [" + AdminReceiversPlayerIds + "]}"; */

                            /*string fcm_response = "{ \"registration_ids\": [ " + AdminReceiversPlayerIds + " ], " +
                              "\"priority\":\"" + "high" + "\"" +
                                //                              "\"notification\": {\"sound\":\"" + "default" + "\"," + "\"click_action\":\"" + "FCM_PLUGIN_ACTIVITY" + "\"," +
                                //                                          "\"body\":\"" + NotificationContent + "\",\"title\":\"" + NotificationTitle + "\"}" +
                                "\"data\": {\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"," +
                                "\"Type\":\"" + NotificationType + "\"," +
                                "\"userPointsBalance\":\"" + "0" + "\"," +
                                "\"userTotalSent\":\"" + "0" + "\"," +
                                "\"userTotalReceived\":\"" + "0" + "\"," +
                                "\"content\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\"," +
                                "\"sourceId\":\"" + SourceId + "\"," +
                                "\"NotificationId\":\"" + NoId + "\"," +
                                "\"ImageUrl\":\"" + ImageURL + "\"," +
                                "\"ReferenceType\":\"" + ReferenceType + "\"," +
                                "\"ReferenceId\":\"" + ReferenceId + "\"," +
                                "\"CheckOutType\":\"" + CheckOutType + "\"," +
                                "\"quantityReservationRenewalCount\":\"" + QuantityReservationRenewalCount + "\"," +
                                "\"notification\": {\"sound\":\"" + "default" + "\"," +
                                          "\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"}" +
                                "\"userTotalReceivedWaiting\":\"" + "0" + "\"}}";

                            */


                            /*string fcm_responseMobile = "{ \"registration_ids\": [ " + AdminReceiversPlayerIds + " ], " +
  "\"priority\":\"" + "high" + "\"" +

  "\"notification\": {\"sound\":\"" + "default" + "\"," + "\"click_action\":\"" + "FLUTTER_NOTIFICATION_CLICK" + "\"," +
  "\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\"," +
  "\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"}" +
  //"\"image\":\"" + ImageURL + "\"}" +
  "\"content_available\":" + "true" + "," +    //with this ios won't receive notification (this is for data messages)
                                               //"\"notification\": {\"sound\":\"" + "default" + "\"," + "\"click_action\":\"" + "MainActivity" + "\"," +
                                               //"\"body\":\"" + NotificationContent + "\",\"title\":\"" + NotificationTitle + "\"}" +

    "\"data\": {\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"," +
    "\"Type\":\"" + NotificationType + "\"," +
    "\"userPointsBalance\":\"" + "0" + "\"," +
    "\"userTotalSent\":\"" + "0" + "\"," +
    "\"userTotalReceived\":\"" + "0" + "\"," +
    "\"content\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\"," +
    "\"sourceId\":\"" + SourceId + "\"," +
    "\"NotificationId\":\"" + NoId + "\"," +
    "\"image\":\"" + ImageURL + "\"," +
    "\"ReferenceType\":\"" + ReferenceType + "\"," +
    "\"ReferenceId\":\"" + ReferenceId + "\"," +
    "\"CheckOutType\":\"" + CheckOutType + "\"," +
    "\"quantityReservationRenewalCount\":\"" + QuantityReservationRenewalCount + "\"," +
    "\"notification\": {\"sound\":\"" + "default" + "\"," +
              "\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"}" +
    "\"userTotalReceivedWaiting\":\"" + "0" + "\"}}";*/

                            string fcm_response = "{ \"registration_ids\": [ " + AdminReceiversPlayerIds + " ], " +
                              "\"priority\":\"" + "high" + "\"" +
                                "\"data\": {\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"," +
                                "\"Type\":\"" + NotificationType.Replace(Environment.NewLine, " ") + "\"," +
                                "\"userPointsBalance\":\"" + "0" + "\"," +
                                "\"userTotalSent\":\"" + "0" + "\"," +
                                "\"userTotalReceived\":\"" + "0" + "\"," +
                                "\"content\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\"," +
                                "\"sourceId\":\"" + SourceId + "\"," +
                                "\"NotificationId\":\"" + NoId + "\"," +
                                "\"ImageUrl\":\"" + ImageURL + "\"," +
                                "\"ReferenceType\":\"" + ReferenceType + "\"," +
                                "\"ReferenceId\":\"" + ReferenceId + "\"," +
                                "\"CheckOutType\":\"" + CheckOutType + "\"," +
                                "\"quantityReservationRenewalCount\":\"" + QuantityReservationRenewalCount + "\"," +
                                "\"notification\": {\"sound\":\"" + "default" + "\"," +
                                          "\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"}" +
                                "\"userTotalReceivedWaiting\":\"" + "0" + "\"}}";


                            /*{
                                string postDataContentType = "application/json; charset=utf-8";
                                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FCMClass.ValidateServerCertificate);
                                HttpWebRequest Request;
                                Stream dataStream;
                                byte[] byteArray;


                                //  MESSAGE CONTENT
                                //byteArray = Encoding.UTF8.GetBytes(onesignal_response);
                                byteArray = Encoding.UTF8.GetBytes(fcm_responseMobile);

                                //
                                //  CREATE REQUEST
                                //Request = (HttpWebRequest)WebRequest.Create("https://onesignal.com/api/v1/notifications");
                                Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                                Request.Method = "POST";
                                Request.KeepAlive = true;
                                Request.ContentType = postDataContentType;
                                Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                                Request.ContentLength = byteArray.Length;

                                dataStream = Request.GetRequestStream();
                                dataStream.Write(byteArray, 0, byteArray.Length);
                                dataStream.Close();

                                string error;
                                //
                                //  SEND MESSAGE
                                try
                                {
                                    WebResponse Response = Request.GetResponse();
                                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                                    if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                                    {
                                        error = "Unauthorized - need new token";
                                    }
                                    else if (!ResponseCode.Equals(HttpStatusCode.OK))
                                    {
                                        error = "Response from web service isn't OK";
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
                            }*/

                            {
                                string postDataContentType = "application/json; charset=utf-8";
                                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FCMClass.ValidateServerCertificate);
                                HttpWebRequest Request;
                                Stream dataStream;
                                byte[] byteArray;


                                //  MESSAGE CONTENT
                                //byteArray = Encoding.UTF8.GetBytes(onesignal_response);
                                byteArray = Encoding.UTF8.GetBytes(fcm_response);

                                //
                                //  CREATE REQUEST
                                //Request = (HttpWebRequest)WebRequest.Create("https://onesignal.com/api/v1/notifications");
                                Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                                Request.Method = "POST";
                                Request.KeepAlive = true;
                                Request.ContentType = postDataContentType;
                                Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                                Request.ContentLength = byteArray.Length;

                                dataStream = Request.GetRequestStream();
                                dataStream.Write(byteArray, 0, byteArray.Length);
                                dataStream.Close();

                                string error;
                                //
                                //  SEND MESSAGE
                                try
                                {
                                    WebResponse Response = Request.GetResponse();
                                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                                    if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                                    {
                                        error = "Unauthorized - need new token";
                                    }
                                    else if (!ResponseCode.Equals(HttpStatusCode.OK))
                                    {
                                        error = "Response from web service isn't OK";
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
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Errors.LogError(1, e.Message, e.StackTrace, "1.0.3", "API", "SendNotification", e.Source, "");
            }

            return "Admins: " + AdminReceiversPlayerIds + " ****** Not Admins Web: " + ReceiversWebPlayerIds + " ****** Not Admins Mobile:" + ReceiversMobilePlayerIds + " ****** response :" + onesignal_responseLine;
        }        

        public string DeliverySendNotification(List<int> Receivers, string NotificationTitle, string NotificationContent, string NotificationType, string Platform, string SourceId, string QuantityReservationRenewalCount, string NotificationId)
        {
            string onesignal_responseLine = "";
            string ReceiversMobilePlayerIds = "";
            string NoId = "";

            if ((NotificationId == "") || (NotificationId == null))
                NoId = "0";
            else
                NoId = NotificationId;

            try
            {
                //Get Receivers Tokens
                using (SqlConnection conn = ConnectionClass.DataConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "Admin_GetDeliveryUsersFCMTokens";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    List<SqlParameter> Params = new List<SqlParameter>()
                    { };

                    if (Receivers.Count > 0)
                    {

                        //receivers
                        DataTable users;
                        using (users = new DataTable())
                        {
                            users.Columns.Add("Item", typeof(string));
                            foreach (int x in Receivers)
                                users.Rows.Add(x);
                        }
                        var pList = new SqlParameter("@DeliveryUsersIds", System.Data.SqlDbType.Structured);
                        pList.Value = users;
                        Params.Add(pList);
                    }

                    cmd.Parameters.AddRange(Params.ToArray());

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                        List<DeliveryUserFCMTokenClass> ReceiversMobileTokens = new List<DeliveryUserFCMTokenClass>();
                        DeliveryUserFCMTokenClass receiverToken;

                        while (reader.Read())
                        {
                            receiverToken = new DeliveryUserFCMTokenClass().PopulateDeliveryUserFCMToken(fieldNames, reader);                            
                            if (receiverToken != null)
                            {

                                ReceiversMobileTokens.Add(receiverToken);
                                ReceiversMobilePlayerIds = ReceiversMobilePlayerIds + "\"" + receiverToken.DeliveryUserFCMToken + "\"" + ",";
                            }
                        }

                        reader.Close();

                        if (ReceiversMobilePlayerIds.EndsWith(","))
                            ReceiversMobilePlayerIds = ReceiversMobilePlayerIds.Substring(0, ReceiversMobilePlayerIds.Length - 1);

                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.Connection = conn;
                        //Add Notifications To History
                        cmd2.CommandText = "Admin_InsertDeliveryNotification";
                        cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                        Params = new List<SqlParameter>();
                        DataTable users;
                        using (users = new DataTable())
                        {
                            users.Columns.Add("Item", typeof(string));
                            foreach (int x in Receivers)
                                users.Rows.Add(x);
                        }
                        var xList = new SqlParameter("@DeliveryUserIds", System.Data.SqlDbType.Structured);
                        xList.Value = users;
                        Params.Add(xList);
                        Params.Add(new SqlParameter("NotificationTypeId", 401));
                        Params.Add(new SqlParameter("NotificationTitle", NotificationTitle));
                        Params.Add(new SqlParameter("NotificationContent", NotificationContent));
                        Params.Add(new SqlParameter("NotificationReferenceId", 0));
                        cmd2.Parameters.AddRange(Params.ToArray());
                        cmd2.ExecuteReader();

                        //Caligra Client
                        /*
                        string OneSignal_app_id = "b3cc9c10-8c6d-4b80-995e-46c42cc0cb80";
                        string Onesignal_apiKey = "Basic NzM2ZTNiMmItMzRjMi00NmVlLWE3ZDEtOTM3NzgwZWIzMWNk";

                        //Calligra Admin
                        string AdminOneSignal_app_id = "9bec9096-d6f3-4ebc-9a95-c8f6844c1a47";
                        string AdminOnesignal_apiKey = "Basic NWZkMmI5ZjItMjFhNi00OGQ5LWFjNWYtOTRlMmRmYTI0NDFk";
                        */

                        /*
                        string OneSignal_app_id = "8878168d-57b5-4fce-80bf-8a40361a7041";
                        string Onesignal_apiKey = "Basic NjBlNmIzNGYtMWNlMC00NWEwLWE3MjYtZTc3MDIyMGExMTRj";

                        string AdminOneSignal_app_id = "d7e990d6-4206-44bc-83cc-cb6f3636dd49";
                        string AdminOnesignal_apiKey = "Basic OTA3ZDQ4YTYtNWJiZS00ZmYxLWI2ZTEtOTVkNmJlN2ZmZGM2";
                        */


                        //DevelopiTech
                        string FCMServerKey = "AAAA0CYRy60:APA91bFOJky-izkt-10AVI13ModOa4_hacCtbQnbYIf6CHpyJbuGHLc8y9ZMlZuNAxt5kOrRT4CSxAfrzIpLLRzrsykmi6V8kDUxUaDnP7-fj6ZHgXJBXs8XQYN2KIhA4RVHcq-Op79V";
                        //Matjar Server Key

                        //string FCMServerKey = "AAAAbpZXL2o:APA91bGg7DHjZrGqNktB-aO3EWBppdBBFw3jMjTWzQkb45VrVimgmfQJQfTGOIxthanKRfumNVj8gu_hFW9hsH9WiJUNAA4Fp6_wjyJVhHozgb5VxQI3xhnrxIzlp3f53ONI_vur_YFN";
                        //string SenderId = "474968698730";

                        //Machlah ServerKey
                        //string FCMServerKey = "AAAAe4p95K0:APA91bGsFqCoIFjym02YeoEuW67DftSaNXSEnZgBYaXDFuUeleuVZkGNwwI2yqhKjtMyPKJXN2hM0tkqRxtbdFt-7mxrJEuFfK9ptOhWZAp5gdUbvoRA125hO544sDQzPmH86CaeqNCe";

                        //Tresbelle
                        //string FCMServerKey = "AAAAlLHd8MI:APA91bE2H6pLOsoAeLl3ToYMAp4qDKNNkC69WfSEfu59wo1Wsl15OhKXtoGIW-caU1DgEfUUJBGZ9NfQO1vSyIIHcAJp4RU36sdl3bW_j0PYcA9SCDj9CBxMtGyn4Epe7FE1vKO_wAp4";

                        /*
                        if (Platform.ToLower() == "web_test") // Those Config For LocalHost (EndUser + Control Panel)
                        {
                            OneSignal_app_id = "061094e3-88d8-409b-9310-a48d311abb21";
                            Onesignal_apiKey = "Basic MDU1ZGMxYzktZDUyYy00MjkwLThlYzAtZGU0NGJmYjU5NmJh";
                            AdminOneSignal_app_id = OneSignal_app_id;
                            AdminOnesignal_apiKey = Onesignal_apiKey;
                        }
                        */


                        if (ReceiversMobilePlayerIds.Trim() != "")
                        {


                            string fcm_response = "{ \"registration_ids\": [ " + ReceiversMobilePlayerIds + " ], " +
                              "\"priority\":\"" + "high" + "\"" +
                              "\"content_available\":" + "true" + "," +    //with this ios won't receive notification (this is for data messages)
                                                                           //"\"notification\": {\"sound\":\"" + "default" + "\"," + "\"click_action\":\"" + "MainActivity" + "\"," +
                                                                           //"\"body\":\"" + NotificationContent + "\",\"title\":\"" + NotificationTitle + "\"}" +

                                "\"data\": {\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"," +
                                "\"Type\":\"" + NotificationType + "\"," +
                                "\"userPointsBalance\":\"" + "0" + "\"," +
                                "\"userTotalSent\":\"" + "0" + "\"," +
                                "\"userTotalReceived\":\"" + "0" + "\"," +
                                "\"content\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\"," +
                                "\"sourceId\":\"" + SourceId + "\"," +
                                "\"NotificationId\":\"" + NoId + "\"," +
                                "\"quantityReservationRenewalCount\":\"" + QuantityReservationRenewalCount + "\"," +
                                "\"notification\": {\"sound\":\"" + "default" + "\"," +
                                          "\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"}" +
                                "\"userTotalReceivedWaiting\":\"" + "0" + "\"}}";


                            /*string onesignal_response = "{"
                           + "\"app_id\": \"" + OneSignal_app_id + "\","
                           + "\"data\": {\"Type\": \"" + NotificationType +
                                 "\",\"userPointsBalance\": \"" + "0" +
                                 "\",\"userTotalSent\": \"" + "0" +
                                 "\",\"userTotalReceived\": \"" + "0" +
                                 "\",\"content\": \"" + NotificationContent +
                                 "\",\"userTotalReceivedWaiting\": \"" + "0" +
                                 "\",\"sourceId\": \"" + SourceId +
                                 "\",\"NotificationTitle\": \"" + NotificationTitle +
                                 "\",\"NotificationId\": \"" + "0" +
                                 "\",\"quantityReservationRenewalCount\": \"" + "0" + "\"},"
                           + "\"contents\": {\"en\": \"" + NotificationContent + "\"},"
                           + "\"include_player_ids\": [" + ReceiversPlayerIds + "]}";
                           */

                            {
                                string postDataContentType = "application/json; charset=utf-8";
                                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FCMClass.ValidateServerCertificate);
                                HttpWebRequest Request;
                                Stream dataStream;
                                byte[] byteArray;


                                //  MESSAGE CONTENT
                                //byteArray = Encoding.UTF8.GetBytes(onesignal_response);
                                byteArray = Encoding.UTF8.GetBytes(fcm_response);

                                //
                                // CREATE REQUEST
                                //Request = (HttpWebRequest)WebRequest.Create("https://onesignal.com/api/v1/notifications");
                                Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                                Request.Method = "POST";
                                Request.KeepAlive = true;
                                Request.ContentType = postDataContentType;
                                //Request.Headers.Add(string.Format("Authorization: key={0}", Onesignal_apiKey));
                                Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                                Request.ContentLength = byteArray.Length;

                                dataStream = Request.GetRequestStream();
                                dataStream.Write(byteArray, 0, byteArray.Length);
                                dataStream.Close();

                                string error;
                                //
                                //  SEND MESSAGE
                                try
                                {
                                    WebResponse Response = Request.GetResponse();
                                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                                    if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                                    {
                                        error = "Unauthorized - need new token";
                                    }
                                    else if (!ResponseCode.Equals(HttpStatusCode.OK))
                                    {
                                        error = "Response from web service isn't OK";
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
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Errors.LogError(1, e.Message, e.StackTrace, "1.0.3", "API", "DeliverySendNotification", e.Source, "");
            }

            return " Not Admins Mobile:" + ReceiversMobilePlayerIds + " ****** response :" + onesignal_responseLine;
        }

        public string ICBTNotification(List<int> Receivers, string FullName, string SenderId, string MessageId)
        {
            string responseLine = "";
            string ReceiversWebPlayerIds = "";
            try
            {
                //Get Receivers Tokens
                using (SqlConnection conn = ConnectionClass.DataConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "Admin_GetUsersFCMTokens";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    List<SqlParameter> Params = new List<SqlParameter>()
                    { };

                    if (Receivers.Count > 0)
                    {

                        //receivers
                        DataTable users;
                        using (users = new DataTable())
                        {
                            users.Columns.Add("Item", typeof(string));
                            foreach (int x in Receivers)
                                users.Rows.Add(x);
                        }
                        var pList = new SqlParameter("@UsersIds", System.Data.SqlDbType.Structured);
                        pList.Value = users;
                        Params.Add(pList);
                    }

                    cmd.Parameters.AddRange(Params.ToArray());

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                        List<FCMTokenClass> ReceiversWebTokens = new List<FCMTokenClass>();

                        FCMTokenClass receiverToken;

                        while (reader.Read())
                        {
                            receiverToken = new FCMTokenClass().PopulateClass(fieldNames, reader);
                            if (receiverToken != null)
                            {
                                ReceiversWebTokens.Add(receiverToken);
                                ReceiversWebPlayerIds = ReceiversWebPlayerIds + "\"" + receiverToken.FCMToken + "\"" + ",";
                            }
                        }

                        if (ReceiversWebPlayerIds.EndsWith(","))
                            ReceiversWebPlayerIds = ReceiversWebPlayerIds.Substring(0, ReceiversWebPlayerIds.Length - 1);
                       
                        //ICBT Server Key   
                        string FCMServerKey = "AAAAb1K8Jpc:APA91bFax3xoUhq1Os9270ftKCdaVENQSrZmIeQSSRcpU8IC271uKJmxNqZPvai0bqTjWFiQOeHTxqVQk2sgYZsJd1h-XiWzJpVSbFUnWx3rOGmKiMPRw6XnhBKGBi7NetjqO3fknhd4";



                        if (ReceiversWebPlayerIds.Trim() != "")
                        {

                            string NotificationTitle = "Sky Education";
                            string NotificationContent = FullName + " has sent you a message";

                            string fcm_response = "{ \"registration_ids\": [ " + ReceiversWebPlayerIds + " ], " +
                              "\"priority\":\"" + "high" + "\"" +
                                "\"data\": {\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"," +
                                "\"content\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\"," +
                                "\"MessageId\":\"" + MessageId + "\"," +
                                "\"notification\": {\"sound\":\"" + "default" + "\"," +
                                          "\"body\":\"" + NotificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + NotificationTitle.Replace(Environment.NewLine, " ") + "\"}" +
                                "\"SenderId\":\"" + SenderId + "\"}}";


                            {
                                string postDataContentType = "application/json; charset=utf-8";
                                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FCMClass.ValidateServerCertificate);
                                HttpWebRequest Request;
                                Stream dataStream;
                                byte[] byteArray;


                                //  MESSAGE CONTENT
                                byteArray = Encoding.UTF8.GetBytes(fcm_response);

                                //
                                // CREATE REQUEST
                                Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                                Request.Method = "POST";
                                Request.KeepAlive = true;
                                Request.ContentType = postDataContentType;
                                Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                                Request.ContentLength = byteArray.Length;

                                dataStream = Request.GetRequestStream();
                                dataStream.Write(byteArray, 0, byteArray.Length);
                                dataStream.Close();

                                string error;
                                //
                                //  SEND MESSAGE
                                try
                                {
                                    WebResponse Response = Request.GetResponse();
                                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                                    if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                                    {
                                        error = "Unauthorized - need new token";
                                    }
                                    else if (!ResponseCode.Equals(HttpStatusCode.OK))
                                    {
                                        error = "Response from web service isn't OK";
                                    }

                                    StreamReader Reader = new StreamReader(Response.GetResponseStream());
                                    responseLine = Reader.ReadToEnd();
                                    Reader.Close();
                                }
                                catch (Exception e)
                                {
                                    Errors.LogError(-1, e.Message, e.StackTrace, "1.0.3", "API", "SendFCMNotification", e.Source, "");
                                    responseLine = e.Message;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Errors.LogError(1, e.Message, e.StackTrace, "1.0.3", "API", "SendNotification", e.Source, "");
            }

            return "Web: " + ReceiversWebPlayerIds + " ****** response :" + responseLine;
        }        

    }
}
