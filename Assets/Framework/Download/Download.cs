using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using UnityEngine;
namespace QP.Framework
{
    public class Download
    {
        private DownloadConfig _downloadConfig;
        public Action<DownloadConfig> onCompleted;
        public Action<DownloadConfig> onError;
        public Action<DownloadConfig,int> onProgress;
        public Download(DownloadConfig config)
        {
            _downloadConfig = config;
        }
        public void Start()
        {
            string fileDirPath = Path.GetDirectoryName(_downloadConfig.localPath_url);
            if (!Directory.Exists(fileDirPath))
            {
                Directory.CreateDirectory(fileDirPath);
            }
            try
            {
                using (WebClient client = new WebClient())
                {
                    Uri muri = new Uri(_downloadConfig.download_url);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                    client.DownloadFileAsync(muri, _downloadConfig.localPath_url);
                }
            }
            catch (Exception e)
            {
                if (onError != null)
                {
                    Debug.LogError(e.Message);
                    onError(_downloadConfig);
                }
            }
        }
        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            WebClient webClient = (WebClient)sender;
            if (e.Error == null)
            {
                if (onCompleted != null)
                    onCompleted(_downloadConfig);
            }
            //else
            //{
            //    if (onError != null)
            //        onError(_downloadConfig);
            //}
            webClient.Dispose();
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //int value = (int)e.BytesReceived;
            //int total = (int)e.TotalBytesToReceive;
            //string text = e.ProgressPercentage + "%";
            if (onProgress != null)
            {
                onProgress(_downloadConfig, e.ProgressPercentage);
            }
            //if (value == total && e.ProgressPercentage == 100)
            //{
               
            //}
        }
    }
}

