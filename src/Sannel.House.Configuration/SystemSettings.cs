using Sannel.Configuration;
using Sannel.House.Configuration.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Configuration
{
	internal class SystemSettings : SettingsBase, ISystemSettings
	{

		public override void Initialize()
		{
			Init(this);
		}
		[SettingsProperty("Server Api Url", SettingsPropertyType.Uri)]
		public string ServerApiUrl
		{
			get => GetValue();
			set => CheckAndSetUri(value, "Server Api Url is invalid");
		}

		[SettingsProperty("Server Username", SettingsPropertyType.String)]
		public string ServerUsername
		{
			get => GetValue();
			set => SetValue(value);
		}

		[SettingsProperty("Server Password", SettingsPropertyType.Password)]
		public string ServerPassword
		{
			get => GetValue();
			set => SetValue(value);
		}

		[SettingsProperty("Sensor Capture Port", SettingsPropertyType.Integer)]
		public int SensorsPort
		{
			get => GetValue(defaultValue: 8175);
			set => SetValue(value);
		}
	}
}
