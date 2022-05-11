using Cipher;
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
        StringCipher _cipher = new StringCipher("8508AD4A - B3CD - 45F4 - 88A8 - B2DB6A180FDC");
        GenerateSmsText _generateSmsText = new GenerateSmsText();
        ErrorRepository _errorRepository=new ErrorRepository();
        Sms _smsSender;
        private const int MILISECONDS_IN_MINUTE=60000;
        private readonly int _intervalInMinutes;
        private Timer _timer=new Timer(MILISECONDS_IN_MINUTE);
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private string NOT_ENCRYPTED_PASSWORD_PREFIX = "encrypt:";

        public ReportService()
        {
            InitializeComponent();
            _intervalInMinutes = 1;
            _timer = new Timer(_intervalInMinutes * MILISECONDS_IN_MINUTE);            
            _smsSender = new Sms(new SmsParams
            {                
                AuthToken = DecryptToken(),
                ReceiverTelNumber = ConfigurationManager.AppSettings["ReceiverTelNumber"],
                SenderName = ConfigurationManager.AppSettings["SenderName"]
            });            
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
        private string DecryptToken()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["AuthToken"];

            if (encryptedPassword.StartsWith(NOT_ENCRYPTED_PASSWORD_PREFIX))
            {
                encryptedPassword = _cipher.Encrypt(encryptedPassword.Replace(NOT_ENCRYPTED_PASSWORD_PREFIX, ""));
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.AppSettings.Settings["AuthToken"].Value = encryptedPassword;
                configFile.Save();
            }
            return _cipher.Decrypt(encryptedPassword);
        }
    }
}
