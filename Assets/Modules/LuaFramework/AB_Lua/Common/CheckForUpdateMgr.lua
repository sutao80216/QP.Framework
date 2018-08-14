require ("LuaFramework/G_CS")
require ("LuaFramework/Private/G_CheckForUpdateMgr")

local CheckForUpdateMgr={}
local this=CheckForUpdateMgr
--[[
    检查模块更新
    module      模块名
    complete    完成时回调
    error       错误时回调
]]
function this.CheckModule(module,complete,error)
    G_CheckForUpdateMgr.CheckModule(module,complete,error)
end
--[[
    移除事件
]]
function this.ClearEvent(module)
    G_CheckForUpdateMgr.ClearEvent(module)
end
return this