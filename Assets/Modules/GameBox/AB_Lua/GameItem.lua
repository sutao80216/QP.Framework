require ("LuaFramework/Global")
-- local Module = require("LuaFramework/Public/Module")

local _transform=self.transform
local _button
local _downloadBtn
local _downloadText

local _module
local _count
local _isDownloading=false
function Init()
    GameMgr.GetSingle("Event").AddEvent(CheckUpdateCmd,CheckUpdateCmd.Complete,CheckUpdateComplete)
    GameMgr.GetSingle("Event").AddEvent(CheckUpdateCmd,CheckUpdateCmd.Error,CheckUpdateError)
    
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.Error,DownloadError)
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.Befor,DownloadBefor)
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.AllComplete,DownloadAllComplete)
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.OneComplete,DownloadOneComplete)
    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.Progress,DownloadProgress)
end
function Set(module)
    _module=module
    -- 检查更新
    GameMgr.GetSingle("Module").CheckUpdate(_module,true)
end
function Awake()
    _button=_transform:GetComponent(typeof(UI.Button))
    _button.onClick:AddListener(onClick)
    _downloadBtn=_transform:Find("downloadBtn"):GetComponent(typeof(UI.Button))
    _downloadText=_downloadBtn.transform:Find("downloadText"):GetComponent(typeof(UI.Text))
    _downloadBtn.onClick:AddListener(onDownloadClick)
    _count=1
    _downloadBtn.image.fillAmount=0
end


function onDownloadClick()
    if _isDownloading==false then
        GameMgr.GetSingle("Module").Download(_module)
        _downloadText.text="暂停"
        _isDownloading=true
    else
        GameMgr.GetSingle("Module").StopDownload(_module)
        _downloadText.text="继续"
        _isDownloading=false
    end
end
-- 检查更新完成
function CheckUpdateComplete(data)
    if data.moduleName~=_module then 
        return
    end
    _count=data.downloadCount
    if _count==0 then
        _downloadBtn.gameObject:SetActive(false)
    end
    _downloadText.text=data.size
    Log.Info("检查更新完成",data)
    RemCheckUpdateEvent()
end
-- 检查更新失败
function CheckUpdateError(data)
    if data.moduleName~=_module then 
        return
    end
    Log.Info("检查更新失败",data)
end
-- 每个文件下载的进度
function DownloadProgress(data)
    if data.moduleName~=_module then 
        return
    end
    local progress=data.result.downloadedLength/data.result.contentLength
    _downloadBtn.image.fillAmount=progress
end

-- 更新下载进度(current:当前下载完成的数量，total:需要下载总数)
function DownloadOneComplete(data)
    if data.moduleName~=_module then 
        return
    end
    -- local progress=current/total
    -- _downloadBtn.image.fillAmount=progress
    -- _text.text= math.floor((1-progress)*100).."%"
end
-- 下载完成
function DownloadAllComplete(data)
    if data.moduleName~=_module then 
        return
    end
    _downloadBtn.gameObject:SetActive(false)
    _count=0
end
-- 下载失败
function DownloadError(data)

end

function onClick()
    if _count>0 then return end
    GameMgr.GetSingle("Module").OverScene(_module,_module.."_Main")
end

function OnDestroy()
    GameMgr.GetSingle("Module").StopDownload(_module)
    RemCheckUpdateEvent()
    RemDownloadEvent()
end
function RemCheckUpdateEvent()
    GameMgr.GetSingle("Event").RemoveEvent(CheckUpdateCmd,CheckUpdateCmd.Complete,CheckUpdateComplete)
    GameMgr.GetSingle("Event").RemoveEvent(CheckUpdateCmd,CheckUpdateCmd.Error,CheckUpdateError)
end
function RemDownloadEvent()
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.Error,DownloadError)
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.Befor,DownloadBefor)
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.AllComplete,DownloadAllComplete)
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.OneComplete,DownloadOneComplete)
    GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.Progress,DownloadProgress)
end