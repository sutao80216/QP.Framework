using System;
using UnityEngine;
namespace QP.Framework
{
    public class Version
    {
        public string version;
        public string root_module;
        public string app_download_url;
        public string res_download_url;
        public int download_fail_retry;
        public bool preTamperLua;
    }
    public enum VersionResType
    {
        DownloadFail,
        DownloadSuccess,
        Different,
        Unusual,
    }
    public class DownloadVersionFile
    {
        private string _url;
        private int _failRetryCount;
        private float _failRetryDelay;
        private Action<VersionResType,Version> _onCompleted;
        public DownloadVersionFile(string url,int failRetryCount,float failRetryDelay, Action<VersionResType,Version> callback)
        {
            _url = url;
            _failRetryCount = failRetryCount;
            _failRetryDelay = failRetryDelay;
            _onCompleted = callback != null ? callback : (VersionResType t,Version v) => { };
            if(GameConfig.gameModel== GameModel.Editor)
            {
                _onCompleted(VersionResType.DownloadSuccess, new Version() { root_module = BuildConfig.root_module });
                return;
            }
            StartDownload(0);
        }
        private void StartDownload(float delay)
        {
            //Debug.Log("下载版本文件：" + _url);
            WWWMgr.Instance.Download(_url, DownloadCompleted, delay);
        }
        private void DownloadCompleted(WWW www)
        {
            if (www == null)
            {
                if (_failRetryCount <= 0)
                {
                    _onCompleted(VersionResType.DownloadFail,null);
                    return;
                }
                _failRetryCount--;
                StartDownload(_failRetryDelay);
                return;
            }
            CheckVersion(www.text);
        }
        private void CheckVersion(string text)
        {
            //远程版本文件
            Version remoteVersion = VersionHelp.JsonForVersion(text);
            if (remoteVersion == null)
            {
                _onCompleted(VersionResType.Unusual,null);
                return;
            }

            //获取本地版本文件
            Version localVersion = VersionHelp.GetLocalVersionForApp();
            //版本是否一致（具体对比规则自定义）
            if (localVersion != null && localVersion.version != remoteVersion.version)
            {
                _onCompleted(VersionResType.Different, null);
                return;
            }
            //更新本地版本文件
           
            VersionHelp.WriteLocalVersionFile(remoteVersion);
            _onCompleted(VersionResType.DownloadSuccess, remoteVersion);
        }
    }
}

