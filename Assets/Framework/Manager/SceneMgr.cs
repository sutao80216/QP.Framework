using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QP.Framework
{
    public class SceneMgr : MonoBehaviour
    {
        private static SceneMgr _instance = null;
        public static SceneMgr Instance { get { return Util.GetInstance(ref _instance, "_SceneMgr"); } }
        /// <summary>
        /// 是否正在加载场景
        /// </summary>
        private bool _isLoading;
        /// <summary>
        /// 当前模块名
        /// </summary>
        private string _moduleName;
        /// <summary>
        /// 当前跳转的场景名
        /// </summary>
        private string _sceneName;
        /// <summary>
        /// 是否使用异步跳转
        /// </summary>
        private bool _isAsync;
        private Action _complete;
        private SceneTable _sceneTable;
        public void Jump(string moduleName,string sceneName,bool isAsync, SceneTable table,Action complete)
        {
            if (_isLoading)
            {
                Debug.LogError("正在跳转中。。请不要连续跳转");
                return;
            }
            _isLoading = true;
            _moduleName = moduleName;
            _sceneName = sceneName;
            _isAsync = isAsync;
            _sceneTable = table;
            _complete = complete;

            if (Application.CanStreamedLevelBeLoaded(_sceneName))
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                if (_isAsync)
                {
                    StartCoroutine(IELoadSceneHandler());
                }
                else
                {
                    SceneManager.LoadScene(_sceneName);
                }
            }
            else
            {
                Debug.LogError("场景："+sceneName+ " 跳转场景失败, 检查是否已经把Modules下面所有场景添加到File->Build Settings中");
                if (_sceneTable != null&& _sceneTable.Error!=null) _sceneTable.Error(moduleName);
                Clear();
            }

        }

        private IEnumerator IELoadSceneHandler()
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(_sceneName);
            int displayProgress = 90;
            int toProgress = 0;
            async.allowSceneActivation = false;
            while (!async.isDone)
            {
                if (async.progress < 0.9f)
                {
                    toProgress = (int)async.progress * 100;
                    ++displayProgress;
                    if (_sceneTable != null && _sceneTable.Progress != null) _sceneTable.Progress(_moduleName, displayProgress / 100f);
                }
                else
                {
                    toProgress = 100;
                    if (displayProgress < toProgress)
                    {
                        ++displayProgress;
                        if (_sceneTable != null && _sceneTable.Progress != null) _sceneTable.Progress(_moduleName,displayProgress / 100f);
                    }
                    else
                    {
                        async.allowSceneActivation = true;
                    }

                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (_complete != null)_complete();
            if (_sceneTable != null && _sceneTable.Complete != null) _sceneTable.Complete(_moduleName);
            Clear();
        }
        private void Clear()
        {
            _isLoading = false;
            _moduleName = string.Empty;
            _sceneName = string.Empty;
            _isAsync = false;
            _complete = null;
        }
    }
}

