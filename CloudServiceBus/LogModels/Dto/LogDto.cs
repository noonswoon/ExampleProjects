﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LogModels.Dto
{
    [DataContract(Name = "LogDto")]
    public class LogDto
    {
        [DataMember(Name = "LogTime")]
        public DateTime LogTime { get; set; }
        [DataMember(Name = "LogLevel")]
        public string LogLevel { get; set; }
        [DataMember(Name = "LogDetail")]
        public string LogDetail { get; set; }
    }
}
