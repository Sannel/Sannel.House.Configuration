using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Configuration.BackgroundTasks
{
	public interface ISystemSettings
	{
		string ServerApiUrl
		{
			get;
			set;
		}

		string ServerUsername
		{
			get;
			set;
		}

		string ServerPassword
		{
			get;
			set;
		}

		int SensorsPort
		{
			get;
			set;
		}
	}
}
