using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Configuration.Common
{
	public sealed class Setting
	{
		public string Key { get; set; }
		public string DisplayName { get; set; }
		public object Value { get; set; }
		public SettingType SettingType { get; set; }
	}
}
