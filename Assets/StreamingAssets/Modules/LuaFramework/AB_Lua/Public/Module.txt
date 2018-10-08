require ("LuaFramework/G_CS")
require ("LuaFramework/G_Global")
require ("LuaFramework/Private/_CheckUpdateTable")
require ("LuaFramework/Private/_DownloadTable")
require ("LuaFramework/Private/_SceneTable")
local Module = {}
local this = Module
--[[
    检查更新
    参数：
        moduleName:                              模块名
        eventTable:                              对应 c#的CheckUpdateTable委托
            Complete(模块名 , 需要更新的文件数量): 检查更新完成
            Error(模块名):                       检查更新错误
            isGetSize:                           是否获取文件大小（所有文件总大小）
]]
function this.CheckUpdate(moduleName,eventTable,isGetSize)
    if moduleName==nil then return end
    _CheckUpdateTable.AddEvent(moduleName,eventTable);
    local module =QP.ModuleMgr.Instance:GetModule(moduleName)
    if isGetSize==nil then isGetSize=false end;
    module:CheckUpdate("_CheckUpdateTable",isGetSize)
end
--[[
    下载模块
    参数：
        moduleName:                      模块名
        eventTable:                      对应 c#的DownloadTable委托
            Befor(模块名，下载数量)                               下载前
            Progress(模块名 , SDownloadFileResult结构体):         单个文件的下载进度
            OneComplete(模块名，当前完成数量，总数量):             单个文件下载完成
            AllComplete(模块名)                                  所有文件下载完成
            Error(模块名):                                      下载错误
]]
function this.Download(moduleName,eventTable)
    if moduleName==nil then return end
    _DownloadTable.AddEvent(moduleName,eventTable);
    local module =QP.ModuleMgr.Instance:GetModule(moduleName)
    module:Download("_DownloadTable")
end

function this.StopDownload(moduleName)
    local module =QP.ModuleMgr.Instance:GetModule(moduleName)
    module:StopDownload()
    this.ClearEvent(moduleName);
end

--[[
    检查更新+下载模块
    参数：
        moduleName:                                              模块名
        eventTable:                                              对应 c#的DownloadTable委托
            Befor(模块名，下载数量)                               下载前
            Progress(模块名 , SDownloadFileResult结构体):         单个文件的下载进度
            OneComplete(模块名，当前完成数量，总数量):             单个文件下载完成
            AllComplete(模块名)                                  所有文件下载完成
            Error(模块名):                                      下载错误
]]
function this.CheckAndDownload(moduleName,eventTable)
    if moduleName==nil then return end
    _DownloadTable.AddEvent(moduleName,eventTable);
    local module =QP.ModuleMgr.Instance:GetModule(moduleName)
    module:CheckAndDownload("_DownloadTable")
end
--[[
    跳转场景
    参数：
        moduleName:                 模块名
        sceneName:                  场景名
        isAsync:                    true=异步加载
        isUnloadOtherAssetBundle:   跳转结束时卸载除moduleName之外的其他模块AssetBundle
        eventTable:                 对应 c#的SceneTable委托
            Progress(模块名 , 进度): 加载进度
            Complete(模块名):        加载完成
            Error(模块名):           加载错误
]]
function this.JumpScene(moduleName,sceneName,isAsync,isUnloadOtherAssetBundle,eventTable)
    if moduleName==nil then return end
    if sceneName==nil then return end
    _SceneTable.AddEvent(moduleName,eventTable);
    local module =QP.ModuleMgr.Instance:GetModule(moduleName)
    module:JumpScene(sceneName,isAsync,isUnloadOtherAssetBundle,"_SceneTable")
end
--[[
    过度场景
    参数：
        moduleName:模块名
        sceneName: 场景名
]]
function this.OverScene(moduleName,sceneName)
    if moduleName==nil then return end
    if sceneName==nil then return end
    local unload=true
    -- 如果模块相同 不卸载资源
    if moduleName==__TARGET_MODULE__ then unload=false end

    __TARGET_MODULE__=moduleName
    __TARGET_SCENE__=sceneName
    local module =QP.ModuleMgr.Instance:GetModule(__OVER_MODULE__)
    module:JumpScene(__OVER_SCENE__,false,unload,"_SceneTable")
end

function this.ClearEvent(moduleName)
    _CheckUpdateTable.RemEvent(moduleName);
    _DownloadTable.RemEvent(moduleName);
    _SceneTable.RemEvent(moduleName);
end
return this