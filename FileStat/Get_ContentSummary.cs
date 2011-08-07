using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Collections.Generic;
using System.Collections;

namespace PSFileUtils.FileStat
{
	[Cmdlet(VerbsCommon.Get, "ContentSummary")]
	public class Get_ContentSummary : PSCmdlet
	{
		private bool _recurse = false;
		private static int activityId = new Random(DateTime.UtcNow.Millisecond).Next();
		private static string activity = "Counting Files";

		[Parameter(Mandatory = false)]
		public SwitchParameter Recurse
		{
			get { return _recurse;  }
			set { _recurse = value; }
		}
		[Parameter(Mandatory = false, Position = 0)]
		public string RootPath = null;

		protected override void ProcessRecord()
		{
			int count = 0;
			Dictionary<string, int> extensionCounts = new Dictionary<string, int>();            

			if (RootPath == null)
				RootPath = this.SessionState.Path.CurrentFileSystemLocation.Path;
			DirectoryInfo rootInfo = new DirectoryInfo(RootPath);

			//if (Recurse)
			//    count = CountFiles(rootInfo);
			//else
			foreach (var fi in rootInfo.EnumerateFiles(fi => true,
				(Recurse) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly, (di, e) => true))
			{
				WriteProgress(new ProgressRecord(activityId, activity, fi.DirectoryName));
				string extKey = Path.GetExtension(fi.Name).ToLowerInvariant();
				if (String.IsNullOrEmpty(extKey))
					extKey = "''";
				if (extensionCounts.ContainsKey(extKey))
					extensionCounts[extKey]++;
				else
					extensionCounts[extKey] = 0;
				
				count++;
			}

			WriteDebug(String.Format("RootPath: {0}", RootPath));
			WriteObject(new {
				Count = count,
				Types = new Hashtable(extensionCounts)
			});
		}  
	  
		//private int E(DirectoryInfo directory, string searchPattern, SearchOption searchOption, 
		//    Func<DirectoryInfo, Exception, bool> accessExceptionHandler)
		//{
		//    FileSystemInfo[] entries = null;
		//    int count = 0;
		//    Queue<DirectoryInfo> nodeQueue = new Queue<DirectoryInfo>(new DirectoryInfo[] { directory });

		//    while (nodeQueue.Count > 0)
		//    {
		//        try
		//        {
		//            var di = nodeQueue.Dequeue();
		//            WriteProgress(new ProgressRecord(activityId, activity, di.FullName));
		//            entries = di.GetFileSystemInfos();
		//            WriteDebug(String.Format("Got {0} entries in {1}", entries.Count(), di.FullName));
		//            if (entries != null)
		//                count += entries.Where(fsi => fsi is FileInfo).Count(); 
		//        }
		//        catch (Exception e)
		//        {
		//            WriteDebug("Encountered error in getting filesyteeminfos: " + e);
		//            if (accessExceptionHandler == null || !accessExceptionHandler(directory, e))
		//                throw;
		//            entries = null;
		//        }

		//        if (searchOption == SearchOption.AllDirectories && entries != null)
		//        {
		//            foreach (DirectoryInfo item in entries.Where(fsi => fsi is DirectoryInfo))
		//                nodeQueue.Enqueue(item as DirectoryInfo);    
		//        }              
		//    }

		//    return count;
		//}        
	}
}
