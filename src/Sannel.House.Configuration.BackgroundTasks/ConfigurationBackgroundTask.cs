using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace Sannel.House.Configuration.BackgroundTasks
{
	public sealed class ConfigurationBackgroundTask : IBackgroundTask
	{
		private BackgroundTaskDeferral deferral;
		private AppServiceConnection appServiceConnection;

		public void Run(IBackgroundTaskInstance taskInstance)
		{
			deferral = taskInstance.GetDeferral();
			taskInstance.Canceled += onCanceled;

			// Retrieve the app service connection and set up a listener for incoming app service requests.
			var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
			appServiceConnection = details.AppServiceConnection;
			appServiceConnection.RequestReceived += onRequestReceived;
		}

		private async void onRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
		{
			var messageDeferral = args.GetDeferral();

			var message = args.Request.Message;
			var returnData = new ValueSet();

			try
			{
				if (message.ContainsKey("Command") && message["Command"] is string command)
				{
					switch (command)
					{
						case "GetAllSettings":
							returnData["AllSettings"] = SystemSettings.Current.GetAllSettings().GetValue();
							break;
					}
				}
				else if (message.ContainsKey("Settings"))
				{
					if (message["Settings"] is string setting)
					{
						returnData[setting] = SystemSettings.Current[setting];
					}
					else if (message["Settings"] is string[] settings)
					{
						foreach (var set in settings)
						{
							returnData[set] = SystemSettings.Current[set];
						}
					}
				}

				await args.Request.SendResponseAsync(returnData);
			}
			catch(Exception ex)
			{
				returnData["Exception"] = ex.ToString();
				try
				{
					await args.Request.SendResponseAsync(returnData);
				}
				catch { }
			}
			finally
			{
				messageDeferral.Complete();
			}
		}

		private void onCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			deferral?.Complete();
		}
	}
}
