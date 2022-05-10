using SMSApi.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportServiceViaSms
{
    public class SmsParams
    {
        public string AuthToken { get; set; }
        public string TextMessage { get; set; }
        public string ReceiverTelNumber { get; set; }
        public string SenderName { get; set; }
    }
}
