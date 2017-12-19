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

		public ConfigurationConnection()
		{
			connection = new AppServiceConnection();
			connection.PackageFamilyName = "Sannel.House.Configuration_s8t1p3zkxq1s8";
			connection.AppServiceName = "com.sannel.house.configuration";
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
					if(result.Message["AllSettings"] is string value)
					{
						list.Load(value);
					}
				}

				return list;
			}).AsAsyncOperation();
		}

		public void Dispose()
		{
			connection?.Dispose();
		}
	}
}
