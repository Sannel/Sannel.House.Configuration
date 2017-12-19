using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Configuration.Common
{
	public sealed class SettingsList : IReadOnlyList<Setting>
	{
		private IList<Setting> list = new List<Setting>();

		public int Count => throw new NotImplementedException();

		public Setting this[int index] => throw new NotImplementedException();

		public void Load(string data)
		{
			try
			{
				var l = JsonConvert.DeserializeObject<List<Setting>>(data);
				l.Clear();
				l.AddRange(list);
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
