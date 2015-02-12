using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace LogAPI.Models.Dto
{

    //[DataContract(Name = "LogDataDto")]
    public class LogDataDto
    {

        //[DataMember(Name = "LogTime")]
        [System.Xml.Serialization.XmlElement("LogTime")]
        public DateTime LogTime { get; set; }
        //[DataMember(Name = "LogLevel")]
        [System.Xml.Serialization.XmlElement("LogLevel")]
        public string LogLevel { get; set; }
        //[DataMember(Name = "LogDetail")]
        [System.Xml.Serialization.XmlElement("LogDetail")]
        public string LogDetail { get; set; }
    }
}