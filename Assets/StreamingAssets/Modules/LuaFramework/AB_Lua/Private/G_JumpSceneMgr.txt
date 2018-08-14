--[[
    禁止手动加载此模块 使用Common封装后的模块
]] 
require ("LuaFramework/G_CS")
require ("LuaFramework/G_Global")

G_JumpSceneMgr={}
local this=G_JumpSceneMgr
local _lockModule={}
local _progress={}

function this.Over(module,sceneName)
    if _lockModule[__OVER_SCENE__]~=nil then return end
    _lockModule[__OVER_SCENE__]=true
    local unload=true
    if module==__TARGET_MODULE__ then unload=false end

    __TARGET_MODULE__=module
    __TARGET_SCENE__=sceneName
    
    QP.JumpSceneMgr.Instance:Jump(__OVER_SCENE__,nil,unload,true)
end

function this.Jump(module,sceneName,progressFunc,notAsync)
    if _lockModule[module]~=nil then return end
    _lockModule[module]=true
    _progress[module]=progressFunc
    if notAsync==nil then notAsync=false end
    QP.JumpSceneMgr.Instance:Jump(module,sceneName,true,notAsync)
end

function this.Progress(module,progress)
    if _progress[module]==nil then return end
    _progress[module](module,progress)
end
function this.Complete(module)
    this.Clear(module)
end
function this.Error(module)
    this.Clear(module)
end
function this.Clear(module)
    _lockModule[__OVER_SCENE__]=nil
    _lockModule[module]=nil
    _progress[module]=nil
end