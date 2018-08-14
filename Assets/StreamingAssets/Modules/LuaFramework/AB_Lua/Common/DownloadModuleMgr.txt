require ("LuaFramework/G_CS")
require ("LuaFramework/Private/G_DownloadModuleMgr")

local DownloadModuleMgr={}
local this=DownloadModuleMgr
--[[
    下载或更新模块
    module              模块名
    completeFunc        下载完成时回调
    progressFunc        单个文件下载进度回调
    totalProgressFunc   总进度回调
    errorFunc           错误回调
]]
function this.DownloadModule(module,completeFunc,progressFunc,totalProgressFunc,errorFunc)
    G_DownloadModuleMgr.DownloadModule(module,completeFunc,progressFunc,totalProgressFunc,errorFunc)
end
--[[
    移除 module 的所有回调事件
    module  模块名
]]
function this.ClearEvent(module)
    G_DownloadModuleMgr.ClearEvent(module)
end
return this