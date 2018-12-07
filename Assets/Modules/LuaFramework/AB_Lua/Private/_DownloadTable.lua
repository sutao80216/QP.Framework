-- [[禁止require]]
require ("LuaFramework/Global")
_DownloadTable={}
local this=_DownloadTable

function this.Befor(moduleName,downloadTotal)
    GameMgr.GetSingle("Event").Execute(DownloadCmd,DownloadCmd.Befor,{
        moduleName=moduleName,
        downloadTotal=downloadTotal
    })
end
function this.Progress(moduleName,result)
    GameMgr.GetSingle("Event").Execute(DownloadCmd,DownloadCmd.Progress,{
        moduleName=moduleName,
        result=result
    })
end

function this.OneComplete(moduleName,downloadedCount,downloadTotal)
    GameMgr.GetSingle("Event").Execute(DownloadCmd,DownloadCmd.OneComplete,{
        moduleName=moduleName,
        downloadedCount=downloadedCount,
        downloadTotal=downloadTotal
    })
end

function this.AllComplete(moduleName)
    GameMgr.GetSingle("Event").Execute(DownloadCmd,DownloadCmd.AllComplete,{
        moduleName=moduleName,
    })
end

function this.Error(moduleName)
    GameMgr.GetSingle("Event").Execute(DownloadCmd,DownloadCmd.Error,{
        moduleName=moduleName,
    })
end