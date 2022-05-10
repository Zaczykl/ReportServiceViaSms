using SMSApi.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsSendConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
			try
			{
				IClient client = new ClientOAuth("gBlKc2j2jwGNHZplbpGY3o65XibqHrNKJdbV2hwQ");

				var smsApi = new SMSFactory(client);
				// for SMSAPI.com clients:
				// var smsApi = new SMSApi.Api.SMSFactory(client, ProxyAddress.SmsApiCom);

				var result =
					smsApi.ActionSend()
						.SetText("test message")
						.SetTo("517213438")
						.SetSender("Test") //Sender name
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
				System.Console.WriteLine(e.Message);
			}
			catch (SMSApi.Api.HostException e)
			{
				/* 
				 * Server errors
				 * SMSApi.Api.HostException.E_JSON_DECODE - problem with parsing data
				 */
				System.Console.WriteLine(e.Message);
			}
			catch (SMSApi.Api.ProxyException e)
			{
				// communication problem between client and sever
				System.Console.WriteLine(e.Message);
			}
		}
    }
}
