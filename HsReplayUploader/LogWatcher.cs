#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using static HsReplayUploader.Constants;

#endregion

namespace HsReplayUploader
{
	[Service]
	public class LogWatcher : Service
	{
		public List<string> LastGame { get; } = new List<string>();
		public List<string> CurrentGame { get; } = new List<string>();
		public override IBinder OnBind(Intent intent) => null;

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			Log.Debug("LogWatcher", "LogWatcher started");
			WatchPowerLog();
			return StartCommandResult.Sticky;
		}

		public void WatchPowerLog()
		{
			var powerLog = Path.Combine(Dir, "Logs/Power.log");
			var t = new Thread(() =>
			{
				var gameStart = LogHelper.FindEntryPoint(GameStart, powerLog, true);
				var gameEnd = LogHelper.FindEntryPoint(GameEnd, powerLog, false);
				var pos = gameStart > gameEnd ? gameStart : gameEnd;
				while(true)
				{
					using(var fs = new FileStream(powerLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					using(var sr = new StreamReader(fs, Encoding.ASCII))
					{
						if(pos < fs.Length)
						{
							fs.Seek(pos, SeekOrigin.Begin);
							var newLines = sr.ReadToEnd().Split('\n');
							foreach(var line in newLines.Where(IsRelevantLine))
							{
								Log.Debug("LogWatcher", line);
								if(line.Contains(GameStart))
								{
									Log.Debug("LogWatcher", "NEW GAME");
									CurrentGame.Clear();
									CurrentGame.Add(line);
								}
								else if(line.Contains(GameEnd))
								{
									CurrentGame.Add(line);
									Log.Debug("LogWatcher", "COMPLETE GAME");
									LastGame.Clear();
									LastGame.AddRange(CurrentGame);
								}
								else
									CurrentGame.Add(line);
							}
							pos = fs.Length;
						}
						else if(pos > fs.Length)
						{
							WatchPowerLog();
							return;
						}
					}
					Thread.Sleep(5000);
				}
			});
			t.Start();
		}

		private bool IsRelevantLine(string line) => line.Length > 23 && line[19] == 'G' && line[23] == 'S';
	}
}