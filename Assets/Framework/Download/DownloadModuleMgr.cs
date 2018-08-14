using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace QP.Framework
{
    public class DownloadModuleTable
    {
        public DelegateDefined.DStringFloat Progress;
        public DelegateDefined.DStringFloat TotalProgress;
        public DelegateDefined.DString Complete;
        public DelegateDefined.DString Error;
    }
    [LuaCallCSharp]
    public class DownloadModuleMgr : MonoBehaviour
    {
        private static DownloadModuleMgr _instance;
        public static DownloadModuleMgr Instance
        {
            get { return Util.GetInstance(ref _instance, "_DownloadModuleMgr"); }
        }
        private DownloadModuleTable G_Table;
        public void DownLoadForCS(string module,DownloadModuleTable CSTable){
            this.DownLoadHandler(module, CSTable);
        }
        public void DownLoad(string module)
        {
            if (G_Table == null) G_Table = LuaEnvMgr.Instance.LuaEnv.Global.Get<DownloadModuleTable>("G_DownloadModuleMgr");
            this.DownLoadHandler(module, G_Table);
        }

        public void DownLoadHandler(string module,DownloadModuleTable table){
            //CheckForUpdateMgr.Instance.CheckModule(module);
            if (GameConfig.gameModel == GameModel.Editor)
            {
                if (table != null)table.Complete(module);
                return;
            }
            new CheckForUpdate(module, (CheckForUpdate checkForUpdate, Queue<DownloadConfig> list) =>
            {
                if (list == null)
                {
                    if(table!=null)table.Error(module);
                    return;
                }
                if (list.Count > 0)
                {
                    DownloadModule dm = new GameObject("DownloadModule", typeof(DownloadModule)).GetComponent<DownloadModule>();
                    dm.transform.SetParent(transform);
                    DontDestroyOnLoad(dm.gameObject);
                    dm.Download(module, list, checkForUpdate, table);
                }
                else
                {
                    if (table != null)table.Complete(module);
                }
            });
        }
      
    }

}
