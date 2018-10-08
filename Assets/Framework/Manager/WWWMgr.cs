using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QP.Framework{
    public class WWWMgr : MonoBehaviour
    {
        private static WWWMgr _instance;
        public static WWWMgr Instance{
            get { return Util.GetInstance(ref _instance, "_WWWMgr"); }
        }

        public void Download(string url,Action<WWW>done,float delay=0)
        {
            if (done == null) done = (WWW www) => { };
            StartCoroutine(IEDownload(url, done, delay));
        }
        private IEnumerator IEDownload(string url,Action<WWW> done, float delay)
        {
            yield return new WaitForSeconds(delay);
            using (WWW www=new WWW(url))
            {
                yield return www;
                if (www.error != null)
                {
                    Debug.LogError(string.Format("{0}：{1}",url,www.error));
                    done(null);
                    yield break;
                }
                if (www.isDone)
                {
                    done(www);
                }
            }
        }
    }
}

