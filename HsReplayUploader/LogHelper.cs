#region

using System;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace HsReplayUploader
{
	public class LogHelper
	{
		public static long FindEntryPoint(string entryPoint, string filePath, bool includeFirstLine)
		{
			const int bufferSize = 4096;
			var fileInfo = new FileInfo(filePath);
			if(!fileInfo.Exists)
				return 0;
			var target = new string(entryPoint.Reverse().ToArray());
			using(var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using(var sr = new StreamReader(fs, Encoding.ASCII))
			{
				var offset = 0;
				while(offset < fs.Length)
				{
					offset += bufferSize;
					var buffer = new char[bufferSize];
					fs.Seek(Math.Max(fs.Length - offset, 0), SeekOrigin.Begin);
					sr.ReadBlock(buffer, 0, bufferSize);
					var skip = 0;
					for(var i = 0; i < bufferSize; i++)
					{
						skip++;
						if(buffer[i] == '\n')
							break;
					}
					if(skip >= bufferSize)
						continue;
					offset -= skip;
					var reverse = new string(buffer.Skip(skip).Reverse().ToArray());
					var targetOffset = reverse.LastIndexOf(target, StringComparison.Ordinal);
					if(targetOffset != -1)
					{
						var line = new string(reverse.Substring(targetOffset).TakeWhile(c => c != '\n').ToArray());
						if(line.Contains("etatSemaG"))
							return fs.Length - (offset - bufferSize + skip + targetOffset + (includeFirstLine ? line.Length : 0));
					}
				}
			}
			return 0;
		}
	}
}