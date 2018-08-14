using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace QP.Framework
{
    public class CheckForUpdateTable
    {
        public DelegateDefined.DStringInt Complete;
        public DelegateDefined.DString Error;
    }
    public class CheckForUpdate
    {
        private CheckForUpdateTable _table;
        private DownloadMd5File _md5File;
        public CheckForUpdate(string module, CheckForUpdateTable table)
        {
            this._table = table;
            new DownloadMd5File(VersionHelp.version, module, (Queue<DownloadConfig> list) => {
                if (list == null)
                    this._table.Error(module);
                else
                    this._table.Complete(module, list.Count);
            });
        }
        public CheckForUpdate(string module, Action< CheckForUpdate, Queue<DownloadConfig>> callback)
        {
            _md5File = new DownloadMd5File(VersionHelp.version, module, (Queue<DownloadConfig> list) => {
                callback(this,list);
            });
        }
        public void UpdateLoaclMd5File(string key)
        {
            if (_md5File != null)
                _md5File.UpdateLoaclMd5File(key);
        }
        public void WriteToLocalFile(string module)
        {
            if (_md5File != null)
                _md5File.WriteToLocalFile(module);
        }
    }

    [LuaCallCSharp]
    public class CheckForUpdateMgr : MonoBehaviour
    {

        private static CheckForUpdateMgr _instance;
        public static CheckForUpdateMgr Instance
        {
            get { return Util.GetInstance(ref _instance, "_CheckForUpdateMgr"); }
        }
           
        private CheckForUpdateTable G_Table;
        public void CheckModule(string module)
        {
            if (string.IsNullOrEmpty(module)) return;
            if (G_Table == null) G_Table = LuaEnvMgr.Instance.LuaEnv.Global.Get<CheckForUpdateTable>("G_CheckForUpdateMgr");
            if (GameConfig.gameModel == GameModel.Editor)
            {
                G_Table.Complete(module, 0);
                return;
            }
           new CheckForUpdate(module, G_Table);
        }
        private void OnDestroy()
        {
            G_Table = null;
            _instance = null;
        }
    }
}

