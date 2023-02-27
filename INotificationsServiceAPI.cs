using NotificationsService.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NotificationsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INotificationsServiceAPI" in both code and config file together.
    [ServiceContract]
    public interface INotificationsServiceAPI
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "JSON/FakeCall")]
        string FakeCall();


        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "JSON/SendNotification")]
        string SendNotification(List<int> Receivers, string NotificationTitle, string NotificationContent, string NotificationType, string Platform, string SourceId,
            string QuantityReservationRenewalCount, string NotificationId, string ImageURL, string ReferenceType, string ReferenceId, string CheckOutType);


        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "JSON/SendNotifications")]
        string SendNotifications(List<string> receivers, string manifestNumber, string stationName, string manifestGUID,
        string stationGUID, string title, string content, bool isLastStation);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "JSON/DeliverySendNotification")]
        string DeliverySendNotification(List<int> Receivers, string NotificationTitle, string NotificationContent, string NotificationType, string Platform, string SourceId, string QuantityReservationRenewalCount, string NotificationId);

        [OperationContract]
        [WebInvoke(Method = "POST",
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "JSON/ICBTNotification")]
        string ICBTNotification(List<int> Receivers, string FullName, string SenderId, string MessageId);
    }


}
