require ("LuaFramework/G_CS")
local Module = require("LuaFramework/Public/Module")

local _transform=self.transform
local _button
local _downloadBtn
local _downloadText

local _module
local _count
local _isDownloading=false

function Init(str)
    _module=str
    -- 检查更新
    Module.CheckUpdate(_module,{
        Complete=CheckUpdateComplete,
        Error=CheckUpdateError
    },true)
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
        Module.Download(_module,{
            Progress=DownloadProgress,
            AllComplete=DownloadAllComplete,
            OneComplete=DownloadOneComplete,
            Error=DownloadError
        })
        _downloadText.text="暂停"
        _isDownloading=true
    else
        Module.StopDownload(_module)
        _downloadText.text="继续"
        _isDownloading=false
    end
    
end
-- 检查更新完成
function CheckUpdateComplete(module,count,size)
    _count=count
    if _count==0 then
        _downloadBtn.gameObject:SetActive(false)
    end
    _downloadText.text=size
end
-- 检查更新失败
function CheckUpdateError(module)

end
-- 每个文件下载的进度
function DownloadProgress(module,result)
    local progress=result.downloadedLength/result.contentLength
    _downloadBtn.image.fillAmount=progress
end

-- 更新下载进度(current:当前下载完成的数量，total:需要下载总数)
function DownloadOneComplete(module,current,total)
    -- local progress=current/total
    -- _downloadBtn.image.fillAmount=progress
    -- _text.text= math.floor((1-progress)*100).."%"
end
-- 下载完成
function DownloadAllComplete(module)
    _downloadBtn.gameObject:SetActive(false)
    _count=0
end
-- 下载失败
function DownloadError(module)

end

function onClick()
    if _count>0 then return end
    Module.OverScene(_module,_module.."_Main")
end

function OnDestroy()
    Module.ClearEvent(_module)
end