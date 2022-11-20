using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace FileWatcher
{
    public class FileWatcherService
    {
        public static Dictionary<string, FileSystemWatcher> Watchers = new Dictionary<string, FileSystemWatcher>();
        #region 啟動監看
        public void FileWatch(colleague colleague)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            Watchers.Add(colleague.Name, watcher);
            watcher.Path = WatchingFolder(colleague.Name);//監視目錄
            watcher.Filter = "*.*";//檔案類型
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = false;
            watcher.Changed += new FileSystemEventHandler((s, e) => OnChanged(s, e, colleague));
        }
        public static void OnChanged(object source, FileSystemEventArgs watchEvent, colleague colleague)
        {
            var watcher = source as FileSystemWatcher;
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {colleague.Name} {colleague.Action}");
        }
        #endregion
        /// <summary>
        /// 路徑
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public string WatchingFolder(string name)
        {
            string path = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (name.ToUpper())
                {
                    case "EDDY":
                        path = @"C:\Users\user\Desktop\Test\temp3";
                        break;
                    case "TOM":
                        path = @"C:\Users\user\Desktop\Test\temp2";
                        break;
                }
            }
            else
            {
                switch (name.ToUpper())
                {
                    case "EDDY":
                        path = "/home/kway/TestSample/test3";
                        break;
                    case "TOM":
                        path = "/home/kway/TestSample/test2";
                        break;
                }
            }
            return path;
        }
        public void CancelWatcher(string name)
        {
            var watcher = Watchers[name];
            watcher.Dispose();
        }
    }
}
