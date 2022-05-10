using SMSApi.Api;
using System;

namespace SmsSender
{
    public class Sms
    {       
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private IClient _client;
        private string _authToken;
        private string _receiverTelNumber;
        private string _senderName;
        public Sms(SmsParams smsParams)
        {
            try
            {
                _authToken= smsParams.AuthToken;
                _receiverTelNumber= smsParams.ReceiverTelNumber;
                _senderName= smsParams.SenderName;
                _client = new ClientOAuth(_authToken);
            }
            catch (SMSApi.Api.Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        public void Send(string textMessage)
        {
            try
            {
                var smsApi = new SMSFactory(_client);

                var result =
                    smsApi.ActionSend()
                        .SetText(textMessage)
                        .SetTo(_receiverTelNumber)
                        .SetSender(_senderName) //Sender name
                        .Execute();

                Console.WriteLine("Send: " + result.Count);

                string[] ids = new string[result.Count];

                for (int i = 0, l = 0; i < result.List.Count; i++)
                {
                    if (!result.List[i].isError())
                    {
                        if (!result.List[i].isFinal())
                        {
                            ids[l] = result.List[i].ID;
                            l++;
                        }
                    }
                }

                Console.WriteLine("Get:");
                result =
                    smsApi.ActionGet()
                        .Ids(ids)
                        .Execute();

                foreach (var status in result.List)
                {
                    Console.WriteLine("ID: " + status.ID + " NUmber: " + status.Number + " Points:" + status.Points + " Status:" + status.Status + " IDx: " + status.IDx);
                }
            }
            catch (ActionException e)
            {
                /**
				 * Action error
				 */
                Logger.Error(e.Message);
                Console.WriteLine(e.Message);
            }
            catch (ClientException e)
            {
                /**
				 * Error codes (list available in smsapi docs). Example:
				 * 101 	Invalid authorization info
				 * 102 	Invalid username or password
				 * 103 	Insufficient credits on Your account
				 * 104 	No such template
				 * 105 	Wrong IP address (for IP filter turned on)
				 * 110	Action not allowed for your account
				 */
                Logger.Error(e.Message);
                System.Console.WriteLine(e.Message);
            }
            catch (HostException e)
            {
                /* 
				 * Server errors
				 * SMSApi.Api.HostException.E_JSON_DECODE - problem with parsing data
				 */
                Logger.Error(e.Message);
            }
            catch (ProxyException e)
            {
                // communication problem between client and sever
                Logger.Error(e.Message);
            }
        }
    }
}
