using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PSFileUtils.FileStat
{
	public static class DirectoryInfoExtensions
	{
		public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo directory, Func<FileInfo,bool> filter, SearchOption searchOption,
			Func<DirectoryInfo, Exception, bool> accessExceptionHandler)
		{
			Queue<DirectoryInfo> dQueue = new Queue<DirectoryInfo>(new DirectoryInfo[] { directory });
			FileSystemInfo[] entries = null;

			while (dQueue.Count > 0)
			{
				try
				{
					var di = dQueue.Dequeue();
					entries = di.GetFileSystemInfos();                  
				}
				catch (Exception e)
				{
					if (accessExceptionHandler == null || !accessExceptionHandler(directory, e))
						throw;
					entries = null;
				}

				if (entries != null)
				{
					foreach (var item in entries)
					{
						if (item is DirectoryInfo)
							dQueue.Enqueue(item as DirectoryInfo);
						else
							yield return item as FileInfo;
					}
				}
			}
		}
	}
}
