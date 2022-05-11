using SMSApi.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsSendConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string token = config.AppSettings.Settings["AuthToken"].Value;
            string tel = ConfigurationManager.AppSettings["ReceiverTelNumber"];
            Console.WriteLine(token);
            
        }
    }
}
