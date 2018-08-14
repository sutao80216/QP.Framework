using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using XLua;

namespace QP.Framework
{
    public class JumpSceneTable
    {
        public DelegateDefined.DString Complete;
        public DelegateDefined.DStringFloat Progress;
        public DelegateDefined.DString Error;
    }
    [LuaCallCSharp]
    public class JumpSceneMgr : MonoBehaviour
    {
        private static JumpSceneMgr _instance;
        public static JumpSceneMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("_JumpSceneMgr", typeof(JumpSceneMgr)).GetComponent<JumpSceneMgr>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        JumpSceneTable G_Table;
     
        /// <summary>
        /// 跳转场景
        /// </summary>
        /// <param name="module">模块名.</param>
        /// <param name="sceneName">场景名 默认 Main.</param>
        /// <param name="unload">如果为true 切换场景时Unload(true) 卸载除module的所有AssetBundle.</param>
        /// <param name="notAsync">如果为true 不会触发加载场景的进度回调.</param>
        public void Jump(string module, string sceneName = null,bool unload=true,bool notAsync=false)
        {
            if(G_Table==null)G_Table = LuaEnvMgr.Instance.LuaEnv.Global.Get<JumpSceneTable>("G_JumpSceneMgr");
            if (GameConfig.gameModel == GameModel.Editor)
            {
                LoadScene(module, sceneName, unload,notAsync);
                return;
            }
            ResMgr.Instance.InitAssetBundle(module,(float progress)=> {
                float a = notAsync == true ? 1 : 0.9f;
                if (G_Table != null) G_Table.Progress(module, progress * a);
            }, () => {
                LoadScene(module, sceneName, unload, notAsync);
            });
        }
        private void LoadScene(string module, string sceneName, bool unload,bool notAsync)
        {
            sceneName = sceneName ?? "Main";

            string scene = string.Format("{0}_{1}", module, sceneName);
            LuaEnvMgr.Instance.FastTick();

            if (Application.CanStreamedLevelBeLoaded(scene))
            {
                StartCoroutine(IELoadScene(scene, module, unload, notAsync));
            }
            else
            {
                if (G_Table != null) G_Table.Error(module);
            }
        }
        private IEnumerator IELoadScene(string scene,string module,bool unload,bool notAsync)
        {
            
            AsyncOperation async = SceneManager.LoadSceneAsync(scene);
            if (!notAsync)
            {
                int displayProgress = 90;
                int toProgress = 0;
                async.allowSceneActivation = false;
                while (!async.isDone)
                {
                    if (async.progress < 0.9f)
                    {
                        toProgress = (int)async.progress * 100;
                        ++displayProgress;
                        if (G_Table != null) G_Table.Progress(module, displayProgress / 100f);
                    }
                    else
                    {
                        toProgress = 100;
                        if (displayProgress < toProgress)
                        {
                            ++displayProgress;
                            if (G_Table != null) G_Table.Progress(module, displayProgress / 100f);
                        }
                        else
                        {
                            async.allowSceneActivation = true;
                        }

                    }
                    yield return new WaitForEndOfFrame();
                }

            }else{
                yield return async;
            }


            if (unload)
                ResMgr.Instance.ClearOtherModule(module);
            if (G_Table != null) G_Table.Complete(module);
           
        }
    }
}

