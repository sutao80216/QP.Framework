--[[
    建议禁止手动加载此模块 使用Common封装后的模块
]] 
require ("LuaFramework/G_CS")
G_CheckForUpdateMgr={}
local this=G_CheckForUpdateMgr
local _completes={}
local _errors={}
function this.CheckModule(module,complete,error)
    _completes[module]=complete
    _errors[module]=error
    QP.CheckForUpdateMgr.Instance:CheckModule(module)
end
function this.ClearEvent(module)
    _completes[module]=nil
    _errors[module]=nil
end


function this.Complete(module,count)
    if _completes[module]==nil then return end
    _completes[module](module,count)
    this.ClearEvent(module)
end
function this.Error(module)
    if _errors[module]==nil then return end
    _errors[module](module)
    this.ClearEvent(module)
end

