using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace QP.Framework
{
    public class DownloadMd5File
    {
        private Action< Queue<DownloadConfig>> callback;
        private Dictionary<string, string> localMd5Dict;
        private Dictionary<string, string> remoteMd5Dict;
        private string module;
        private string fullModule;
        private Version version;
        private int failCount;
        public DownloadMd5File(Version version,string module,Action<Queue<DownloadConfig>> callback)
        {
            this.callback = callback;
            if (module == GameConfig.module_name)
            {
                this.fullModule = GameConfig.module_name;
            }else
            {
                this.fullModule = string.Format("{0}/{1}", GameConfig.module_name, module);
            }
            this.module = module;
            this.version = version;
            failCount = 0;
            Download(0);
        }
        private void Download(int delay)
        {
            string url = string.Format("{0}/{1}/{2}", version.res_download_url, fullModule, "md5file.txt");
            WWWMgr.Instance.Download(url, DownloadCompleted, delay);
        }
        private void DownloadCompleted(WWW www)
        {
            if (www == null)
            {
                if (failCount == GameConfig.download_Fail_Count)
                {
                    if (callback != null) callback(null);
                    return;
                }
                Download(GameConfig.download_Fail_Retry_Delay);
                failCount++;
                return;
            }

            localMd5Dict = Md5FileHelp.LocalFileForDict(module);
            remoteMd5Dict = Md5FileHelp.ForDict(www.text);
            Dictionary<string, string>.Enumerator e = remoteMd5Dict.GetEnumerator();
            Queue<DownloadConfig> DownloadList = new Queue<DownloadConfig>();
            while (e.MoveNext())
            {
                string md5 = null;
                localMd5Dict.TryGetValue(e.Current.Key, out md5);
                string file = string.Format("{0}/{1}", Util.DeviceResPath, e.Current.Key);
                if (md5 == null || md5.Trim() != e.Current.Value.Trim() || !File.Exists(file))
                {
                    DownloadConfig fileConfig = new DownloadConfig();
                    fileConfig.key = e.Current.Key;
                    fileConfig.download_url = string.Format("{0}/{1}/{2}", version.res_download_url, GameConfig.module_name, e.Current.Key);
                    fileConfig.localPath_url = string.Format("{0}/{1}", Util.DeviceResPath, e.Current.Key);
                    DownloadList.Enqueue(fileConfig);

                    if (!e.Current.Key.EndsWith(".txt"))
                    {
                        DownloadConfig manifestConfig = new DownloadConfig();
                        fileConfig.key = null;
                        manifestConfig.download_url = string.Format("{0}.{1}", fileConfig.download_url, "manifest");
                        manifestConfig.localPath_url = string.Format("{0}.{1}", fileConfig.localPath_url, "manifest");
                        DownloadList.Enqueue(manifestConfig);
                    }
                }
            }
            e.Dispose();
            if (callback != null) callback(DownloadList);
        }
        public void UpdateLoaclMd5File(string key)
        {
            if (key != null)
            {
                string md5line = null;
                if (localMd5Dict.TryGetValue(key, out md5line))
                    localMd5Dict[key] = remoteMd5Dict[key];
                else
                    localMd5Dict.Add(key, remoteMd5Dict[key]);
            }
        }
        public void WriteToLocalFile(string module)
        {
            Md5FileHelp.ForFile(localMd5Dict, module);
        }
    }
}

