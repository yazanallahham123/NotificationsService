using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NotificationsService.Classes
{
   [DataContract]
   public class ResultClass<T>
   {
      [DataMember(Order = 1)]
      public int Code { get; set; }
      [DataMember(Order = 2)]
      public string Message { get; set; }
      [DataMember(Order = 3)]
      public T Result { get; set; }

   }
}