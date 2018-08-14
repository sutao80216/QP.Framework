--[[
    建议禁止手动加载此模块 使用Common封装后的模块
]] 
require ("LuaFramework/G_CS")
G_DownloadModuleMgr={}
local this=G_DownloadModuleMgr

local _lockModule={}

local _progress={}
local _totalProgress={}
local _complete={}
local _error={}
--[[
    module  模块名
    completeFunc        下载完成回调
    progressFunc        下载进度回调
    totalProgressFunc   下载总进度回调
    errorFunc           错误回调
]]
function this.DownloadModule(module,completeFunc,progressFunc,totalProgressFunc,errorFunc)
    if module==nil then return end
    if _lockModule[module]~=nil then return end
    _lockModule[module]=true
    _complete[module]=completeFunc
    _progress[module]=progressFunc
    _totalProgress[module]=totalProgressFunc
    _error[module]=errorFunc
    QP.DownloadModuleMgr.Instance:DownLoad(module)
end

function this.ClearEvent(module)
    _lockModule[module]=nil
    _complete[module]=nil
    _progress[module]=nil
    _totalProgress[module]=nil
    _error[module]=nil
end

function this.Progress(module,progress)
    if _progress[module]==nil then return end
    _progress[module](module,progress)
end

function this.TotalProgress(module,progress)
    if _totalProgress[module]==nil then return end
    _totalProgress[module](module,progress)
end

function this.Error(module)
    if _error[module]==nil then return end
    _error[module](module)
    this.ClearEvent(module)
end

function this.Complete(module)
    if _complete[module]==nil then return end
    _complete[module](module)
    this.ClearEvent(module)
end
