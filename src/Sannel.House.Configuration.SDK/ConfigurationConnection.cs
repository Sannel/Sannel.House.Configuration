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
					var key = result.Message.Keys.ToArray();
					return result.Message;
				}

				return new ValueSet();
			}).AsAsyncOperation();
		}	

		public void Dispose()
		{
			connection?.Dispose();
		}
	}
}
