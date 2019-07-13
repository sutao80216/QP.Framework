require ("LuaFramework/Global")
--[[
    一切逻辑从这里开始
    这个脚本比较特殊 不关联MonoBehaviour 只是普通的Lua脚本
]]
function Main()
    print("一切逻辑从这里开始")
    GameMgr.AddSingle("LuaFramework/Single/Event", "Event",true)
    GameMgr.AddSingle("LuaFramework/Single/Module", "Module",true)
    

    GameMgr.GetSingle("Event").AddEvent(DownloadCmd,DownloadCmd.AllComplete,AllComplete)
    -- Modules 必须先下载 主要是为了获取Modules.manifest
    GameMgr.GetSingle("Module").CheckAndDownload("Modules")
    
end


function AllComplete(data)
    if data.moduleName=="Modules" then
    GameMgr.GetSingle("Module").CheckAndDownload("Loading")
    elseif data.moduleName=="Loading" then
        GameMgr.GetSingle("Event").RemoveEvent(DownloadCmd,DownloadCmd.AllComplete,AllComplete)
        GameMgr.GetSingle("Module").JumpScene("Loading","Loading_Main",false,true)
    end
end