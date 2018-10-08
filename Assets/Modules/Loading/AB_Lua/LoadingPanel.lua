require ("LuaFramework/G_CS")
local Module = require("LuaFramework/Public/Module")
local _slider
local _progressText
local _sizeText
local _totalImage
local _totalText
local _transform=self.transform
function Awake()
    _slider=_transform:Find("Slider"):GetComponent(typeof(UI.Slider))
    _progressText=_transform:Find("progressText"):GetComponent(typeof(UI.Text))
    _sizeText=_transform:Find("sizeText"):GetComponent(typeof(UI.Text))
    _totalImage=_transform:Find("totalImage"):GetComponent(typeof(UI.Image))
    _totalText=_transform:Find("totalImage/totalText"):GetComponent(typeof(UI.Text))
    -- 首先下载公共资源
    Download("Common",function(module)
        -- 之后下载跳转模块
        Download("JumpScene",function(module)
            -- 最后下载游戏大厅
            Download("GameBox",function(module)
                -- 这里就用到了JumpScene模块来过度场景 所以JumpScene在GameBox之前先下载
                Module.OverScene(module,"GameBox_Main")
            end)
        end)
    end)
end
function DownloadBefor(module,total)
    _totalText.text="0/"..total
end
function DownloadProgress(module,result)
    local progress=result.downloadedLength/result.contentLength
    _slider.value=progress
    _progressText.text=math.floor(progress*100).."%"
    _sizeText.text=result.downloadedLengthStr.."/"..result.contentLengthStr
end
function DownloadOneComplete(module,current,total)
    local progress=current/total
    _totalImage.fillAmount=progress
    _totalText.text=current.."/"..total
end

function DownloadError(module)
    print(module,"下载失败")
end

function Download(module,DownloadAllComplete)
    Module.CheckAndDownload(module,{
        Befor = DownloadBefor,
        Progress = DownloadProgress,
        OneComplete = DownloadOneComplete,
        AllComplete = DownloadAllComplete,
        Error = DownloadError
    })
end

