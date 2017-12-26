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

		public event TypedEventHandler<AppServiceConnection, AppServiceClosedEventArgs> ServiceClosed;

		public ConfigurationConnection()
		{
			connection = new AppServiceConnection();
			connection.PackageFamilyName = "Sannel.House.Configuration_s8t1p3zkxq1s8";
			connection.AppServiceName = "com.sannel.house.configuration";
			connection.ServiceClosed += connection_ServiceClosed;
		}

		private void connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
		{
			ServiceClosed?.Invoke(sender, args);
		}

		public IAsyncOperation<bool> ConnectAsync()
		{
			return Task.Run(async () =>
			{
				var result = await connection.OpenAsync();
				return result == AppServiceConnectionStatus.Success;
			}).AsAsyncOperation();
		}

		public IAsyncOperation<ValueSet> GetConfiguration(params string[] keys)
		{
			return Task.Run(async () =>
			{
				var set = new ValueSet();
				set["Settings"] = keys;

				var result = await connection.SendMessageAsync(set);
				if (result.Status == AppServiceResponseStatus.Success)
				{
					return result.Message;
				}

				return new ValueSet();
			}).AsAsyncOperation();
		}

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

		public void Dispose()
		{
			connection?.Dispose();
		}
	}
}
