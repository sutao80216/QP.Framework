-- [[禁止require]]
require ("LuaFramework/Global")
_SceneTable={}
local this=_SceneTable

function this.Progress(moduleName,progress)
    GameMgr.GetSingle("Event").Execute(SceneCmd,SceneCmd.Progress,{
        moduleName=moduleName,
        progress=progress
    })
end
function this.Complete(moduleName)
    GameMgr.GetSingle("Event").Execute(SceneCmd,SceneCmd.Complete,{
        moduleName=moduleName,
    })
end
function this.Error(moduleName)
    GameMgr.GetSingle("Event").Execute(SceneCmd,SceneCmd.Error,{
        moduleName=moduleName,
    })
end
