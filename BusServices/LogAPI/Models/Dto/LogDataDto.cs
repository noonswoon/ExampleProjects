using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace LogAPI.Models.Dto
{

    [DataContract]
    public class LogDataDto
    {
        [DataMember]
        public DateTime LogTime { get; set; }
        [DataMember]
        public string LogLevel { get; set; }
        [DataMember]
        public string LogDetail { get; set; }
    }
}