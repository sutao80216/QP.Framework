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
        public string luaPath;
        public LuaTable Table { get { return scriptEnv; } }

        private Action luaOnEnable;
        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDisable;
        private Action luaOnDestroy;
        private LuaTable scriptEnv;
       
        LuaEnv luaEnv;
        public void Awake()
        {
            if (string.IsNullOrEmpty(luaPath)) return;
            luaEnv = LuaEnvMgr.Instance.LuaEnv;
            scriptEnv = luaEnv.NewTable();
            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();
            scriptEnv.Set("self", this);
            this.Init();
        }
        private void OnEnable()
        {
            if (luaOnEnable != null) luaOnEnable();
        }
        private void Init()
        {
            byte[] lua = LuaEnvMgr.Instance.GetLuaText(this.luaPath);
            //luaEnv.DoString(@"require " + "'" + module + "'", "LuaScript", scriptEnv);
            luaEnv.DoString(lua, "LuaScript", scriptEnv);
            Action luaAwake = scriptEnv.Get<Action>("Awake");
            scriptEnv.Get("Start", out luaStart);
            scriptEnv.Get("Update", out luaUpdate);
            scriptEnv.Get("OnEnable", out luaOnEnable);
            scriptEnv.Get("OnDisable", out luaOnDisable);
            scriptEnv.Get("OnDestroy", out luaOnDestroy);

            if (luaAwake != null) luaAwake();
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
        }
    }
}

