﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace QP.Framework
{
    /// <summary>
    /// 对比MD5 获取需要下载的文件列表
    /// 更新本地MD5文件
    /// </summary>
    public class Md5File
    {
        private string _module;
        private Version _version;
        private Dictionary<string, string> _localMd5Dict;
        private Dictionary<string, string> _remoteMd5Dict;
        private Dictionary<string, string> _tmpMd5Dict;
        private Queue<SDownloadModuleConfig> _downloadQueue;
        public Queue<SDownloadModuleConfig> DownloadQueue
        {
            get { return _downloadQueue; }
            set { _downloadQueue = value; }
        }
        public Md5File(string module,string removeText)
        {
            _module = module;
            _version = VersionHelp.version;
            _localMd5Dict = Md5FileHelp.LocalFileForDict(_module, "md5file.txt");
            _remoteMd5Dict= Md5FileHelp.ForDict(removeText);
            _tmpMd5Dict = Md5FileHelp.LocalFileForDict(_module, "tmp.txt");
            _downloadQueue = new Queue<SDownloadModuleConfig>();
            InitUpdateFile();
        }
        public string GetRemoteMd5(string key)
        {
            string md5 = string.Empty;
            _remoteMd5Dict.TryGetValue(key, out md5);
            return md5;
        }

        public void InitUpdateFile()
        {
            Queue<SDownloadModuleConfig> DownloadList = new Queue<SDownloadModuleConfig>();
            Dictionary<string, string>.Enumerator e = _remoteMd5Dict.GetEnumerator();
            while (e.MoveNext())
            {
                string file = e.Current.Key;
                string remoteMd5 = e.Current.Value;
                string localMd5 = string.Empty;

                string md5FileUrl = string.Format("{0}/{1}", Util.DeviceResPath, file);
                if (file.EndsWith(".txt") && _version.preTamperLua && File.Exists(md5FileUrl))
                {
                    localMd5 = Util.Md5File(md5FileUrl);
                }
                else
                {
                    _localMd5Dict.TryGetValue(e.Current.Key, out localMd5);
                }

                if (string.IsNullOrEmpty(localMd5) || localMd5.Trim() != remoteMd5.Trim() || File.Exists(md5FileUrl) ==false)
                {
                    //TODO 临时输出
                    if (string.IsNullOrEmpty(localMd5))
                    {
                        Debug.LogWarning(file + " 下载理由：localMd5=null");
                    }
                    else if(localMd5.Trim() != remoteMd5.Trim())
                    {
                        Debug.LogWarning(file + " 下载理由：文件内容改变");
                    }else if (File.Exists(md5FileUrl) == false)
                    {
                        Debug.LogWarning(file + " 下载理由：文件不存在");
                    }


                    


                    //如果续传的文件发生了变化 删除临时文件 重新下载
                    string tmpMd5 = string.Empty;
                    if (_tmpMd5Dict.TryGetValue(file,out tmpMd5))
                    {
                        if (tmpMd5.Trim() != remoteMd5.Trim())
                        {
                            string tmpFile = string.Format("{0}/{1}.tmp", Util.DeviceResPath, file);
                            if (File.Exists(tmpFile))
                            {
                                Debug.Log(file + " 部分已下载，但是远程文件发生了改变，所以删除重新下载");
                                File.Delete(tmpFile);
                            }
                        }
                    }
                    PushUpdateFile(file);
                }
            }
            e.Dispose();
            _tmpMd5Dict.Clear();
        }
        private void PushUpdateFile(string file)
        {
            SDownloadModuleConfig fileConfig = new SDownloadModuleConfig();
            fileConfig.key = file;
            fileConfig.download_url = string.Format("{0}/{1}/{2}", _version.res_download_url, GameConfig.module_name, file);
            fileConfig.localPath_url = string.Format("{0}/{1}", Util.DeviceResPath, file);
            fileConfig.download_fail_retry = _version.download_fail_retry;
            _downloadQueue.Enqueue(fileConfig);

            //添加manifest文件
            string manifestFile = string.Empty;
            //跳过lua脚本
            if (!file.EndsWith(".txt"))
            {
                SDownloadModuleConfig manifestConfig = new SDownloadModuleConfig();
                manifestConfig.key = string.Format("{0}.manifest", file);
                manifestConfig.download_url = string.Format("{0}.manifest", fileConfig.download_url);
                manifestConfig.localPath_url = string.Format("{0}.manifest", fileConfig.localPath_url);
                manifestConfig.download_fail_retry = _version.download_fail_retry;
                _downloadQueue.Enqueue(manifestConfig);
            }
        }

        /// <summary>
        /// 将当前下载的文件md5信息添加到下载队列中
        /// </summary>
        public void PushTmpFile(string file)
        {
            //if (file.EndsWith(".manifest")) return;
            string md5 = string.Empty;
            if(_remoteMd5Dict.TryGetValue(file,out md5))
            {
                if (_tmpMd5Dict.ContainsKey(file))
                {
                    _tmpMd5Dict[file] = md5;
                }
                else
                {
                    _tmpMd5Dict.Add(file, md5);
                }
            }
        }
        /// <summary>
        /// 下载完成时移除它
        /// </summary>
        public void PopTmpFile(string file)
        {
            if (_tmpMd5Dict.ContainsKey(file))
            {
                _tmpMd5Dict.Remove(file);
            }
        }
        /// <summary>
        /// 下载完成时更新本地md5
        /// </summary>
        public void UpdateLocalMd5File(string file)
        {
            if (file.EndsWith(".manifest")) return;
            string md5 = _remoteMd5Dict[file];
            if (_localMd5Dict.ContainsKey(file))
            {
                _localMd5Dict[file] = md5;
            }
            else
            {
                _localMd5Dict.Add(file, md5);
            }
        }
        /// <summary>
        /// 退出时将md5写到文件
        /// </summary>
        public void Destroy()
        {
            string fullModule;
            if (_module != GameConfig.module_name)
            {
                fullModule = string.Format("{0}/{1}", Util.DeviceResPath, _module);
            }
            else
            {
                fullModule = Util.DeviceResPath;
            }
            Md5FileHelp.ForFile(_localMd5Dict, string.Format("{0}/{1}", fullModule, "md5file.txt"));
            Md5FileHelp.ForFile(_tmpMd5Dict, string.Format("{0}/{1}", fullModule, "tmp.txt"));
        }
    }
}

