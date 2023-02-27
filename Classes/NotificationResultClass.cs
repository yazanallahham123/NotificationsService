using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NotificationsService.Classes
{
    public class NotificationResultClass
    {
        [DataMember(Order = 1)]
        public int ReceiverId { get; set; }
        [DataMember(Order = 2)]
        public bool Success { get; set;  }
    }
}