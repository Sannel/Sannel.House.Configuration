using Microsoft.AppCenter.Analytics;
using Sannel.House.Configuration.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Sannel.House.Configuration
{
	public sealed class ConfigurationConnection : IDisposable
	{
		private AppServiceConnection connection;

		/// <summary>
		/// Occurs when the service closes.
		/// </summary>
		public event TypedEventHandler<AppServiceConnection, AppServiceClosedEventArgs> ServiceClosed;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationConnection"/> class.
		/// </summary>
		public ConfigurationConnection()
		{
			connection = new AppServiceConnection
			{
				PackageFamilyName = "Sannel.House.Configuration_s8t1p3zkxq1s8",
				AppServiceName = "com.sannel.house.configuration"
			};
			connection.ServiceClosed += connection_ServiceClosed;
		}

		private void connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
		{ 
			ServiceClosed?.Invoke(sender, args);

			Analytics.TrackEvent("ConfigurationConnection.serviceclosed", new Dictionary<string, string>()
			{
				{"Status", args?.Status.ToString() }
			});
		}

		/// <summary>
		/// Attempt to connect to the Configuration app
		/// </summary>
		/// <returns></returns>
		public IAsyncOperation<bool> ConnectAsync()
		{
			return Task.Run(async () =>
			{
				var result = await connection.OpenAsync();
				return result == AppServiceConnectionStatus.Success;
			}).AsAsyncOperation();
		}

		/// <summary>
		/// Returns a ValueSet containing the giving configurations if they were available
		/// </summary>
		/// <param name="keys">The keys.</param>
		/// <returns></returns>
		public IAsyncOperation<ValueSet> GetConfiguration(params string[] keys)
		{
			return Task.Run(async () =>
			{
				var set = new ValueSet
				{
					["Settings"] = keys
				};

				var result = await connection.SendMessageAsync(set);
				if (result.Status == AppServiceResponseStatus.Success)
				{
					return result.Message;
				}

				return new ValueSet();
			}).AsAsyncOperation();
		}

		/// <summary>
		/// Gets a list of all settings currently configured
		/// </summary>
		/// <returns></returns>
		public IAsyncOperation<SettingsList> GetAllSettingsAsync()
		{
			return Task.Run(async () =>
			{
				var list = new SettingsList();
				var set = new ValueSet
				{
					["Command"] = "GetAllSettings"
				};

				var result = await connection.SendMessageAsync(set);
				if(result.Status == AppServiceResponseStatus.Success)
				{
					var message = result.Message;
					if(message.ContainsKey("AllSettings") && message["AllSettings"] is string value)
					{
						list.Load(value);
					}
				}

				return list;
			}).AsAsyncOperation();
		}

		/// <summary>
		/// Updates the setting asynchronous.
		/// </summary>
		/// <param name="setting">The setting.</param>
		/// <returns></returns>
		public IAsyncOperation<bool> UpdateSettingAsync(Setting setting)
		{
			return Task.Run(async () =>
			{
				var set = new ValueSet
				{
					["Command"] = "UpdateSetting",
					["Setting"] = setting.GetJson()
				};

				var result = await connection.SendMessageAsync(set);
				return result.Status == AppServiceResponseStatus.Success;
			}).AsAsyncOperation();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			connection?.Dispose();
		}
	}
}
