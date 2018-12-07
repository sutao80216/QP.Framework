using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.UI;
namespace QP.Framework{
    [LuaCallCSharp]
    public class LuaScript : MonoBehaviour
    {
        public bool root = false;
        public string LuaPath;
        public LuaTable Table { get { return scriptEnv; } }
        private Action luaOnEnable;
        private Action luaAwake;
        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDisable;
        private Action luaOnDestroy;
        private LuaTable scriptEnv = null;
        private LuaEnv luaEnv;
        public void Init()
        {
            luaEnv = LuaEnvMgr.Instance.LuaEnv;
            scriptEnv = luaEnv.NewTable();
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();
            scriptEnv.Set("self", this);

            byte[] lua = LuaEnvMgr.Instance.GetLuaText(LuaPath);
            luaEnv.DoString(lua, "LuaScript", scriptEnv);
            scriptEnv.Get("Awake", out luaAwake);
            scriptEnv.Get("Start", out luaStart);
            scriptEnv.Get("Update", out luaUpdate);
            scriptEnv.Get("OnEnable", out luaOnEnable);
            scriptEnv.Get("OnDisable", out luaOnDisable);
            scriptEnv.Get("OnDestroy", out luaOnDestroy);
        }

        public void Awake()
        {
            if (string.IsNullOrEmpty(LuaPath)) return;
            if (Table == null)
            {
                Init();
            }
            if (luaAwake != null) luaAwake();
        }
        private void OnEnable()
        {
            if (luaOnEnable != null) luaOnEnable();
        }
        private void OnDisable()
        {
            if (luaOnDisable != null) luaOnDisable();
        }
        void Start()
        {
            if (luaStart != null) luaStart();
        }
        void Update()
        {
            if (luaUpdate != null) luaUpdate();
        }
        void OnDestroy()
        {
            if (luaOnDestroy != null) luaOnDestroy();
            scriptEnv.Dispose();
            luaOnDestroy = null;
            luaOnDisable = null;
            luaOnEnable = null;
            luaUpdate = null;
            luaStart = null;
            scriptEnv = null;
            luaEnv = null;
            //if (root)
            //{
                    
            //}
        }
        IEnumerator UnloadUnusedAssets()
        {
            yield return new WaitForSeconds(3);
            LuaEnvMgr.Instance.RestoreTick();
            Resources.UnloadUnusedAssets();
        }

    }
}

