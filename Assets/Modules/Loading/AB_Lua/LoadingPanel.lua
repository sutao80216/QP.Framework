require ("LuaFramework/G_CS")
local DownloadModuleMgr = require("LuaFramework/Common/DownloadModuleMgr")
local JumpSceneMgr = require("LuaFramework/Common/JumpSceneMgr")
local _slider
local _text
local _transform=self.transform
function Awake()
    _slider=_transform:Find("Slider"):GetComponent(typeof(UI.Slider))
    _text=_transform:Find("Text"):GetComponent(typeof(UI.Text))
    DownloadModuleMgr.DownloadModule("GameBox",OnComplete,nil,OnTotalProgress)
end

function OnTotalProgress(module,progress)
    _slider.value=progress
    _text.text=math.floor(progress*100).."%"
end
function OnComplete(module)
    DownloadModuleMgr.DownloadModule("JumpScene",function(module)
        JumpSceneMgr.Jump("GameBox",nil,OnTotalProgress)
    end,nil,OnTotalProgress)
end

