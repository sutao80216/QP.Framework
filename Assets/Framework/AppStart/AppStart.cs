using QP.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AppStart : MonoBehaviour {
    public VideoPlayer player;
    public Image _image;
    public Text _text;
    private bool isDownloaded;
    private bool isPlayend;
    private string moduleName;
    void Awake()
    {
        _image.gameObject.SetActive(false);
        DownloadVersion();
        PlayVideo();
    }
    private void PlayVideo()
    {
        if (GameConfig.gameModel == GameModel.Editor)
        {
            isPlayend = true;
            StartUpModule();
        }
        else
        {
            player.Play();
            player.loopPointReached += delegate (VideoPlayer source)
            {
                isPlayend = true;
                StartUpModule();
            };
        }
    }
    private void DownloadVersion()
    {
        string version_url = string.Format("{0}/{1}/{2}",
            GameConfig.version_download_url, GameConfig.module_name, GameConfig.version_name);
        new DownloadVersionFile(version_url, GameConfig.download_Fail_Count, GameConfig.download_Fail_Retry_Delay, DownloadVersionCompleted);
    }
    private void DownloadVersionCompleted(VersionResType type, Version version)
    {
        switch (type)
        {
            case VersionResType.DownloadFail:
                Debug.Log("Version 下载失败");
                break;
            case VersionResType.DownloadSuccess:
                moduleName = version.root_module;
                DownloadRootModule();
                break;
            case VersionResType.Different:
                Debug.Log("Version 版本不同");
                break;
            case VersionResType.Unusual:
                Debug.Log("Version 解析异常");
                break;
        }
    }
    private void DownloadRootModule()
    {
        Module module = ModuleMgr.Instance.GetModule(moduleName);
        DownloadTable table = new DownloadTable()
        {
            Befor = DownloadBefor,
            Progress = DownloadProgress,
            OneComplete = DownloadOneComplete,
            AllComplete = DownloadAllComplete,
            Error = DownloadError
        };
        module.CheckAndDownload(table);
    }
    private void StartUpModule()
    {
        if (!isDownloaded || !isPlayend) return;
        if (string.IsNullOrEmpty(moduleName)) return;
        LuaEnvMgr.Instance.CallLua(string.Format("{0}/Main", moduleName));
    }
    private void DownloadBefor(string moduleName,int count)
    {

    }
    private void DownloadProgress(string moduleName, SDownloadFileResult result)
    {

    }
    private void DownloadOneComplete(string moduleName, int downloadedCount, int downloadTotal)
    {
        float progress = (float)downloadedCount / (float)downloadTotal;
        _image.fillAmount = progress;
        _text.text = Mathf.FloorToInt(progress*100) + "%";
        _image.gameObject.SetActive(true);
    }
    private void DownloadAllComplete(string moduleName)
    {
        _text.text ="OK";
        isDownloaded = true;
        StartUpModule();
    }
    private void DownloadError(string moduleName)
    {
        Debug.LogError(moduleName + " 下载失败 ");
    }
}
