using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QP.Framework{
    
    public class BuildConfig
    {
        public const string version = "1.0.0";
        public const string app_download_url = "http://192.168.0.25";
        public const string res_download_url = "http://192.168.0.25";
    }
    public enum GameModel
    {
        Local,
        Editor,
        Remote,
    }

    public class GameConfig
    {
        public const string module_name = "Modules";
        public const string default_module = "LuaFramework";
        public const string version_download_url = "http://192.168.0.25";

        public const int download_Fail_Count = 3;
        public const int download_Fail_Retry_Delay = 2;
        public const GameModel gameModel = GameModel.Editor;
    }
}

 