using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QP.Framework
{
    public class DownloadModule : MonoBehaviour
    {
        private int _downloadCount;
        private int _currentCount;
        private bool _isWriteMd5File;
        private string _module;
        private Queue<DownloadConfig> _downloadList;
        private DownloadModuleTable _table;
        private CheckForUpdate _checkForUpdate;
        private ThreadDownload _thread;

        private void Awake()
        {
            _downloadCount = 0;
            _currentCount = 0;
            _isWriteMd5File = false;
        }
        public void Download(string module,Queue<DownloadConfig> list, CheckForUpdate checkForUpdate, DownloadModuleTable table)
        {
            _table = table;
            _downloadList = list;
            _module = module;
            _checkForUpdate = checkForUpdate;
            if (_downloadList.Count > 0)
            {
                _downloadCount = _downloadList.Count;
                _isWriteMd5File = true;
                _thread = new ThreadDownload(_downloadList);
                _thread.Start();
            }
        }
        private void Update()
        {
            if (_thread != null && _thread.Events.Count>0)
            {
                DownloadEvent e = _thread.Events.Dequeue();
                switch (e.eventType)
                {
                    case DownloadEventType.Progress: OnProgress(e); break;
                    case DownloadEventType.Completed: OnCompleted(e); break;
                    case DownloadEventType.Error: OnError(e); break;
                }
            }
        }
        private void OnProgress(DownloadEvent e)
        {
            if (_table != null) _table.Progress(_module, (float)((int)e.param / 100f));
        }
        private void OnCompleted(DownloadEvent e)
        {
            _currentCount++;
            _checkForUpdate.UpdateLoaclMd5File(e.config.key);

            if (_table != null) _table.TotalProgress(_module, (float)_currentCount / (float)_downloadCount);
            if (_currentCount == _downloadCount)
            {
                _checkForUpdate.WriteToLocalFile(_module);
                _isWriteMd5File = false;
                if (_table != null) _table.Complete(_module);
                Destroy(gameObject);
            }
        }
        private void OnError(DownloadEvent e)
        {
            Debug.Log("失败了");
            if (_table != null) _table.Error(_module);
        }

        private void OnDestroy()
        {
            if (_isWriteMd5File)
            {
                _checkForUpdate.WriteToLocalFile(_module);
            }
            if (_thread != null)
                _thread.Stop();
            _checkForUpdate = null;
            _thread = null;
            _downloadList = null;
            _table = null;
        }
    }
}

