using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Configuration.Common
{
	public sealed class Setting
	{
		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>
		/// The key.
		/// </value>
		public string Key { get; set; }
		/// <summary>
		/// Gets or sets the display name.
		/// </summary>
		/// <value>
		/// The display name.
		/// </value>
		public string DisplayName { get; set; }
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public object Value { get; set; }
		/// <summary>
		/// Gets or sets the type of the setting.
		/// </summary>
		/// <value>
		/// The type of the setting.
		/// </value>
		public SettingType SettingType { get; set; }

		/// <summary>
		/// Gets the json.
		/// </summary>
		/// <returns></returns>
		public string GetJson()
		{
			try
			{
				return JsonConvert.SerializeObject(this);
			}
			catch (Exception ex)
			{
				ex.TrackEvent("Exception Serializing this Settings.GetJson");
			}
			return string.Empty;
		}

		/// <summary>
		/// Loads the specified Setting from the provided json.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static Setting Load(string json)
		{
			try
			{
				return JsonConvert.DeserializeObject<Setting>(json);
			}
			catch (Exception ex)
			{
				ex.TrackEvent("Exception Deserializing to this Settings.Load");
			}
			return null;
		}
	}
}
