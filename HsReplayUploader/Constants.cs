namespace HsReplayUploader
{
	public static class Constants
	{
		public const string GameEnd = "TAG_CHANGE Entity=GameEntity tag=STATE value=COMPLETE";
		public const string GameStart = "CREATE_GAME";
		public const string Dir = "/sdcard/Android/data/com.blizzard.wtcg.hearthstone/files";
		public const string LogConfigContent = @"[Power]
LogLevel=1
FilePrinting=True
ConsolePrinting=False
ScreenPrinting=False
Verbose=True";
	}
}