using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Sannel.House.Configuration.Common;
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

			new SystemSettings();

			AppCenter.Start(SystemSettings.Current.ConfigurationAppSecret, typeof(Analytics));
			Analytics.TrackEvent("Background Task Started");

			// Retrieve the app service connection and set up a listener for incoming app service requests.
			var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
			appServiceConnection = details.AppServiceConnection;
			appServiceConnection.RequestReceived += onRequestReceived;
		}

		private async void onRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
		{
			var messageDeferral = args.GetDeferral();

			Analytics.TrackEvent("onRequestRecived");

			var message = args.Request.Message;
			var returnData = new ValueSet();

			try
			{
				if (message.ContainsKey("Command") && message["Command"] is string command)
				{
					switch (command)
					{
						case "GetAllSettings":
							var value = SystemSettings.Current.GetAllSettings().GetValue();
							returnData["AllSettings"] = value;
							break;
						case "UpdateSetting":
							if (message.ContainsKey("Setting"))
							{
								updateSetting(message["Setting"] as string);
							}
							break;
						default:
							Analytics.TrackEvent("Unknown Command", new Dictionary<string, string>()
							{
								{"Command", command }
							});
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
				else
				{
					Analytics.TrackEvent("NonValid Message");
				}

			}
			catch(Exception ex)
			{
				returnData["Exception"] = ex.ToString();
				ex.TrackEvent("Exception Processing request");
			}

			await args.Request.SendResponseAsync(returnData);
			messageDeferral.Complete();
		}

		private Setting updateSetting(string json)
		{
			if (json != null)
			{
				var u = Setting.Load(json);
				if (u != null)
				{
					return SystemSettings.Current.UpdateSetting(u);
				}
			}

			return null;
		}

		private void onCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			Analytics.TrackEvent("onCanceled", new Dictionary<string, string>()
			{
				{"reason", reason.ToString() }
			});
	
			deferral?.Complete();
		}
	}
}
