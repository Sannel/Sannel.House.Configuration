using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Configuration.Common
{
	public static class Extensions
	{
		/// <summary>
		/// Tracks the event.
		/// </summary>
		/// <param name="ex">The ex.</param>
		/// <param name="message">The message.</param>
		/// <param name="extra">The extra.</param>
		public static async void TrackEvent(this Exception ex, string message, params KeyValuePair<string,string>[] extras)
		{
			if (await Analytics.IsEnabledAsync())
			{
				var dict = new Dictionary<string, string>();

				if (ex != null)
				{
					dict["Exception Message"] = ex.Message;
					dict["Exception Stack"] = ex.StackTrace;

					if (ex.InnerException != null)
					{
						var i = ex.InnerException;
						dict["InnerException Message"] = i.Message;
						dict["InnerException Stack"] = i.StackTrace;
					}
				}


				if (extras != null)
				{
					foreach (var k in extras)
					{
						dict[k.Key] = k.Value;
					}
				}

				Analytics.TrackEvent(message, dict);
			}
		}
	}
}
