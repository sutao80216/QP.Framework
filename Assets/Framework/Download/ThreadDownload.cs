using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
namespace QP.Framework
{
    public enum DownloadEventType
    {
        Progress,
        Completed,
        Error,
    }
    public struct DownloadEvent
    {
        public DownloadEventType eventType;
        public DownloadConfig config;
        public object param;
        public DownloadEvent(DownloadEventType eventType,DownloadConfig config,object param=null)
        {
            this.eventType=eventType;
            this.config=config;
            this.param = param;
        }
    }
    public struct DownloadConfig {
        public string key;
        public string download_url;
        public string localPath_url;
    }

    public class ThreadDownload
    {
        public Action<int> onProgress;
        private bool isWait;
        private Thread _thread;
        private Queue<DownloadConfig> _DownloadList;
        private Queue<DownloadEvent> _EventQueue = new Queue<DownloadEvent>();
        public Queue<DownloadEvent> Events
        {
            get  { return _EventQueue; }
        }
        public ThreadDownload(Queue<DownloadConfig> DownloadList)
        {
            _DownloadList = DownloadList;
          
        }

        public void Start()
        {
            isWait = false;
            _thread = new Thread(new ParameterizedThreadStart(StartThread));
            _thread.Start();
        }
        public void Stop()
        {
            _thread.Abort();
        }
        private void StartThread(object obj)
        {
            while (_DownloadList.Count > 0)
            {
                if (isWait == false)
                {
                    DownloadConfig config = _DownloadList.Dequeue();
                    Download dw = new Download(config);
                    dw.onProgress = OnProgress;
                    dw.onCompleted = OnCompleted;
                    dw.onError = OnError;
                    dw.Start();
                    isWait = true;
                }
                Thread.Sleep(1);
            }
        }
       
        private void OnProgress(DownloadConfig config,int progress)
        {
            _EventQueue.Enqueue(new DownloadEvent(DownloadEventType.Progress, config, progress));
        }
        private void OnCompleted(DownloadConfig config)
        {
            _EventQueue.Enqueue(new DownloadEvent(DownloadEventType.Completed, config));
            isWait = false;
        }
        private void OnError(DownloadConfig config)
        {
            _DownloadList.Enqueue(config);
            _EventQueue.Enqueue(new DownloadEvent(DownloadEventType.Error, config));
            isWait = false;
        }

    }
}

