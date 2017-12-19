using Sannel.Configuration;
using Sannel.House.Configuration.BackgroundTasks;
using Sannel.House.Configuration.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Configuration
{
	internal class SystemSettings : SettingsBase
	{
		private static SystemSettings current;

		public static new SystemSettings Current => current ?? (current = new SystemSettings());

		public override void Initialize()
		{
			Init(this);
		}

		public object this[string key]
		{
			get
			{
				return settings.Values[key];
			}
		}

		public SettingsList GetAllSettings()
		{
			var t = GetType();
			var list = new SettingsList();
			foreach(var p in t.GetProperties().OrderBy(i => i?.Name))
			{
				var att = p.GetCustomAttribute<SettingsPropertyAttribute>();
				if(att != null)
				{
					var s = new Setting()
					{
						Key = p.Name,
						DisplayName = att.Header,
						Value = p.GetValue(this),
						SettingType = (SettingType)att.Type
					};
				}
			}

			return list;
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
			get => GetValue<int>();
			set => SetValue(value);
		}
	}
}
