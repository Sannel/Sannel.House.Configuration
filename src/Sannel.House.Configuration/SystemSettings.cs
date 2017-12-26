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
			foreach(var p in t.GetProperties())
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
					list.Add(s);
				}
			}

			return list;
		}

		public Setting UpdateSetting(Setting setting)
		{
			var t = GetType();
			var prop = t.GetProperties().FirstOrDefault(i => string.Compare(i.Name, setting.Key) == 0);
			if(prop != null)
			{
				var att = prop.GetCustomAttribute<SettingsPropertyAttribute>();
				if(att != null)
				{
					switch (att.Type)
					{
						case SettingsPropertyType.Integer:
							var v = (int)(setting.Value as long? ?? default(long));
							prop.SetMethod.Invoke(this, new object[] { v });
							setting.Value = prop.GetMethod.Invoke(this, new object[] { });
							break;
						default:
							var s = setting.Value as string ?? string.Empty;
							prop.SetMethod.Invoke(this, new object[] { s });
							setting.Value = prop.GetMethod.Invoke(this, new object[] { });
							break;
					}
				}
			}
			return setting;
		}

		[SettingsProperty("Server Api Url", SettingsPropertyType.Uri)]
		public string ServerApiUrl
		{
			get => GetValue(nameof(ServerApiUrl));
			set => CheckAndSetUri(value, "Server Api Url is invalid", nameof(ServerApiUrl));
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
