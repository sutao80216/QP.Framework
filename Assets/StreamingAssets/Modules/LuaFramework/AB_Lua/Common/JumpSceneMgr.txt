require ("LuaFramework/G_CS")
require ("LuaFramework/Private/G_JumpSceneMgr")
local JumpSceneMgr={}
local this=JumpSceneMgr
--[[
    过度到 module 模块的 sceneName 场景
    module:         模块名
    sceneName:      默认Main场景
]]
function this.Over(module,sceneName)
    G_JumpSceneMgr.Over(module,sceneName)
end
--[[
    直接跳转到 module 模块的 sceneName 场景
    module:         模块名
    sceneName:      默认Main场景
    progressFunc:   进度回调
    notAsync:       异步加载场景 默认false(默认异步加载)
]]
function this.Jump(module,sceneName,progressFunc,notAsync)
    G_JumpSceneMgr.Jump(module,sceneName,progressFunc,notAsync)
end
return this