using System.IO;
using System.Text;
using UnityEngine;
using XLua;
namespace QP.Framework
{
    public class LuaEnvMgr : MonoBehaviour
    {
        private static LuaEnvMgr _instance;
        public static LuaEnvMgr Instance
        {
            get { return Util.GetInstance(ref _instance, "_LuaEnvMgr"); }
        }
        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal static float GCInterval =1;//1 second 
        public LuaEnv LuaEnv{
            get { return luaEnv; }
        }
        public LuaScript Create(GameObject go, string luaPath)
        {
            foreach (var b in go.GetComponents<LuaScript>())
            {
                if (b.LuaPath == luaPath)
                    return b;
            }
            var script = go.AddComponent<LuaScript>();
            script.LuaPath = luaPath;
            script.Awake();
            return script;
        }
        public LuaScript CreateScript(GameObject go, string luaPath)
        {
            var script = go.AddComponent<LuaScript>();
            
            //script.LuaPath = Util.GetLuaPath(luaPath);
            script.LuaPath = luaPath;
            script.Init();
            if (script.gameObject.activeSelf == true)
            {
                script.Awake();
            }
            return script;
        }
        public LuaScript CreateSingle(GameObject go, string luaPath)
        {
            foreach (var b in go.GetComponents<LuaScript>())
            {
                if (b.LuaPath == luaPath)
                    return b;
            }
            return CreateScript(go, luaPath);
        }

        public byte[] GetLuaText(string path)
        {
            string url = Util.GetLuaPath(path);
            if (File.Exists(url))
            {
                return File.ReadAllBytes(url);
            }
            else
            {
                Debug.LogError(url);
                return null;
            }
        }
        public void CallLua(string lua)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("require ('");
            sb.Append(lua);
            sb.Append("') Main() ");
            luaEnv.DoString(sb.ToString());
        }
        void Awake()
        {
            luaEnv.AddLoader((ref string filepath) =>
            {
                return GetLuaText(filepath);
            });
        }
      
        void Update()
        {
            if (Time.time - lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                lastGCTime = Time.time;
            }
        }
       public void FastTick()
        {
            GCInterval = 0;
        }
        public void RestoreTick()
        {
            GCInterval = 1;
        }
    }
}

