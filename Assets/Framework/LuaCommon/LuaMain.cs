using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XLua;
namespace QP.Framework
{
    public class LuaMain : MonoBehaviour
    {
        public string luaPath = null;
        [CSharpCallLua]
        private Action luaOnDestroy;
        [CSharpCallLua]
        private Action luaStart;
        private LuaTable scriptEnv;
        void Awake()
        {
            if (string.IsNullOrEmpty(luaPath)) luaPath=name;
            scriptEnv = LuaEnvMgr.Instance.LuaEnv.NewTable();
            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = LuaEnvMgr.Instance.LuaEnv.NewTable();
            meta.Set("__index", LuaEnvMgr.Instance.LuaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose(); 

            scriptEnv.Set("self", this);
            byte[] lua = LuaEnvMgr.Instance.GetLuaText(luaPath);
            LuaEnvMgr.Instance.LuaEnv.DoString(lua, "LuaMain", scriptEnv);
            //如果使用 require加载的话 self为nil
            //luaEnv.DoString(@"require " + "'" + name + "'", "LuaMain", scriptEnv);
            Action luaAwake = scriptEnv.Get<Action>("Awake");
            scriptEnv.Get("Start", out luaStart);
            scriptEnv.Get("OnDestroy", out luaOnDestroy);
            if (luaAwake != null) luaAwake();
        }
        void Start()
        {
            if (luaStart != null)
                luaStart();
        }
        void OnDestroy()
        {
            if (luaOnDestroy != null) luaOnDestroy();
            StopAllCoroutines();
            luaOnDestroy = null;
            luaStart = null;
            scriptEnv.Dispose();
        }
        IEnumerator UnloadUnusedAssets()
        {
            yield return new WaitForSeconds(1);
            Resources.UnloadUnusedAssets();
            LuaEnvMgr.Instance.RestoreTick();
        }

    }
}

