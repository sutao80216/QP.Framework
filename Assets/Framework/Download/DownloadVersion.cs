using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QP.Framework
{
    public class DownloadVersion
    {
        private Action<Version> callback;
        private int failCount;
        public DownloadVersion(Action<Version> callback)
        {
            this.callback = callback;
            failCount = 0;
            Download(0);
        }
        private void Download(int delay)
        {
            string version_url = string.Format("{0}/{1}/{2}",
                                                 GameConfig.version_download_url,
                                                 GameConfig.module_name,
                                                 "version.txt");
            WWWMgr.Instance.Download(version_url, DownloadCompleted, delay);
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
            //检查是否需要重新下载游戏
            Version remote_version = VersionHelp.JsonForVersion(www.text);
            if (remote_version == null)
            {
                Debug.LogError("version.txt 解析失败");
                return;
            }
            Version local_version = VersionHelp.GetLocalVersionForApp();
            if (local_version != null && local_version.version != remote_version.version)
            {
                //第一次安装游戏
                Debug.Log("请重新下载游戏");
                return;
            }
            //更新本地版本文件
            VersionHelp.WriteLocalVersionFile(remote_version);
            if (callback != null) callback(remote_version);
        }
    }
}

