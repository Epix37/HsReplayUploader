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
				Log.Debug("LogConfig", "log.config not found");
				using(var sr = new StreamWriter(logConfig))
					sr.Write(Constants.LogConfigContent);
				Log.Debug("LogConfig", "created log.config");
			}
			else
				Log.Debug("LogConfig", "found log.config");

		}
	}
}