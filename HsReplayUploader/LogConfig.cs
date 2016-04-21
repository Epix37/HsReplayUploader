using System.IO;
using Android.Util;

namespace HsReplayUploader
{
	public class LogConfig
	{
		public static void Setup()
		{
			var logConfig = Path.Combine(Constants.Dir, "log.config");
			if(!File.Exists(logConfig))
			{
				MainActivity.Instance.Log("log.config not found");
				using(var sr = new StreamWriter(logConfig))
					sr.Write(Constants.LogConfigContent);
				MainActivity.Instance.Log("Created log.config");
			}
			else
				MainActivity.Instance.Log("Found log.config");
		}
	}
}