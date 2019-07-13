using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace QP.Framework
{
    public struct RequestHandler
    {
        public UnityWebRequest request;
        public UnityDownloadFileHandler handler;
        public RequestHandler(UnityWebRequest request, UnityDownloadFileHandler handler)
        {
            this.request = request;
            this.handler = handler;
        }
    }
    public class UnityWeb : MonoBehaviour, IWebDownload
    {
        Dictionary<string, RequestHandler> requestDict = new Dictionary<string, RequestHandler>();
        public void DownloadFileSize(string url,int timeout,Action<bool, int, int> done)
        {
            StartCoroutine(DownloadFileSizeHandler(url, timeout, done));
        }
        public void DownloadFile(string download_url,string local_url,int timeout, Action<SDownloadFileResult> progress, Action<int> complete)
        {
            StartCoroutine(DownloadFileHandler(download_url, local_url, timeout, progress, complete));
        }
        IEnumerator DownloadFileSizeHandler(string url, int timeout, Action<bool, int, int> done)
        {
            HeadHandler handler = new HeadHandler();
            UnityWebRequest request = UnityWebRequest.Head(url);
            request.downloadHandler = handler;
            request.timeout = timeout;
            request.chunkedTransfer = true;
            request.disposeDownloadHandlerOnDispose = true;
            yield return request.Send();
            if (done != null)
            {
                done(request.responseCode == 200, (int)request.responseCode, handler.ContentLength);
            }
            request.Abort();
            request.Dispose();
        }
        IEnumerator DownloadFileHandler(string download_url, string localPath_url, int timeout,Action<SDownloadFileResult>progress,Action<int>complete)
        {
            UnityDownloadFileHandler handler = new UnityDownloadFileHandler(download_url, localPath_url);
            handler.Progress = progress;
            UnityWebRequest request = UnityWebRequest.Get(download_url);
            request.SetRequestHeader("Range", string.Format("bytes={0}-", handler.DownloadedLength)); //成功返回206
            request.downloadHandler = handler;
            request.timeout = timeout;
            request.chunkedTransfer = true;
            request.disposeDownloadHandlerOnDispose = true;
            requestDict.Add(download_url, new RequestHandler(request,handler));
            yield return request.Send();
            int code = (int)request.responseCode;
            Dispose(download_url);
            if (complete != null) complete(code);

        }
        private void Dispose(string key)
        {
            RequestHandler result ;
            if(requestDict.TryGetValue(key,out result))
            {
                result.handler.Dispose();
                result.request.Abort();
                requestDict.Remove(key);
            }
        }
        public void Close()
        {
            StopAllCoroutines();
        }
        void OnDestroy()
        {
            Dictionary<string, RequestHandler>.Enumerator e = requestDict.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Value.handler.Dispose();
                e.Current.Value.request.Abort();
            }
            e.Dispose();
            StopAllCoroutines();
            requestDict.Clear();
            requestDict = null;
        }
    }
}

