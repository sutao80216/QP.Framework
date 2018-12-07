namespace QP.Framework{
    /// <summary>
    /// 打包资源配置
    /// </summary>
    public class BuildConfig
    {
        /// <summary>
        /// 安装包版本
        /// </summary>
        public const string version = "1.0.1";
        /// <summary>
        /// 默认启动模块
        /// </summary>
        public const string root_module = "LuaFramework";
        /// <summary>
        /// 安装包下载地址
        /// </summary>
        public const string app_download_url = "http://192.168.0.143";
        /// <summary>
        /// 资源下载地址
        /// </summary>
        public const string res_download_url = "http://192.168.0.143";
        /// <summary>
        /// 下载失败重试次数
        /// </summary>
        public const int download_fail_retry = 3;
        /// <summary>
        /// true 重新获取文件MD5对比远程MD5   false 本地md5文件对比远程MD5 (只针对lua文件)
        /// </summary>
        public const bool preTamperLua = false;
    }
    public enum GameModel
    {
        /// <summary>
        /// 本地测试热更新
        /// </summary>
        Local,
        /// <summary>
        /// 本地开发模式 不更新
        /// </summary>
        Editor,
        /// <summary>
        /// 打包发布模式
        /// </summary>
        Remote,
    }

    public class GameConfig
    {
        /// <summary>
        /// 模块根目录
        /// </summary>
        public const string module_name = "Modules";
        /// <summary>
        /// 版本配置文件下载地址
        /// </summary>
        public const string version_download_url = "http://192.168.0.143";
        /// <summary>
        /// 版本配置文件名
        /// </summary>
        public const string version_name = "version.txt";
        /// <summary>
        /// md5文件名
        /// </summary>
        public const string md5_name = "md5file.txt";
        /// <summary>
        /// version || md5file 下载失败重试次数
        /// </summary>
        public const int download_Fail_Count = 3;
        /// <summary>
        /// version || md5file 下载失败延迟重试时间 秒
        /// </summary>
        public const int download_Fail_Retry_Delay = 2;
        /// <summary>
        /// 开发时使用Editor || Local  打包发布使用Remote
        /// </summary>
        public const GameModel gameModel = GameModel.Editor;
    }
}

 