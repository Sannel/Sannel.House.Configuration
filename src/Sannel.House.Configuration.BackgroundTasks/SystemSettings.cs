using Sannel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Configuration.BackgroundTasks
{
	internal class SystemSettings : SettingsBase, ISystemSettings
	{
		public override void Initialize()
		{
			Init(this);
		}
		public string ServerApiUrl
		{
			get => GetValue();
			set => SetValue(value);
		}

		public string ServerUsername
		{
			get => GetValue();
			set => SetValue(value);
		}

		public string ServerPassword
		{
			get => GetValue();
			set => SetValue(value);
		}

		public int SensorsPort
		{
			get => GetValue<int>();
			set => SetValue(value);
		}
	}
}
