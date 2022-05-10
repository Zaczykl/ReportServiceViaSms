using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ReportServiceViaSms
{
    public partial class ReportService : ServiceBase
    {
        private const int msInMinute=60000;
        private readonly int _intervalInMinutes;
        private Timer _timer=new Timer(msInMinute);
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        SmsSender _smsSender = new SmsSender(new SmsParams 
        { 
          AuthToken= ConfigurationManager.AppSettings["AuthToken"],
          ReceiverTelNumber= ConfigurationManager.AppSettings["ReceiverTelNumber"],
          SenderName = ConfigurationManager.AppSettings["SenderName"],
          TextMessage ="Test message"
        });
        

        public ReportService()
        {
            InitializeComponent();
            _intervalInMinutes = 1 * msInMinute;
            _timer = new Timer(_intervalInMinutes);
        }

        protected override void OnStart(string[] args)
        {            
            _timer.Elapsed += DoWork;
            _timer.Start();
            Logger.Info("Service started...");
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            Logger.Info("Wysyłam SMS.");
            _smsSender.Send();
            Logger.Info("Wysłano SMS");
        }

        protected override void OnStop()
        {
            Logger.Info("Service stopped.");
        }
    }
}
