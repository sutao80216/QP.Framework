require ("LuaFramework/G_CS")
local CheckForUpdateMgr=require("LuaFramework/Common/CheckForUpdateMgr")
local DownloadModuleMgr = require("LuaFramework/Common/DownloadModuleMgr")
local JumpSceneMgr = require("LuaFramework/Common/JumpSceneMgr")

local _transform=self.transform
local _button
local _slider
local _text
local _module
local _count
function Awake()
    _button=_transform:GetComponent(typeof(UI.Button))
    _button.onClick:AddListener(onClick)
    _slider=_transform:Find("Slider"):GetComponent(typeof(UI.Slider))
    _text=_transform:Find("CheckForUpdate"):GetComponent(typeof(UI.Text))
    _count=1
end
function onClick()
    if _count>0 then
        DownloadModuleMgr.DownloadModule(_module,completeFunc,nil,totalProgressFunc,errorFunc)
    else
        JumpSceneMgr.Over(_module)
    end
end

function completeFunc(module)
    print(module," 下载完成")
    _text.gameObject:SetActive(false)
    _slider.gameObject:SetActive(false)
    _count=0
end

function totalProgressFunc(module,progress)
    _slider.value=1-progress
    _text.text= math.floor((1-progress)*100).."%"
end

function errorFunc(module)
    print(module," 下载失败")
    _text.text="下载失败"..module
end

function Init(str)
    _module=str
    CheckForUpdateMgr.CheckModule(_module,CheckModuleComplete,CheckModuleError)
end

function CheckModuleComplete(module,count)
    _count=count
    if _count==0 then
        _text.gameObject:SetActive(false)
        _slider.gameObject:SetActive(false)
    end
    _text.text="需要下载：".._count.."个文件"
end
function CheckModuleError(module)
    _text.text="检查更新失败"..module
end

function OnDestroy()
    CheckForUpdateMgr.ClearEvent(_module)
    DownloadModuleMgr.ClearEvent(_module)
end