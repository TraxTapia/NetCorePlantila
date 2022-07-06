using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAC.AONPocket.Web.App_Code
{
	public static class ReadConfig
	{
		public static String ReadKey(string section,string key)
		{
			string result = String.Empty;
			IConfiguration setting = ConfigHelper.GetConfig();

			IConfiguration servicesetting = setting.GetSection(section);
			result = servicesetting[key];
			return result;
		}
	}
}
