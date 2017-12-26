using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Configuration.Common
{
	public sealed class SettingsList : IReadOnlyList<Setting>
	{
		private List<Setting> list = new List<Setting>();

		public int Count 
			=> list.Count;

		public Setting this[int index] => list[index];

		public void Add(Setting setting)
		{
			list.Add(setting);
		}

		public void Load(string data)
		{
			try
			{
				var l = JsonConvert.DeserializeObject<List<Setting>>(data);
				list.Clear();
				list.AddRange(l);
			}
			catch { }
		}

		public string GetValue()
		{
			return JsonConvert.SerializeObject(this);
		}

		public IEnumerator<Setting> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}
}
