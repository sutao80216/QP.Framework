require ("LuaFramework/Global")
-- local Module = require("LuaFramework/Public/Module")
local Event=GameMgr.GetSingle("Event")
local _slider
local _progressText
local _sizeText
local _totalImage
local _totalText
local _transform=self.transform
function Init()
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.Error,Error)
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.Befor,Befor)
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.AllComplete,AllComplete)
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.OneComplete,OneComplete)
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.Progress,Progress)
end
function Awake()
    _slider=_transform:Find("Slider"):GetComponent(typeof(UI.Slider))
    _progressText=_transform:Find("progressText"):GetComponent(typeof(UI.Text))
    _sizeText=_transform:Find("sizeText"):GetComponent(typeof(UI.Text))
    _totalImage=_transform:Find("totalImage"):GetComponent(typeof(UI.Image))
    _totalText=_transform:Find("totalImage/totalText"):GetComponent(typeof(UI.Text))

end
function Start()
    -- 首先下载公共资源
    GameMgr.GetSingle("Module").CheckAndDownload("Common")
end
function AllComplete(data)
    if data.moduleName=="Common" then 
       -- 之后下载跳转模块
        GameMgr.GetSingle("Module").CheckAndDownload("JumpScene")
    elseif data.moduleName=="JumpScene" then 
        -- 最后下载游戏大厅
        GameMgr.GetSingle("Module").CheckAndDownload("GameBox")
    elseif data.moduleName=="GameBox" then 
        -- 这里就用到了JumpScene模块来过度场景 所以JumpScene在GameBox之前先下载
        GameMgr.GetSingle("Module").OverScene("GameBox","GameBox_Main")
    end
end


function Error(data)
    Log.Error("下载失败",data.moduleName)
end
function Befor(data)
 _totalText.text="0/"..data.downloadTotal
end
function OneComplete(data)
    local progress=data.downloadedCount/data.downloadTotal
    _totalImage.fillAmount=progress
    _totalText.text=data.downloadedCount.."/"..data.downloadTotal
end
function Progress(data)
    local progress=data.result.downloadedLength/data.result.contentLength
    _slider.value=progress
    _progressText.text=math.floor(progress*100).."%"
    _sizeText.text=data.result.downloadedLengthStr.."/"..data.result.contentLengthStr
end

function OnDestroy()
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.Error,Error)
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.Befor,Befor)
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.AllComplete,AllComplete)
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.OneComplete,OneComplete)
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.Progress,Progress)
end

