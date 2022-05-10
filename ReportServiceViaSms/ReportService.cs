using ReportServiceCore;
using SmsSender;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace ReportServiceViaSms
{
    public partial class ReportService : ServiceBase
    {
        GenerateSmsText _generateSmsText = new GenerateSmsText();
        ErrorRepository _errorRepository=new ErrorRepository();
        private const int msInMinute=60000;
        private readonly int _intervalInMinutes;
        private Timer _timer=new Timer(msInMinute);
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        Sms _smsSender = new Sms(new SmsParams 
        { 
          AuthToken= ConfigurationManager.AppSettings["AuthToken"],
          ReceiverTelNumber= ConfigurationManager.AppSettings["ReceiverTelNumber"],
          SenderName = ConfigurationManager.AppSettings["SenderName"]          
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
            SendError();
            Logger.Info("Wysłano SMS");
        }

        protected override void OnStop()
        {
            Logger.Info("Service stopped.");
        }
        private void SendError()
        {
            if (!_errorRepository.GetErrors().Any())
                return;

            string errorMessage = _generateSmsText.GenerateTextFromErrors(_errorRepository.GetErrors(), _intervalInMinutes);
            _smsSender.Send(errorMessage);
        }
    }
}
