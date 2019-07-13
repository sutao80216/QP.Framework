-- [[禁止require]]
require ("LuaFramework/Global")
_CheckUpdateTable={}
local this=_CheckUpdateTable

local eventTables={}
function this.Complete(moduleName,downloadCount,size)
    GameMgr.GetSingle("Event").Execute(CheckUpdateCmd,CheckUpdateCmd.Complete,{
        moduleName=moduleName,
        downloadCount=downloadCount,
        size=size
    })
end
function this.Error(moduleName)
    GameMgr.GetSingle("Event").Execute(CheckUpdateCmd,CheckUpdateCmd.Complete,{
        moduleName=moduleName,
    })
end