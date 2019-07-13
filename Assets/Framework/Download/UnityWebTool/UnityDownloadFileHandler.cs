using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace QP.Framework
{
    public class UnityDownloadFileHandler : DownloadHandlerScript
    {
        public Action<SDownloadFileResult> Progress;
        public int ContentLength { get; private set; }
        public int DownloadedLength { get; private set; }

        private FileStream _fileStream;
        private string _download_url;
        private string _localPath_url;
        private SDownloadFileResult _downloadFileResult;

        public UnityDownloadFileHandler(string download_url,string localPath_url) : base(new byte[1024 * 200])
        {
            _download_url = download_url;
            _localPath_url = localPath_url;
            _downloadFileResult = new SDownloadFileResult();
            string dir = Path.GetDirectoryName(localPath_url);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _fileStream = new FileStream(localPath_url + ".tmp", FileMode.Append, FileAccess.Write);
            DownloadedLength = (int)_fileStream.Length;
            ContentLength = 0;
            _downloadFileResult.downloadedLength = DownloadedLength;
            _downloadFileResult.downloadedLengthStr = Util.HumanReadableFilesize(DownloadedLength);
        }
        /// <summary>
        /// 文件数据长度
        /// </summary>
        protected override void ReceiveContentLength(int contentLength)
        {
            ContentLength = contentLength + DownloadedLength;
            _downloadFileResult.contentLength = ContentLength;
            _downloadFileResult.contentLengthStr = Util.HumanReadableFilesize(ContentLength);
        }
        /// <summary>
        /// 接受到数据时
        /// </summary>
        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || dataLength == 0 || _fileStream == null)
            {
                return false;
            }
            _fileStream.Write(data, 0, dataLength);
            DownloadedLength += dataLength;
            _downloadFileResult.downloadedLength = DownloadedLength;
            _downloadFileResult.downloadedLengthStr = Util.HumanReadableFilesize(DownloadedLength);
            Progress(_downloadFileResult);
            return true;
        }
        protected override void CompleteContent()
        {
            //Debug.Log(_download_url + "  下载完毕");
        }
        public void Dispose()
        {
            if (_fileStream != null)
            {
                //Debug.Log("关闭流");
                _fileStream.Close();
                _fileStream.Dispose();
                _fileStream = null;
            }
        }
    }
}

