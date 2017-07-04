using Sannel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Configuration.BackgroundTasks
{
	internal class SystemSettings : SettingsBase
	{
		private static SystemSettings current;

		public static new SystemSettings Current => current ?? (current = new SystemSettings());

		public override void Initialize()
		{
			Init(this);

			var keys = settings.Values.Keys.ToList();

			var q = keys;
		}

		public object this[string key]
		{
			get
			{
				return settings.Values[key];
			}
		}
	}
}
