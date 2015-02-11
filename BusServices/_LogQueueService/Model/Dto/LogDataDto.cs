using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogQueueService.Model.Dto
{
    public class LogDataDto
    {
        public DateTime LogTime { get; set; }
        public string LogLevel { get; set; }
        public string LogDetail { get; set; }
    }
}
