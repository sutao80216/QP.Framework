using System;
namespace QP.Framework
{
    public interface IWebDownload
    {
        /// <summary>
        /// 获取远程文件大小
        /// </summary>
        void DownloadFileSize(string url, int timeout, Action<bool, int, int> complete);
        /// <summary>
        /// 下载一个文件
        /// </summary>
        void DownloadFile(string download_url, string localPath_url, int timeout, Action<SDownloadFileResult> progress, Action<int> complete);

        void Close();
    }
}

