require("LuaFramework/Global")
require("LuaFramework/Private/_CheckUpdateTable")
require("LuaFramework/Private/_DownloadTable")
require("LuaFramework/Private/_SceneTable")
local Event = GameMgr.GetSingle("Event")
function Init()
end
function CheckUpdate(moduleName,isGetSize)
    if moduleName == nil then
        return
    end
    local module = QP.ModuleMgr.Instance:GetModule(moduleName)
    if isGetSize == nil then
        isGetSize = false
    end
    module:CheckUpdate("_CheckUpdateTable", isGetSize)
end
function CheckAndDownload(moduleName)
    if moduleName == nil then
        return
    end
    local module = QP.ModuleMgr.Instance:GetModule(moduleName)
    module:CheckAndDownload("_DownloadTable")
end


function Download(moduleName)
    if moduleName == nil then
        return
    end
    local module = QP.ModuleMgr.Instance:GetModule(moduleName)
    module:Download("_DownloadTable")
end

function JumpScene(moduleName,sceneName,isAsync,isUnloadOtherAssetBundle)
    if moduleName == nil then
        return
    end
    if sceneName == nil then
        return
    end
    local module = QP.ModuleMgr.Instance:GetModule(moduleName)
    module:JumpScene(sceneName, isAsync, isUnloadOtherAssetBundle, "_SceneTable")
end
function OverScene(moduleName,sceneName)
   
    if moduleName == nil then
        return
    end
    if sceneName == nil then
        return
    end
    local unload = true
    -- 如果模块相同 不卸载资源
    if moduleName == __TARGET_MODULE__ then
        unload = false
    end
    G_SetTargetModule(moduleName)
    G_SetTargetScene(sceneName)
    local module = QP.ModuleMgr.Instance:GetModule(__OVER_MODULE__)
    module:JumpScene(__OVER_SCENE__, false, unload, "_SceneTable")
end
function StopDownload(moduleName)
    local module =QP.ModuleMgr.Instance:GetModule(moduleName)
    module:StopDownload()
end