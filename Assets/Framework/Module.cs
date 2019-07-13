using System;
using System.Collections.Generic;
using UnityEngine;
namespace QP.Framework
{
    /// <summary>
    /// 模块类
    /// 负责当前模块的检查更新 下载 跳转场景
    /// </summary>
    public class Module : MonoBehaviour
    {
        /// <summary>
        /// 模块名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 资源大小
        /// </summary>
        public long Size { get { return _size; } }
        /// <summary>
        /// 资源大小字符串
        /// </summary>
        public string SizeStr { get { return _sizeStr; } }

        private long _size;
        private string _sizeStr;
        private Md5File _md5File;
        private DownloadBehaviour db;
        private SceneTable _sceneTable;// 缓存全局Table 避免每次Get的开销 
        private DownloadTable _downloadTable;// 缓存全局Table 避免每次Get的开销 
        private CheckUpdateTable _checkUpdateTable;// 缓存全局Table 避免每次Get的开销 
        private Queue<SDownloadModuleConfig> _downloadQueue;
        void Awake()
        {
            Name = gameObject.name;
        }
        #region 检查更新并下载
        /// <summary>
        /// 检查更新并下载
        /// </summary>
        public void CheckAndDownload(string tableName)
        {
            if(_downloadTable==null && !string.IsNullOrEmpty(tableName))
            {
                _downloadTable = LuaEnvMgr.Instance.LuaEnv.Global.Get<DownloadTable>(tableName);
            }
            CheckAndDownload(_downloadTable);
        }
        public void CheckAndDownload(DownloadTable table)
        {
            CheckUpdateTable checkUpdateTable = new CheckUpdateTable()
            {
                Complete = (string moduleName, int downloadCount, string sizeStr) =>
                  {
                      //2.下载模块
                      Download(table);
                  },
                Error = (string moduleName) =>
                  {
                      Debug.LogError("下载失败");
                  }
            };
            //1.检查更新
            CheckUpdate(checkUpdateTable,false);
        }
        #endregion

        #region 检查更新
        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="tableName">全局Table名</param>
        /// <param name="isGetSize">是否获取文件总大小</param>
        public void CheckUpdate(string tableName, bool isGetSize)
        {
            if (_checkUpdateTable == null && !string.IsNullOrEmpty(tableName))
            {
                _checkUpdateTable = LuaEnvMgr.Instance.LuaEnv.Global.Get<CheckUpdateTable>(tableName);
            }
            CheckUpdate(_checkUpdateTable, isGetSize);
        }
        public void CheckUpdate(CheckUpdateTable table,bool isGetSize)
        {
            //编辑器模式模拟检查完成
            if (GameConfig.gameModel == GameModel.Editor)
            {
                if (table != null && table.Complete != null)
                {
                    table.Complete(Name, 0,string.Empty);
                    return;
                }
            }
            CheckUpdateBehaviour cub = new GameObject(Name + "_CheckUpdateBehaviour").AddComponent<CheckUpdateBehaviour>();
            cub.CheckUpdate(Name, isGetSize, (Md5File md5File, long size)=>{
                _size = size;
                _md5File = md5File;
                _downloadQueue = md5File.DownloadQueue;
                _sizeStr = Util.HumanReadableFilesize(Convert.ToDouble(_size));
                if (table == null) return;
                if (_downloadQueue == null)
                {
                    Debug.LogError(string.Format("{0}：检查更新失败！", Name));
                    if (table.Error != null) table.Error(Name);
                    return;
                }
                Debug.Log(Name + " 需要下载 " + _sizeStr);
                if (table.Complete != null) table.Complete(Name, _downloadQueue.Count, _sizeStr);
                Destroy(cub.gameObject);
                cub = null;
            });
        }
        #endregion

        #region 下载
        /// <summary>
        /// 下载 全局Table监听事件
        /// </summary>
        public void Download(string tableName)
        {
            if (_downloadTable == null && !string.IsNullOrEmpty(tableName))
            {
                _downloadTable = LuaEnvMgr.Instance.LuaEnv.Global.Get<DownloadTable>(tableName);
            }
            Download(_downloadTable);
        }
        public void Download(DownloadTable table)
        {
            if (GameConfig.gameModel== GameModel.Editor)
            {
                if (table != null && table.AllComplete != null) table.AllComplete(Name);
                return;
            }

            if(_downloadQueue==null || _downloadQueue.Count == 0)
            {
                if (table != null && table.AllComplete != null) table.AllComplete(Name);
                return;
            }

            int downloadedCount = 0;
            int downloadTotal = _downloadQueue.Count;
            if (db == null)
            {
                db = new GameObject(Name + "_DownloadBehaviour").AddComponent<DownloadBehaviour>();
                db.transform.SetParent(transform);
                //下载进度
                db.Progress = (SDownloadEventResult result) =>
                {
                    //Debug.Log("----" + (float)result.FileResult.downloadedLength / (float)result.FileResult.contentLength);
                    if (table != null && table.Progress != null) table.Progress(Name, result.FileResult);
                };
                db.OneComplete = (SDownloadEventResult result) =>
                  {
                      downloadedCount++;
                      //下载一个完成
                      if (table != null && table.OneComplete != null)
                      {
                          table.OneComplete(Name, downloadedCount, downloadTotal);
                      }
                  };
                db.AllComplete = (SDownloadEventResult e) =>
                  {
                      if (table != null && table.AllComplete != null) table.AllComplete(Name);
                      Destroy(db.gameObject);
                      db = null;
                  };
                //下载失败
                db.Error = (SDownloadEventResult e) =>
                {
                    if (table != null && table.Error != null) table.Error(Name);
                    Destroy(db.gameObject);
                    db = null;
                };
                if (table != null && table.Befor != null) table.Befor(Name, _downloadQueue.Count);
                db.Download(Name, _md5File);
            }
        }
        #endregion
        /// <summary>
        /// 暂停下载
        /// </summary>
        public void StopDownload()
        {
            if (db == null) return;
            Destroy(db.gameObject);
            db= null;
        }

        /// <summary>
        /// 跳转场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="isAsync">是否异步加载</param>
        /// <param name="isUnloadOtherAssetBundle">加载后是否卸载其他的AssetBundle</param>
        /// <param name="tableName">全局Table名</param>
        public void JumpScene(string sceneName, bool isAsync, bool isUnloadOtherAssetBundle, string tableName)
        {
            if (_sceneTable == null && !string.IsNullOrEmpty(tableName)) _sceneTable = LuaEnvMgr.Instance.LuaEnv.Global.Get<SceneTable>(tableName);
            JumpScene(sceneName, isAsync, isUnloadOtherAssetBundle, _sceneTable);
        }
        public void JumpScene(string sceneName, bool isAsync, bool isUnloadOtherAssetBundle, SceneTable table)
        {
            //初始化资源
            LoadAssetBundle(isAsync, table,() =>
            {
                LuaEnvMgr.Instance.FastTick();
                SceneMgr.Instance.Jump(Name, sceneName, isAsync, table,()=>
                {
                    if (isUnloadOtherAssetBundle) ResMgr.Instance.ClearOtherModule(Name);
                });
            });
        }
        /// <summary>
        /// 加载模块资源
        /// </summary>
        private void LoadAssetBundle(bool isAsync, SceneTable table, Action complete)
        {
            ResMgr.Instance.InitAssetBundle(Name, (float progress) => {
                if (table != null && table.Progress != null)
                {
                    float progressEnd = isAsync == true ? 1 : 0.9f;
                    table.Progress(Name, progress * progressEnd);
                }
            }, complete);
        }
        void OnDestroy()
        {
           
        }
    }
}

