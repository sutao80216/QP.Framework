using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace QP.Framework
{
    public class AppStart : MonoBehaviour
    {
        private Version version;
        private bool isWriteMd5File;
        private ThreadDownload thread;
        private DownloadMd5File md5File;
        private int downloadCount;
        private int completeCount;
        private bool canStartUp;
        private bool playinged;
        public VideoPlayer player;
        void Awake()
        {
            canStartUp = false;
            playinged = false;
        }
        private void Start()
        {
            if (GameConfig.gameModel == GameModel.Editor)
            {
                playinged = true;
                canStartUp = true;
                Ready();
                return;
            }
            player.loopPointReached += new VideoPlayer.EventHandler(playerStop);
            player.Play();
            downloadCount = 0;
            completeCount = 0;
            isWriteMd5File = false;
            new DownloadVersion((Version v) => {
                version = v;
                md5File = new DownloadMd5File(version, GameConfig.default_module, onMd5File);
            });
        }
        void playerStop(VideoPlayer p)
        {
            playinged = true;
            StartUp();
        }
        void onMd5File(Queue<DownloadConfig> DownloadList)
        {
            if (DownloadList.Count > 0)
            {
                downloadCount = DownloadList.Count;
                isWriteMd5File = true;
                thread = new ThreadDownload(DownloadList);
                thread.Start();
            }
            else
            {
                Ready();
            }
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        void StartUp()
        {
            if (playinged && canStartUp)
            {
                Debug.Log("启动游戏");
                LuaEnvMgr.Instance.CallLua("LuaFramework/Main");
            }
        }


        void Ready()
        {
            canStartUp = true;
            StartUp();
        }
        private void Update()
        {
            if (thread != null)
            {
                if (thread.Events.Count > 0)
                {
                    DownloadEvent e = thread.Events.Dequeue();
                    switch (e.eventType)
                    {
                        case DownloadEventType.Progress:
                            break;
                        case DownloadEventType.Completed:
                            md5File.UpdateLoaclMd5File(e.config.key);
                            completeCount++;
                            if (completeCount == downloadCount)
                            {
                                md5File.WriteToLocalFile(GameConfig.default_module);
                                isWriteMd5File = false;
                                Ready();
                            }
                            break;
                        case DownloadEventType.Error:
                            Debug.Log("下载失败了");
                            break;
                    }
                }
            }
        }

        void OnDestroy()
        {
            if (isWriteMd5File)
            {
                md5File.WriteToLocalFile(GameConfig.default_module);
            }
            if (thread != null)
                thread.Stop();
        }
    }
}

